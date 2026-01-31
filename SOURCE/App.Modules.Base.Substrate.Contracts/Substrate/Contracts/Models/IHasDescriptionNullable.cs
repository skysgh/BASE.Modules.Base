using System.ComponentModel;

namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for Entities and other models that 
    /// provide a text description. 
    /// <para>
    /// Prefer using <see cref="IHasDescription"/>
    /// as most if not all list items should have
    /// both a Title AND a Description.
    /// </para>
    /// <para>
    /// Descriptions reduces training 
    /// and tech support costs.
    /// </para>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]

    public interface IHasDescriptionNullable
    {
        /// <summary>
        /// The textual Description.
        /// <para>
        /// <see cref="IHasDescription"/>
        /// </para>
        /// <para>
        /// Note that it records put Descriptions
        /// in DBs makes it harder to be international later.
        /// </para>
        /// <para>
        /// Consider using the field for a token that a post processor
        /// could recognise and replace with a localised text resource.
        /// </para>
        /// </summary>
        string? Description { get; set; }
    }



}