using App.Modules.Sys.Shared.Models.Entities.Base;


// using System.Collections.Generic;
// using System.Collections.ObjectModel;

namespace App.Modules.Sys.Shared.Models.Messages._TOREVIEW.Entities.TenancySpecific
{
    /// <summary>
    /// A Role that can be assigned to a Principal's Security Profile.
    /// <para>
    /// Permissions are in turn (+/-) Assigned  to the Roles 
    /// (or directly to the Security Profile)
    /// </para>
    /// </summary>
    public class PrincipalSecurityProfileRole : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase, IHasTitleAndDescription
    {
        /// <summary>
        /// Get/Set the Title
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// Get/Set the Description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Collection of Permissions assigned to this role.
        /// </summary>
        public ICollection<PrincipalSecurityProfilePermission> Permissions
        {
            get => _permissions ??= [];
            set => _permissions = value;
        }
        
        private ICollection<PrincipalSecurityProfilePermission>? _permissions;



        /// <summary>
        /// The Assignement of Permissions to this Role.
        /// <para>
        /// TODO: Confirm documentation
        /// </para>
        /// </summary>
        public ICollection<PrincipalSecurityProfileRolePrincipalSecurityProfilePermissionAssignment> PermissionsAssignments
        {
            get => _permissionsAssignments ??= [];
            set => _permissionsAssignments = value;
        }
        
        private ICollection<PrincipalSecurityProfileRolePrincipalSecurityProfilePermissionAssignment>? _permissionsAssignments;

    }

}

