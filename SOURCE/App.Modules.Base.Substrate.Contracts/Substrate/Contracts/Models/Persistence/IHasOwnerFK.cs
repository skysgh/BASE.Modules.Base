namespace App.Modules.Base.Substrate.Contracts.Models.StorageMetadata
{
    /// <summary>
    /// Contract to apply to System Entities
    /// to return the record Id that they belong to.
    /// <para>
    /// (not all nested records will provide a navigation Property up to its parent, or even externally visible FK)
    /// </para>
    /// <para>
    /// Note: 
    /// Not the same purpose as <see cref="IHasParentFK"/>
    /// </para>
    /// <para>
    /// There is no Nullable equivalency.
    /// </para>
    /// </summary>
    public interface IHasOwnerFK
    {
        /// <summary>
        /// Returns the FK of the Owner/Parent Record.
        /// <para>
        /// (not all nested records will provide a navigation Property up to its parent, or even externally visible FK)
        /// </para>
        /// </summary>
        /// <returns></returns>
        Guid GetOwnerFk();
    }
}