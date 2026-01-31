using App.Modules.Base.Substrate.Models.Entities.Base;
using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums;

// using System;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// The direct Assignment of a Permission to a Tenancy Security Profile
    /// (rather than through a Group).
    /// </summary>
    public class TenancySecurityProfileTenancySecurityProfilePermissionAssignment : TenantFKAuditedRecordStatedTimestampedNoIdEntityBase
    {
        /// <summary>
        /// The FK of the Security Profile
        /// </summary>
        public Guid AccountFK { get; set; }
        /// <summary>
        /// The Security Profile
        /// </summary>
        public TenancySecurityProfile? Account { get; set; }


        /// <summary>
        /// The FK of the Permission
        /// </summary>
        public Guid PermissionFK { get; set; }
        /// <summary>
        /// The Permission
        /// </summary>
        public TenancySecurityProfilePermission Permission { get; set; }

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

