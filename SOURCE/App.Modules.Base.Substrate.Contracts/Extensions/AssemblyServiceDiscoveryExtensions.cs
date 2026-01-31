using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Modules.Base.Infrastructure.Lifecycles;
using App.Modules.Base.Substrate.Models.Messages;

namespace App
{
    /// <summary>
    /// Extension methods for discovering services via reflection.
    /// </summary>
    public static class AssemblyServiceDiscoveryExtensions
    {
        /// <summary>
        /// Discovers services with lifecycle marker interfaces in assembly.
        /// </summary>
        public static List<ServiceDescriptor> DiscoverServices(
            this Assembly assembly,
            StartupLog? log = null)
        {
            var descriptors = new List<ServiceDescriptor>();
            
            try
            {
                var types = assembly.GetExportedTypes()
                    .Where(t => t.IsClass && !t.IsAbstract);
                
                foreach (var implType in types)
                {
                    var lifetime = implType.DetermineServiceLifetime();
                    if (!lifetime.HasValue) continue;
                    
                    // Get all interfaces except lifecycle markers
                    var interfaces = implType.GetInterfaces()
                        .Where(i => !i.IsLifecycleMarker());
                    
                    foreach (var iface in interfaces)
                    {
                        var descriptor = new ServiceDescriptor(iface, implType, lifetime.Value);
                        descriptors.Add(descriptor);
                        
                        log?.Services.Add(new ImplementationDetails
                        {
                            Interface = iface,
                            Implementation = implType,
                            Description = $"{lifetime.Value}: {iface.Name} ? {implType.Name}"
                        });
                    }
                    
                    // Also register concrete type if has lifecycle but no interfaces
                    if (!interfaces.Any() && lifetime.HasValue)
                    {
                        descriptors.Add(new ServiceDescriptor(implType, implType, lifetime.Value));
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                // Log warning but continue - some types may not load
                log?.Notes.Add($"Warning: Could not load some types from {assembly.GetName().Name}");
            }
            
            return descriptors;
        }
        
        /// <summary>
        /// Determines the DI lifetime from lifecycle marker interfaces.
        /// </summary>
        public static ServiceLifetime? DetermineServiceLifetime(this Type type)
        {
            if (typeof(IHasSingletonLifecycle).IsAssignableFrom(type))
                return ServiceLifetime.Singleton;
            if (typeof(IHasScopedLifecycle).IsAssignableFrom(type))
                return ServiceLifetime.Scoped;
            if (typeof(IHasTransientLifecycle).IsAssignableFrom(type))
                return ServiceLifetime.Transient;
            
            return null; // Not a service
        }
        
        /// <summary>
        /// Checks if interface is a lifecycle marker (should not be registered).
        /// </summary>
        public static bool IsLifecycleMarker(this Type interfaceType)
        {
            return interfaceType.Name.Contains("Lifecycle") ||
                   interfaceType == typeof(IHasSingletonLifecycle) ||
                   interfaceType == typeof(IHasScopedLifecycle) ||
                   interfaceType == typeof(IHasTransientLifecycle);
        }
    }
}
