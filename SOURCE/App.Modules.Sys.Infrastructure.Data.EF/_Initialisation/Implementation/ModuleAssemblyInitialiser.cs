using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.DbContexts.Implementations;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services.Implementations;
using App.Modules.Sys.Initialisation;
using App.Modules.Sys.Initialisation.Implementation.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App.Modules.Sys.Infrastructure.Data.EF.Initialisation.Implementation
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Assembly specific implementation of IModuleAssemblyInitialiser.
    /// Registers DbContext, scoped provider, and discovers save handlers.
    /// </summary>
    public class ModuleAssemblyInitialiser : ModuleAssemblyInitialiserBase
    {

        ///<inheritdoc/>
        public override void DoBeforeBuild(IServiceCollection services)
        {
            // Register ModuleDbContext with SQL Server
            services.AddDbContext<ModuleDbContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("Default");
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException(
                        "Connection string 'Default' not found in configuration. " +
                        "Ensure ConnectionStrings:Default is defined in appsettings.json");
                }
                
                options.UseSqlServer(connectionString);
            });

            // Register ScopedDbContextProviderService as SINGLETON
            // This allows singleton repositories to inject it safely
            // Provider uses HttpContextAccessor internally to resolve DbContext from request scope at runtime
            services.AddSingleton<IScopedDbContextProviderService, ScopedDbContextProviderService>();
        }

        /// <inheritdoc/>
        public override void DoAfterBuild(IServiceProvider serviceProvider)
        {

        // Get the handler registry (singleton)
        var registry = serviceProvider.GetService<IDbContextSaveHandlerRegistryService>();
            if (registry != null)
            {
                registry.DiscoverAndRegister(Assembly.GetExecutingAssembly());
            }
        }

    }
}
