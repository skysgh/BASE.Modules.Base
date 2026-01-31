using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// System entity (not exposed to the system's exterior) for
    /// A Principal can login from several foreign IdPs (FB, Google, ML, thisApp...).
    /// The user will be known by the remote Idp under a different login (eg: foo@google.com)
    /// as well as a unique reference id for the user (often a guid, but can be anything, hence string storage is best).
    /// When an IdP sends back the response, it has to be correlated back to a System Principal.
    /// That's done via this object.
    /// </summary>
    public class PrincipalLogin :
        UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasEnabled,
        IHasOwnerFK
    {

        /// <summary>
        /// The FK to the parent <see cref="Principal"/>.
        /// </summary>
        public Guid PrincipalFK { get; set; }

        /// <summary>
        /// Can be used to disallow an external IdP login that was previsoulsy trusted.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The Credential Name/Login the external IdP uses to distinguish users by (eg: google.com, sts, etc.).
        /// </summary>
        public string IdPLogin { get; set; }

        /// <summary>
        /// The Subject Identifier the external IdP uses to distinguish users by (eg: 'some guid, joeblow', 'joeblow@google.com', etc.).
        /// </summary>
        public string SubLogin { get; set; }

        /// <summary>
        /// Last datetime the user signed in via this login.
        /// </summary>
        public DateTime LastLoggedInUtc { get; set; }


        /// <summary>
        /// The Parent <see cref="Principal"/>'s FK.
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
