// using System;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
{
    /// <summary>
    /// A Claim belonging to the Profile belonging to the Principal.
    /// </summary>
    public class PrincipalProfileClaim
        : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasRecordState,
        IHasOwnerFK
    {
        /// <summary>
        /// The authority that is backing this claim.
        /// </summary>
        public virtual string? Authority { get; set; }
        /// <summary>
        /// The untamperable signature of the authority
        /// </summary>
        public virtual string? AuthoritySignature { get; set; }

        /// <summary>
        /// The unique Key of the Claim.
        /// </summary>
        public virtual string Key { get; set; } = string.Empty;
        /// <summary>
        /// The string value of the Claim
        /// </summary>
        public virtual string Value { get; set; } = string.Empty;

        /// <summary>
        /// Get/Set the FK to the parent Profile of the Principal.
        /// </summary>
        public virtual Guid PrincipalProfileFK { get; set; }
        //public virtual RecordPersistenceState RecordState { get; set; }



        /// <summary>
        /// Get the FK to the parent Profile of the Principal.
        /// </summary>
        public Guid GetOwnerFk()
        {
            return PrincipalProfileFK;
        }
    }
}