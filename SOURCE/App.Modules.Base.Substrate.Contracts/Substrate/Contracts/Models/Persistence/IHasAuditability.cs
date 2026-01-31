namespace App.Modules.Base.Substrate.Contracts.Models.StorageMetadata
{
    /// <summary>
    /// Contract for persistable system entities
    /// to include basic values in the record.
    /// <para>
    /// Note:
    /// <c>Created</c>/<c>Updated</c> 
    /// properties always expect 
    /// a value, whereas the 
    /// <c>Deleted</c> properties are nullable.
    /// </para>
    /// <para>
    /// Note: there is no equivalent 
    /// <c>IHasInRecordAuditabilityNullable.</c>
    /// </para>
    /// </summary>
    public interface IHasInRecordAuditability
    {
        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        DateTime CreatedOnUtc { get; set; }


        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        string CreatedByPrincipalId { get; set; }

        /// <summary>
        ///     Gets or sets the last modified on.
        /// </summary>
        DateTime LastModifiedOnUtc { get; set; }

        /// <summary>
        ///     Gets or sets the last modified by.
        /// </summary>
        string LastModifiedByPrincipalId { get; set; }

        /// <summary>
        ///     Gets or sets the last modified on.
        /// </summary>
        DateTime? DeletedOnUtc { get; set; }

        /// <summary>
        ///     Gets or sets the last modified by.
        /// </summary>
        string? DeletedByPrincipalId { get; set; }
    }
}