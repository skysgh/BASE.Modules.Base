using App.Modules.Sys.Infrastructure.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace App.Modules.Sys.Infrastructure.Services.Implementations;

/// <summary>
/// Production implementation of environment service.
/// Provides deployment environment information and configuration access.
/// </summary>
/// <remarks>
/// Wraps IHostEnvironment to provide clean abstraction for application layer.
/// Singleton lifetime - environment doesn't change during runtime.
/// </remarks>
public sealed class EnvironmentService : IEnvironmentService
{
    private readonly IHostEnvironment _hostEnvironment;

    public EnvironmentService(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    // ========================================
    // ENVIRONMENT IDENTIFICATION
    // ========================================

    public string EnvironmentName => _hostEnvironment.EnvironmentName;

    public bool IsDevelopment => _hostEnvironment.IsDevelopment();

    public bool IsStaging => _hostEnvironment.IsStaging();

    public bool IsProduction => _hostEnvironment.IsProduction();

    public string ApplicationName => _hostEnvironment.ApplicationName;

    // ========================================
    // FILE SYSTEM PATHS
    // ========================================

    public string ContentRootPath => _hostEnvironment.ContentRootPath;

    public string WebRootPath
    {
        get
        {
            // IHostEnvironment doesn't have WebRootPath, but IWebHostEnvironment does
            // For now, return wwwroot under content root
            return Path.Combine(ContentRootPath, "wwwroot");
        }
    }

    public string GetAbsolutePath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            throw new ArgumentException("Relative path cannot be null or whitespace", nameof(relativePath));
        }

        // Remove leading slash if present
        relativePath = relativePath.TrimStart('/', '\\');

        return Path.Combine(ContentRootPath, relativePath);
    }

    // ========================================
    // ENVIRONMENT CHECKS
    // ========================================

    public bool IsEnvironment(string environmentName)
    {
        if (string.IsNullOrWhiteSpace(environmentName))
        {
            throw new ArgumentException("Environment name cannot be null or whitespace", nameof(environmentName));
        }

        return _hostEnvironment.IsEnvironment(environmentName);
    }

    // ========================================
    // FEATURE FLAGS
    // ========================================

    public bool IsFeatureEnabled(string featureName)
    {
        if (string.IsNullOrWhiteSpace(featureName))
        {
            throw new ArgumentException("Feature name cannot be null or whitespace", nameof(featureName));
        }

        // In development, most features are enabled by default
        if (IsDevelopment)
        {
            return true;
        }

        // In production, check environment variables or configuration
        var envVarName = $"FEATURE_{featureName.ToUpperInvariant().Replace('.', '_')}";
        var envValue = Environment.GetEnvironmentVariable(envVarName);

        return !string.IsNullOrEmpty(envValue) && 
               (envValue.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                envValue.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                envValue.Equals("enabled", StringComparison.OrdinalIgnoreCase));
    }

    public bool ShouldExposeDetailedErrors()
    {
        // Only expose detailed errors in development
        return IsDevelopment;
    }

    public bool ShouldEnableSwagger()
    {
        // Swagger in development and staging, not in production
        return IsDevelopment || IsStaging;
    }

    // ========================================
    // CONFIGURATION
    // ========================================

    public string GetEnvironmentVariable(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or whitespace", nameof(key));
        }

        return Environment.GetEnvironmentVariable(key) ?? string.Empty;
    }

    public string GetEnvironmentVariableOrDefault(string key, string defaultValue)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or whitespace", nameof(key));
        }

        var value = Environment.GetEnvironmentVariable(key);
        return string.IsNullOrEmpty(value) ? defaultValue : value;
    }
}
