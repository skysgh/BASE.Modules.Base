// using System.ComponentModel;

namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for Entities and other models that 
    /// provide a text description. 
    /// <para>
    /// Most Reference Items should have both a Title
    /// AND a Description. 
    /// </para>
    /// <para>
    /// Descriptions of the purpose of Items
    /// reduces Training and Tech Support costs.
    /// </para>
    /// <para>
    /// Prefer this contract to 
    /// <see cref="IHasDescription"/>
    /// </para>
    /// </summary>
    public interface IHasDescription
    {
        /// <summary>
        /// The textual Description.
        /// 
        /// <para>
        /// Note that it records put Descriptions
        /// in DBs makes it harder to be international later.
        /// </para>
        /// <para>
        /// Consider using the field for a token that a post processor
        /// could recognise and replace with a localised text resource.
        /// </para>
        /// </summary>
        string Description { get; set; }
    }



}