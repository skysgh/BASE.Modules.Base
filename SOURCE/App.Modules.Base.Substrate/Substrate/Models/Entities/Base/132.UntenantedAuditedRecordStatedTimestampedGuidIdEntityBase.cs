using System.Runtime.Serialization;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Factories;

namespace App.Modules.Base.Substrate.Models.Entities.Base
{
    /// <summary>
    /// <para>
    /// Note that this Base runs parrallel to
    /// <see cref="UntenantedAuditedRecordStatedTimestampedCustIdEntityBase{TId}"/>
    /// (inheritence line is based on Id Type).
    /// </para>
    /// 
    /// <para>
    /// Does Not Implements:
    /// <list type="bullet">
    /// <item><see cref="IHasTenantFK"/></item>,
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// Does Implement:
    /// <list type="bullet">
    /// <item><see cref="IHasGuidId"/></item>,
    /// <item><see cref="IHasTimestampRecordStateInRecordAuditability"/></item>,
    /// </list>
    /// </para>    
    /// 
    /// </summary>
    [DataContract]
    public abstract class UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase :
        UntenantedAuditedRecordStatedTimestampedCustIdEntityBase<Guid>,
        IHasGuidId
    /*Enherited: IHasId<Guid>, IHasTimestamp, IHasInRecordAuditability, IHasRecordState*/
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase"/> class.
        /// </summary>
        protected UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase()
        {
            Id = GuidFactory.NewGuid();
        }
    }
}