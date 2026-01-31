using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Configuration bag collected during module initialization.
    /// Contains all discovered services, mappers, schemas, and configurers.
    /// </summary>
    public class ModuleAssemblyInitialiserBag
    {
        /// <summary>
        /// Name of the module (e.g., "Base", "Core", "Application")
        /// </summary>
        public string ModuleName { get; init; } = string.Empty;
        
        /// <summary>
        /// Services discovered via reflection (lifecycle markers).
        /// </summary>
        public List<ServiceDescriptor> LocalServices { get; } = new();
        
        /// <summary>
        /// Remote service placeholders (configured in Phase 2).
        /// </summary>
        public List<ServiceDescriptor> RemoteServicePlaceholders { get; } = new();
        
        /// <summary>
        /// Service configurers that need IServiceProvider to activate (Phase 2).
        /// </summary>
        public List<IServiceConfigurer> ServiceConfigurers { get; } = new();
        
        /// <summary>
        /// AutoMapper Profile types and descriptions.
        /// </summary>
        public List<(Type Profile, string Description)> MapperProfiles { get; } = new();
        
        /// <summary>
        /// EF Core schema configurations (IEntityTypeConfiguration).
        /// </summary>
        public List<Action<ModelBuilder>> DbSchemas { get; } = new();
        
        /// <summary>
        /// Notes about the initialization process.
        /// </summary>
        public List<string> Notes { get; } = new();
    }
    
    /// <summary>
    /// Service configurer interface - for services needing credentials/complex setup.
    /// Executed in Phase 2 after IServiceProvider is built.
    /// </summary>
    public interface IServiceConfigurer
    {
        /// <summary>
        /// Name of the service being configured (for logging).
        /// </summary>
        string ServiceName { get; }
        
        /// <summary>
        /// Phase 2: Configure service after IServiceProvider is built.
        /// Can resolve ICredentialService and other dependencies.
        /// </summary>
        /// <param name="serviceProvider">Built service provider</param>
        /// <param name="configuration">Configuration object (IConfiguration cast to object to avoid dependency)</param>
        /// <param name="log">Startup log (StartupLog cast to object to avoid dependency)</param>
        void ConfigureService(
            IServiceProvider serviceProvider,
            object configuration,
            object log);
    }
}
