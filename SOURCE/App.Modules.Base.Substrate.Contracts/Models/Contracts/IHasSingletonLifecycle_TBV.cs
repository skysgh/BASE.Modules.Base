namespace App.Modules.Base.Substrate.Models.Contracts
{
    /// <summary>
    /// Marker interface indicating the service should have a Singleton lifecycle.
    /// <para>
    /// Used for service discovery and registration patterns.
    /// </para>
    /// <para>
    /// Should be implemented by services that maintain state across the application lifetime.
    /// </para>
    /// </summary>
    /// <seealso cref="IHasLifecycle" />
    public interface IHasSingletonLifecycle : IHasLifecycle
    {
    }
}
