namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract to appy to an item to Hint
    /// to a UX has to how to render something.
    /// </summary>
    public interface IHasDisplayStyleHintNullable
    {
        /// <summary>
        /// A Hint on how to display the item.
        /// <para>
        /// Consider using the field for a Classname
        /// that will mean something to the UX interface.
        /// </para>
        /// </summary>
        string? DisplayStyleHint { get; set; }
    }
}