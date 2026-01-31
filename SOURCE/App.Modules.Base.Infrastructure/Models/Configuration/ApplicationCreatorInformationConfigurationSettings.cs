// using System;
using App.Modules.Base.Infrastructure.Constants;
using App.Modules.Base.Substrate.Attributes;
using App.Modules.Base.Substrate.Contracts.Models;

namespace App.Modules.Base.Substrate.Models.ConfigurationSettings
{
    /// <summary>
    /// An immutable host configuration object 
    /// describing the Creator of the application
    /// (distinct from the Distributor/Resellers) in many commercial cases.
    /// <para>
    /// An Immutable Host Settings configuration object
    /// retrieved from the Host Settings.
    /// </para>
    /// </summary>
    /// <seealso cref="Contracts.Models.IHasName" />
    /// <seealso cref="Contracts.Models.IHasDescription" />
    public class ApplicationCreatorInformationConfigurationSettings : IHostSettingsBasedConfigurationObject, IHasName, IHasDescription
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ApplicationCreatorInformationConfigurationSettings"/> class.
        /// </summary>
        public ApplicationCreatorInformationConfigurationSettings()
        {
        }

        /// <summary>
        /// OData always needs an Id.
        /// <para>
        /// It can be another field, but too much bother
        /// to configure it...
        /// </para>
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;

        /// <summary>
        /// The name of the Application Creator
        /// <para>
        /// (eg: "Acme Something, Inc.")
        /// </para>
        /// </summary>
        [ConfigurationSettingSource(ConfigurationSettingSource.SourceType.AppSetting)]
        [Alias(ConfigurationKeys.AppCoreApplicationCreatorName)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The Description of the System Creator.
        /// </summary>
        [ConfigurationSettingSource(ConfigurationSettingSource.SourceType.AppSetting)]
        [Alias(ConfigurationKeys.AppCoreApplicationCreatorDescription)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Url for the System Creator's general Website.
        /// <para>
        /// It's generally not the product page.
        /// </para>
        /// </summary>
        [ConfigurationSettingSource(ConfigurationSettingSource.SourceType.AppSetting)]
        [Alias(ConfigurationKeys.AppCoreApplicationCreatorSiteUrl)]
        public string SiteUrl { get; set; } = string.Empty;

        /// <summary>
        /// The Url for the System Creator's ContactUs page.
        /// </summary>
        [ConfigurationSettingSource(ConfigurationSettingSource.SourceType.AppSetting)]
        [Alias(ConfigurationKeys.AppCoreApplicationCreatorContactUrl)]
        public string ContactUrl { get; set; } = string.Empty;
    }
}