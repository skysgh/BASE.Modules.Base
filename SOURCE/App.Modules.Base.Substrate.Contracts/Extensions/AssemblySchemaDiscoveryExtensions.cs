using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Modules.Base.Substrate.Models.Messages;
using App.Modules.Base.Substrate.Contracts.Models;

namespace App
{
    /// <summary>
    /// Extension methods for discovering EF Core schema configurations.
    /// </summary>
    public static class AssemblySchemaDiscoveryExtensions
    {
        /// <summary>
        /// Discovers EF Core IEntityTypeConfiguration implementations.
        /// Only scans Infrastructure.Data layers by convention.
        /// </summary>
        public static List<Action<ModelBuilder>> DiscoverDbSchemas(
            this Assembly assembly,
            StartupLog? log = null)
        {
            var results = new List<Action<ModelBuilder>>();
            var assemblyName = assembly.GetName().Name;
            
            // Only scan Infrastructure.Data layers by convention
            if (assemblyName?.Contains("Infrastructure.Data", StringComparison.OrdinalIgnoreCase) != true)
                return results;
            
            try
            {
                // Find IEntityTypeConfiguration<T> implementations
                var schemaTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && 
                               !t.IsAbstract && 
                               t.GetInterfaces().Any(i => 
                                   i.IsGenericType && 
                                   i.GetGenericTypeDefinition().Name.Contains("IEntityTypeConfiguration")));
                
                foreach (var schemaType in schemaTypes)
                {
                    // Capture schemaType in closure
                    var capturedType = schemaType;
                    
                    results.Add(modelBuilder =>
                    {
                        // Create instance and apply configuration
                        var instance = Activator.CreateInstance(capturedType);
                        
                        // Get entity type from IEntityTypeConfiguration<TEntity>
                        var entityType = capturedType.GetInterfaces()
                            .First(i => i.IsGenericType && 
                                       i.GetGenericTypeDefinition().Name.Contains("IEntityTypeConfiguration"))
                            .GetGenericArguments()[0];
                        
                        // Call modelBuilder.ApplyConfiguration<TEntity>(instance)
                        var applyMethod = typeof(ModelBuilder)
                            .GetMethod("ApplyConfiguration")!
                            .MakeGenericMethod(entityType);
                        
                        applyMethod.Invoke(modelBuilder, new[] { instance });
                    });
                    
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
