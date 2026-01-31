using App.Modules.Base.Infrastructure.Constants;
using App.Modules.Base.Substrate.Attributes;

namespace App.Modules.Base.Substrate.Models.ConfigurationSettings
{
    /// <summary>
    /// An immutable host configuration object 
    /// describing the configuration of 
    /// EF CodeFirst.
    /// </summary>
    public class CodeFirstMigrationConfigurationSettings : IHostSettingsBasedConfigurationObject
    {
        /// <summary>
        /// Attach the debugger to Code First
        /// </summary>
        [ConfigurationSettingSource(ConfigurationSettingSource.SourceType.AppSetting)]
        [Alias(ConfigurationKeys.AppCoreCodeFirstAttachDebuggerToPSSeeding)]
        public bool CodeFirstAttachDebugger { get; set; }

        /// <summary>
        /// Include Demo records when seeding.
        /// </summary>
        [ConfigurationSettingSource(ConfigurationSettingSource.SourceType.AppSetting)]
        [Alias(ConfigurationKeys.AppCoreCodeFirstSeedIncludeDemoEntries)]
        public bool CodeFirstSeedDemoStuff { get; set; }

    }
}