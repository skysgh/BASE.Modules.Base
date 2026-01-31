using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// System entity (not exposed to the system's exterior) for
    /// a Claim (attribute provided/verified by a trusted 3rd party)
    /// about a <see cref="Principal"/>.
    /// </summary>
    public class PrincipalClaim :
        UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasRecordState,
        IHasOwnerFK,
        IHasKeyGenericValue<string>
    {
        /// <summary>
        /// The name of the 3rd party Authority backing the claim.
        /// </summary>
        public virtual string? Authority { get; set; }
        /// <summary>
        /// Their digital Signature.
        /// </summary>
        public virtual string? AuthoritySignature { get; set; }
        /// <summary>
        /// The Claim's key
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// The Claim's string value (json, etc.)
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// The FK to the <see cref="Principal"/> record
        /// this Claim is describing.
        /// <para>
        /// Note that it is not associated with a Navigation property
        /// back up to the <see cref="Principal"/>
        /// </para>
        /// </summary>
        public virtual Guid PrincipalFK { get; set; }
        //public virtual RecordPersistenceState RecordState { get; set; }


        /// <summary>
        /// Get the FK to the Owner
        /// <para>
        /// TODO: Describe purpose better.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public Guid GetOwnerFk()
        {
            return PrincipalFK;
        }
    }
}