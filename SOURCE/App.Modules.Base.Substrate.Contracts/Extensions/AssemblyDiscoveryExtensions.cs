using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace App
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
                    continue;
                
                // Only include OUR assemblies (App.*)
                if (!current.IsModuleAssembly())
                    continue;
                
                moduleAssemblies.Add(current);
                
                // Queue referenced assemblies that are also module assemblies
                var referencedNames = current.GetReferencedAssemblies()
                    .Select(a => a.Name)
                    .ToHashSet();
                
                foreach (var asm in allAssemblies.Where(a => referencedNames.Contains(a.GetName().Name)))
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
    }
}
