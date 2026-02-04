using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Scalar.AspNetCore;
using System.IO;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Extension methods for API documentation configuration.
    /// Supports three UI options (all versioned):
    /// - OpenAPI (native .NET 10): /documentation/apis/v1/openapi.json
    /// - Swagger UI: /documentation/apis/v1/swagger
    /// - Scalar UI: /scalar/v1
    /// CRITICAL: All endpoints MUST include version information for maintainability!
    /// </summary>
    public static class ApiDocumentationExtensions
    {
        /// <summary>
        /// Base path for all API documentation endpoints.
        /// ALWAYS includes version number - this is NOT optional!
        /// </summary>
        private const string DocumentationBasePath = "/documentation/apis";
        
        /// <summary>
        /// Default API version (update when releasing new major versions).
        /// </summary>
        private const string DefaultApiVersion = "v1";

        /// <summary>
        /// Add API documentation services (all three: OpenAPI + Swagger + Scalar).
        /// Call this during service configuration (before builder.Build()).
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="apiVersion">API version (default: v1). MUST be specified!</param>
        public static IServiceCollection AddApiDocumentation(
            this IServiceCollection services,
            string apiVersion = DefaultApiVersion)
        {
            // 1. Built-in OpenAPI (.NET 9+) - Modern, lightweight
            services.AddOpenApi(apiVersion);
            
            // 2. API versioning support
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            // 3. Swagger/Swashbuckle - Traditional, widely adopted
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(apiVersion, new()
                {
                    Title = $"BASE System API {apiVersion}",
                    Version = apiVersion,
                    Description = "Multi-tenant system management API with versioned endpoints"
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
        /// Configure API documentation middleware (all three UIs).
        /// Call this after app.Build() in the middleware pipeline.
        /// </summary>
        /// <param name="app">Web application</param>
        /// <param name="apiVersion">API version (default: v1). MUST match AddApiDocumentation!</param>
        /// <param name="enableOpenApi">Enable built-in OpenAPI endpoint (default: true)</param>
        /// <param name="enableSwagger">Enable Swagger UI (default: true)</param>
        /// <param name="enableScalar">Enable Scalar UI (default: true)</param>
        public static WebApplication UseApiDocumentation(
            this WebApplication app,
            string apiVersion = DefaultApiVersion,
            bool enableOpenApi = true,
            bool enableSwagger = true,
            bool enableScalar = true)
        {
            if (enableOpenApi)
            {
                // Native .NET OpenAPI: /openapi/v1.json (STANDARD LOCATION for tools)
                app.MapOpenApi($"/openapi/{apiVersion}.json")
                    .WithName($"openapi-{apiVersion}");
            }

            if (enableSwagger)
            {
                // Swagger JSON: /swagger/v1/swagger.json (STANDARD LOCATION for tools)
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "/swagger/{documentName}/swagger.json";
                });
                
                // Swagger UI: /documentation/apis/v1/swagger (CUSTOM LOCATION for humans)
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{apiVersion}/swagger.json",
                        $"BASE System API {apiVersion}");
                    c.RoutePrefix = $"{DocumentationBasePath.TrimStart('/')}/{apiVersion}/swagger";
                });
            }

            if (enableScalar)
            {
                // Scalar UI: /scalar/v1
                // Points to standard OpenAPI location for tool compatibility
                app.MapScalarApiReference(options =>
                {
                    options
                        .WithTitle($"BASE System API {apiVersion}")
                        .WithTheme(Scalar.AspNetCore.ScalarTheme.DeepSpace)
                        .WithOpenApiRoutePattern($"/openapi/{apiVersion}.json");
                });
            }

            return app;
        }
    }
}









