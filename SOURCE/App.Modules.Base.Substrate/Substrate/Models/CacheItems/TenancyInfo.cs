namespace App.Modules.Base.Substrate.Models.CacheItems
{
    /// <summary>
    /// Cacheable information about a Tenancy
    /// </summary>
    public class TenancyInfo
    {
        /// <summary>
        /// The id/subdomain identifying 
        /// the tenancy.
        /// </summary>
        public string? TenancyId { get; set; }

        /// <summary>
        /// A list of environment 
        /// identities associated with this tenancy.
        /// </summary>
        public string[]? AssociatedEnvNames { get; set; }
    }
}
