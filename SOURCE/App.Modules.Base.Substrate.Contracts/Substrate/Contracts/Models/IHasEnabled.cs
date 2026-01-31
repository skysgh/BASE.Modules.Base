namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract to apply to models
    /// to define whether they are enabled or not.
    /// </summary>
    public interface IHasEnabled
    {
        /// <summary>
        /// Get/Set whether the entity is enabled or not.
        /// <para>
        /// Eg: used for <c>Session</c>, <c>Users</c>, etc.
        /// </para>
        /// </summary>
        bool Enabled { get; set; }
    }
}