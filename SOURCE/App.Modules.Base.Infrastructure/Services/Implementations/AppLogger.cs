using App.Modules.Base.Infrastructure.Diagnostics;
using Microsoft.Extensions.Logging;

namespace App.Modules.Base.Infrastructure.Services.Implementations
{
    /// <inheritdoc/>
    /// <remarks>
    /// This is a thin abstraction wrapper over Microsoft.Extensions.Logging.ILogger.
    /// While LoggerMessage source generators (CA1848) and constant message templates (CA2254) offer better performance, 
    /// this implementation prioritizes simplicity and maintainability for a general-purpose abstraction layer.
    /// For performance-critical paths, consider using ILogger directly with LoggerMessage delegates.
    /// </remarks>
#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA2254 // Template should be a static expression
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppLogger{T}"/> class.
        /// </summary>
        /// <param name="logger">The underlying Microsoft.Extensions.Logging logger.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is null.</exception>
        public AppLogger(ILogger<T> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public void LogTrace(string message) => _logger.LogTrace(message);
        /// <inheritdoc/>
        public void LogDebug(string message) => _logger.LogDebug(message);
        /// <inheritdoc/>
        public void LogInformation(string message) => _logger.LogInformation(message);
        /// <inheritdoc/>
        public void LogWarning(string message) => _logger.LogWarning(message);
        /// <inheritdoc/>
        public void LogError(string message) => _logger.LogError(message);
        /// <inheritdoc/>
        public void LogError(Exception exception, string message) => _logger.LogError(exception, message);
        /// <inheritdoc/>
        public void LogCritical(string message) => _logger.LogCritical(message);
        /// <inheritdoc/>
        public void LogCritical(Exception exception, string message) => _logger.LogCritical(exception, message);
    }
#pragma warning restore CA2254
#pragma warning restore CA1848
}