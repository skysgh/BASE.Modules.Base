using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Shared.Services.Caching
{
    /// <summary>
    /// Base class for self-contained cache objects.
    /// Inherit from this to create cache objects that are automatically discovered at startup.
    /// Thread-safe with circuit breaker pattern for resilience.
    /// </summary>
    /// <typeparam name="T">Type of the cached value</typeparam>
    public abstract class CacheObjectBase<T> : ICacheObject
    {
        private T? _cachedValue;
        private Task<T>? _refreshTask;
        private readonly SemaphoreSlim _refreshLock = new(1, 1);
        
        // Circuit breaker state
        private DateTime? _lastFailure;
        private int _consecutiveFailures;
        private const int MaxFailuresBeforeBackoff = 3;
        
        private bool _disposed;

        /// <inheritdoc/>
        public abstract string Key { get; }

        /// <inheritdoc/>
        public abstract TimeSpan? Duration { get; }

        /// <inheritdoc/>
        public Type ValueType => typeof(T);

        /// <inheritdoc/>
        public DateTime LastRefreshed { get; set; } = DateTime.MinValue;

        /// <inheritdoc/>
        public bool IsExpired
        {
            get
            {
                if (!Duration.HasValue)
                    return false; // Never expires

                if (LastRefreshed == DateTime.MinValue)
                    return true; // Never loaded

                return DateTime.UtcNow > LastRefreshed.Add(Duration.Value);
            }
        }

        /// <summary>
        /// Function to retrieve/compute the cached value.
        /// Called when cache is empty or expired.
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The value to cache</returns>
        protected abstract Task<T> GetValueAsync(CancellationToken ct = default);

        /// <inheritdoc/>
        public async Task RefreshAsync(CancellationToken ct = default)
        {
            ThrowIfDisposed();
            
            // Quick check before acquiring lock
            if (!IsExpired && _consecutiveFailures == 0)
                return;

            // Check circuit breaker - exponential backoff after repeated failures
            if (_consecutiveFailures >= MaxFailuresBeforeBackoff && _lastFailure.HasValue)
            {
                var backoffDuration = TimeSpan.FromSeconds(Math.Pow(2, _consecutiveFailures - MaxFailuresBeforeBackoff));
                if (DateTime.UtcNow < _lastFailure.Value.Add(backoffDuration))
                {
                    // Still in backoff period - don't retry yet
                    return;
                }
            }

            await _refreshLock.WaitAsync(ct);
            try
            {
                // Double-check inside lock
                if (!IsExpired && _consecutiveFailures == 0)
                    return;

                try
                {
                    var newValue = await GetValueAsync(ct);
                    _cachedValue = newValue;
                    LastRefreshed = DateTime.UtcNow;
                    
                    // Reset circuit breaker on success
                    _consecutiveFailures = 0;
                    _lastFailure = null;
                }
                catch
                {
                    // Circuit breaker - track failures
                    _consecutiveFailures++;
                    _lastFailure = DateTime.UtcNow;
                    throw; // Re-throw to caller
                }
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        /// <summary>
        /// Get the cached value (type-safe).
        /// If expired, triggers a refresh first.
        /// Implements anti-thundering-herd pattern - only one refresh task per expiration.
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The cached value</returns>
        internal async Task<T?> GetAsync(CancellationToken ct = default)
        {
            ThrowIfDisposed();
            
            if (!IsExpired)
            {
                return _cachedValue;
            }

            // Anti-thundering-herd: reuse in-progress refresh task
            var refreshTask = _refreshTask;
            if (refreshTask == null || refreshTask.IsCompleted)
            {
                // Create new refresh task atomically
                var newTask = Task.Run(async () =>
                {
                    await RefreshAsync(ct);
                    return _cachedValue!;
                }, ct);

                refreshTask = Interlocked.CompareExchange(ref _refreshTask, newTask, refreshTask) ?? newTask;
            }

            await refreshTask;
            return _cachedValue;
        }

        /// <inheritdoc/>
        public object? GetValue()
        {
            ThrowIfDisposed();
            return _cachedValue;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose pattern implementation
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _refreshLock?.Dispose();
            }

            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_disposed, GetType().Name);
        }
    }
}
