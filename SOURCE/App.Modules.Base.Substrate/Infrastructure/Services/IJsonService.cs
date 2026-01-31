using App.Modules.Base.Substrate.Contracts.Services;

namespace App.Modules.Base.Infrastructure.Services
{
    /// <summary>
    /// Contract for a service to 
    /// provide common 
    /// JSON management operations.
    /// <para>
    /// The purpose of the service is to isolate the rest of the s
    /// system from having to have a Reference from an external 
    /// library, which leads to subtle errors when assemblies refer
    /// to different versions.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Implements <see cref="IHasAppInfrastructureService"/>,
    /// which is a Module specific specialisation of
    /// <see cref="IHasService"/> in order for the Dependency 
    /// Injection service to easily find it at startup.
    /// </para>
    /// </remarks>
    public interface IJsonService : IHasAppInfrastructureService
    {

        /// <summary>
        /// Parses the given string based serialisation
        /// back into an Object of Type 
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of the expected object.</typeparam>
        /// <param name="input">The serialised input.</param>
        /// <returns></returns>
        T Parse<T>(string input);

        /// <summary>
        /// Serializes the specified object into a JSON string.
        /// </summary>
        /// <typeparam name="T">Type of Object to serialise</typeparam>
        /// <param name="model">The object to serialise.</param>
        /// <returns>string result</returns>
        string Serialize<T>(T model);
    }
}
