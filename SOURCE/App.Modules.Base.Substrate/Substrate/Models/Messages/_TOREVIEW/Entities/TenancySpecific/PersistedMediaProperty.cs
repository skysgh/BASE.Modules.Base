// using System;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
{
    /// <summary>
    ///     Optional attributes to associate to the
    ///     <see cref="PersistedMedia" />
    /// </summary>
    public class PersistedMediaProperty :
        TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasOwnerFK,
        IHasKeyGenericValue<string>,
        IHasTagNullable
    {
        /// <summary>
        ///     Gets or sets the key.
        ///     <para>Member defined in<see cref="IHasKey" /></para>
        /// </summary>
        public virtual string Key { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public virtual string Value { get; set; } = string.Empty;


        /// <summary>
        ///     Gets or sets the Id of the
        ///     <see cref="PersistedMedia" />
        ///     that manages a collection of these
        ///     <see cref="PersistedMedia" />
        ///     attributes.
        /// </summary>
        public virtual Guid PersistedMediaFK { get; set; }

        /// <summary>
        ///     Gets the tag of the object.
        ///     <para>
        ///     Member defined in <c>XAct.IHasTag</c>
        ///     </para>
        ///     <para>Can be used to associate information -- such as an image ref -- to a SelectableItem.</para>
        /// </summary>
        public virtual string? Tag { get; set; }

        /// <summary>
        /// Get/Set the FK of the parent <see cref="PersistedMedia"/>
        /// </summary>
        /// <returns></returns>
        public Guid GetOwnerFk()
        {
            return PersistedMediaFK;
        }
    }
}