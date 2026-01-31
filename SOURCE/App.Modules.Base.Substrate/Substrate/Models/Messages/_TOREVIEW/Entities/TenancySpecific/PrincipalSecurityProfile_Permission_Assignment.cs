using App.Modules.Base.Substrate.Models.Entities.Base;
using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums;

// using System;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
{
    /// <summary>
    /// Assignment of a Permission to a Principal's Security Profile.
    /// </summary>
    public class PrincipalSecurityProfile_Permission_Assignment : TenantFKAuditedRecordStatedTimestampedNoIdEntityBase
    {
        /// <summary>
        /// FK of Security Profile
        /// <para>
        /// TODO: Improve documentation
        /// </para>
        /// </summary>
        public Guid AccountFK { get; set; }
        /// <summary>
        /// Get/Set Security Profile
        /// <para>
        /// TODO: Improve Documentation
        /// </para>
        /// </summary>
        public PrincipalSecurityProfile? Account { get; set; }

        /// <summary>
        /// Get/Set FK of Permisison.
        /// <para>
        /// TODO: Improve Documentation
        /// </para>
        /// </summary>
        public Guid PermissionFK { get; set; }
        /// <summary>
        /// Get/Set Permission
        /// <para>
        /// TODO: Improve Documentation
        /// </para>
        /// </summary>
        public PrincipalSecurityProfilePermission? Permission
        { get; set; }

        /// <summary>
        /// Whether the Assignment is additive, or subtractive
        /// (an Account can be added to Groups to which Roles have been assigned,
        /// or assigned directly to Roles,
        /// and can be assigned Permissions that remove Permissions assigned by 
        /// one of the previous two methods.
        /// </summary>
        public AssignmentType AssignmentType { get; set; }
    }

}

