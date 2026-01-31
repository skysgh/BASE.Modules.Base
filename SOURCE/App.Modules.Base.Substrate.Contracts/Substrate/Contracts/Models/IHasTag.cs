using System.ComponentModel;

namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for assigning a single
    /// Tag to an entity.
    /// <para>
    /// Note: Prefer using 
    /// <see cref="IHasTagNullable"/>
    /// </para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IHasTag
    {
        /// <summary>
        /// The Tag
        /// </summary>
        string Tag { get; set; }
    }
}