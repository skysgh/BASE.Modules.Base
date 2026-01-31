namespace App.Modules.Base.Infrastructure.Services
{
    /// <summary>
    /// Service for managing per-request context storage.
    /// </summary>
    public interface IContextService: IHasAppInfrastructureService
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
    }
}