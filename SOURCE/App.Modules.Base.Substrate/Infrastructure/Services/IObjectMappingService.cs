// using App.Modules.Base.Substrate.Services;

namespace App.Modules.Base.Infrastructure.Services
{

    /// <summary>
    /// Contract for a service to map the properties of
    /// one object to another object.
    /// <para>
    /// Primarily used at the boundaries of the system, 
    /// at the Presentation layer mapping incoming DTOs
    /// to system entities, and back again.</para>
    /// </summary>
    /// <remarks>
    /// IMPORTANT:
    /// Note that the implementation of this service
    /// is actually in <c>App.Base.Application</c>
    /// so that this assembly doesn't have to have a
    /// Reference to Automapper (as it's not used
    /// anywhere else than at the Application API barrier).
    /// </remarks>
    public interface IObjectMappingService : IHasAppInfrastructureService
    {
        /// <summary>
        /// Sets the internal mapper configuration and 
        /// creates a new mapper from it.
        /// </summary>
        /// <param name="configuration"></param>
        public void SetConfiguration<T>(T configuration)
            where T : class;

        /// <summary>
        /// Gets the internal Configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetConfiguration<T>()
            where T : class;

        /// <summary>
        /// Gets the internal Mapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetMapper<T>()
            where T : class;

        /// <summary>
        /// Maps the specified source to the target Type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        TTarget Map<TSource, TTarget>(TSource source) where TSource : class where TTarget : new();

        /// <summary>
        /// Maps the specified source object to the given Target object.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        TTarget Map<TSource, TTarget>(TSource source, TTarget target) where TSource : class where TTarget : class;
    }
}