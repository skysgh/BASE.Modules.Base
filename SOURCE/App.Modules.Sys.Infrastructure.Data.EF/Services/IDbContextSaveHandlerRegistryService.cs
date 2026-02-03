using App.Modules.Sys.Infrastructure.Data.EF.Interceptors;
using App.Modules.Sys.Infrastructure.Lifecycles;
using App.Modules.Sys.Shared.Services;
using System.Reflection;

namespace App.Modules.Sys.Infrastructure.Data.EF.Services
{
    /// <summary>
    /// Contract for a Service that keeps
    /// a registry of all
    /// <see cref="IDbCommitPreCommitProcessingStrategy"/>
    /// implementations found.
    /// </summary>
    public interface IDbContextSaveHandlerRegistryService : IHasRegistryService
    {
        /// <summary>
        /// Scan the specified assembly for 
        /// <see cref="IDbCommitPreCommitProcessingStrategy"/>
        /// </summary>
        /// <param name="assembly"></param>
        void DiscoverAndRegister(Assembly assembly);

        /// <summary>
        /// Gets all registered <see cref="IDbCommitPreCommitProcessingStrategy"/>
        /// </summary>
        /// <returns></returns>
        IDbCommitPreCommitProcessingStrategy[] GetOrderedHandlers();

        /// <summary>
        /// Gets a count of registered handlers.
        /// </summary>
        int HandlerCount { get; }
    }
}
