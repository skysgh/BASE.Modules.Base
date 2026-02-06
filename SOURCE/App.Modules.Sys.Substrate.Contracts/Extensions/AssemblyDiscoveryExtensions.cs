using System;
using System.Collections.Generic;
using System.IO;
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
        /// Force-load all module assemblies from disk into AppDomain.
        /// MUST be called BEFORE reflection-based discovery.
        /// </summary>
        /// <remarks>
        /// Problem: AppDomain.GetAssemblies() only returns LOADED assemblies.
        /// Solution: Pre-load all App.Modules.*.dll files from bin directory.
        /// 
        /// This enables:
        /// - Development: Loads from bin/
        /// - NuGet: Loads from bin/modules/
        /// - Convention-based discovery
        /// </remarks>
        public static void PreloadModuleAssembliesFromDisk()
        {
            var binPath = AppDomain.CurrentDomain.BaseDirectory;
            var loadedNames = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetName().Name)
                .Where(n => n != null)
                .ToHashSet(StringComparer.OrdinalIgnoreCase)!;
            
            // STRATEGY 1: Load from bin/ (development scenario)
            LoadModuleAssembliesFrom(binPath, loadedNames);
            
            // STRATEGY 2: Load from bin/modules/ (packaged scenario)
            var modulesPath = Path.Combine(binPath, "modules");
            if (Directory.Exists(modulesPath))
            {
                foreach (var moduleDir in Directory.GetDirectories(modulesPath))
                {
                    LoadModuleAssembliesFrom(moduleDir, loadedNames);
                }
            }
        }
        
        /// <summary>
        /// Load all App.Modules.*.dll and App.Host*.dll files from a directory.
        /// </summary>
        private static void LoadModuleAssembliesFrom(string path, HashSet<string?> loadedNames)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            
            var patterns = new[] { "App.Modules.*.dll", "App.Host*.dll", "App.Service*.dll" };
            
            foreach (var pattern in patterns)
            {
                var dlls = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);
                
                foreach (var dll in dlls)
                {
                    try
                    {
                        var assemblyName = AssemblyName.GetAssemblyName(dll);
                        
                        // Skip if already loaded
                        if (loadedNames.Contains(assemblyName.Name!))
                        {
                            continue;
                        }
                        
                        // Load into AppDomain
                        Assembly.LoadFrom(dll);
                        loadedNames.Add(assemblyName.Name!);
                    }
                    catch
                    {
                        // Ignore load failures (wrong architecture, dependencies missing, etc.)
                    }
                }
            }
        }
        
        /// <summary>
        /// Discovers all module assemblies starting from entry point.
        /// Uses TWO strategies:
        /// 1. BFS through referenced assemblies
        /// 2. Scan ALL loaded assemblies in AppDomain
        /// </summary>
        /// <param name="entryPoint">Starting assembly (typically App.Host)</param>
        /// <returns>Set of all discovered module assemblies</returns>
        /// <remarks>
        /// IMPORTANT: Call PreloadModuleAssembliesFromDisk() FIRST!
        /// Otherwise Strategy 2 won't find unloaded assemblies.
        /// </remarks>
        public static HashSet<Assembly> DiscoverModuleAssemblies(this Assembly entryPoint)
        {
            var moduleAssemblies = new HashSet<Assembly>();
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            // STRATEGY 1: BFS from entry point through references
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
                    if (asm.IsModuleAssembly() && !moduleAssemblies.Contains(asm))
                    {
                        queue.Enqueue(asm);
                    }
                }
            }
            
            // STRATEGY 2: Scan ALL loaded assemblies for any we missed
            // This catches assemblies loaded via reflection or indirect references
            foreach (var asm in allAssemblies)
            {
                if (asm.IsModuleAssembly() && !moduleAssemblies.Contains(asm))
                {
                    moduleAssemblies.Add(asm);
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

