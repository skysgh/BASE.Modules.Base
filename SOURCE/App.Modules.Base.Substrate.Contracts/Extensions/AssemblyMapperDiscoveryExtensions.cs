using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Modules.Base.Substrate.Models.Messages;

namespace App
{
    /// <summary>
    /// Extension methods for discovering AutoMapper profiles.
    /// </summary>
    public static class AssemblyMapperDiscoveryExtensions
    {
        /// <summary>
        /// Discovers AutoMapper Profile classes in assembly.
        /// Only scans Application-layer assemblies by convention.
        /// </summary>
        public static List<(Type Profile, string Description)> DiscoverMapperProfiles(
            this Assembly assembly,
            StartupLog? log = null)
        {
            var results = new List<(Type, string)>();
            var assemblyName = assembly.GetName().Name;
            
            // Only scan Application layers by convention
            if (assemblyName?.Contains("Application", StringComparison.OrdinalIgnoreCase) != true)
                return results;
            
            try
            {
                // Look for types inheriting from Profile (AutoMapper base class)
                // Using name check to avoid hard dependency on AutoMapper here
                var profiles = assembly.GetTypes()
                    .Where(t => t.IsClass && 
                               !t.IsAbstract && 
                               t.BaseType?.Name == "Profile");
                
                foreach (var profile in profiles)
                {
                    results.Add((profile, $"{assemblyName}: {profile.Name}"));
                    
                    log?.ObjectMapDefinitions.Add(new ImplementationDetails
                    {
                        Implementation = profile,
                        Description = profile.Name
                    });
                }
            }
            catch (ReflectionTypeLoadException)
            {
                log?.Notes.Add($"Warning: Could not load mapper profiles from {assemblyName}");
            }
            
            return results;
        }
    }
}
