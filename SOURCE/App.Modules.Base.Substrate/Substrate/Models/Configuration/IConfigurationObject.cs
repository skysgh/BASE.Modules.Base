namespace App.Modules.Base.Substrate.Models.Configuration
{
    /// <summary>
    /// A (generally) singleton configuration object used 
    /// to configure the behaviour
    /// of a <c>IHasService</c> implementation.
    /// <para>
    /// Most often its developed by an implementation of a
    /// <c>IServiceConfigurer</c> of some kind.
    /// </para>
    /// <para>
    /// Can be specialised as a 
    /// <c>IHostSettingsBasedConfigurationObject</c>
    /// <c>IKeyVaultBasedConfigurationObject</c>
    /// </para>
    /// to indicate where to look for values in order to hydrate
    /// the <see cref="IConfigurationObject"/> implementation.
    /// </summary>
    public interface IConfigurationObject { }

}
