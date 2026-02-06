using App.Modules.Sys.Shared.Lifecycles;

namespace App.Modules.Sys.Infrastructure.Services
{
    /// <summary>
    /// Service for managing per-request context storage and service resolution.
    /// Registered as SINGLETON - stateless wrapper around IHttpContextAccessor.
    /// </summary>
    /// <remarks>
    /// Safe as Singleton because:
    /// - No instance state captured
    /// - All operations go through IHttpContextAccessor.HttpContext (per-request)
    /// - Each request gets its own HttpContext automatically
    /// </remarks>
    public interface IContextService : IHasSingletonLifecycle
    {
        /// <summary>
        /// Sets a value in the context storage for the specified key.
        /// </summary>
        /// <param name="key">The key to associate with the value.</param>
        /// <param name="value">The value to store.</param>
        void Apply(string key, object value);

        /// <summary>
        /// Gets a value from the context storage for the specified key.
        /// </summary>
        /// <param name="key">The key whose value to retrieve.</param>
        /// <returns>The value associated with the key, or null if not found.</returns>
        object? GetValue(string key);

        /// <summary>
        /// Gets a value of type <typeparamref name="T"/> from the context storage for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="key">The key whose value to retrieve.</param>
        /// <returns>The value of type <typeparamref name="T"/> associated with the key, or null if not found.</returns>
        T? GetValue<T>(string key);



        /// <summary>
        /// Resolves a service from the current request's service provider.
        /// This allows accessing scoped services from within the current HTTP request context.
        /// </summary>
        /// <typeparam name="TService">The type of service to resolve.</typeparam>
        /// <returns>The resolved service instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if called outside an HTTP request context.</exception>
        /// <remarks>
        /// Use this to bridge from singleton services to scoped services.
        /// The service is resolved from HttpContext.RequestServices.
        /// </remarks>
        TService GetService<TService>() where TService : class;

    }
}