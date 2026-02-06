using App.Modules.Sys.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Sys.Infrastructure.Web.Services.Implementations
{
    /// <summary>
    /// .NET 8+ implementation of per-request context storage using HttpContextAccessor.
    /// <para>
    /// This replaces the .NET Framework pattern of HttpContext.Current.Items which is not available in ASP.NET Core.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <b>Migration from .NET Framework:</b><br/>
    /// - Old: <c>HttpContext.Current.Items[key] = value</c><br/>
    /// - New: <c>_httpContextAccessor.HttpContext.Items[key] = value</c>
    /// <para>
    /// <b>Thread Safety:</b><br/>
    /// HttpContextAccessor properly handles async/await scenarios unlike HttpContext.Current
    /// </para>
    /// <para>
    /// <b>Registration Required:</b><br/>
    /// Requires <c>services.AddHttpContextAccessor()</c> in DI registration
    /// </para>
    /// </remarks>
    public class ContextService : IContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor with HttpContextAccessor injection.
        /// </summary>
        /// <param name="httpContextAccessor">Accessor for the current HTTP context</param>
        /// <exception cref="ArgumentNullException">Thrown if httpContextAccessor is null</exception>
        public ContextService(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        /// <summary>
        /// Stores a value in the current request's context.
        /// The value is scoped to the lifetime of the HTTP request.
        /// </summary>
        /// <param name="key">The key to store the value under. Must not be null or empty.</param>
        /// <param name="value">The value to store. Can be any object.</param>
        /// <exception cref="ArgumentException">Thrown if key is null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown if called outside an HTTP request context</exception>
        /// <remarks>
        /// <b>Usage:</b><br/>
        /// Store middleware-level data that needs to be accessed throughout the request pipeline.
        /// Common use cases: User context, correlation IDs, request-scoped caching.
        /// </remarks>
        public void Apply(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            }
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext is not available. Ensure this is called within an HTTP request.");
            }
            context.Items[key] = value;
        }

        /// <summary>
        /// Retrieves a value from the current request's context.
        /// </summary>
        /// <param name="key">The key to retrieve. Must not be null or empty.</param>
        /// <returns>The stored value, or null if not found or no HTTP context available</returns>
        /// <exception cref="ArgumentException">Thrown if key is null or empty</exception>
        /// <remarks>
        /// Returns null if:
        /// - The key doesn't exist in the context
        /// - Called outside an HTTP request
        /// Use the generic <see cref="Get{T}(string)"/> for type-safe retrieval.
        /// </remarks>
        public object? GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty", nameof(key));
            }
            var context = _httpContextAccessor.HttpContext;
            return context?.Items.TryGetValue(key, out var value) == true ? value : null;
        }

        /// <summary>
        /// Retrieves a strongly-typed value from the current request's context.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to</typeparam>
        /// <param name="key">The key to retrieve. Must not be null or empty.</param>
        /// <returns>
        /// The stored value cast to type T, or default(T) if:
        /// - Key not found
        /// - Value cannot be cast to T
        /// - No HTTP context available
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if key is null or empty</exception>
        /// <remarks>
        /// <b>Example:</b>
        /// <code>
        /// // Store
        /// contextService.Set("UserId", Guid.NewGuid());
        /// 
        /// // Retrieve with type safety
        /// var userId = contextService.Get&lt;Guid&gt;("UserId");
        /// </code>
        /// </remarks>
        public T? GetValue<T>(string key)
        {
            object? value = this.GetValue(key);
            return value is T typedValue ? typedValue : default;
        }

        /// <inheritdoc/>
        public TService GetService<TService>() where TService : class
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext is not available. Ensure this is called within an HTTP request.");
            }
            
            return context.RequestServices.GetRequiredService<TService>();
        }
    }
}