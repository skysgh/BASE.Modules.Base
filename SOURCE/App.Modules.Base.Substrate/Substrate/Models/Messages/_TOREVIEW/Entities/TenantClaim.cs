// using System;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    ///  System entity (not exposed to the system's exterior) for
    /// A Claim about a Tenant in the system.
    /// </summary>
    public class TenantClaim
        : UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasOwnerFK
    {
        /// <summary>
        /// The Authority backing the Claim
        /// </summary>
        public virtual string? Authority { get; set; }
        /// <summary>
        /// The signature of the Authority
        /// </summary>
        public virtual string? AuthoritySignature { get; set; }
        /// <summary>
        /// The Claim's key
        /// </summary>
        public virtual string Key { get; set; }
        /// <summary>
        /// The Claim's string value
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// The FK of the parent <see cref="Tenant"/>
        /// </summary>
        public virtual Guid TenantFK { get; set; }

        /// <summary>
        /// Get the FK of the parent Tenant
        /// </summary>
        /// <returns></returns>
        public Guid GetOwnerFk()
        {
            return TenantFK;
        }
    }
}