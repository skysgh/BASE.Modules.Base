namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for services and objects requiring initialisation.
    /// </summary>
    public interface IInitialize
    {
        /// <summary>
        /// Initialise!
        /// </summary>
        void Initialize();
    }
}