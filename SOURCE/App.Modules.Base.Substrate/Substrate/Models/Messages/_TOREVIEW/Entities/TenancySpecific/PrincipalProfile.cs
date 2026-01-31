// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;
// using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities;
using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums;



// using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
{
    /// <summary>
    /// The profile (not same as Security Profile) of a Principal.
    /// </summary>
    public class PrincipalProfile : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase, IHasEnabled
    {

        /// <summary>
        /// Get/Set from when the Principal is enabled.
        /// </summary>
        public DateTime? EnabledBeginningUtc { get; set; }

        /// <summary>
        /// Get/Set until when the Principal is enabled (eg: Contract)
        /// </summary>
        public DateTime? EnabledEndingUtc { get; set; }

        /// <summary>
        /// Is the Principal Enabled.
        /// </summary>
        public virtual bool Enabled { get; set; }


        /// <summary>
        /// Display (user Modifiable) name of Principal
        /// </summary>
        public virtual string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Get/Set the FK of the Data Classification of the Principal.
        /// </summary>
        public virtual NZDataClassification? DataClassificationFK { get; set; }
        /// <summary>
        /// Get/Set the Data Classification of the Principal.
        /// </summary>
        public virtual DataClassification? DataClassification { get; set; }

        /// <summary>
        /// Get/Set the Category of Principal
        /// </summary>
        public virtual PrincipalProfileCategory? Category { get; set; }
        /// <summary>
        /// Get/Set the FK of the Category of the Principal
        /// </summary>
        public virtual Guid CategoryFK { get; set; }


        /// <summary>
        /// Get the Collection of Tags associated.
        /// </summary>
        public virtual ICollection<PrincipalProfileTag> Tags
        {
            get
            {
                _tags ??= [];// new Collection<PrincipalProfileTag>();
                return _tags;
            }
            set => _tags = value;
        }
        private ICollection<PrincipalProfileTag>? _tags;




        /// <summary>
        /// Get the collection of properties of this Profile.
        /// </summary>
        public virtual ICollection<PrincipalProfileProperty> Properties
        {
            get
            {
                _properties ??= [];// new Collection<PrincipalProfileProperty>();
                return _properties;
            }
            set => _properties = value;
        }
        private ICollection<PrincipalProfileProperty>? _properties;

        /// <summary>
        /// Get the Claims associated to this <see cref="Principal"/>
        /// </summary>
        public virtual ICollection<PrincipalProfileClaim> Claims
        {
            get
            {
                _claims ??= [];// new Collection<PrincipalProfileClaim>();
                return _claims;
            }
            set => _claims = value;
        }
        private ICollection<PrincipalProfileClaim>? _claims;





    }
}