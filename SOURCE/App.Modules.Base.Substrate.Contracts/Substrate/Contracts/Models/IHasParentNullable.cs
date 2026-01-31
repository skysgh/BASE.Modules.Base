using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;

namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for models that have a Parent object
    /// <para>
    /// Note:
    /// <see cref="IHasParentFK"/> 
    /// does not fill the 
    /// same purpose as
    /// <see cref="IHasOwnerFK"/>
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasParentNullable<T> : IHasParentFKNullable
    {

        /// <summary>
        /// The parent object (if it exists--ie ParentFK has a value for starters)
        /// </summary>
        T? Parent { get; set; }
    }


}