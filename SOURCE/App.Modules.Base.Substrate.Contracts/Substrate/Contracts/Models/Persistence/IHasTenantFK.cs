namespace App.Modules.Base.Substrate.Contracts.Models.StorageMetadata
{

    /// <summary>
    /// Contract applied to Models
    /// that are specific to a single 
    /// Tenant.
    /// <para>
    /// Most models - including the ones within Core - are specific 
    /// to a single Tenant.
    /// </para>
    /// <para>
    /// There is no equivalent
    /// <c>IHasTenantFK</c>
    /// </para>
    /// </summary>
    public interface IHasTenantFK
    {

        /// <summary>
        /// Gets or sets the FK of the Tenancy this mutable model belongs to.
        /// <para>
        /// When referenced from within the Core Module's DbContext
        /// the TenantFK is logically enforced by the database (normalized), 
        /// whereas from other DbContexts it is not.
        ///
        /// The Logic behind this choice stems from the understanding that
        /// a Business Model (eg: Foo) has no need to Navigate to a Tenant to which 
        /// the Business Model belongs. It's actually a different Domain Context (System).
        ///
        /// The above logic is actually enforced in EF's natural constraint that a Model
        /// belong to only one DbContext (one Bounded Context).
        ///
        /// The advantage is natural Domain Separation. In the same way as we trust external
        /// IdP Services to manage Users.
        ///
        /// THe consideration is that Application logic is required to ensure TenantId
        /// is applied at the Application Logic tier, as it is not enforced at the database.
        ///
        /// TenantFK is not required on anything else but the TenantProperties entity, and Users
        /// in order to know which Tenant a user is allowed to be a member of.
        /// </para>
        /// </summary>
        /// <value>
        /// The tenant fk.
        /// </value>
        Guid TenantFK { get; set; }
    }
}