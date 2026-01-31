using App.Modules.Base.Substrate.Contracts.Models.Enums;

namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for assigning a State to a Record.
    /// <para>
    /// Note that although similar, it is not the same as a Document Workflow state.
    /// (ie, It's to be limited to Record persistence).
    /// </para>
    /// </summary>
    public interface IHasRecordState
    {
        /// <summary>
        /// The state of the Record in terms of persistence.
        /// </summary>
        RecordPersistenceState RecordState { get; set; }
    }


}