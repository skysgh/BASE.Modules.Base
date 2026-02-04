using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Scalar.AspNetCore;
using System.IO;
using System.Linq;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Extension methods for API documentation configuration.
    /// Supports three UI options (all versioned):
    /// - OpenAPI (native .NET 10): /openapi/{module}-{version}.json
    /// - Swagger UI: /documentation/apis/{module}/{version}/swagger
    /// - Scalar UI: /documentation/apis/{module}/{version}/scalar
    /// CRITICAL: All endpoints MUST include version AND module for maintainability!
    /// </summary>
    public static class ApiDocumentationExtensions
    {
        /// <summary>
        /// Base path for all API documentation endpoints.
        /// ALWAYS includes module and version - this is NOT optional!
        /// </summary>
        private const string DocumentationBasePath = "/documentation/apis";
        
        /// <summary>
        /// Default API version (update when releasing new major versions).
        /// </summary>
        private const string DefaultApiVersion = "v1";

        /// <summary>
        /// Add API documentation services for a specific module.
        /// Call this for EACH logical module (Sys, Social, Work, etc.)
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="moduleName">Module name (e.g., "sys", "social", "work")</param>
        /// <param name="apiVersion">API version (default: v1). MUST be specified!</param>
        public static IServiceCollection AddApiDocumentation(
            this IServiceCollection services,
            string moduleName,
            string apiVersion = DefaultApiVersion)
        {
            var documentName = $"{moduleName}-{apiVersion}";
            var moduleTitle = $"{char.ToUpper(moduleName[0])}{moduleName.Substring(1)} Module API";
            
            // 1. Built-in OpenAPI (.NET 9+) - Modern, lightweight
            services.AddOpenApi(documentName, options =>
            {
                options.AddDocumentTransformer((document, context, ct) =>
                {
                    document.Info = new()
                    {
                        Title = $"BASE {moduleTitle}",
                        Version = apiVersion,
                        Description = $"Multi-tenant {moduleName} module management API with versioned endpoints"
                    };
                    return Task.CompletedTask;
                });
            });
            
            // 2. API versioning support (if not already added)
            if (!services.Any(x => x.ServiceType.Name.Contains("ApiVersioning")))
            {
                services.AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                });
            }

            // 3. Swagger/Swashbuckle - Traditional, widely adopted
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(documentName, new()
                {
                    Title = $"BASE {moduleTitle}",
                    Version = apiVersion,
                    Description = $"Multi-tenant {moduleName} module API with versioned endpoints"
                });
                
                // CRITICAL: Filter endpoints by module namespace
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (docName != documentName)
                    {
                        return false;
                    }
                    
                    // Get controller type from action descriptor
                    if (apiDesc.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controllerActionDesc)
                    {
                        var controllerType = controllerActionDesc.ControllerTypeInfo;
                        var controllerNamespace = controllerType.Namespace ?? "";
                        
                        // Check if controller belongs to this module (case-insensitive)
                        var moduleNameCapitalized = char.ToUpper(moduleName[0]) + moduleName.Substring(1);
                        var pattern = $".Modules.{moduleNameCapitalized}.";
                        
                        return controllerNamespace.Contains(pattern, StringComparison.OrdinalIgnoreCase);
                    }
                    
                    // Include if we can't determine (safer default)
                    return true;
                });
                
                // Include XML comments if available
                var xmlFile = Path.Combine(AppContext.BaseDirectory, "App.Host.xml");
                if (File.Exists(xmlFile))
                {
                    options.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
                }
            });
            
            return services;
        }

        /// <summary>
        /// Configure API documentation middleware for a specific module.
        /// Call this after app.Build() in the middleware pipeline.
        /// </summary>
        /// <param name="app">Web application</param>
        /// <param name="moduleName">Module name (must match AddApiDocumentation call)</param>
        /// <param name="apiVersion">API version (must match AddApiDocumentation call)</param>
        /// <param name="enableOpenApi">Enable built-in OpenAPI endpoint</param>
        /// <param name="enableSwagger">Enable Swagger UI</param>
        /// <param name="enableScalar">Enable Scalar UI redirect (actual UI configured once globally)</param>
        public static WebApplication UseApiDocumentation(
            this WebApplication app,
            string moduleName,
            string apiVersion = DefaultApiVersion,
            bool enableOpenApi = true,
            bool enableSwagger = true,
            bool enableScalar = true)
        {
            var documentName = $"{moduleName}-{apiVersion}";
            
            if (enableOpenApi)
            {
                // STANDARD: /openapi/sys-v1.json (tools/libraries expect pattern)
                app.MapOpenApi($"/openapi/{documentName}.json")
                    .WithName($"openapi-{documentName}");
            }

            if (enableSwagger)
            {
                // STANDARD: /swagger/sys-v1/swagger.json (tools/libraries expect pattern)
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "/swagger/{documentName}/swagger.json";
                });
                
                // CUSTOM: /documentation/apis/swagger/sys/v1/ (consistent pattern)
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{documentName}/swagger.json",
                        $"{char.ToUpper(moduleName[0])}{moduleName.Substring(1)} Module API {apiVersion}");
                    c.RoutePrefix = $"{DocumentationBasePath.TrimStart('/')}/swagger/{moduleName}/{apiVersion}";
                });
            }

            if (enableScalar)
            {
                // Redirect from: /documentation/apis/scalar/sys/v1/ → /scalar
                app.MapGet($"{DocumentationBasePath}/scalar/{moduleName}/{apiVersion}/", () =>
                {
                    return Results.Redirect("/scalar");
                })
                .WithName($"scalar-redirect-{documentName}")
                .ExcludeFromDescription();
            }

            return app;
        }

        /// <summary>
        /// Configure Scalar UI once for ALL modules at /scalar (default).
        /// Creates convenience redirects from /documentation/apis/scalar/ paths.
        /// Call this AFTER all UseApiDocumentation() calls.
        /// Scalar auto-discovers all /openapi/*.json documents.
        /// </summary>
        /// <param name="app">Web application</param>
        public static WebApplication UseScalarForAllModules(
            this WebApplication app)
        {
            // Scalar at default /scalar path (Scalar's internal routing is inflexible)
            app.MapScalarApiReference(options =>
            {
                options
                    .WithTitle("BASE Platform API Documentation")
                    .WithTheme(Scalar.AspNetCore.ScalarTheme.DeepSpace);
                // Scalar auto-discovers all /openapi/*.json endpoints
            });
            
            // Convenience redirect: /documentation/apis/scalar/ → /scalar
            app.MapGet($"{DocumentationBasePath}/scalar/", () => Results.Redirect("/scalar"))
                .WithName("scalar-base-redirect")
                .ExcludeFromDescription();
            
            return app;
        }
    }
}





















