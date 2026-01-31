// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;
using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{


    /// <summary>
    ///  System entity (not exposed to the system's exterior) for
    /// A Tenant is an Application Account.
    /// <para>
    /// A Tenant/ApplicationAccount has 1+ UserAccounts/Principals
    /// An ApplicationAccount/Tenant 
    /// </para>
    /// </summary>
    public class Tenant : UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase, IHasKey, IHasEnabled
    {

        /// <summary>
        /// Gets or sets whether the Account is enabled.
        /// </summary>
        public virtual bool Enabled { get; set; }


        ///// <summary>
        ///// The foreign key to the Subscription object
        ///// related to this Application Account/Tenancy.
        ///// <para>
        ///// The Subscription provides Access information (ie, Beginning/End dates, etc.)
        ///// to the logical Account/Tenancy.
        ///// </para>
        ///// </summary>
        //public Guid SubscriptionFK { get; set; }
        //public Subscription Subscription { get; set; }


        /// <summary>
        /// The key is unique, 
        /// and provides a human readable element 
        /// to paths.
        /// </summary>
        public virtual string Key { get; set; }


        /// <summary>
        ///     Only one Tenant can be marked as Default.
        /// </summary>
        public virtual bool? IsDefault { get; set; }

        /// <summary>
        ///     The hostname or key to match on.
        ///     <para>
        ///         Valid entries might be 'org1.service.tld' or 'org1.tld', or 'localhost:43311' (but I don't recommend the use
        ///         of ports)
        ///     </para>
        /// </summary>
        public virtual string HostName { get; set; }

        /// <summary>
        /// The name to display
        /// </summary>
        public virtual string DisplayName { get; set; }


        /// <summary>
        /// The FK of the Tenancy's DataClassification
        /// </summary>
        public virtual NZDataClassification? DataClassificationFK { get; set; }
        /// <summary>
        /// The Max <see cref="DataClassification"/> of the tenancy
        /// </summary>
        public virtual DataClassification DataClassification { get; set; }

        /// <summary>
        /// The properties of the tenancy.
        /// </summary>
        public virtual ICollection<TenantProperty> Properties
        {
            get => _properties ??= [];
            set => _properties = value;
        }
        private ICollection<TenantProperty>? _properties;

        /// <summary>
        /// The claims about the Tenancy.
        /// </summary>
        public virtual ICollection<TenantClaim> Claims
        {
            get
            {
                _claims ??= [];// new Collection<TenantClaim>();
                return _claims;
            }
            set => _claims = value;
        }
        private ICollection<TenantClaim>? _claims;

    }
}