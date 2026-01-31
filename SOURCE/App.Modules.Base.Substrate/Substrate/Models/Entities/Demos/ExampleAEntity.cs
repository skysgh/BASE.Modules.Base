using App.Modules.Base.Substrate.Factories;
using App.Modules.Base.Substrate.Attributes;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Contracts.Models.Enums;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;

namespace App.Modules.Base.Substrate.Models.Entities.Demos
{
    /// <summary>
    /// Internal Simple (no Children) Demo Entity
    /// <para>
    /// Used to demonstrate correct mapping
    /// from an internal entity to an external
    /// entity, using an ObjectMapping 
    /// implementing
    /// <c></c>
    /// </para>
    /// </summary>
    [ForDemoOnly]
    public class ExampleAEntity :
        IHasGuidId,
        IHasRecordState,
        IHasEnabled,
        IHasTitle,
        IHasDescription
    {
        /// <inheritdoc/>
        public Guid Id { get; set; } = GuidFactory.NewGuid();

        /// <inheritdoc/>
        public RecordPersistenceState RecordState { get; set; }

        /// <inheritdoc/>
        public bool Enabled { get; set; }

        /// <inheritdoc/>
        public string Title { get; set; }

        /// <inheritdoc/>
        public string Description { get; set; }
    }
}
