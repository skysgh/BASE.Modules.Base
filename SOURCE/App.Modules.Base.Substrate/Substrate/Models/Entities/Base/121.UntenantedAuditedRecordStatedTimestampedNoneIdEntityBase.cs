using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using App.Modules.Base.Substrate.Contracts.Models.Enums;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;

namespace App.Modules.Base.Substrate.Models.Entities.Base
{
    /// <summary>
    /// Abstract base class of entities.
    /// 
    /// <para>
    /// Does not implement
    /// <list type="bullet">
    /// <item><see cref="IHasId{T}"/></item>
    /// <item><see cref="IHasGuidId"/></item>
    /// <item><see cref="IHasTenantFK"/></item>
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// Does implement:
    /// <list type="bullet">
    /// <item><see cref="IHasTimestampRecordStateInRecordAuditability"/></item>
    /// </list>
    /// </para>
    /// </summary>
    [DataContract]
    public abstract class UntenantedAuditedRecordStatedTimestampedNoneIdEntityBase
        : IHasTimestampRecordStateInRecordAuditability
    {


        /// <summary>
        ///     Gets or sets the datastore concurrency check timestamp.
        ///     <para>
        ///         Note that this is filled in when persisted in the db --
        ///         so it's usable to determine whether Record is New or not.
        ///     </para>
        /// </summary>
        [ConcurrencyCheck]
        public virtual byte[] Timestamp { get; set; }
        /// <inheritdoc/>

        // Do not include in DTOs:[DataMember]
        [NotMapped]
        public virtual RecordPersistenceState RecordState { get; set; }
        /// <inheritdoc/>


        //TODO: Convert to DateTimeOffset
        // Do not include in DTOs:[DataMember]
        [NotMapped]
        public virtual DateTime CreatedOnUtc { get; set; }
        /// <inheritdoc/>
        // Do not include in DTOs:[DataMember]
        [NotMapped]
        public virtual string CreatedByPrincipalId { get; set; }
        /// <inheritdoc/>
        // Do not include in DTOs:[DataMember]
        [NotMapped]
        public virtual DateTime LastModifiedOnUtc { get; set; }
        /// <inheritdoc/>
        // Do not include in DTOs:[DataMember]
        [NotMapped]
        public virtual string LastModifiedByPrincipalId { get; set; }
        /// <inheritdoc/>
        // Do not include in DTOs:[DataMember]
        [NotMapped]
        public virtual DateTime? DeletedOnUtc { get; set; }
        /// <inheritdoc/>
        // Do not include in DTOs:[DataMember]
        [NotMapped]
        public virtual string? DeletedByPrincipalId { get; set; }



    }
}