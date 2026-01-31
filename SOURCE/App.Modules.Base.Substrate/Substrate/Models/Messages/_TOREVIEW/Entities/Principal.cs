// using System.Collections.ObjectModel;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;
using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// System entity (not exposed to the system's exterior) for
    /// The Principal within the System.
    /// <para>
    /// On screen or elsewhere, it's common to refer to this type of entity as a User.
    /// </para>
    /// <para>
    /// But it's *not* called User because a security Principal can be a User, but also a Device or Service.
    /// </para>
    /// </summary>
    public class Principal : UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase, IHasEnabled
    {
        /// <summary>
        /// The UTC DateTime at which the User is enabled.
        /// <para>
        /// (eg: the date at which their contract starts)
        /// </para>
        /// </summary>
        public DateTime? EnabledBeginningUtc { get; set; }
        /// <summary>
        /// The UTC DateTime up to which the User/Principal is enabled to use the system
        /// <para>
        /// (eg: up to their contractor contract's end date).
        /// </para>
        /// </summary>
        public DateTime? EnabledEndingUtc { get; set; }
        /// <summary>
        /// Enabled or not.
        /// <para>
        /// Note that this supercedes their use date start/end
        /// </para>
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// The name of the user.
        /// <para>
        /// TODO: Not good here. 
        /// Move to an associated Person identity
        /// so that this table has the least PI as possible.
        /// </para>
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// This is the Principal's displayed preferred Name 
        /// which they can set (it starts off as being equal
        /// to their <see cref="FullName"/>.
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// The FK to the <see cref="DataClassification"/> record of the person.
        /// </summary>
        public virtual NZDataClassification? DataClassificationFK { get; set; }
        /// <summary>
        /// The Data Classification record of the person.
        /// </summary>
        public virtual DataClassification? DataClassification { get; set; }

        /// <summary>
        /// The <see cref="PrincipalCategory"/> of the Principal.
        /// </summary>
        public virtual PrincipalCategory? Category { get; set; }
        /// <summary>
        /// The FK of the <see cref="Principal"/>'s <see cref="PrincipalCategory"/>.
        /// </summary>
        public virtual Guid CategoryFK { get; set; }

        /// <summary>
        /// The collection of externally defined (ie, from an IdP) 
        /// Digital Identities of the <see cref="Principal"/>.
        /// </summary>
        public virtual ICollection<PrincipalLogin> Logins
        {
            get
            {
                _logins ??= [];
                return _logins;
            }
            set => _logins = value;
        }
        private ICollection<PrincipalLogin>? _logins;


        /// <summary>
        /// The collection of <see cref="PrincipalTag"/>'s
        /// associated to the <see cref="Principal"/>.
        /// </summary>
        public virtual ICollection<PrincipalTag> Tags
        {
            get
            {
                _tags ??= [];
                return _tags;
            }
            set => _tags = value;
        }
        private ICollection<PrincipalTag>? _tags;




        /// <summary>
        /// The collection of <see cref="PrincipalProperty"/>
        /// that belong to the <see cref="Principal"/>.
        /// </summary>
        public virtual ICollection<PrincipalProperty> Properties
        {
            get
            {
                _properties ??= [];
                return _properties;
            }
            set => _properties = value;
        }
        private ICollection<PrincipalProperty>? _properties;

        /// <summary>
        /// The collection of the <see cref="Principal"/>'s
        /// <see cref="PrincipalClaim"/>s
        /// (properties verified by a trusted 3rd party).
        /// </summary>
        public virtual ICollection<PrincipalClaim> Claims
        {
            get
            {
                _claims ??= [];
                return _claims;
            }
            set => _claims = value;
        }
        private ICollection<PrincipalClaim>? _claims;





        /// <summary>
        /// The System Roles (not Group Roles) 
        /// associated to the <see cref="Principal"/>.
        /// </summary>
        public virtual ICollection<SystemRole> Roles
        {
            get
            {
                _roles ??= [];
                return _roles;
            }
            set => _roles = value;
        }
        private ICollection<SystemRole>? _roles;

    }
}