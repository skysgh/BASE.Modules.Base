using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace App
{
    public static class AssembliesExtensions
    {       /// <summary>
            /// Sorts assemblies in dependency order (dependencies first, dependents last).
            /// </summary>
        public static List<Assembly> TopologicalSort(this IEnumerable<Assembly> assemblies)
        {
            var assemblyList = assemblies.ToList();
            var sorted = new List<Assembly>();
            var visited = new HashSet<string>();
            var visiting = new HashSet<string>();

            foreach (var assembly in assemblyList)
            {
                TopologicalSortVisit(assembly, assemblyList, visited, visiting, sorted);
            }

            return sorted;
        }

        private static void TopologicalSortVisit(
            Assembly assembly,
            List<Assembly> allAssemblies,
            HashSet<string> visited,
            HashSet<string> visiting,
            List<Assembly> sorted)
        {
            var name = assembly.GetName().Name!;

            if (visited.Contains(name)) return;

            if (visiting.Contains(name))
            {
                throw new InvalidOperationException($"Circular dependency detected involving {name}");
            }

            visiting.Add(name);

            // Visit dependencies first (referenced assemblies)
            var referencedNames = assembly.GetReferencedAssemblies()
                .Select(a => a.Name)
                .ToHashSet();

            foreach (var dep in allAssemblies.Where(a => referencedNames.Contains(a.GetName().Name)))
            {
                TopologicalSortVisit(dep, allAssemblies, visited, visiting, sorted);
            }

            visiting.Remove(name);
            visited.Add(name);
            sorted.Add(assembly); // Add AFTER dependencies processed
        }

        /// <summary>
        /// Gets assemblies in same logical module, sorted by dependency order.
        /// Automatically uses AppDomain.CurrentDomain.GetAssemblies() to find loaded assemblies.
        /// </summary>
        /// <param name="topAssembly">The top-level assembly (e.g., Presentation layer)</param>
        /// <returns>List of module assemblies sorted bottom-up (dependencies first)</returns>
        public static List<Assembly> GetModuleAssembliesSorted(this Assembly topAssembly)
        {
            // Automatically get ALL currently loaded assemblies from AppDomain
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Get all assemblies in same module as topAssembly
            var moduleAssemblies = topAssembly
                .GetRelatedAppModuleAssemblies(loadedAssemblies)
                .ToList();

            // Sort them topologically (dependencies first)
            return moduleAssemblies.TopologicalSort();
        }
        /// <summary>
        /// Overload if you want to provide specific assemblies (for testing or special cases).
        /// </summary>
        public static List<Assembly> GetModuleAssembliesSorted(
            this Assembly topAssembly,
            IEnumerable<Assembly> loadedAssemblies)
        {
            var moduleAssemblies = topAssembly
                .GetRelatedAppModuleAssemblies(loadedAssemblies)
                .ToList();

            return moduleAssemblies.TopologicalSort();
        }
    }
}
