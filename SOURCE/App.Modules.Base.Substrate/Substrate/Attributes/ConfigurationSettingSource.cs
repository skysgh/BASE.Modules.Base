namespace App.Modules.Base.Substrate.Attributes
{
    /// <summary>
    /// Attribute to decorate the properties of 
    /// Service Configuration objects, to hint
    /// as to where to retrieve information from.
    /// </summary>
    /// <remarks>
    /// Attribute Constructor, used to define where it's safe to 
    /// look for information.
    /// </remarks>
    /// <param name="source"></param>
    [AttributeUsage(AttributeTargets.All)]
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public class ConfigurationSettingSource(ConfigurationSettingSource.SourceType source) : Attribute
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        /// <summary>
        /// The source to look for the settings value within.
        /// </summary>
        public enum SourceType
        {
            /// <summary>
            /// Use the KeyVault unless already provided via 
            /// AppSettings, whether defined in the codebase
            /// or provided by the build/deployment pipeline.
            /// </summary>
            Any = 7,

            /// <summary>
            /// Directly within the code base.
            /// <para>
            /// Only Appropriate for relative configuration settings
            /// that are neither deployment identifiers (eg: name of AAD Tenancy, or
            /// DNS of System) or worse, client secrets/passwords.
            /// </para>
            /// </summary>
            AppSetting = 1,

            /// <summary>
            /// Not included in the Code base directly (ie, won't be found 
            /// in a public GitHub repository),
            /// the AppSetting is injected into the web.config
            /// when the Resources are Built up.
            /// <para>
            /// Appropriate for VSTS/Build Pipeline injected identifiers
            /// such as AAD Tenancy name, DNS domain names, etc. that 
            /// must not end up in the code repository (all Repositories 
            /// are basically public and have zero security) but are ok
            /// to be known to the development and other team members
            /// via the build pipeline interface which will divulge them.
            /// </para>
            /// <para>
            /// Important: this setting cannot be enforced. It's up to the deployer 
            /// to see the source -- and not ignore it because it's maybe 
            /// faster/more convenient to.
            /// </para>
            /// </summary>
            AppSettingsViaDeploymentPipeline = 2,

            /// <summary>
            /// Reserved for deployment and integration secrets/passwords.
            /// <para>
            /// The keyvault can be locked down to access by only the service
            /// (and build pipeline) ensuring that secrets are not accessible
            /// by team members, since they don't need to know them.
            /// </para>
            /// <para>
            /// Potentially also for vSTS/Build pipeline injected identifiers,
            /// although this probably makes it a little more unwieldy, and 
            /// certainly slower at startup.
            /// </para>
            /// </summary>
            KeyVault = 4,
        }

        /// <summary>
        /// TODO: Improve documentation
        /// </summary>
        public SourceType Source { get; private set; } = source;
    }
}