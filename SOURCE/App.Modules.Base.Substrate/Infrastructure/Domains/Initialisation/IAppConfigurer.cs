namespace App.Modules.Base.Substrate.Infrastructure.Domains.Initialisation
{
    /// <summary>
    /// Class implemented by Configurers
    /// invoked, at startup, by reflection
    /// *after* the builder has created the
    /// app.
    /// <para>
    /// One example of an 
    /// <see cref="IAppConfigurer"/>
    /// is when an instance of it is invoked
    /// to initialise AutoMapper's maps.
    /// </para>
    /// </summary>
    public interface IAppConfigurer
    {
        /// <summary>
        /// Perform the configuration expected of this Configurer.
        /// </summary>
        void Configure();
    }
}
