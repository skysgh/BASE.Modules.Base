using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;

namespace App.Modules.Base.Substrate.Models.Entities.Base
{
    /// <summary>
    /// <para>
    /// Note that this Base runs parrallel to
    /// <see cref="TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase"/>
    /// (inheritence line is based on Id Type).
    /// </para>
    /// 
    /// <para>
    /// Does Not Implements:
    /// <list type="bullet">
    /// <item><see cref="IHasGuidId"/></item>,
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
    /// <typeparam name="TId"></typeparam>
    public abstract class TenantFKAuditedRecordStatedTimestampedCustomIdEntityBase<TId> :
        UntenantedAuditedRecordStatedTimestampedCustIdEntityBase<TId>,
        IHasTenantFK
        where TId : struct
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public TenantFKAuditedRecordStatedTimestampedCustomIdEntityBase() : base()
        {
            //REMEMBER: As not a Guid ID Must be provided somehow...
        }
        /// <inheritdoc/>

        public virtual Guid TenantFK { get; set; }
    }
}