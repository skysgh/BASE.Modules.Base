// using System.Collections.ObjectModel;
using App.Modules.Sys.Shared.Models.Entities.Base;


// using System.Collections.Generic;
// using System.Collections.ObjectModel;

namespace App.Modules.Sys.Shared.Models.Messages._TOREVIEW.Entities
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
        public string Title { get; set; } = default!;
        /// <summary>
        /// The Description.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Get/set the permissions
        /// </summary>
        public ICollection<TenancySecurityProfilePermission> Permissions
        {
            get => _permissions ??= [];
            set => _permissions = value;
        }
        
        private ICollection<TenancySecurityProfilePermission>? _permissions;


        /// <summary>
        /// Get/set the Permission Assignments
        /// </summary>
        public ICollection<TenancySecurityProfileRoleTenancySecurityProfilePermissionAssignment> PermissionsAssignments
        {
            get => _permissionsAssignments ??= [];
            set => _permissionsAssignments = value;
        }
        
        private ICollection<TenancySecurityProfileRoleTenancySecurityProfilePermissionAssignment>? _permissionsAssignments;

    }

}

