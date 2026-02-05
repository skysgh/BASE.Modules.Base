using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace App.Modules.Sys.Infrastructure.Web.Configuration;

/// <summary>
/// Global API configuration applied via startup configuration.
/// Replaces per-controller attributes like [Produces], [Consumes], etc.
/// </summary>
/// <remarks>
/// ARCHITECTURAL RULE: Avoid abstract base classes for cross-cutting concerns.
/// Use middleware or startup configuration instead.
/// 
/// Benefits:
/// - Single place to configure all controllers
/// - No inheritance needed
/// - Easier to maintain
/// - Consistent across all APIs
/// </remarks>
public static class ApiGlobalConfiguration
{
    /// <summary>
    /// Configure global API defaults for all controllers.
    /// Call from Program.cs/Startup.cs.
    /// </summary>
    public static IServiceCollection AddGlobalApiConfiguration(
        this IServiceCollection services,
        IHostEnvironment environment)
    {
        services.AddControllers(options =>
        {
            // Global response format (replaces [Produces("application/json")])
            options.Filters.Add(new ProducesAttribute("application/json"));
            
            // Global request format (replaces [Consumes("application/json")])
            options.Filters.Add(new ConsumesAttribute("application/json"));
            
            // Global exception handling
            // options.Filters.Add<GlobalExceptionFilter>();
            
            // Require HTTPS in production
            if (!environment.IsDevelopment())
            {
                options.Filters.Add(new RequireHttpsAttribute());
            }
        });
        
        return services;
    }
    
    /// <summary>
    /// Configure authorization policies globally.
    /// Replaces per-controller [Authorize] where appropriate.
    /// </summary>
    public static IServiceCollection AddGlobalAuthorizationPolicies(
        this IServiceCollection services,
        IHostEnvironment environment)
    {
        services.AddAuthorization(options =>
        {
            // Default policy: Require authenticated user
            options.FallbackPolicy = environment.IsDevelopment() 
                ? null // Development: No default auth required
                : new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            
            // Diagnostic endpoints: Dev = anonymous, Prod = authenticated
            options.AddPolicy("Diagnostics", policy =>
            {
                if (environment.IsDevelopment())
                {
                    policy.RequireAssertion(_ => true); // Allow all
                }
                else
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Admin", "Developer");
                }
            });
            
            // System endpoints: Always require admin
            options.AddPolicy("SystemAdmin", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("SystemAdmin");
            });
        });
        
        return services;
    }
}
