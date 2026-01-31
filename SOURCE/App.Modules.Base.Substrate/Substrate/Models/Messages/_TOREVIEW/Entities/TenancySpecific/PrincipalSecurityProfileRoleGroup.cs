// using System.Collections.ObjectModel;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Models.Entities.Base;

// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
{
    /// <summary>
    /// A Group of Roles
    /// </summary>
    public class PrincipalSecurityProfileRoleGroup
        : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasTitle,
        IHasTitleAndDescription,
        IHasParentFKNullable,
        IHasParentNullable<PrincipalSecurityProfileRoleGroup>
    {
        /// <summary>
        /// Get/Set the Title of the Group
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Get/Set the Description of the Group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The FK of the parent Role Group (they're nested)
        /// </summary>
        public Guid? ParentFK { get; set; }
        /// <summary>
        /// The parent Role Group (they're nested)
        /// </summary>
        public PrincipalSecurityProfileRoleGroup? Parent { get; set; }

        /// <summary>
        /// Collection of child Role Groups.
        /// </summary>
        public ICollection<PrincipalSecurityProfileRoleGroup> AccountGroups
        {
            get => _accountGroups ??= [];
            set => _accountGroups = value;
        }
        /// <summary>
        /// TODO: Why public?
        /// </summary>
        public ICollection<PrincipalSecurityProfileRoleGroup>? _accountGroups;


        /// <summary>
        /// Collection of Roles in this Group.
        /// </summary>
        public ICollection<PrincipalSecurityProfileRole> Roles
        {
            get => _roles ??= [];
            set => _roles = value;
        }

        /// <summary>
        /// TODO: Why public?
        /// </summary>
        public ICollection<PrincipalSecurityProfileRole>? _roles;

        //TODO: Could get large. Do we want this? Maybe it should only be on Account.
        //public ICollection<Account> Accounts { get; set; } 

    }

}

