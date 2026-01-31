namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for objects that have a typed scalar value.
    /// <para>
    /// See: <see cref="IHasGenericValueNullable{T}"/> if 
    /// it's an object/not Scalar.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasGenericValue<T>
    {
        /// <summary>
        /// Get/set value
        /// </summary>
        T Value { get; set; }
    }
}