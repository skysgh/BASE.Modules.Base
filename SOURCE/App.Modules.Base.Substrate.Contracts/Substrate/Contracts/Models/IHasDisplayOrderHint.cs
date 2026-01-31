namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract to add information to Hint
    /// to the UX in which order to render items.
    /// <para>
    /// Note that although the related
    /// <see cref="IHasDisplayStyleHintNullable"/>
    /// is nullable, this is not (default will be 0).
    /// </para>
    /// </summary>
    public interface IHasDisplayOrderHint
    {
        /// <summary>
        /// A Hint to the Interface (UI/API) 
        /// to organise item order.
        /// </summary>
        int DisplayOrderHint { get; set; }
    }

}