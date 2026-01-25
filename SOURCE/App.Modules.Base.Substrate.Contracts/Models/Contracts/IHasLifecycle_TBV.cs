using App.Modules.Base.Substrate.Contracts.Lifecycles;

namespace App.Modules.Base.Substrate.Models.Contracts
{
    /// <summary>
    /// A marker interface to hint at the default IoC lifespan of the implementor.
    /// <para>
    /// Used for service discovery and registration patterns.
    /// </para>
    /// <para>
    /// Implemented by <see cref="IHasSingletonLifecycle"/> and <see cref="IHasTransientLifecycle"/>.
    /// </para>
    /// </summary>
    public interface IHasLifecycle
    {
    }
}
