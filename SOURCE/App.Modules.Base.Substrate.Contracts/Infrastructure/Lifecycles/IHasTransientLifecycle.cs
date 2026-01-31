namespace App.Modules.Base.Infrastructure.Lifecycles
{
    /// <summary>
    /// A Transient version of <see cref="IHasLifecycle"/>.
    /// <para>
    /// At startup the IoC Container (ie, StructureMap)
    /// searches all assemblies for services, etc.
    /// and uses <see cref="IHasLifecycle"/> to hint as to how 
    /// to register them.
    /// </para>
    /// </summary>
    /// <seealso cref="IHasLifecycle" />
    public interface IHasTransientLifecycle : IHasLifecycle
    {
    }
}