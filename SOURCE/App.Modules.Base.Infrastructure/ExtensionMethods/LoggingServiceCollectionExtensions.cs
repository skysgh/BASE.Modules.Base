using App.Modules.Base.Infrastructure.Services.Implementations;
using App.Modules.Base.Substrate.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Base.Infrastructure.Extensions
{
    public static class LoggingServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLogging(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
            return services;
        }
    }
}