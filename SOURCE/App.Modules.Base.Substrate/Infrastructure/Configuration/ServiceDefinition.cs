using App.Modules.Base.Substrate.Contracts.Services.Registration;

namespace App.Modules.Base.Infrastructure.Configuration
{
    /// <inheritdoc/>
    public class ServiceDefinition : IServiceDefinition
    {
        /// <inheritdoc/>
        public Type ContractType { get; set; }
        /// <inheritdoc/>
        public Type ImplementationType { get; set; }
    }

    /// <inheritdoc/>
    public class ServiceDefinition<TContract, TInstance> : IServiceDefinition<TContract, TInstance>
    {
        /// <inheritdoc/>
        public Type ContractType { get; set; } = typeof(TContract);
        /// <inheritdoc/>
        public Type ImplementationType { get; set; } = typeof(TInstance);
    }
}
