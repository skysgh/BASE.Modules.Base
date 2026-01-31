using App.Modules.Base.Substrate.Contracts.Models.Enums;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages
{
    /// <summary>
    /// System entity (not exposed to the system's exterior) for
    /// a System Exception.
    /// <para>
    /// These records, scrubbed of Personal Information (PI)
    /// are persisted indefinitely. 
    /// </para>
    /// <para>
    /// They are persisted for easier queryability for remaining
    /// known errors, indicative of the operational quality.
    /// </para>
    /// <para>
    /// Certainly far longer than
    /// DiagnosticTraceRecords which are only kept for a configurable
    /// amount of time before being rolled over.
    /// </para>
    /// 
    /// </summary>
    public class ExceptionRecord :
        // Entity has Id and other attributes due to inheriting from:
        UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase
    {
        /// <summary>
        /// The trace level of the record.
        /// </summary>
        public TraceLevel Level { get; set; }
        /// <summary>
        /// The display Title of the Event.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The display Description of the Event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A PI cleansed copy of full stack at the time of the event.
        /// </summary>
        public string Stack { get; set; }
    }
}
