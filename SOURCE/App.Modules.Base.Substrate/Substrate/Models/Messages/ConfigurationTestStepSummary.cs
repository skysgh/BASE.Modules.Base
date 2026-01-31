using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Factories;
using App.Modules.Base.Substrate.Models.Messages.Enums;

namespace App.Modules.Base.Substrate.Models.Messages
{
    /// <summary>
    /// A model to hold the results of a 
    /// self assessement of integration, etc.
    /// Again, this is to provide to support
    /// stakeholders a way of ensuring the system
    /// is up and running.
    /// </summary>
    public class ConfigurationTestStepSummary : IHasGuidId
    {
        private string title = string.Empty;
        private string description = string.Empty;

        /// <summary>
        /// Note than although this model is not persisted in 
        /// a datastore, an Id is still required, as it is expressed
        /// via OData.
        /// </summary>
        public ConfigurationTestStepSummary()
        {
            Id = GuidFactory.NewGuid();
        }
        /// <summary>
        /// TODO: Describe
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// TODO: Describe
        /// </summary>
        public ConfigurationStepType Type { get; set; }

        /// <summary>
        /// TODO: Describe
        /// </summary>
        public ConfigurationStepStatus Status { get; set; }

        /// <summary>
        /// TODO: Describe
        /// </summary>
        public DateTimeOffset DateTime { get; set; }
        /// <summary>
        /// TODO: Describe
        /// </summary>
        public string Title { get => title; set => title = value ?? string.Empty; }
        /// <summary>
        /// TODO: Describe
        /// </summary>
        public string Description { get => description; set => description = value ?? string.Empty; }
    }
}
