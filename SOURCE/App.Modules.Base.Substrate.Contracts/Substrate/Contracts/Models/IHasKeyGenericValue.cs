namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract to apply to models that need
    /// a mandatory Key and a 
    /// non nullable typed value (generally a string)
    /// <para>
    /// Implements both 
    /// <see cref="IHasKey"/>
    /// and 
    /// <see cref="IHasKeyGenericValue{T}"/>
    /// (where T is a scalar string, int or param)
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasKeyGenericValue<T> : IHasKey, IHasGenericValue<T>

    {
    }
}