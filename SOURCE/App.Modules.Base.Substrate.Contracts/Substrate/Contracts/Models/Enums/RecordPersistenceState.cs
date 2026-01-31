namespace App.Modules.Base.Substrate.Contracts.Models.Enums

{
    /// <summary>
    /// <para>
    /// TODO: Enums are evil (offset issue of Interface Localization)
    /// </para>
    /// </summary>
    public enum RecordPersistenceState
    {
        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Active/normal state.
        /// </summary>
        Active = 1,

        /// <summary>
        /// Archived.
        /// <para>
        /// Pretty sure this is invalid for a Record State -- ok for an EntityState, but that's different: Disabled=true,
        /// </para>
        /// </summary>
        Archived = 2,
        /// <summary>
        /// Record is superceded by another record.
        /// </summary>
        Superceded = 4,
        /// <summary>
        /// Record is merged into another.
        /// </summary>
        Merged = 8,
        /// <summary>
        /// Record is to dispose (but Undeletable back to another state)
        /// </summary>
        ToDispose = 16 /*Garbage*/,

        /// <summary>
        /// Record is disposed. 
        /// Cannot be restored to another state.
        /// </summary>
        Disposed = 32
    }
}