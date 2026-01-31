namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    ///     Contract for models that have a Name 
    ///     (not automatically unique).
    ///     <para>
    ///     NOTE: prefer the use of 
    ///     <see cref="IHasKey"/> 
    ///     if the value is unique.
    /// </para>
    /// <para>
    /// Consider also 
    /// <see cref="IHasTitle"/> 
    /// if it's just a displayable name.
    /// </para>
    /// <para>
    /// There is no equivalent
    /// <c>IHasNameNullable</c>
    /// </para>
    /// </summary>
    public interface IHasName
    {

        /// <summary>
        /// The name of the model.
        /// <para>
        /// NOTE: prefer the use of <see cref="IHasKey"/> if the value is unique.
        /// </para>
        /// <para>
        /// Consider also <see cref="IHasTitle"/> if it's just a displayable name. 
        /// </para>
        /// </summary>
        string Name { get; set; }
    }
}