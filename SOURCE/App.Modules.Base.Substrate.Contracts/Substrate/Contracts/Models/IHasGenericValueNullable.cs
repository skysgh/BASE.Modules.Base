namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for objects that have a typed value.
    /// <para>
    /// See: <see cref="IHasKeyGenericValue{T}"/>
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasGenericValueNullable<T>
    {
        /// <summary>
        /// Get/set value
        /// </summary>
        T? Value { get; set; }
    }
}