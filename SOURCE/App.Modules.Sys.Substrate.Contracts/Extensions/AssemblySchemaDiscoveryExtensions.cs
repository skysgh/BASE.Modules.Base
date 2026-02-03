using App.Modules.Sys.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Modules.Sys.Infrastructure.Azure.Models.Implementations.Base;
using App.Modules.Sys.Substrate.Contracts.Models;
using App.Modules.Sys.Shared.Models.Implementations;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Extension methods for discovering EF Core schema configurations.
    /// NO EntityFramework dependency - just stores Type metadata!
    /// </summary>
    public static class AssemblySchemaDiscoveryExtensions
    {
        /// <summary>
        /// Convention: Only Infrastructure.Data layer assemblies contain EF schema configurations.
        /// </summary>
        private const string DataLayerConvention = "Infrastructure.Data";

        /// <summary>
        /// Discovers EF Core IEntityTypeConfiguration implementation TYPES.
        /// Returns Type objects (not Actions) to avoid EF dependency in Contracts.
        /// The actual ModelBuilder configuration happens later in Infrastructure.Data.EF layer.
        /// </summary>
        public static List<Type> DiscoverDbSchemaTypes(
            this Assembly assembly,
            StartupLog? log = null)
        {
            var results = new List<Type>();
            var assemblyName = assembly.GetName().Name;

            // Only scan Infrastructure.Data layers by convention
            if (assemblyName?.Contains(DataLayerConvention, StringComparison.OrdinalIgnoreCase) != true)
            {
                return results;
            }

            try
            {
                // Find IEntityTypeConfiguration<T> implementations using NAME check
                // (avoids hard dependency on EntityFrameworkCore)
                var schemaTypes = assembly.GetTypes()
                    .Where(t => t.IsClass &&
                               !t.IsAbstract &&
                               t.GetInterfaces().Any(i =>
                                   i.IsGenericType &&
                                   i.GetGenericTypeDefinition().Name.Contains("IEntityTypeConfiguration")));

                // Just store the TYPES - no ModelBuilder action needed!
                results.AddRange(schemaTypes);

                foreach (var schemaType in schemaTypes)
                {
                    log?.DbContexts.Add(new ImplementationDetails
                    {
                        Implementation = schemaType,
                        Description = schemaType.Name
                    });
                }
            }
            catch (ReflectionTypeLoadException)
            {
                log?.Notes.Add($"Warning: Could not load DB schemas from {assemblyName}");
            }

            return results;
        }

        /// <summary>
        /// Discovers IServiceConfigurer implementations.
        /// </summary>
        public static List<IServiceConfigurer> DiscoverServiceConfigurers(
            this Assembly assembly,
            StartupLog? log = null)
        {
            var results = new List<IServiceConfigurer>();

            try
            {
                // Find types implementing IServiceConfigurer
                var configurerTypes = assembly.GetTypes()
                    .Where(t => typeof(IServiceConfigurer).IsAssignableFrom(t) &&
                               t.IsClass &&
                               !t.IsAbstract);

                foreach (var configurerType in configurerTypes)
                {
                    var configurer = (IServiceConfigurer)Activator.CreateInstance(configurerType)!;
                    results.Add(configurer);

                    log?.Notes.Add($"    ServiceConfigurer: {configurer.ServiceName}");
                }
            }
            catch (Exception)
            {
                log?.Notes.Add($"Warning: Could not load service configurers from {assembly.GetName().Name}");
            }

            return results;
        }
    }
}