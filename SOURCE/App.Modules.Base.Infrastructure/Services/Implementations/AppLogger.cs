using App.Modules.Base.Substrate.Contracts.Services;
using Microsoft.Extensions.Logging;

namespace App.Modules.Base.Infrastructure.Services.Implementations
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public AppLogger(ILogger<T> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void LogTrace(string message) => _logger.LogTrace(message);
        public void LogDebug(string message) => _logger.LogDebug(message);
        public void LogInformation(string message) => _logger.LogInformation(message);
        public void LogWarning(string message) => _logger.LogWarning(message);
        public void LogError(string message) => _logger.LogError(message);
        public void LogError(Exception exception, string message) => _logger.LogError(exception, message);
        public void LogCritical(string message) => _logger.LogCritical(message);
        public void LogCritical(Exception exception, string message) => _logger.LogCritical(exception, message);
    }
}