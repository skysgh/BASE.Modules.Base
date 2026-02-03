using App.Modules.Sys.Application.Contracts.Services;
using App.Modules.Sys.Infrastructure.Diagnostics;
using App.Modules.Sys.Shared.Services.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.Services.Workspace.Implementations
{
    /// <summary>
    /// Workspace validation using YOUR ICacheObjectRegistryService pattern.
    /// Uses registered WorkspaceIdsCacheObject for cached lookups.
    /// </summary>
    public sealed class CachedWorkspaceValidationService : IWorkspaceValidationService, IWorkspaceCacheManager
    {
        private readonly ICacheObjectRegistryService _cacheRegistry;
        private readonly IAppLogger _logger;
        private const string CACHE_KEY = "Workspace.Ids";

        /// <summary>
        /// Initializes a new instance of the CachedWorkspaceValidationService class.
        /// </summary>
        /// <param name="cacheRegistry">The cache object registry service.</param>
        /// <param name="logger">The application logger.</param>
        public CachedWorkspaceValidationService(
            ICacheObjectRegistryService cacheRegistry,
            IAppLogger<CachedWorkspaceValidationService> logger)
        {
            _cacheRegistry = cacheRegistry;
            _logger = logger;
            _logger.LogInformation("Workspace validation service initialized (using cache registry)");
        }

        /// <inheritdoc/>
        public async Task<bool> WorkspaceExistsAsync(string workspaceId, CancellationToken ct = default)
        {
            var workspaceIds = await _cacheRegistry.GetValueAsync<HashSet<string>>(CACHE_KEY, ct);
            
            if (workspaceIds == null)
            {
                _logger.LogWarning($"Workspace cache not loaded - defaulting to false for '{workspaceId}'");
                return false;
            }

            return workspaceIds.Contains(workspaceId.ToLowerInvariant());
        }

        /// <inheritdoc/>
        public Task<bool> IsWorkspaceActiveAsync(string workspaceId, CancellationToken ct = default)
        {
            return WorkspaceExistsAsync(workspaceId, ct);
        }

        /// <inheritdoc/>
        public async Task RefreshCacheAsync(CancellationToken ct = default)
        {
            _logger.LogInformation("Refreshing workspace cache...");
            await _cacheRegistry.RefreshAsync(CACHE_KEY, ct);
            _logger.LogInformation("Workspace cache refreshed");
        }
    }
}



