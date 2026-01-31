using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;

namespace App.Modules.Base.Substrate.Models.Entities.Base
{
    /// <summary>
    /// <para>
    /// Note that this Base runs parrallel to
    /// <see cref="UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase"/>
    /// (inheritence line is based on Id Type).
    /// </para>
    /// 
    /// <para>
    /// Does Not Implements:
    /// <list type="bullet">
    /// <item><see cref="IHasGuidId"/></item>,
    /// <item><see cref="IHasTenantFK"/></item>,
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// Does Implements:
    /// <list type="bullet">
    /// <item><see cref="IHasId{T}"/></item>,
    /// <item><see cref="IHasTimestampRecordStateInRecordAuditability"/></item>
    /// </list>
    /// </para>
    /// 
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    [DataContract]
    public abstract class UntenantedAuditedRecordStatedTimestampedCustIdEntityBase<TId> :
        UntenantedAuditedRecordStatedTimestampedNoneIdEntityBase,
        IHasId<TId>
        /* Inherited:  IHasTimestamp, IHasInRecordAuditability, IHasRecordState */
        where TId : struct
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected UntenantedAuditedRecordStatedTimestampedCustIdEntityBase()
        {
            //REMEMBER: ID MUST BE PROVIDED IN THIS CASE...
        }
        /// <inheritdoc/>


        [DataMember]
        [Key]
        public virtual TId Id { get; set; } //Can't set as it's Generic.
    }
}