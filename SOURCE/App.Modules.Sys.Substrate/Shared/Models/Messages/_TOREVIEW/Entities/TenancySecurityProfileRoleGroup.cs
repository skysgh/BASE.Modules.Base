using App.Modules.Sys.Shared.Models.Entities.Base;
using App.Modules.Sys.Shared.Models.Persistence;



// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;

namespace App.Modules.Sys.Shared.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// A nestable Group of Roles,
    /// attachable to a Tenancy Security Profile.
    /// <para>
    /// TODO: Document purpose better
    /// </para>
    /// </summary>
    public class TenancySecurityProfileRoleGroup :
        TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasTitle,
        IHasTitleAndDescription,
        IHasParentFKNullable,
        IHasParentNullable<TenancySecurityProfileRoleGroup>
    {
        /// <summary>
        /// Get/Set the title 
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Get/Set the description
        /// </summary>
         public string Description { get; set; } = string.Empty;


        /// <summary>
        /// Nullable FK of the parent Role Group.
        /// </summary>
        public Guid? ParentFK { get; set; }

        /// <summary>
        /// The parent Role Group.
        /// </summary>
        public TenancySecurityProfileRoleGroup? Parent { get; set; }

        /// <summary>
        /// The collection of child Role Groups
        /// </summary>
        public ICollection<TenancySecurityProfileRoleGroup> AccountGroups
        {
            get => _accountGroups ??= [];
            set => _accountGroups = value;
        }
        
        private ICollection<TenancySecurityProfileRoleGroup>? _accountGroups;


        /// <summary>
        /// THe collection of Roles assigned to this Role Group
        /// </summary>
        public ICollection<TenancySecurityProfileAccountRole> Roles
        {
            get => _roles ??= [];
            set => _roles = value;
        }
        
        private ICollection<TenancySecurityProfileAccountRole>? _roles;

        //TODO: Could get large. Do we want this? Maybe it should only be on Account.
        //public ICollection<Account> Accounts { get; set; } 

    }

}

