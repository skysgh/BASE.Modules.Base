// using System;
// using System.Collections.Generic;
// using System.Linq;
using App.Modules.Base.Infrastructure.Lifecycles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;

// Extensions are always put in root namespace
// for maximum usability from elsewhere:

namespace App
{
    /// <summary>
    /// Extension methods to the Assembly type.
    /// </summary>
    public static class AssemblyExtensions
    {

        /// <summary>
        /// Get other Assemblies 
        /// in same Logical Module.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        /// <c>DevelopmentException</c>
        public static IEnumerable<Assembly> GetRelatedAppModuleAssemblies(
            this Assembly assembly, IEnumerable<Assembly> assemblies)
        {

            if (!GetAppModuleName(assembly, out string? result, out string? prefix))
            {
#pragma warning disable CA2201 // Do not raise reserved exception types
                throw new Exception("Assembly given has no discernible prefix.");
#pragma warning restore CA2201 // Do not raise reserved exception types
            }

            Assembly[] tmp = assemblies.ToArray();

            return tmp
                .Where(a => a.GetName()!.Name!.StartsWith(prefix!, ignoreCase: true, CultureInfo.InvariantCulture))
                .OrderByDescending(x => x.GetReferencedAssemblies().Length);
        }

        /// <summary>
        /// Gets the Module Name from the Assembly
        /// (presumably a Presentation Assembly 
        /// containing the Controller Action found
        /// via the Route)
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string? GetAppModuleName(this Assembly assembly)
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            GetAppModuleName(assembly, out string? result, out string? prefix);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            return result;
        }

        /// <summary>
        /// Gets the Module Name from the Assembly
        /// (presumably a Presentation Assembly 
        /// containing the Controller Action found
        /// via the Route)
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="result"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static bool GetAppModuleName(this Assembly assembly, out string? result, out string? prefix)
        {
            return assembly.GetName().GetAppModuleName(out result, out prefix);
        }


        /// <summary>
        /// Finds types in an assembly that have the specified Contract.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerHidden]
        public static IEnumerable<Type>? GetInstantiableTypesImplementing(this Assembly assembly, Type type)
        {
            // Return only types that are subsets of the given type (or are same)
            // that are instantiable, and not generic.

            //An Alternate way of investigating whether it is an interface or not
            //is typeof(HasDo).GetInterfaces().Any(x=>x == typeof(IHasDo)).Dump("Implements Interface");
            try
            {
                return assembly.GetTypes().Where(x =>
                    type.IsAssignableFrom(x)
                    &&
                    !x.IsAbstract
                    &&
                    !x.IsInterface
                    &&
                    !x.IsGenericTypeDefinition
                );
            }
            catch (ReflectionTypeLoadException)
            {
                //No biggie
                System.Diagnostics.Trace.TraceInformation($"Running 'GetInstantiableTypesImplementing', could not find {type.Name}");
            }
            return null;
        }

        /// <summary>
        /// Finds types in assemblies that have the specified Contract.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetInstantiableTypesImplementing(
            this Assembly[] assemblies,
            Type type)
        {
            var results = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var resultSet = assembly.GetInstantiableTypesImplementing(type);
                if (resultSet == null)
                {
                    continue;
                }
                results.AddRange(resultSet);
            }
            return results;
        }


        /// <summary>
        /// Determine whether Assembly contains type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ContainsType(this Assembly assembly, Type type)
        {

            return type.Assembly == assembly;
        }




            /// <summary>
            /// Discovers all services in assembly marked with lifecycle interfaces.
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
                        var lifetime = DetermineLifetime(implType);
                        if (!lifetime.HasValue) continue;

                        // Get all interfaces except lifecycle markers
                        var interfaces = implType.GetInterfaces()
                            .Where(i => !IsLifecycleMarker(i));

                        foreach (var iface in interfaces)
                        {
                            var descriptor = new ServiceDescriptor(iface, implType, lifetime.Value);
                            descriptors.Add(descriptor);

                            log?.Services.Add(new ImplementationDetails
                            {
                                Interface = iface,
                                Implementation = implType,
                                Description = $"{lifetime.Value}: {iface.Name} → {implType.Name}"
                            });
                        }

                        // Also register concrete type if no interfaces found
                        if (!interfaces.Any())
                        {
                            descriptors.Add(new ServiceDescriptor(implType, implType, lifetime.Value));
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    log?.Log(LogLevel.Warning,
                        $"Could not load some types from {assembly.GetName().Name}: {ex.Message}");
                }

                return descriptors;
            }

            private static ServiceLifetime? DetermineLifetime(Type type)
            {
                if (typeof(IHasSingletonLifecycle).IsAssignableFrom(type))
                    return ServiceLifetime.Singleton;
                if (typeof(IHasScopedLifecycle).IsAssignableFrom(type))
                    return ServiceLifetime.Scoped;
                if (typeof(IHasTransientLifecycle).IsAssignableFrom(type))
                    return ServiceLifetime.Transient;

                return null; // Not a service
            }

            private static bool IsLifecycleMarker(Type interfaceType)
            {
                return interfaceType.Name.Contains("Lifecycle") ||
                       interfaceType == typeof(IHasSingletonLifecycle) ||
                       interfaceType == typeof(IHasScopedLifecycle) ||
                       interfaceType == typeof(IHasTransientLifecycle);
            }
    }
}