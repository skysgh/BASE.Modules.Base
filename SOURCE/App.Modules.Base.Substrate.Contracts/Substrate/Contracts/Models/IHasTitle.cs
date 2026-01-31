namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for objects that have a displayable title.
    /// </summary>
    public interface IHasTitle
    {
        /// <summary>
        /// The (display) title
        /// </summary>
        string Title { get; set; }
    }
}