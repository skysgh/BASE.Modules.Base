using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;

namespace App.Modules.Base.Substrate.Models.Entities.Base
{
    /// <summary>
    /// 
    /// <para>
    /// Note that this Base runs *parrallel* to
    /// <see cref="UntenantedAuditedRecordStatedTimestampedGuidIdReferenceDataEntityBase"/>
    /// (as one can't define a Generic Id and then redefine it with a Guid Id).
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
    /// Does Implement:
    /// <list type="bullet">
    /// <item><see cref="IHasTimestampRecordStateInRecordAuditability"/></item>,
    /// <item><see cref="IHasReferenceDataOfGenericIdEnabledTitleDescImgUrlDisplayHints{TId}"/></item>,
    /// </list>
    /// </para>    
    /// 
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class UntenantedAuditedRecordStatedTimestampedCustomIdReferenceDataEntityBase<TId> :
        UntenantedAuditedRecordStatedTimestampedCustIdEntityBase<TId>,
       /*Enherited: IHasId<Guid>, IHasTimestamp, IHasInRecordAuditability, IHasRecordState*/
       IHasReferenceDataOfGenericIdEnabledTitleDescImgUrlDisplayHints<TId>
        where TId : struct
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected UntenantedAuditedRecordStatedTimestampedCustomIdReferenceDataEntityBase()
        {
            //As the Id is custom, cannot set:
            // NO: Id = GuidFactory.NewGuid();
        }




        /// <inheritdoc/>
        public virtual bool Enabled { get; set; }
        /// <inheritdoc/>

        public virtual string Title { get; set; } = string.Empty;
        /// <inheritdoc/>

        public virtual string Description { get; set; } = string.Empty;

        /// <inheritdoc/>

        public virtual string ImageUrl { get; set; } = string.Empty;
        /// <inheritdoc/>

        public virtual int DisplayOrderHint { get; set; }
        /// <inheritdoc/>

        public virtual string? DisplayStyleHint { get; set; }
    }


}
