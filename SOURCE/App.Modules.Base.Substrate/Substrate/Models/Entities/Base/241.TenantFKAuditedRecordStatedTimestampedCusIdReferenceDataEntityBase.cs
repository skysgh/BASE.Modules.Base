using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;

namespace App.Modules.Base.Substrate.Models.Entities.Base
{
    /// <summary>
    /// Base abstract class for Mutable 
    /// Reference data. 
    /// 
    /// <para>
    /// Does Not Implements:
    /// <list type="bullet">
    /// <item><see cref="IHasGuidId"/></item>,
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// Does Implement:
    /// <list type="bullet">
    /// <item><see cref="IHasTimestampRecordStateInRecordAuditability"/></item>,
    /// <item><see cref="IHasReferenceDataOfGenericIdEnabledTitleDescImgUrlDisplayHints{TId}"/></item>,
    /// </list>
    /// </para>    
    /// 
    /// </summary>
    public abstract class TenantFKAuditedRecordStatedTimestampedCustIdReferenceDataEntityBase<TId>
        : UntenantedAuditedRecordStatedTimestampedCustomIdReferenceDataEntityBase<TId>,
        IHasTenantFK
        where TId : struct
    {

        /// <summary>
        /// Gets or sets the FK of the Tenancy this mutable model belongs to.
        /// </summary>
        public virtual Guid TenantFK { get; set; }
        /// <summary>
        /// Constructor:
        /// <para>
        /// Initializes a new instance of the <c>TenantFKTimestampedAuditedRecordStatedCustomIdReferenceDataEntityBase</c> class.
        /// </para>
        /// </summary>
        protected TenantFKAuditedRecordStatedTimestampedCustIdReferenceDataEntityBase()
        {
            //As the Id is custom, cannot set:
            //NO: Id = GuidFactory.NewGuid();
        }



    }
}