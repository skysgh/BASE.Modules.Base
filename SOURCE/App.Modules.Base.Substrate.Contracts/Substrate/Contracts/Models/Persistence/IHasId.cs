namespace App.Modules.Base.Substrate.Contracts.Models.StorageMetadata
{
    /// <summary>
    /// Contract to assign Typed Ids to entities.
    /// <para>
    /// See: <see cref="IHasGuidId"/> which enherits from it.
    /// </para>
    /// <para>
    /// Note there is no equivalent
    /// <c>IHasIdNullable{T}</c>
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasId<T>
    //where T  /*No: because string is not a struct : struct */
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// <para>
        /// Decorating this property with [JsonPropertyName(PropertyName = "id")]        
        /// This is needed for entities that will be persisted using DocumentDB.
        /// I'm so far resisting putting a reference on Newtonsoft's library, because
        /// it would cause all downstream assemblies to Reference this lib. Not good practices
        /// if it can be avoided.
        /// IH
        /// </para>
        /// </summary>
        T Id { get; set; }
    }
}