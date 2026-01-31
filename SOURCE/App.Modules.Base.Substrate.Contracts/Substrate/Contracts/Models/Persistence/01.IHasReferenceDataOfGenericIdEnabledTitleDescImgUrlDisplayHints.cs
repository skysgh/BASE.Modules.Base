namespace App.Modules.Base.Substrate.Contracts.Models.StorageMetadata
{
    /// <summary>
    /// <para>
    /// Does NOT implement
    /// <list type="bullet">
    /// <item><see cref="IHasGuidId"/></item>
    /// </list>
    /// </para>
    /// <para>
    /// Does Implements 
    /// <list type="bullet">
    /// <item><see cref="IHasId{TId}"/></item>
    /// <item><see cref="IHasEnabled"/></item>
    /// <item><see cref="IHasTitleAndDescriptionAndImageUrlAndDisplayHints"/></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IHasReferenceDataOfGenericIdEnabledTitleDescImgUrlDisplayHints<TId> :
       IHasId<TId>,
       IHasReferenceDataOfNoIdEnabledTitleDescImgUrlDisplayHints
    {

    }

}