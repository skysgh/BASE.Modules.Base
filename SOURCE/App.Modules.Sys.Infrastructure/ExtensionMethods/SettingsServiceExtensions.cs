using App.Modules.Sys.Domain.Domains.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

// Extensions are always put in root namespace
// for maximum usability from elsewhere:

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Extensions for ISettingsService to support strongly-typed configuration objects.
    /// Provides compatibility with Microsoft's Options pattern while adding hierarchy support.
    /// </summary>
    public static class SettingsServiceExtensions
    {
        /// <summary>
        /// Get strongly-typed configuration object from settings.
        /// Compatible with Microsoft's IOptions pattern but with added hierarchy support.
        /// </summary>
        /// <typeparam name="T">Configuration type (POCO)</typeparam>
        /// <param name="settingsService">Settings service</param>
        /// <param name="sectionPath">Section path (e.g., "Settings:System:Security")</param>
        /// <param name="workspaceId">Optional workspace ID for hierarchy</param>
        /// <param name="userId">Optional user ID for hierarchy</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Strongly-typed configuration object</returns>
        /// <remarks>
        /// Example:
        /// <code>
        /// var security = await settingsService.GetSectionAsync&lt;SecuritySettings&gt;(
        ///     "Settings/System/Security");
        /// Console.WriteLine(security.TwoFactorAuthEnabled);
        /// </code>
        /// 
        /// This provides the same experience as IOptions&lt;T&gt; but with:
        /// - Hierarchy support (System/Workspace/User)
        /// - Async loading (can come from DB)
        /// - Runtime reloading (when DB values change)
        /// </remarks>
        public static async Task<T?> GetSectionAsync<T>(
            this ISettingsService settingsService,
            string sectionPath,
            Guid? workspaceId = null,
            Guid? userId = null,
            CancellationToken ct = default) where T : class, new()
        {
            // Normalize path - accept BOTH colon and slash separators
            var normalizedPath = NormalizePath(sectionPath);
            
            // Use our hierarchical settings service
            return await settingsService.GetValueAsync<T>(
                normalizedPath,
                workspaceId,
                userId,
                defaultValue: new T(),
                ct);
        }

        /// <summary>
        /// Synchronous version for compatibility with IOptions&lt;T&gt; pattern.
        /// Note: Blocks on async call - prefer GetSectionAsync when possible.
        /// </summary>
        public static T? GetSection<T>(
            this ISettingsService settingsService,
            string sectionPath,
            Guid? workspaceId = null,
            Guid? userId = null) where T : class, new()
        {
            return settingsService.GetSectionAsync<T>(
                sectionPath, 
                workspaceId, 
                userId).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Normalize configuration path to use consistent slash separators.
        /// Accepts ALL common separators for flexibility.
        /// </summary>
        private static string NormalizePath(string path)
        {
            return path.Replace(":", "/").Replace("__", "/");
        }

        /// <summary>
        /// Register ISettingsService as drop-in replacement for IOptions&lt;T&gt;.
        /// Allows users to use standard Microsoft pattern with YOUR enhanced features.
        /// </summary>
        /// <remarks>
        /// Usage:
        /// <code>
        /// // In Program.cs - register compatibility wrapper
        /// builder.Services.AddSettingsAsOptions&lt;SecuritySettings&gt;("Settings:System:Security");
        /// 
        /// // In your service - inject standard IOptions
        /// public MyService(IOptions&lt;SecuritySettings&gt; options)
        /// {
        ///     var settings = options.Value;  // Works!
        /// }
        /// </code>
        /// </remarks>
        public static IServiceCollection AddSettingsAsOptions<T>(
            this IServiceCollection services,
            string sectionPath) where T : class, new()
        {
            services.AddOptions<T>().Configure((T options, IServiceProvider sp) =>
            {
                var settingsService = sp.GetRequiredService<ISettingsService>();
                var loaded = settingsService.GetSection<T>(sectionPath);
                
                if (loaded != null)
                {
                    // Copy properties from loaded to options
                    var type = typeof(T);
                    foreach (var prop in type.GetProperties())
                    {
                        if (prop.CanWrite)
                        {
                            var value = prop.GetValue(loaded);
                            prop.SetValue(options, value);
                        }
                    }
                }
            });
            
            return services;
        }
    }
}
