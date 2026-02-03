using App.Modules.Sys.Shared.Models.Entities.Base;
using App.Modules.Sys.Shared.Models.Messages._TOREVIEW.Entities;

// using App.Modules.Sys.Substrate.Models.Messages._TOREVIEW.Entities;

// using App.Modules.Sys.Substrate.Models.Messages._TOREVIEW.Entities;
// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace App.Modules.Sys.Shared.Models.Messages._TOREVIEW.Entities.TenancySpecific
{
    /// <summary>
    /// The Security Profile of a <see cref="Principal"/>
    /// </summary>
    public class PrincipalSecurityProfile : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase, IHasKey
    {

        /// <summary>
        /// Whether the user is Enabled or not.
        /// <para>
        /// TODO: See if it should be here, or on parent Principal.
        /// And not both.
        /// </para>
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The unique key of this user (ie, the UserName).
        /// </summary>
        public string Key { get; set; } = string.Empty;


        /// <summary>
        /// Collection of RoleGroups.
        /// </summary>
        public ICollection<PrincipalSecurityProfileRoleGroup> AccountGroups
        {
            get => _accountGroups ??= [];
            set => _accountGroups = value;
        }
        
        private ICollection<PrincipalSecurityProfileRoleGroup>? _accountGroups;

        /// <summary>
        /// System Roles (not Group Roles)
        /// </summary>
        public ICollection<PrincipalSecurityProfileRole> Roles
        {
            get => _roles ??= [];
            set => _roles = value;
        }
        
        private ICollection<PrincipalSecurityProfileRole>? _roles;


        /// <summary>
        /// Assignment of Permissions
        /// </summary>
        public ICollection<PrincipalSecurityProfile_Permission_Assignment> PermissionsAssignments
        {
            get => _permissionsAssignments ??= [];
            set => _permissionsAssignments = value;
        }
        
        private ICollection<PrincipalSecurityProfile_Permission_Assignment>? _permissionsAssignments;



    }
}
