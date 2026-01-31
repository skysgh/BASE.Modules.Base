using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;
// using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities;

// using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities;
// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
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
            get => _accountGroups ??= [];// new Collection<PrincipalSecurityProfileRoleGroup>();
            set => _accountGroups = value;
        }
        /// <summary>
        /// TODO: Why public
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ICollection<PrincipalSecurityProfileRoleGroup> _accountGroups;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// System Roles (not Group Roles)
        /// </summary>
        public ICollection<PrincipalSecurityProfileRole> Roles
        {
            get => _roles ??= [];//new Collection<PrincipalSecurityProfileRole>();
            set => _roles = value;
        }

        /// <summary>
        /// TODO: Why public
        /// </summary>
        public ICollection<PrincipalSecurityProfileRole>? _roles;


        /// <summary>
        /// Assignment of Permissions
        /// </summary>
        public ICollection<PrincipalSecurityProfile_Permission_Assignment> PermissionsAssignments
        {
            get => _permissionsAssignments ??= [];//new Collection<PrincipalSecurityProfile_Permission_Assignment>();
            set => _permissionsAssignments = value;
        }
        /// TODO: Why public
        public ICollection<PrincipalSecurityProfile_Permission_Assignment>? _permissionsAssignments;



    }
}
