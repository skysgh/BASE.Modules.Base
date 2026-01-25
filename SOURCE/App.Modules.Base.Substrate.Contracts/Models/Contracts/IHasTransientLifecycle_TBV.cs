namespace App.Modules.Base.Substrate.Models.Contracts
{
    /// <summary>
    /// Marker interface indicating the service should have a Transient lifecycle.
    /// <para>
    /// Used for service discovery and registration patterns.
    /// </para>
    /// <para>
    /// Services with this interface are created each time they are requested.
    /// </para>
    /// </summary>
    /// <seealso cref="IHasLifecycle" />
    public interface IHasTransientLifecycle : IHasLifecycle
    {
    }
}
