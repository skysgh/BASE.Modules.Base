using App.Modules.Sys.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace App.Modules.Sys.Infrastructure.Web.Services.Implementations;

/// <summary>
/// Web-specific implementation of environment service.
/// Provides deployment environment information and configuration access.
/// </summary>
/// <remarks>
/// Uses IWebHostEnvironment for web-specific features (WebRootPath).
/// Singleton lifetime - environment doesn't change during runtime.
/// 
/// Located in Infrastructure.Web because it depends on ASP.NET Core types.
/// </remarks>
public sealed class EnvironmentService : IEnvironmentService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;

    public EnvironmentService(
        IWebHostEnvironment webHostEnvironment,
        IConfiguration configuration)
    {
        _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    // ========================================
    // ENVIRONMENT IDENTIFICATION
    // ========================================

    public string EnvironmentName => _webHostEnvironment.EnvironmentName;

    public bool IsDevelopment => _webHostEnvironment.IsDevelopment();

    public bool IsStaging => _webHostEnvironment.IsStaging();

    public bool IsProduction => _webHostEnvironment.IsProduction();

    public bool IsEnvironment(string environmentName)
    {
        if (string.IsNullOrWhiteSpace(environmentName))
        {
            throw new ArgumentException("Environment name cannot be null or whitespace", nameof(environmentName));
        }

        return _webHostEnvironment.IsEnvironment(environmentName);
    }

    // ========================================
    // APPLICATION PATHS
    // ========================================

    public string ContentRootPath => _webHostEnvironment.ContentRootPath;

    public string? WebRootPath => _webHostEnvironment.WebRootPath;

    // ========================================
    // CONFIGURATION
    // ========================================

    public IConfiguration Configuration => _configuration;

    public string? GetConfigurationValue(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or whitespace", nameof(key));
        }

        return _configuration[key];
    }

    public T? GetConfigurationSection<T>(string sectionKey) where T : class, new()
    {
        if (string.IsNullOrWhiteSpace(sectionKey))
        {
            throw new ArgumentException("Section key cannot be null or whitespace", nameof(sectionKey));
        }

        var section = _configuration.GetSection(sectionKey);
        if (!section.Exists())
        {
            return null;
        }

        var instance = new T();
        section.Bind(instance);
        return instance;
    }

    // ========================================
    // RUNTIME INFORMATION
    // ========================================

    public string RuntimeVersion => RuntimeInformation.FrameworkDescription;

    public string OperatingSystem
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "Windows";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "Linux";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "macOS";
            }
            return "Unknown";
        }
    }

    public string ApplicationName => _webHostEnvironment.ApplicationName;

    public string ApplicationVersion
    {
        get
        {
            var assembly = Assembly.GetEntryAssembly();
            var version = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                         ?? assembly?.GetName().Version?.ToString()
                         ?? "1.0.0";
            return version;
        }
    }

    // ========================================
    // FEATURE FLAGS
    // ========================================

    public bool ShowDetailedErrors => IsDevelopment;

    public bool EnableSwagger => IsDevelopment || IsStaging;

    public bool EnableAutoMigrations
    {
        get
        {
            // Check configuration first
            var configValue = _configuration["Features:EnableAutoMigrations"];
            if (bool.TryParse(configValue, out var enabled))
            {
                return enabled;
            }

            // Default: enabled in development only
            return IsDevelopment;
        }
    }
}

