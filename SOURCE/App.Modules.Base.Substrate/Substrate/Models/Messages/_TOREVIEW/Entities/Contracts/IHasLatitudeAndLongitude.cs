namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Contracts
{
    /// <summary>
    /// Contract for models that define their 
    /// Global Position.
    /// </summary>
    public interface IHasLatitudeAndLongitude
    {
        /// <summary>
        /// The latitude
        /// </summary>
        decimal Latitude { get; set; }
        /// <summary>
        /// The longitude
        /// </summary>
        decimal Longitude { get; set; }
    }
}