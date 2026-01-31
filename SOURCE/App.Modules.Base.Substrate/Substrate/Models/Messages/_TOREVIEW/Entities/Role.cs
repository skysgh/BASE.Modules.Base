using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;
using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// System entity (not exposed to the system's exterior) for
    /// Roles within this System (not the same as Claims Roles that came in via remote IdP)
    /// And not the same as custom Tenant roles (still to solve).
    /// <para>
    /// Note that Role needs to be upgraded to be just a logical 
    /// grouping of Permissions. 
    /// </para>
    /// </summary>
    public class SystemRole : UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase, IHasEnabled, IHasKey
    {
        /// <summary>
        /// Get/Set whether the Role is enabled.
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// Module within which the Role was developed/designed for.
        /// </summary>
        public virtual string ModuleKey { get; set; } = string.Empty;

        /// <summary>
        ///     The in-system rolename.
        /// </summary>
        public virtual string Key { get; set; } = string.Empty;


        /// <summary>
        /// The FK to the <see cref="DataClassification"/>
        /// used to describe the Role.
        /// </summary>
        public virtual NZDataClassification? DataClassificationFK { get; set; }

        /// <summary>
        /// The <see cref="DataClassification"/>
        /// describing the Role.
        /// <para>
        /// TODO: Improve Description to better describe use.
        /// </para>
        /// </summary>
        public virtual DataClassification? DataClassification { get; set; }
    }
}