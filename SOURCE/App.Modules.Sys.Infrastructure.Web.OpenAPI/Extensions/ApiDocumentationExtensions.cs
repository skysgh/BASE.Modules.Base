using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Extension methods for API documentation configuration.
    /// CRITICAL: All API endpoints MUST include version information (/v1/, /v2/, etc.)
    /// to ensure long-term maintainability and backward compatibility.
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
        /// Add API documentation services (Swagger + Scalar with versioning).
        /// Call this during service configuration (before builder.Build()).
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="apiVersion">API version (default: v1). MUST be specified!</param>
        public static IServiceCollection AddApiDocumentation(
            this IServiceCollection services,
            string apiVersion = DefaultApiVersion)
        {
            // API versioning support
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

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
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "App.Host.xml"), includeControllerXmlComments: true);
            });
            
            return services;
        }

        /// <summary>
        /// Configure API documentation middleware (Swagger + Scalar).
        /// Call this after app.Build() in the middleware pipeline.
        /// </summary>
        /// <param name="app">Web application</param>
        /// <param name="apiVersion">API version (default: v1). MUST match AddApiDocumentation!</param>
        /// <param name="enableSwagger">Enable Swagger UI (default: true)</param>
        /// <param name="enableScalar">Enable Scalar UI (reserved for future use)</param>
#pragma warning disable IDE0060 // Remove unused parameter - Reserved for Scalar implementation
        public static WebApplication UseApiDocumentation(
            this WebApplication app,
            string apiVersion = DefaultApiVersion,
            bool enableSwagger = true,
            bool enableScalar = true)
#pragma warning restore IDE0060
        {
            if (enableSwagger)
            {
                // Swagger JSON: /documentation/apis/v1/swagger.json
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = $"{DocumentationBasePath}/{apiVersion}/swagger.json";
                });
                
                // Swagger UI: /documentation/apis/v1/swagger
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(
                        $"{DocumentationBasePath}/{apiVersion}/swagger.json",
                        $"BASE System API {apiVersion}");
                    c.RoutePrefix = $"{DocumentationBasePath.TrimStart('/')}/{apiVersion}/swagger";
                });
            }

            // TODO: Add Scalar UI support when API is clarified
            // Scalar.AspNetCore 2.x has different API than 1.x
            // For now, Swagger provides full documentation at versioned endpoint
            // if (enableScalar)
            // {
            //     app.MapScalarApiReference();
            // }

            return app;
        }
    }
}



