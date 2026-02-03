
using System;
using System.Linq;
using System.Reflection;
using App.Modules.Sys.Shared.Models.Enums;
using App.Modules.Sys.Shared.Models.Implementations;
using App.Modules.Sys.Substrate.Contracts.Models;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Sys.Initialisation.Implementation
{
    /// <summary>
    /// Entry point module initializer - discovers all modules and initializes them.
    /// All logic delegated to extension methods for maximum reusability.
    /// </summary>
    public class EntryPointModuleAssemblyInitialiser
    {
        /// <summary>
        /// ONE METHOD - discovers and initializes everything recursively.
        /// </summary>
        /// <param name="entryPointAssembly">Entry assembly (App.Host)</param>
        /// <param name="configuration">Configuration object (reserved for future use)</param>
        /// <param name="log">Startup log</param>
        /// <param name="services"></param>
        public static ModuleConfigurationBag Initialize(
            Assembly entryPointAssembly,
#pragma warning disable IDE0060 // Remove unused parameter - Reserved for future configuration scenarios
            object configuration,
#pragma warning restore IDE0060
            StartupLog log,
            IServiceCollection services)
        {
            log.Log(TraceLevel.Info, "=== DISCOVERING MODULES ===");
            
            // STEP 1: Discover all module assemblies recursively (extension method)
            var moduleAssemblies = entryPointAssembly.DiscoverModuleAssemblies();
            
            // STEP 2: Sort in dependency order using extension method
            var sortedAssemblies = moduleAssemblies.ToList().TopologicalSort();
            
            log.Log(TraceLevel.Info, $"Found {sortedAssemblies.Count} module assemblies in dependency order:");
            for (int i = 0; i < sortedAssemblies.Count; i++)
            {
                log.Log(TraceLevel.Info, $"  [{i}] {sortedAssemblies[i].GetName().Name}");
            }
            
            // STEP 3: Process each assembly using extension methods
            log.Log(TraceLevel.Info, "=== PROCESSING ASSEMBLIES ===");
            var bag = new ModuleConfigurationBag { ModuleName = "Application" };
            var allInitializers = new System.Collections.Generic.List<App.Modules.Sys.Initialisation.IModuleAssemblyInitialiser>();
            
            foreach (var assembly in sortedAssemblies)
            {
                ProcessAssembly(assembly, bag, log);
                
                // NEW: Discover module initializers
                var initializers = assembly.DiscoverModuleInitializers();
                allInitializers.AddRange(initializers);
            }
            
            // STEP 4: NEW - Call DoBeforeBuild on all module initializers
            if (allInitializers.Count > 0)
            {
                log.Log(TraceLevel.Info, $"=== INVOKING {allInitializers.Count} MODULE INITIALIZERS (BeforeBuild) ===");
                foreach (var initializer in allInitializers)
                {
                    try
                    {
                        var initType = initializer.GetType().Name;
                        log.Log(TraceLevel.Debug, $"Calling DoBeforeBuild on {initType}");
                        initializer.DoBeforeBuild(services);
                    }
                    catch (Exception ex)
                    {
                        log.Log(TraceLevel.Error, 
                            $"ERROR in {initializer.GetType().Name}.DoBeforeBuild: {ex.Message}");
                    }
                }
            }
            
            // Store initializers in bag for Phase 2 (DoAfterBuild)
            bag.ModuleInitializers = allInitializers;
            
            log.Log(TraceLevel.Info, 
                $"=== INITIALIZATION COMPLETE ===");
            log.Log(TraceLevel.Info,
                $"Services: {bag.LocalServices.Count}, " +
                $"Mappers: {bag.MapperProfiles.Count}, " +
                $"Schemas: {bag.DbSchemaTypes.Count}, " +
                $"Configurers: {bag.ServiceConfigurers.Count}, " +
                $"CacheObjects: {bag.CacheObjects.Count}, " +
                $"Initializers: {allInitializers.Count}");
            
            return bag;
        }
        
        /// <summary>
        /// Process one assembly - all logic delegated to extension methods.
        /// This method is just orchestration.
        /// </summary>
        private static void ProcessAssembly(
            Assembly assembly,
            ModuleConfigurationBag bag,
            StartupLog log)
        {
            var assemblyName = assembly.GetName().Name;
            log.Log(TraceLevel.Info, $"Processing: {assemblyName}");
            
            // Discover services (extension method)
            var services = assembly.DiscoverServices(log);
            bag.LocalServices.AddRange(services);
            
            // Discover mapper profiles (extension method)
            var mappers = assembly.DiscoverMapperProfiles(log);
            bag.MapperProfiles.AddRange(mappers);
            
            // Discover DB schemas (extension method)
            var schemas = assembly.DiscoverDbSchemaTypes(log);
            bag.DbSchemaTypes.AddRange(schemas);
            
            // Discover service configurers (extension method)
            var configurers = assembly.DiscoverServiceConfigurers(log);
            bag.ServiceConfigurers.AddRange(configurers.Cast<IServiceConfigurer>());
            
            // Discover cache objects (extension method)
            var cacheObjects = assembly.DiscoverCacheObjects(log);
            bag.CacheObjects.AddRange(cacheObjects);
            
            // Log summary for this assembly
            if (services.Count > 0 || mappers.Count > 0 || schemas.Count > 0 || configurers.Count > 0 || cacheObjects.Count > 0)
            {
                log.Log(TraceLevel.Info, 
                    $"  ✓ Services: {services.Count}, " +
                    $"Mappers: {mappers.Count}, " +
                    $"Schemas: {schemas.Count}, " +
                    $"Configurers: {configurers.Count}, " +
                    $"CacheObjects: {cacheObjects.Count}");
            }
            else
            {
                log.Log(TraceLevel.Debug, $"  ∅ (empty assembly)");
            }
        }
    }
}
