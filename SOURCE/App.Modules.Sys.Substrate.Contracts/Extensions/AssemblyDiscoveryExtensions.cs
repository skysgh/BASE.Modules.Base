using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Extension methods for discovering module assemblies.
    /// </summary>
    public static class AssemblyDiscoveryExtensions
    {
        /// <summary>
        /// Discovers all module assemblies starting from entry point.
        /// Uses BFS to find all referenced App.* assemblies.
        /// </summary>
        /// <param name="entryPoint">Starting assembly (typically App.Host)</param>
        /// <returns>Set of all discovered module assemblies</returns>
        public static HashSet<Assembly> DiscoverModuleAssemblies(this Assembly entryPoint)
        {
            var moduleAssemblies = new HashSet<Assembly>();
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var queue = new Queue<Assembly>();
            queue.Enqueue(entryPoint);
            
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                
                if (moduleAssemblies.Contains(current))
                {
                    continue;
                }
                
                // Only include OUR assemblies (App.*)
                if (!current.IsModuleAssembly())
                {
                    continue;
                }
                
                moduleAssemblies.Add(current);
                
                // Queue referenced assemblies that are also module assemblies
                var referencedNames = current.GetReferencedAssemblies()
                    .Select(a => a.Name)
                    .ToHashSet();
                
                foreach (var asm in allAssemblies
                                      .Where(a => referencedNames.Contains(a.GetName().Name)))
                {
                    if (asm.IsModuleAssembly())
                    {
                        queue.Enqueue(asm);
                    }
                }
            }
            
            return moduleAssemblies;
        }
        
        /// <summary>
        /// Determines if assembly is one of our module assemblies (not framework).
        /// </summary>
        public static bool IsModuleAssembly(this Assembly assembly)
        {
            var name = assembly.GetName().Name;
            
            return name?.StartsWith("App.Modules.", StringComparison.OrdinalIgnoreCase) == true ||
                   name?.StartsWith("App.Host", StringComparison.OrdinalIgnoreCase) == true ||
                   name?.StartsWith("App.Service", StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Discover module initializer classes in an assembly.
        /// Looks for classes implementing IModuleAssemblyInitialiser.
        /// </summary>
        /// <param name="assembly">Assembly to search</param>
        /// <returns>List of instantiated initializer objects</returns>
        public static List<App.Modules.Sys.Initialisation.IModuleAssemblyInitialiser> DiscoverModuleInitializers(
            this Assembly assembly)
        {
            var initializerType = typeof(App.Modules.Sys.Initialisation.IModuleAssemblyInitialiser);
            var initializers = new List<App.Modules.Sys.Initialisation.IModuleAssemblyInitialiser>();

            try
            {
                var types = assembly.GetTypes()
                    .Where(t => initializerType.IsAssignableFrom(t) 
                             && !t.IsAbstract 
                             && !t.IsInterface)
                    .ToList();

                foreach (var type in types)
                {
                    try
                    {
                        var instance = Activator.CreateInstance(type) 
                            as App.Modules.Sys.Initialisation.IModuleAssemblyInitialiser;
                        
                        if (instance != null)
                        {
                            initializers.Add(instance);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log but continue - don't fail entire discovery
                        Console.WriteLine($"Failed to instantiate initializer {type.Name}: {ex.Message}");
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                // Assembly doesn't have these types - that's OK
            }

            return initializers;
        }
    }
}

