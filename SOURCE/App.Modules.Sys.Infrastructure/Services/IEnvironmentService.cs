using App.Modules.Sys.Shared.Services;
using Microsoft.Extensions.Configuration;
using System;

namespace App.Modules.Sys.Infrastructure.Services;

/// <summary>
/// Deployment environment information.
/// Provides runtime environment data (development/staging/production, configuration, paths).
/// </summary>
/// <remarks>
/// Lifetime: Singleton (environment doesn't change during runtime).
/// 
/// Use cases:
/// - Feature flags (enable debug features in dev only)
/// - Configuration selection (dev/prod connection strings)
/// - Logging verbosity (verbose in dev, minimal in prod)
/// - Error detail exposure (full stack traces in dev, minimal in prod)
/// </remarks>
public interface IEnvironmentService : IHasService
{
    // ========================================
    // ENVIRONMENT IDENTIFICATION
    // ========================================

    /// <summary>
    /// Environment name (Development, Staging, Production, etc.).
    /// Typically from ASPNETCORE_ENVIRONMENT or DOTNET_ENVIRONMENT variable.
    /// </summary>
    string EnvironmentName { get; }

    /// <summary>
    /// Whether running in Development environment.
    /// Use for enabling debug features, verbose logging, etc.
    /// </summary>
    bool IsDevelopment { get; }

    /// <summary>
    /// Whether running in Staging environment.
    /// Use for pre-production testing with production-like configuration.
    /// </summary>
    bool IsStaging { get; }

    /// <summary>
    /// Whether running in Production environment.
    /// Use for disabling debug features, minimizing logging, etc.
    /// </summary>
    bool IsProduction { get; }

    /// <summary>
    /// Check if environment matches specific name (case-insensitive).
    /// </summary>
    /// <param name="environmentName">Environment name to check.</param>
    /// <returns>True if environment matches.</returns>
    bool IsEnvironment(string environmentName);

    // ========================================
    // APPLICATION PATHS
    // ========================================

    /// <summary>
    /// Application content root path (where application files reside).
    /// Example: /app or C:\Projects\MyApp
    /// </summary>
    string ContentRootPath { get; }

    /// <summary>
    /// Web root path (where static files are served from).
    /// Example: /app/wwwroot or C:\Projects\MyApp\wwwroot
    /// </summary>
    string? WebRootPath { get; }

    // ========================================
    // CONFIGURATION
    // ========================================

    /// <summary>
    /// Application configuration (appsettings.json, environment variables, etc.).
    /// Use for accessing configuration values.
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// Get configuration value by key.
    /// </summary>
    /// <param name="key">Configuration key (supports colon notation: "Database:ConnectionString").</param>
    /// <returns>Configuration value if exists, null otherwise.</returns>
    string? GetConfigurationValue(string key);

    /// <summary>
    /// Get strongly-typed configuration section.
    /// </summary>
    /// <typeparam name="T">Type to bind configuration to.</typeparam>
    /// <param name="sectionKey">Configuration section key.</param>
    /// <returns>Bound configuration object.</returns>
    T? GetConfigurationSection<T>(string sectionKey) where T : class, new();

    // ========================================
    // RUNTIME INFORMATION
    // ========================================

    /// <summary>
    /// .NET runtime version (e.g., "10.0.0").
    /// </summary>
    string RuntimeVersion { get; }

    /// <summary>
    /// Operating system description (e.g., "Windows 11", "Ubuntu 22.04").
    /// </summary>
    string OperatingSystem { get; }

    /// <summary>
    /// Application name (from assembly or configuration).
    /// </summary>
    string ApplicationName { get; }

    /// <summary>
    /// Application version (from assembly or configuration).
    /// </summary>
    string ApplicationVersion { get; }

    // ========================================
    // FEATURE FLAGS
    // ========================================

    /// <summary>
    /// Whether detailed error pages should be shown.
    /// Typically true in Development, false in Production.
    /// </summary>
    bool ShowDetailedErrors { get; }

    /// <summary>
    /// Whether Swagger/OpenAPI documentation should be enabled.
    /// Typically true in Development/Staging, false in Production.
    /// </summary>
    bool EnableSwagger { get; }

    /// <summary>
    /// Whether database migrations should auto-run on startup.
    /// Typically true in Development, false in Production (manual migrations).
    /// </summary>
    bool EnableAutoMigrations { get; }
}
