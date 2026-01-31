// using System.Collections.ObjectModel;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;

// using System.Collections.Generic;
// using System.Collections.ObjectModel;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// TODO: Improve documentation
    /// 
    /// </summary>
    public class TenancySecurityProfileAccountRole : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase, IHasTitleAndDescription
    {
        /// <summary>
        /// The Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get/set the permissions
        /// </summary>
        public ICollection<TenancySecurityProfilePermission> Permissions
        {
            get => _permissions ??= [];
            set => _permissions = value;
        }
        /// <summary>
        /// TODO:Public required?
        /// </summary>
        public ICollection<TenancySecurityProfilePermission>? _permissions;


        /// <summary>
        /// Get/set the Permission Assignments
        /// </summary>
        public ICollection<TenancySecurityProfileRoleTenancySecurityProfilePermissionAssignment> PermissionsAssignments
        {
            get => _permissionsAssignments ??= [];// new Collection<TenancySecurityProfileRoleTenancySecurityProfilePermissionAssignment>();
            set => _permissionsAssignments = value;
        }
        /// <summary>
        /// TODO: Public required?
        /// </summary>
        public ICollection<TenancySecurityProfileRoleTenancySecurityProfilePermissionAssignment>? _permissionsAssignments;

    }

}

