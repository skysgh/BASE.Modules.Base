namespace App.Modules.Base.Substrate.Contracts.Models.StorageMetadata
{
    /// <summary>
    /// Timestamp to apply to Records
    /// to ensure that records were updated by 
    /// some other user, while form was being
    /// filled in by another user.
    /// <para>
    /// Note: 
    /// There is no nullable equivalent.
    /// </para>
    /// </summary>
    public interface IHasTimestamp
    {
        /// <summary>
        ///     Gets or sets the datastore concurrency check timestamp.
        ///     <para>
        ///         Note that this is filled in when persisted in the db --
        ///         so it's usable to determine whether Record is New or not.
        ///     </para>
        /// </summary>
        byte[] Timestamp { get; set; }
    }
}