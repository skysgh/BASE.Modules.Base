// using System;
using App.Modules.Base.Substrate.Contracts.Services;

namespace App.Modules.Base.Infrastructure.Services
{
    /// <summary>
    /// Contract for an Infrastructure Service to 
    ///     to convert types to alternate types.
    ///     <para>
    ///         Refer to implementations of
    ///         <c>IHostSettingsService</c>
    /// that use it to convert settings for persistence
    /// in datastores, and back again. It's used in other 
    /// places too (front end conversion).
    ///     </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Implements <see cref="IHasAppInfrastructureService"/>,
    /// which is a Module specific specialisation of
    /// <see cref="IHasService"/> in order for the Dependency 
    /// Injection service to easily find it at startup.
    /// </para>
    /// </remarks>
    public interface IConversionService : IHasAppInfrastructureService
    {
        /// <summary>
        ///     Convert one type to another, falling back to the
        ///     provided default Type if the value is null.
        /// </summary>
        /// <typeparam name="T">The target Type</typeparam>
        /// <param name="source">The source object</param>
        /// <param name="defaultValue">The default target value</param>
        /// <returns></returns>
#pragma warning disable CS8601 // Possible null reference assignment.
        T ConvertTo<T>(object source, T defaultValue = default);
#pragma warning restore CS8601 // Possible null reference assignment.

        /// <summary>
        ///     Get the default value of the provided Type.
        ///     If the Type is a Value object it will be its default value,
        ///     If it is an object, it will be null.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetDefaultValue(Type type);
    }
}