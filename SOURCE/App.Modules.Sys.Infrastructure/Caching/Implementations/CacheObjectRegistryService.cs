using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using App.Modules.Sys.Shared.Services.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Modules.Sys.Infrastructure.Caching.Implementations
{
    /// <summary>
    /// Implementation of cache object registry service.
    /// Discovers and manages self-contained cache objects.
    /// </summary>
    public class CacheObjectRegistryService : ICacheObjectRegistryService, IDisposable
    {
        private readonly ConcurrentDictionary<string, ICacheObject> _cacheObjects = new();
        private readonly ILogger<CacheObjectRegistryService>? _logger;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of CacheObjectRegistryService.
        /// </summary>
        /// <param name="logger">Optional logger for diagnostics</param>
        public CacheObjectRegistryService(ILogger<CacheObjectRegistryService>? logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// Discover all ICacheObject implementations in loaded assemblies.
        /// Call this during application startup (DoAfterBuild phase).
        /// Uses IServiceProvider for dependency injection support.
        /// </summary>
        /// <param name="serviceProvider">Service provider for DI resolution</param>
        public void DiscoverAndRegisterAll(IServiceProvider serviceProvider)
        {
            ThrowIfDisposed();
            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var cacheObjectType = typeof(ICacheObject);

            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes()
                        .Where(t => cacheObjectType.IsAssignableFrom(t) 
                                 && !t.IsInterface 
                                 && !t.IsAbstract);

                    foreach (var type in types)
                    {
                        try
                        {
                            // Use ActivatorUtilities for DI support
                            var instance = ActivatorUtilities.CreateInstance(serviceProvider, type) as ICacheObject;
                            
                            if (instance != null)
                            {
                                Register(instance);
                                _logger?.LogInformation(
                                    "Registered cache object: {Key} ({Type})", 
                                    instance.Key, 
                                    type.Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, 
                                "Failed to instantiate cache object type: {Type}", 
                                type.FullName);
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    _logger?.LogWarning(ex, 
                        "Failed to load types from assembly: {Assembly}", 
                        assembly.FullName);
                }
            }

            _logger?.LogInformation(
                "Cache object discovery complete. Registered {Count} objects.", 
                _cacheObjects.Count);
        }

        /// <inheritdoc/>
        public void Register(ICacheObject cacheObject)
        {
            ThrowIfDisposed();
            
            if (string.IsNullOrWhiteSpace(cacheObject.Key))
            {
                throw new ArgumentException("Cache object key cannot be null or empty.", nameof(cacheObject));
            }

            if (!_cacheObjects.TryAdd(cacheObject.Key, cacheObject))
            {
                throw new InvalidOperationException(
                    $"Cache object with key '{cacheObject.Key}' is already registered.");
            }
        }

        /// <inheritdoc/>
        public async Task<T?> GetValueAsync<T>(string key, CancellationToken ct = default)
        {
            ThrowIfDisposed();
            
            if (!_cacheObjects.TryGetValue(key, out var cacheObject))
            {
                _logger?.LogWarning("Cache object not found for key: {Key}", key);
                return default;
            }

            // Check if expired and refresh if needed
            if (cacheObject.IsExpired)
            {
                _logger?.LogDebug("Cache object expired, refreshing: {Key}", key);
                
                try
                {
                    await cacheObject.RefreshAsync(ct);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Failed to refresh cache object: {Key}", key);
                    // Return stale value if available, otherwise throw
                    var staleValue = cacheObject.GetValue();
                    if (staleValue != null)
                    {
                        _logger?.LogWarning("Returning stale value for: {Key}", key);
                        return staleValue is T typedStale ? typedStale : default;
                    }
                    throw;
                }
            }

            var result = cacheObject.GetValue();
            return result is T typedResult ? typedResult : default;
        }

        /// <inheritdoc/>
        public async Task RefreshAsync(string key, CancellationToken ct = default)
        {
            ThrowIfDisposed();
            
            if (_cacheObjects.TryGetValue(key, out var cacheObject))
            {
                _logger?.LogInformation("Manually refreshing cache object: {Key}", key);
                await cacheObject.RefreshAsync(ct);
            }
            else
            {
                _logger?.LogWarning("Cannot refresh - cache object not found: {Key}", key);
            }
        }

        /// <inheritdoc/>
        public async Task RefreshExpiredAsync(CancellationToken ct = default)
        {
            ThrowIfDisposed();
            
            var expiredKeys = _cacheObjects
                .Where(kvp => kvp.Value.IsExpired)
                .Select(kvp => kvp.Key)
                .ToList();

            _logger?.LogInformation("Refreshing {Count} expired cache objects", expiredKeys.Count);

            var tasks = expiredKeys.Select(key => RefreshAsync(key, ct));
            await Task.WhenAll(tasks);
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetKeys() => _cacheObjects.Keys;

        /// <inheritdoc/>
        public bool Contains(string key) => _cacheObjects.ContainsKey(key);

        /// <inheritdoc/>
        public void Remove(string key)
        {
            ThrowIfDisposed();
            
            if (_cacheObjects.TryRemove(key, out var removed))
            {
                removed?.Dispose();
                _logger?.LogInformation("Removed cache object: {Key}", key);
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            ThrowIfDisposed();
            
            foreach (var cacheObject in _cacheObjects.Values)
            {
                cacheObject?.Dispose();
            }
            
            _cacheObjects.Clear();
            _logger?.LogInformation("Cleared all cache objects from registry");
        }

        /// <summary>
        /// Dispose all cache objects
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            foreach (var cacheObject in _cacheObjects.Values)
            {
                try
                {
                    cacheObject?.Dispose();
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error disposing cache object: {Key}", cacheObject?.Key);
                }
            }

            _cacheObjects.Clear();
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(CacheObjectRegistryService));
            }
        }
    }
}

