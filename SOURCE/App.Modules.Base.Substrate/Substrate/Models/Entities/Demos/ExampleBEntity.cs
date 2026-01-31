using App.Modules.Base.Substrate.Models.Entities.Base;
using App.Modules.Base.Substrate.Attributes;

namespace App.Modules.Base.Substrate.Models.Entities.Demos
{
    /// <summary>
    /// A slighly more complex example entity,
    /// with a single navigable property
    /// </summary>
    [ForDemoOnly]
    public class ExampleBEntity :
        UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase
    {

        /// <summary>
        /// A Demo Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A Demo Description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// A single Property FK
        /// </summary>
        public Guid SinglePropertyFK { get; set; }
        /// <summary>
        /// A single Property
        /// </summary>
        public ExampleBReferenceTypeEntity SingleProperty { get; set; }

    }
}
