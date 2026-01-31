namespace App.Modules.Base.Substrate.Contracts.Services.Registration
{
    /// <summary>
    /// The definition of a service to be registered.
    /// </summary>
    public interface IServiceDefinition
    {
        /// <summary>
        /// Type of Interface/Contract
        /// </summary>
        Type ContractType { get;set;}
        /// <summary>
        /// Type of implementation.
        /// </summary>
        Type ImplementationType { get; set; }
    }
    ///<inheritdoc/>
    public interface IServiceDefinition<TContract, TInstance> : IServiceDefinition
    {
        ///<inheritdoc/>
        new Type ContractType { get; set; }
        ///<inheritdoc/>
        new Type ImplementationType { get; set; }
    }
}
