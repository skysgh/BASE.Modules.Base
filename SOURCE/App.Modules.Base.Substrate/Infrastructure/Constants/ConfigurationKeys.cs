namespace App.Modules.Base.Infrastructure.Constants
{
    /// <summary>
    /// A helper class to help build up
    /// the keys of configuration settings
    /// to diminish spelling/mistakes.
    /// </summary>
    public static class ConfigurationKeys
    {
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string SystemKeyPrefix = "Service-";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string SystemIntegrationKeyPrefix = SystemKeyPrefix + "Integration-";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string SystemAzureIntegrationKeyPrefix = SystemIntegrationKeyPrefix + "Azure-";

        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string SystemModuleApiScopeServiceUrl = "connecting-to-oauthservice-app-id-url";

        // -----
        // Demo mode

        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreDemoMode = SystemKeyPrefix + "DemoMode";
        // -----
        // App Name & Description

        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreApplicationName = SystemKeyPrefix + "Application-Info-Name";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreApplicationDescription = SystemKeyPrefix + "Application-Info-Description";


        // -----
        // App Information

        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreApplicationProviderName = SystemKeyPrefix + "Application-Provider-Name";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreApplicationProviderDescription = SystemKeyPrefix + "Application-Provider-Description";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreApplicationProviderSiteUrl = SystemKeyPrefix + "Application-Provider-SiteUrl";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreApplicationProviderContactUrl = SystemKeyPrefix + "Application-Provider-ContactUrl";

        // -----
        // Creator Information

        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreApplicationCreatorName = SystemKeyPrefix + "Application-Creator-Name";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreApplicationCreatorDescription = SystemKeyPrefix + "Application-Creator-Description";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreApplicationCreatorSiteUrl = SystemKeyPrefix + "Application-Creator-SiteUrl";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreApplicationCreatorContactUrl = SystemKeyPrefix + "Application-Creator-ContactUrl";


        // -----
        // Media Management
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreMediaHashType = SystemKeyPrefix + "Media-HashType";
        // -----
        // Azure AAD (if not moved on to using MSI, which is far safer):

        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIDAAADClientId = SystemIntegrationKeyPrefix + "Oidc-AAD:ClientId";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIDAAADClientSecret = SystemIntegrationKeyPrefix + "Oidc-AAD:ClientSecret";

        // -----
        // Environment / Azure (pushed in by deployment engine):

        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreEnvironmentSubscriptionId = SystemAzureIntegrationKeyPrefix + "SubscriptionId";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreEnvironmentTenantId = SystemAzureIntegrationKeyPrefix + "TenantId";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreEnvironmentResourceGroupName = SystemAzureIntegrationKeyPrefix + "resourcegroup-name";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreEnvironmentResourceGroupLocation = SystemAzureIntegrationKeyPrefix + "resourcegroup-location";


        // -----
        // Integration / Azure:
        // WHen reosurces are not named (eg: DocumentDbResourceName), falls back to this setting:
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCommonResourceName = SystemAzureIntegrationKeyPrefix + "Arm-Resources-DefaultName";

        // -----
        // Integration / Azure / AppInsights 

        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureApplicationInsightsInstrumentationKey =
            SystemAzureIntegrationKeyPrefix + "ApplicationInsights-InstrumentationKey";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureApplicationInsightsInstrumentationKeyEnabled =
            SystemAzureIntegrationKeyPrefix + "ApplicationInsights-Enabled";
        // -----
        // Integration / Azure / Key Vault
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureKeyVaultStoreResourceName = SystemAzureIntegrationKeyPrefix + "KeyVaultStores-Default-ResourceName";
        // -----
        // Integration / Azure / Storage Account / Common
        // -----
        // Integration / Azure / Storage Account / Diagnostics
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureStorageAccountDiagnosticsResourceName = SystemAzureIntegrationKeyPrefix + "StorageAccount-Diagnostics-ResourceName";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureStorageAccountDiagnosticsResourceNameSuffix = SystemAzureIntegrationKeyPrefix + "StorageAccount-Diagnostics-ResourceName-Suffix";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureStorageAccountDiagnosticsKey = SystemAzureIntegrationKeyPrefix + "StorageAccount-Diagnostics-Key";
        // -----
        // Integration / Azure / Storage Account / Default
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureStorageAccountDefaultResourceName = SystemAzureIntegrationKeyPrefix + "StorageAccount-Default-ResourceName";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureStorageAccountDefaultResourceNameSuffix = SystemAzureIntegrationKeyPrefix + "StorageAccount-Default-ResourceName-Suffix";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureStorageAccountDefaultKey = SystemAzureIntegrationKeyPrefix + "StorageAccount-Default-Key";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureStorageAccountConnectionString = SystemAzureIntegrationKeyPrefix + "StorageAccount-Default-ConnectionString";
        // -----
        // Integration / Azure / Microsoft / Redis / Cache / Default (key has Cache before Redis):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureRedisCacheResourceName = SystemAzureIntegrationKeyPrefix + "Cache-Redis-Default-ResourceName";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureRedisCacheDefaultAuthorizationKey = SystemAzureIntegrationKeyPrefix + "Cache-Redis-Default-Key";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureRedisEnabled = SystemAzureIntegrationKeyPrefix + "Cache-Redis-Enabled";

        // -----
        // Integration / Azure / Microsoft / DocumentDb / Default (do not name as Default):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureDocumentDbResourceName = SystemAzureIntegrationKeyPrefix + "DocumentDb-Default-ResourceName";
        //public const string AppCoreIntegrationAzureDocumentDbResourceEndpointUrl = SystemKeyPrefix + "Integration:Azure:DocumentDb:EndpointUrl";
        // The following should not be use if we are using MSI:
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureDocumentDbAuthorizationKey = SystemAzureIntegrationKeyPrefix + "DocumentDb-Default-Key";
        // Integration / Azure / Search / Default (key has plural name):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureSearchDefaultResourceName = SystemAzureIntegrationKeyPrefix + "SearchServices-Default-ResourceName";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureSearchDefaultAuthorizationKey = SystemAzureIntegrationKeyPrefix + "SearchServices-Default-Key";
        // -----
        // Integration / Azure / Maps / Default (note that AzureMaps is refered to as singluar map and Services is plural to stay in line with everything else...):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureMapsDefaultResourceName = SystemAzureIntegrationKeyPrefix + "MapServices-Default-ResourceName";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureMapsDefaultAuthorizationKey = SystemAzureIntegrationKeyPrefix + "MapServices-Default-Key";
        // -----
        // Integration / Azure / Maps / CognitiveServices / ContentModerator / Default  (key has plural name):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCognitiveServicesContentModeratorDefaultResourceName = SystemAzureIntegrationKeyPrefix + "CognitiveServices-ContentModerator-Default-ResourceName";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCognitiveServicesContentModeratorDefaultAuthorizationKey = SystemAzureIntegrationKeyPrefix + "CognitiveServices-ContentModerator-Default-Key";
        // -----
        // Integration / Azure / Maps / CognitiveServices / ContentModerator / Default (key has plural name):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCognitiveServicesLanguageUnderstandingDefaultResourceName = SystemAzureIntegrationKeyPrefix + "CognitiveServices-LanguageUnderstanding-Default-ResourceName";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCognitiveServicesLanguageUnderstandingDefaultAuthorizationKey = SystemAzureIntegrationKeyPrefix + "CognitiveServices-LanguageUnderstanding-Default-Key";
        // -----
        // Integration / Azure / Maps / CognitiveServices / ComputerVision / Default (key has plural name):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCognitiveServicesComputerVisionDefaultResourceName = SystemAzureIntegrationKeyPrefix + "CognitiveServices-ComputerVision-Default-ResourceName";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCognitiveServicesComputerVisionDefaultAuthorizationKey = SystemAzureIntegrationKeyPrefix + "CognitiveServices-ComputerVision-Default-Key";
        // -----
        // Integration / Azure / Maps / CognitiveServices / CustomVision / Default (key has plural name):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCognitiveServicesCustomVisionTrainingDefaultResourceName = SystemAzureIntegrationKeyPrefix + "CognitiveServices-CustomVision-Training-Default-ResourceName";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCognitiveServicesCustomVisionTrainingDefaultAuthorizationKey = SystemAzureIntegrationKeyPrefix + "CognitiveServices-ComputerVision-Training-Default-Key";

        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCognitiveServicesCustomVisionPredictionDefaultResourceName = SystemAzureIntegrationKeyPrefix + "CognitiveServices-CustomVision-Prediction-Default-ResourceName";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationAzureCognitiveServicesCustomVisionPredictionDefaultAuthorizationKey = SystemAzureIntegrationKeyPrefix + "CognitiveServices-ComputerVision-Prediction-Default-Key";
        // -----








        // -----
        // CodeFirst (dubiously SystemAzureIntegrationKeyPrefix rather than SystemIntegrationKeyPrefix, but thinking of it as a production variable...sortof...?):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreCodeFirstAttachDebuggerToPSSeeding = SystemAzureIntegrationKeyPrefix + "SqlDatabase-CodeFirst-AttachDebuggerToPSSeeding";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreCodeFirstSeedIncludeDemoEntries = SystemAzureIntegrationKeyPrefix + "SqlDatabase-CodeFirst-SeedIncludeDemoEntries";

        // -----
        // SMTP (note that it does not prefix with Azure, as it is hosted somewhere else) / (Do not name as Default):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationSmtpServiceEnabled = SystemIntegrationKeyPrefix + "SmtpService-Enabled";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationSmtpServiceBaseUri = SystemIntegrationKeyPrefix + "SmtpService-Uri";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationSmtpServicePort = SystemIntegrationKeyPrefix + "SmtpService-Port";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationSmtpServiceFrom = SystemIntegrationKeyPrefix + "SmtpService-From";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationSmtpServiceClientId = SystemIntegrationKeyPrefix + "SmtpService-ClientId";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationSmtpServiceClientSecret = SystemIntegrationKeyPrefix + "SmtpService-ClientSecret";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationSmtpServiceClientMiscConfig = SystemIntegrationKeyPrefix + "SmtpService-ClientMisc";


        // -----
        // Scanii (note that it does not prefix with Azure, as it is hosted somewhere else) / (Do not name as Default):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationMalwareDetectionEnabled = SystemIntegrationKeyPrefix + "MalwareDetectionService-Enabled";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationMalwareDetectionBaseUri = SystemIntegrationKeyPrefix + "MalwareDetectionService-Uri";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationMalwareDetectionClientId = SystemIntegrationKeyPrefix + "MalwareDetectionService-ClientId";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationMalwareDetectionClientSecret = SystemIntegrationKeyPrefix + "MalwareDetectionService-ClientSecret";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationMalwareDetectionClientMiscConfig = SystemIntegrationKeyPrefix + "MalwareDetectionService-ClientMisc";

        // -----
        // Integration / [SituationSpecific] / GeoLocationService  / (Do not name as Default):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationGeoIPServiceEnabled = SystemIntegrationKeyPrefix + $"ipgeoconversionservice-{DefaultConstants.Enabled}";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationGeoIPServiceBaseUri = SystemIntegrationKeyPrefix + $"ipgeoconversionservice-{DefaultConstants.Uri}";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationGeoIPServiceClientId = SystemIntegrationKeyPrefix + $"ipgeoconversionservice-{DefaultConstants.Client}{DefaultConstants.Id}";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationGeoIPServiceClientSecret = SystemIntegrationKeyPrefix + $"ipgeoconversionservice-{DefaultConstants.Client}Secret";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationGeoIPServiceClientMiscConfig = SystemIntegrationKeyPrefix + $"ipgeoconversionservice-{DefaultConstants.Client}Misc";
        // -----


        // -----
        // Some Misc Service (note that it does not prefix with Azure, as it is hosted somewhere else) / (Do not name as Default):
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationService01Name = SystemIntegrationKeyPrefix + $"{DefaultConstants.Sevice}01-{DefaultConstants.Name}";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationService01Enabled = SystemIntegrationKeyPrefix + $"{DefaultConstants.Sevice}01-{DefaultConstants.Enabled}";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationService01BaseUri = SystemIntegrationKeyPrefix + $"{DefaultConstants.Sevice}01-{DefaultConstants.Uri}";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationService01ClientId = SystemIntegrationKeyPrefix + $"{DefaultConstants.Sevice}01-{DefaultConstants.Client}-{DefaultConstants.Id}";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationService01ClientSecret = SystemIntegrationKeyPrefix + $"{DefaultConstants.Sevice}01-{DefaultConstants.Client}-Secret";
        /// <summary>
        /// TODO: Describe better
        /// </summary>
        public const string AppCoreIntegrationService01MiscConfig = SystemIntegrationKeyPrefix + $"{DefaultConstants.Sevice}01-{DefaultConstants.Client}-MiscConfig";

        // // -----
        // // Some Misc Service (note that it does not prefix with Azure, as it is hosted somewhere else) / (Do not name as Default):
        // public const string AppCoreIntegrationService02Name = SystemIntegrationKeyPrefix + "Service02-Name";
        // public const string AppCoreIntegrationService02Enabled = SystemIntegrationKeyPrefix + "Service02-Enabled";
        // public const string AppCoreIntegrationService02BaseUri = SystemIntegrationKeyPrefix + "Service02-Uri";
        // public const string AppCoreIntegrationService02ClientId = SystemIntegrationKeyPrefix + "Service02-Client-Id";
        // public const string AppCoreIntegrationService02ClientSecret = SystemIntegrationKeyPrefix + "Service02-Client-Secret";
        // public const string AppCoreIntegrationService02MiscConfig = SystemIntegrationKeyPrefix + "Service02-Client-MiscConfig";

        // -----
        // // Some Misc Service (note that it does not prefix with Azure, as it is hosted somewhere else) / (Do not name as Default):
        // public const string AppCoreIntegrationService03Name = SystemIntegrationKeyPrefix + "Service03-Name";
        // public const string AppCoreIntegrationService03Enabled = SystemIntegrationKeyPrefix + "Service03-Enabled";
        // public const string AppCoreIntegrationService03BaseUri = SystemIntegrationKeyPrefix + "Service03-Uri";
        // public const string AppCoreIntegrationService03ClientId = SystemIntegrationKeyPrefix + "Service03-Client-Id";
        // public const string AppCoreIntegrationService03ClientSecret = SystemIntegrationKeyPrefix + "Service03-Client-Secret";
        // public const string AppCoreIntegrationService03MiscConfig = SystemIntegrationKeyPrefix + "Service03-Client-MiscConfig";
    }
}
