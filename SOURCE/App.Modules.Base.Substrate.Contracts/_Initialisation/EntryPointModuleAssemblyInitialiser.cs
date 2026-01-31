using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Messages;

namespace App.Modules.Base.Substrate.Contracts.Initialisation
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
        /// <param name="configuration">Configuration object (passed as object to minimize dependencies)</param>
        /// <param name="log">Startup log</param>
        public ModuleAssemblyInitialiserBag Initialize(
            Assembly entryPointAssembly,
            object configuration,
            StartupLog log)
        {
            log.Log(LogLevel.Information, "=== DISCOVERING MODULES ===");
            
            // STEP 1: Discover all module assemblies recursively (extension method)
            var moduleAssemblies = entryPointAssembly.DiscoverModuleAssemblies();
            
            // STEP 2: Sort in dependency order using extension method
            var sortedAssemblies = moduleAssemblies.ToList().TopologicalSort();
            
            log.Log(LogLevel.Information, $"Found {sortedAssemblies.Count} module assemblies in dependency order:");
            for (int i = 0; i < sortedAssemblies.Count; i++)
            {
                log.Log(LogLevel.Information, $"  [{i}] {sortedAssemblies[i].GetName().Name}");
            }
            
            // STEP 3: Process each assembly using extension methods
            log.Log(LogLevel.Information, "=== PROCESSING ASSEMBLIES ===");
            var bag = new ModuleAssemblyInitialiserBag { ModuleName = "Application" };
            
            foreach (var assembly in sortedAssemblies)
            {
                ProcessAssembly(assembly, bag, log);
            }
            
            log.Log(LogLevel.Information, 
                $"=== INITIALIZATION COMPLETE ===");
            log.Log(LogLevel.Information,
                $"Services: {bag.LocalServices.Count}, " +
                $"Mappers: {bag.MapperProfiles.Count}, " +
                $"Schemas: {bag.DbSchemas.Count}, " +
                $"Configurers: {bag.ServiceConfigurers.Count}");
            
            return bag;
        }
        
        /// <summary>
        /// Process one assembly - all logic delegated to extension methods.
        /// This method is just orchestration.
        /// </summary>
        private void ProcessAssembly(
            Assembly assembly,
            ModuleAssemblyInitialiserBag bag,
            StartupLog log)
        {
            var assemblyName = assembly.GetName().Name;
            log.Log(LogLevel.Information, $"Processing: {assemblyName}");
            
            // Discover services (extension method)
            var services = assembly.DiscoverServices(log);
            bag.LocalServices.AddRange(services);
            
            // Discover mapper profiles (extension method)
            var mappers = assembly.DiscoverMapperProfiles(log);
            bag.MapperProfiles.AddRange(mappers);
            
            // Discover DB schemas (extension method)
            var schemas = assembly.DiscoverDbSchemas(log);
            bag.DbSchemas.AddRange(schemas);
            
            // Discover service configurers (extension method)
            var configurers = assembly.DiscoverServiceConfigurers(log);
            bag.ServiceConfigurers.AddRange(configurers.Cast<IServiceConfigurer>());
            
            // Log summary for this assembly
            if (services.Any() || mappers.Any() || schemas.Any() || configurers.Any())
            {
                log.Log(LogLevel.Information, 
                    $"  → Services: {services.Count}, " +
                    $"Mappers: {mappers.Count}, " +
                    $"Schemas: {schemas.Count}, " +
                    $"Configurers: {configurers.Count}");
            }
            else
            {
                log.Log(LogLevel.Debug, $"  → (empty assembly)");
            }
        }
    }
}
