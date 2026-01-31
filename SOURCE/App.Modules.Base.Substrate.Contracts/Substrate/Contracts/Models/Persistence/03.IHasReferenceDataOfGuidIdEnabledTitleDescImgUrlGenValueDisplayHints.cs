namespace App.Modules.Base.Substrate.Contracts.Models.StorageMetadata
{
    /// <summary>
    /// <para>
    /// Does Implements: 
    /// <list type="bullet">
    /// <item><see cref="IHasGuidId"/></item>
    /// <item><see cref="IHasEnabled"/></item>
    /// <item><see cref="IHasTitleAndDescriptionAndImageUrlAndDisplayHints"/></item>
    /// <item><see cref="IHasKeyGenericValue{T}"/></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IHasReferenceDataOfGuidIdEnabledTitleDescImgUrlKeyGenValueDisplayHints<TValue> :
        IHasReferenceDataOfGuidIdEnabledTitleDescImgUrlDisplayHints,
        IHasKeyGenericValue<TValue>
        where TValue : struct //int
    {

    }
}
