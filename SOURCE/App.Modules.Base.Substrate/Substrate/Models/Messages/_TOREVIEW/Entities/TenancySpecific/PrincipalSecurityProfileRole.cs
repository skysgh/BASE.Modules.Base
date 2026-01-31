using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;

// using System.Collections.Generic;
// using System.Collections.ObjectModel;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
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
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Get/Set the Description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Collection of Permissions assigned to this role.
        /// </summary>
        public ICollection<PrincipalSecurityProfilePermission> Permissions
        {
            get => _permissions ??= [];//new Collection<PrincipalSecurityProfilePermission>();
            set => _permissions = value;
        }
        /// <summary>
        /// TODO: Why Public?
        /// <para>
        /// TODO: Confirm documentation
        /// </para>
        /// </summary>
        public ICollection<PrincipalSecurityProfilePermission>? _permissions;



        /// <summary>
        /// The Assignement of Permissions to this Role.
        /// <para>
        /// TODO: Confirm documentation
        /// </para>
        /// </summary>
        public ICollection<PrincipalSecurityProfileRolePrincipalSecurityProfilePermissionAssignment> PermissionsAssignments
        {
            get => _permissionsAssignments ??= [];//new Collection<PrincipalSecurityProfileRolePrincipalSecurityProfilePermissionAssignment>();
            set => _permissionsAssignments = value;
        }
        /// <summary>
        /// TODO: Why Public?
        /// </summary>
#pragma warning disable CA1051 // Do not declare visible instance fields
        public ICollection<PrincipalSecurityProfileRolePrincipalSecurityProfilePermissionAssignment>? _permissionsAssignments;
#pragma warning restore CA1051 // Do not declare visible instance fields

    }

}

