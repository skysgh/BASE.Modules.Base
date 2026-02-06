using App.Modules.Sys.Infrastructure.Services;
using App.Modules.Sys.Shared.Models.Enums;
using System;

namespace App.Modules.Sys.Infrastructure.Domains.Diagnostics
{
    /// <summary>
    /// Non-generic app logger implementation.
    /// Used by base classes that can't use generic logger.
    /// Wraps IDiagnosticsTracingService with a default category.
    /// Registered as Singleton via IHasSingletonLifecycle on IAppLogger interface.
    /// </summary>
    public class AppLoggerNonGeneric : IAppLogger
    {
        private readonly IDiagnosticsTracingService _tracingService;

        /// <summary>
        /// Constructor - uses our own tracing service.
        /// </summary>
        public AppLoggerNonGeneric(IDiagnosticsTracingService tracingService)
        {
            _tracingService = tracingService ?? throw new ArgumentNullException(nameof(tracingService));
        }

        /// <inheritdoc/>
        public void Log(TraceLevel level, string message) =>
            _tracingService.Trace<AppLoggerNonGeneric>(level, message);

        /// <inheritdoc/>
        public void LogTrace(string message) => 
            _tracingService.Trace<AppLoggerNonGeneric>(TraceLevel.Verbose, message);
        
        /// <inheritdoc/>
        public void LogDebug(string message) => 
            _tracingService.Trace<AppLoggerNonGeneric>(TraceLevel.Debug, message);
        
        /// <inheritdoc/>
        public void LogInformation(string message) => 
            _tracingService.Trace<AppLoggerNonGeneric>(TraceLevel.Info, message);
        
        /// <inheritdoc/>
        public void LogWarning(string message) => 
            _tracingService.Trace<AppLoggerNonGeneric>(TraceLevel.Warn, message);
        
        /// <inheritdoc/>
        public void LogError(string message) => 
            _tracingService.Trace<AppLoggerNonGeneric>(TraceLevel.Error, message);
        
        /// <inheritdoc/>
        public void LogError(Exception exception, string message) => 
            _tracingService.Trace<AppLoggerNonGeneric>(TraceLevel.Error, $"{message} | Exception: {exception.Message}");
        
        /// <inheritdoc/>
        public void LogCritical(string message) => 
            _tracingService.Trace<AppLoggerNonGeneric>(TraceLevel.Critical, message);
        
        /// <inheritdoc/>
        public void LogCritical(Exception exception, string message) => 
            _tracingService.Trace<AppLoggerNonGeneric>(TraceLevel.Critical, $"{message} | Exception: {exception.Message}");
    }
}
