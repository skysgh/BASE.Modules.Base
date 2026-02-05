using App.Modules.Sys.Infrastructure.Domains.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

// Extensions are always put in root namespace
// for maximum usability from elsewhere:

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Extension methods to the <see cref="IServiceCollection"/>
    /// </summary>
    public static class LoggingServiceCollectionExtensions
    {
        /// <summary>
        /// Add Application Logging
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationLogging(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
            return services;
        }
    }
}