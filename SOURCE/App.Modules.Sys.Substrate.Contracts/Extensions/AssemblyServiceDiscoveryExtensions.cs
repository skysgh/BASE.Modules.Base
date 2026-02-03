using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Modules.Sys.Infrastructure.Lifecycles;
using App.Modules.Sys.Shared.Models;
using App.Modules.Sys.Shared.Models.Implementations;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App
#pragma warning restore IDE0130 // Namespace does not match folder structure
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
                    if (!lifetime.HasValue)
                    {
                        continue;
                    }
                    
                    // Get all interfaces except lifecycle markers
                    var interfaces = implType.GetInterfaces()
                        .Where(i => !i.IsLifecycleMarker());
                    
                    foreach (var iface in interfaces)
                    {
                        // Handle generic types - register as open generic
                        Type serviceType;
                        Type implementationType;
                        
                        if (implType.IsGenericTypeDefinition)
                        {
                            // Skip non-generic interfaces when implementation is generic
                            // (e.g., skip IAppLogger when implementing IAppLogger<T>)
                            if (!iface.IsGenericType)
                            {
                                continue;
                            }
                            
                            // Open generic: IAppLogger<> -> AppLogger<>
                            serviceType = iface.GetGenericTypeDefinition();
                            implementationType = implType;
                        }
                        else if (implType.IsGenericType && !implType.IsGenericTypeDefinition)
                        {
                            // Closed generic constructed from open generic - skip, will be resolved at runtime
                            continue;
                        }
                        else
                        {
                            // Non-generic type
                            serviceType = iface;
                            implementationType = implType;
                        }
                        
                        var descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime.Value);
                        descriptors.Add(descriptor);
                        
                        log?.Services.Add(new ImplementationDetails
                        {
                            Interface = serviceType,
                            Implementation = implementationType,
                            Description = $"{lifetime.Value}: {serviceType.Name} → {implementationType.Name}"
                        });
                    }
                    
                    // Also register concrete type if has lifecycle but no interfaces
                    if (!interfaces.Any() && lifetime.HasValue)
                    {
                        if (!implType.IsGenericTypeDefinition)
                        {
                            descriptors.Add(new ServiceDescriptor(implType, implType, lifetime.Value));
                        }
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
            {
                return ServiceLifetime.Singleton;
            }
            if (typeof(IHasScopedLifecycle).IsAssignableFrom(type))
            {
                return ServiceLifetime.Scoped;
            }
            if (typeof(IHasTransientLifecycle).IsAssignableFrom(type))
            {
                return ServiceLifetime.Transient;
            }
            
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

        /// <summary>
        /// Discovers cache objects (ICacheObject implementations) in assembly.
        /// Cache objects are always registered as singletons via their interface inheritance.
        /// </summary>
        public static List<ServiceDescriptor> DiscoverCacheObjects(
            this Assembly assembly,
            StartupLog? log = null)
        {
            var descriptors = new List<ServiceDescriptor>();
            
            try
            {
                var cacheObjectInterface = typeof(App.Modules.Sys.Shared.Services.Caching.ICacheObject);
                
                var types = assembly.GetExportedTypes()
                    .Where(t => t.IsClass 
                             && !t.IsAbstract 
                             && cacheObjectInterface.IsAssignableFrom(t));
                
                foreach (var implType in types)
                {
                    // Register as ICacheObject (singleton via interface inheritance)
                    var descriptor = new ServiceDescriptor(
                        cacheObjectInterface, 
                        implType, 
                        ServiceLifetime.Singleton);
                    
                    descriptors.Add(descriptor);
                    
                    log?.Services.Add(new ImplementationDetails
                    {
                        Interface = cacheObjectInterface,
                        Implementation = implType,
                        Description = $"CacheObject: {implType.Name}"
                    });
                }
            }
            catch (ReflectionTypeLoadException)
            {
                // Log warning but continue - some types may not load
                log?.Notes.Add($"Warning: Could not load some cache object types from {assembly.GetName().Name}");
            }
            
            return descriptors;
        }
    }
}
