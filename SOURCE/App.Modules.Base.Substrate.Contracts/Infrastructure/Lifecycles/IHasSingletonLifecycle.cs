namespace App.Modules.Base.Infrastructure.Lifecycles
{
    /// <summary>
    /// A Singleton version of <see cref="IHasLifecycle"/>.
    /// <para>
    /// At startup the IoC Container (ie, StructureMap)
    /// searches all assemblies for services, etc.
    /// and uses <see cref="IHasLifecycle"/> to hint as to how 
    /// to register them.
    /// </para>
    /// <para>
    /// Should be implemented by atleast all Services in the system.</para>
    /// </summary>
    /// <seealso cref="IHasLifecycle" />
    public interface IHasSingletonLifecycle : IHasLifecycle
    {
    }
}