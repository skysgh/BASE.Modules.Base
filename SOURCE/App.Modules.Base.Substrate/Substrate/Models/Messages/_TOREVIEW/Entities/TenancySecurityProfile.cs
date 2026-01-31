// using System.Collections.ObjectModel;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;

// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    ///  System entity (not exposed to the system's exterior) for
    /// the Security Profile of a User in a specific Tenancy.
    /// <para>
    /// TODO: Confirm documentation accuracy.
    /// </para>
    /// </summary>
    public class TenancySecurityProfile : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase, IHasKey
    {

        /// <summary>
        /// Whether User is Enabled in this Tenancy.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The unique key of this user (ie, the UserName).
        /// </summary>
        public string Key { get; set; }


        /// <summary>
        /// Collection of <see cref="TenancySecurityProfileRoleGroup"/>
        /// describing TODO.
        /// </summary>
        public ICollection<TenancySecurityProfileRoleGroup> AccountGroups
        {
            get => _accountGroups ??= [];
            set => _accountGroups = value;
        }
        /// <summary>
        /// TODO: Is Public required?
        /// </summary>
        public ICollection<TenancySecurityProfileRoleGroup>? _accountGroups;


        /// <summary>
        /// Collection of <see cref="TenancySecurityProfileAccountRole"/>
        /// describing TODO.
        /// </summary>
        public ICollection<TenancySecurityProfileAccountRole> Roles
        {
            get => _roles ??= [];
            set => _roles = value;
        }
        /// <summary>
        /// TODO: Is this Public needed?
        /// </summary>
        public ICollection<TenancySecurityProfileAccountRole>? _roles;


        /// <summary>
        /// Collection of <see cref="TenancySecurityProfileTenancySecurityProfilePermissionAssignment"/>
        /// describing TODO.
        /// </summary>
        public ICollection<TenancySecurityProfileTenancySecurityProfilePermissionAssignment> PermissionsAssignments
        {
            get => _permissionsAssignments ??= [];
            set => _permissionsAssignments = value;
        }
        /// <summary>
        /// TODO: Is this public needed?
        /// </summary>
        public ICollection<TenancySecurityProfileTenancySecurityProfilePermissionAssignment>? _permissionsAssignments;



    }
}
