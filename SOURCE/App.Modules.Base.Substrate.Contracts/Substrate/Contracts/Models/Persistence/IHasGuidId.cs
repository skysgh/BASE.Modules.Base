namespace App.Modules.Base.Substrate.Contracts.Models.StorageMetadata
{
    /// <summary>
    /// Contract for entities requiring an ID of type Guid.
    /// <para>
    /// ie. Most persistable records.
    /// </para>
    /// <para>
    /// There is no equivalent
    /// <c>IHasGuidId</c>
    /// </para>
    /// </summary>
    public interface IHasGuidId : IHasId<Guid>

    {

    }
}