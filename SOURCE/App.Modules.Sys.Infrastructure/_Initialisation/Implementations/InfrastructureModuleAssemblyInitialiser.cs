using App.Modules.Sys.Initialisation.Implementation.Base;
using App.Modules.Sys.Shared.Services.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using App.Modules.Sys.Infrastructure.Domains.Caching.Implementations;
using App.Modules.Sys.Infrastructure.Domains.Settings.Implementations;
using App.Modules.Sys.Domain.Domains.Configuration;
using App.Modules.Sys.Infrastructure.Domains.Settings;

namespace App.Modules.Sys.Infrastructure.Initialisation.Implementations
{
    /// <summary>
    /// Infrastructure module initializer.
    /// Registers core infrastructure services: caching, settings, etc.
    /// </summary>
    public class InfrastructureModuleAssemblyInitialiser : ModuleAssemblyInitialiserBase
    {
        /// <inheritdoc/>
        public override void DoBeforeBuild(IServiceCollection services)
        {
            // ========================================
            // CACHE OBJECT REGISTRY SERVICE (Tier 1: Self-refreshing objects)
            // ========================================
            services.AddSingleton<ICacheObjectRegistryService, CacheObjectRegistryService>();
            
            // ========================================
            // MEMORY CACHE (Tier 2: Backing store - INTERNAL only)
            // ========================================
            services.AddMemoryCache();
            
            // Note: ICacheService implementations are internal and only
            // accessible through ICacheObjectRegistryService
            
            // TODO: Add distributed cache when needed
            // Services.AddStackExchangeRedisCache(options => {
            //     options.Configuration = Configuration["Redis:ConnectionString"];
            // });
            
            // ========================================
            // SETTINGS SERVICE (Works with or without DB)
            // ========================================
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<SettingsDefaultsLoader>();
            
            // Repository is OPTIONAL - register when DB is ready
            // Services.AddScoped<ISettingsRepository, SettingsRepositoryEFCore>();
        }
        
        /// <inheritdoc/>
        public override void DoAfterBuild(IServiceProvider serviceProvider)
        {
            // ========================================
            // PHASE 2: DISCOVER AND REGISTER CACHE OBJECTS
            // ========================================
            // Use reflection to find all ICacheObject implementations
            // and register them in the cache object registry with DI support
            
            var cacheRegistry = serviceProvider.GetRequiredService<ICacheObjectRegistryService>();
            
            if (cacheRegistry is CacheObjectRegistryService registry)
            {
                // Discover all cache objects via reflection with DI support
                registry.DiscoverAndRegisterAll(serviceProvider);
                
                // Cache objects are now available throughout the application
                // Example usage:
                // var config = await cacheRegistry.GetValueAsync<Dictionary<string, string>>("System.Configuration.AppSettings");
            }
            
            // ========================================
            // PHASE 3: SEED DEFAULT SETTINGS
            // ========================================
            // Load and seed default settings from YAML/JSON if repository is available
            // Note: This will only run once - existing settings are not overwritten
            
            // TODO: Uncomment when repository is implemented
            // var defaultsLoader = ServiceProvider.GetRequiredService<SettingsDefaultsLoader>();
            // var settingsRepo = ServiceProvider.GetService<ISettingsRepository>();
            // if (settingsRepo != null)
            // {
            //     Task.Run(async () => await defaultsLoader.SeedDefaultsAsync(settingsRepo));
            // }
        }
    }
}
