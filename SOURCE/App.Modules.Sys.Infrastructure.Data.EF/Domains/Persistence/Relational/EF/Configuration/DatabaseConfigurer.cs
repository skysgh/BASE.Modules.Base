using App.Modules.Sys.Infrastructure.Services.Configuration;
using Microsoft.Extensions.Configuration;
using System;

namespace App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Configuration
{
    /// <summary>
    /// Configures database services after DI container is built.
    /// Runs FIRST (Order = 0) to ensure DB is ready before other configurers.
    /// Uses ONLY IConfiguration (NOT ISettingsService) to avoid circular dependency.
    /// </summary>
    /// <remarks>
    /// Dependency Chain:
    /// 1. This configurer uses IConfiguration (appsettings.json) for connection strings
    /// 2. Configures DbContext with connection string
    /// 3. NOW other services can use ISettingsService (which uses DbContext via repository)
    /// 
    /// This breaks the circular dependency by using IConfiguration at the bottom.
    /// </remarks>
    public class DatabaseConfigurer : IServiceConfigurer
    {
        /// <inheritdoc/>
        public string ServiceName => "Database";

        /// <inheritdoc/>
        public int Order => 0;  // Run FIRST!

        /// <inheritdoc/>
        public void Configure(IServiceProvider serviceProvider)
        {
            // Get connection string from IConfiguration (NOT ISettingsService!)
            var configuration = serviceProvider.GetService(typeof(IConfiguration)) as IConfiguration;
            if (configuration == null)
            {
                throw new InvalidOperationException(
                    "IConfiguration not available in service provider. Cannot configure database.");
            }

            var connectionString = configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'Default' not found in IConfiguration. " +
                    "Ensure it's defined in appsettings.json under ConnectionStrings section.");
            }

            // TODO: Configure DbContext with connection string
            // var dbContext = serviceProvider.GetRequiredService<ModuleDbContext>();
            // dbContext.Database.SetConnectionString(connectionString);

            // For now, just validate it exists
            Console.WriteLine($"[DatabaseConfigurer] Connection string loaded: {connectionString.Substring(0, Math.Min(30, connectionString.Length))}...");
        }
    }
}
