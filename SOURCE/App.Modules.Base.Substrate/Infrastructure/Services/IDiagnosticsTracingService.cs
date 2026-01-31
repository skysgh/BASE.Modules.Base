using App.Modules.Base.Substrate.Contracts.Models.Enums;

// using App.Modules.Base.Substrate.Services;

namespace App.Modules.Base.Infrastructure.Services
{
    /// <summary>
    /// Contract for an Infrastructure Service to 
    /// provide diagnostic tracing services.
    /// <para>
    /// In Azure these trace messages would probably be recored to 
    /// Blob storage.
    /// </para>
    /// </summary>
    public interface IDiagnosticsTracingService : IHasAppInfrastructureService
    {
        /// <summary>
        /// Provides in-system Service for diagnostics
        /// purposes
        /// </summary>
        /// <param name="traceLevel"></param>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        void Trace<TContext>(TraceLevel traceLevel, string message, params object[] arguments);

        /// <summary>
        /// Provides in-system Service for diagnostics
        /// purposes
        /// </summary>
        /// <param name="contextIdentifier"></param>
        /// <param name="traceLevel"></param>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        void Trace(string contextIdentifier, TraceLevel traceLevel, string message, params object[] arguments);
    }
}