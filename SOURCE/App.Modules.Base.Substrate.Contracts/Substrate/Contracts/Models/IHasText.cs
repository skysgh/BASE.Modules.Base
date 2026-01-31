namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for objects that have a Text field.
    /// </summary>
    public interface IHasText
    {
        /// <summary>
        /// The Text
        /// <para>
        /// TODO: Correct. This probably should be Text? Or was this to ensure that Text is not used?
        /// </para>
        /// </summary>
        string Title { get; set; }
    }
}