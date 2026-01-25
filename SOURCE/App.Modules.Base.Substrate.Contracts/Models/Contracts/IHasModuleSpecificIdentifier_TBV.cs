namespace App.Modules.Base.Substrate.Models.Contracts
{
    /// <summary>
    /// Marker interface that ties commonly named contracts in this module
    /// to a uniquely named contract for the Base module.
    /// <para>
    /// This allows reflection to distinguish between different modules
    /// beyond just namespace inspection.
    /// </para>
    /// </summary>
    public interface IHasModuleSpecificIdentifier : IModuleBase
    {
    }
}
