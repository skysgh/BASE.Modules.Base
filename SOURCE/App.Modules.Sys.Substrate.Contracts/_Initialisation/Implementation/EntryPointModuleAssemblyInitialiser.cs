
using System;
using System.Linq;
using System.Reflection;
using App.Modules.Sys.Infrastructure.Domains.Initialisation;
using App.Modules.Sys.Infrastructure.Services.Configuration;
using App.Modules.Sys.Shared.Models.Enums;
using App.Modules.Sys.Shared.Models.Implementations;
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
        /// Discovers and initializes all modules.
        /// Returns one bag per logical module (Sys, Core, Accounts, etc).
        /// </summary>
        /// <param name="entryPointAssembly">Entry assembly (App.Host)</param>
        /// <param name="configuration">Configuration object (reserved for future use)</param>
        /// <param name="log">Startup log</param>
        /// <param name="services">Service collection for BeforeBuild registration</param>
        /// <returns>Dictionary of module bags keyed by module name</returns>
        public static System.Collections.Generic.Dictionary<string, ModuleConfigurationBag> Initialize(
            Assembly entryPointAssembly,
#pragma warning disable IDE0060 // Remove unused parameter - Reserved for future configuration scenarios
            object configuration,
#pragma warning restore IDE0060
            StartupLog log,
            IServiceCollection services)
        {
            log.Log(TraceLevel.Info, "=== DISCOVERING MODULES ===");
            
            // STEP 0: Pre-load all module assemblies from disk into AppDomain
            // CRITICAL: Must be done BEFORE reflection scan
            log.Log(TraceLevel.Debug, "Pre-loading module assemblies from disk...");
            AssemblyDiscoveryExtensions.PreloadModuleAssembliesFromDisk();
            
            // STEP 1: Discover all module assemblies recursively (extension method)
            var moduleAssemblies = entryPointAssembly.DiscoverModuleAssemblies();
            
            // STEP 2: Sort in dependency order using extension method
            var sortedAssemblies = moduleAssemblies.ToList().TopologicalSort();

            log.Log(TraceLevel.Info, $"Found {sortedAssemblies.Count} module assemblies in dependency order:");
            for (int i = 0; i < sortedAssemblies.Count; i++)
            {
                log.Log(TraceLevel.Info, $"  [{i}] {sortedAssemblies[i].GetName().Name}");
            }
            
            // STEP 3: Group assemblies by module name
            log.Log(TraceLevel.Info, "=== GROUPING BY MODULE ===");
            var assemblyGroups = sortedAssemblies
                .GroupBy(a => ExtractModuleName(a))
                .Where(g => g.Key != null)  // Skip assemblies without module name
                .ToDictionary(g => g.Key!, g => g.ToList());
            
            log.Log(TraceLevel.Info, $"Found {assemblyGroups.Count} logical modules:");
            foreach (var moduleName in assemblyGroups.Keys)
            {
                log.Log(TraceLevel.Info, $"  - {moduleName} ({assemblyGroups[moduleName].Count} assemblies)");
            }
            
            // STEP 4: Create one bag per module and process
            log.Log(TraceLevel.Info, "=== PROCESSING MODULES ===");
            var moduleBags = new System.Collections.Generic.Dictionary<string, ModuleConfigurationBag>();
            
            foreach (var kvp in assemblyGroups)
            {
                var moduleName = kvp.Key;
                var assemblies = kvp.Value;
                
                log.Log(TraceLevel.Info, $"\n=== Processing Module: {moduleName} ===");
                
                var bag = new ModuleConfigurationBag { ModuleName = moduleName };
                bag.Assemblies.AddRange(assemblies);
                
                foreach (var assembly in assemblies)
                {
                    ProcessAssembly(assembly, bag, log);
                    
                    // Discover module initializers
                    var initializers = assembly.DiscoverModuleInitializers();
                    bag.ModuleInitializers.AddRange(initializers);
                }
                
                log.Log(TraceLevel.Info, 
                    $"Module {moduleName} summary: " +
                    $"Services: {bag.LocalServices.Count}, " +
                    $"Mappers: {bag.MapperProfiles.Count}, " +
                    $"Schemas: {bag.DbSchemaTypes.Count}, " +
                    $"Configurers: {bag.ServiceConfigurers.Count}, " +
                    $"CacheObjects: {bag.CacheObjects.Count}, " +
                    $"Initializers: {bag.ModuleInitializers.Count}");
                
                moduleBags[moduleName] = bag;
            }
            
            // STEP 5: Call BeforeBuild for ALL modules
            log.Log(TraceLevel.Info, "\n=== INVOKING MODULE INITIALIZERS (BeforeBuild) ===");
            foreach (var bag in moduleBags.Values)
            {
                if (bag.ModuleInitializers.Count > 0)
                {
                    log.Log(TraceLevel.Info, $"Module {bag.ModuleName}: {bag.ModuleInitializers.Count} initializers");
                    
                    foreach (var initializer in bag.ModuleInitializers)
                    {
                        try
                        {
                            var initType = initializer.GetType().Name;
                            log.Log(TraceLevel.Debug, $"  Calling DoBeforeBuild on {initType}");
                            initializer.DoBeforeBuild(services);
                        }
                        catch (Exception ex)
                        {
                            log.Log(TraceLevel.Error, 
                                $"  ERROR in {initializer.GetType().Name}.DoBeforeBuild: {ex.Message}");
                        }
                    }
                }
            }
            
            log.Log(TraceLevel.Info, "\n=== INITIALIZATION COMPLETE ===");
            log.Log(TraceLevel.Info, $"Total modules processed: {moduleBags.Count}");
            
            return moduleBags;
        }
        
        /// <summary>
        /// Extract logical module name from assembly name.
        /// </summary>
        /// <param name="assembly">Assembly to extract module name from</param>
        /// <returns>Module name (e.g., "Sys", "Core", "Accounts") or null if not a module</returns>
        /// <remarks>
        /// Examples:
        /// - App.Modules.Sys.Application → "Sys"
        /// - App.Modules.Core.Infrastructure → "Core"
        /// - App.Host → "Host"
        /// - Microsoft.Extensions.Logging → null (not our module)
        /// </remarks>
        private static string? ExtractModuleName(Assembly assembly)
        {
            var name = assembly.GetName().Name;

            if (name == null)
            {
                return null;
            }
            
            // Pattern: App.Modules.{ModuleName}.{Layer}
            if (name.StartsWith("App.Modules.", StringComparison.OrdinalIgnoreCase))
            {
                var parts = name.Split('.');
                if (parts.Length >= 3)
                {
                    return parts[2];  // Extract module name (Sys, Core, Accounts)
                }
            }
            
            // Special case: App.Host
            if (name.StartsWith("App.Host", StringComparison.OrdinalIgnoreCase) ||
                name.StartsWith("App.Service", StringComparison.OrdinalIgnoreCase))
            {
                return "Host";
            }
            
            return null;  // Not a module assembly
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
