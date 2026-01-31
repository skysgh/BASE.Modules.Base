using App.Modules.Base.Infrastructure.Diagnostics;
using App.Modules.Base.Infrastructure.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace App
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