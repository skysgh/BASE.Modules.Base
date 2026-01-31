using App.Modules.Base.Infrastructure.Constants;
using App.Modules.Base.Substrate.Attributes;

namespace App.Modules.Base.Substrate.Models.ConfigurationSettings
{
    /// <summary>
    /// Configuration Settings for configuring the GeoIPService
    /// </summary>
    public class GeoIPServiceConfigurationSettings : IServiceConfiguration
    {
        /// <summary>
        /// The Service Credentials key
        /// <para>
        /// Make sure this kind of secrets are not gotten from AppSettings.
        /// </para>
        /// </summary>
        [ConfigurationSettingSource(ConfigurationSettingSource.SourceType.AppSettingsViaDeploymentPipeline)]
        [Alias(ConfigurationKeys.AppCoreIntegrationGeoIPServiceClientId)]
        public string? ClientId
        {
            get; set;
        }

        /// <summary>
        /// The Service Credentials Secret
        /// <para>
        /// Make sure this kind of secrets are not gotten from AppSettings.
        /// </para>
        /// </summary>
        [ConfigurationSettingSource(ConfigurationSettingSource.SourceType.KeyVault)]
        [Alias(ConfigurationKeys.AppCoreIntegrationGeoIPServiceClientSecret)]
        public string? Secret
        {
            get; set;
        }


        /// <summary>
        /// Url of the remote service.
        /// </summary>
        [ConfigurationSettingSource(ConfigurationSettingSource.SourceType.AppSettingsViaDeploymentPipeline)]
        [Alias(ConfigurationKeys.AppCoreIntegrationGeoIPServiceBaseUri)]
        public string? BaseUri
        {
            get; set;
        }


        /// <summary>
        /// Misc configuration that can be used to configure the service.
        /// </summary>
        [ConfigurationSettingSource(ConfigurationSettingSource.SourceType.AppSettingsViaDeploymentPipeline)]
        [Alias(ConfigurationKeys.AppCoreIntegrationGeoIPServiceClientMiscConfig)]
        public string? MiscConfig
        {
            get; set;
        }
    }
}
