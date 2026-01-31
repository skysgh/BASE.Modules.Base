namespace App.Modules.Base.Substrate.Contracts.Models.Contracts2
{

    /// <summary>
    /// Contract for methods that have an Initialize() method.    
    /// </summary>
    public interface IHasInitialize
    {
        /// <summary>
        /// Initialize the object.
        /// </summary>
        void Initialize();
    }


    /// <summary>
    /// Contract for methods that have an Initialize() method.    
    /// </summary>
    public interface IHasInitialize<in T>
    {
        /// <summary>
        /// Initialize the object with an argument.
        /// </summary>
        /// <param name="argument"></param>
        void Initialize(T argument);
    }

}