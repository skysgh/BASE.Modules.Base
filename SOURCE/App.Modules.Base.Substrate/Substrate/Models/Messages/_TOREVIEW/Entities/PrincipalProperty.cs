using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// System entity (not exposed to the system's exterior) for
    /// a property of the parent <see cref="Principal"/>
    /// </summary>
    public class PrincipalProperty
        : UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasOwnerFK
    {
        /// <summary>
        /// THe unique key of the property.
        /// </summary>
        public virtual string Key { get; set; }
        /// <summary>
        /// The value of the property.
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// The FK of the parent <see cref="Principal"/>
        /// </summary>
        public virtual Guid PrincipalFK { get; set; }


        /// <summary>
        /// Return the Parent <see cref="Principal"/>'s FK.
        /// </summary>
        /// <returns></returns>
        public Guid GetOwnerFk()
        {
            return PrincipalFK;
        }
    }
}