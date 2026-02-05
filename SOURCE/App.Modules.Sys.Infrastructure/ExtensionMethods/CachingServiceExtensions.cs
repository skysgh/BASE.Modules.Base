using App.Modules.Sys.Infrastructure.Domains.Caching.Implementations;
using App.Modules.Sys.Infrastructure.Domains.Caching.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// Extensions are always put in root namespace
// for maximum usability from elsewhere:

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App.Modules.Sys.Infrastructure.ExtensionMethods
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// DI registration for caching services.
    /// Registers default in-memory implementation.
    /// Can be overridden with Redis/SQL implementation when Azure detected.
    /// </summary>
    public static class CachingServiceExtensions
    {
        /// <summary>
        /// Add default in-memory cache service (Tier 2).
        /// Automatically replaced if Redis/Azure cache detected.
        /// </summary>
        public static IServiceCollection AddDefaultCacheService(
            this IServiceCollection services)
        {
            // Add framework memory cache
            services.AddMemoryCache();
            
            // Register OUR internal ICacheService (Tier 2)
            // Only if not already registered (allows override)
            services.TryAddSingleton<IBackingCacheService, MemoryBackingCacheService>();
            
            return services;
        }

        /// <summary>
        /// Replace default cache with Redis implementation.
        /// Call this when Azure.Infrastructure assembly detected.
        /// TODO: Implement when Redis cache service created.
        /// </summary>
        public static IServiceCollection UseRedisCacheService(
            this IServiceCollection services,
            string connectionString)
        {
            // TODO: Replace MemoryCacheService with RedisCacheService
            // services.Replace(ServiceDescriptor.Singleton<ICacheService, RedisCacheService>());
            
            throw new System.NotImplementedException(
                "Redis cache implementation pending Azure.Infrastructure integration");
        }
    }
}
