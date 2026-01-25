namespace App.Modules.Base.Substrate.Models.Contracts
{
    /// <summary>
    /// Contract for methods that have an Initialize() method with typed argument.
    /// </summary>
    /// <typeparam name="T">Type of initialization argument</typeparam>
    public interface IHasInitialize<in T>
    {
        /// <summary>
        /// Initialize the object with an argument.
        /// </summary>
        /// <param name="argument">Initialization argument</param>
        void Initialize(T argument);
    }
}
