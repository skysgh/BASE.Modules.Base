//namespace App.Modules.Base.Substrate.Models.Entities.Base
//{
//    public abstract class TenantedAuditedTimestampedRecordStatedCustomIdEntityBase<TId> : TenantFKTimestampedAuditedRecordStatedCustomIdEntityBase<TId>,
//        IHasEnabled,
//        IHasText,
//        IHasDisplayOrderHint
//    {


//        /// <summary>
//        /// Gets or sets a value indicating whether this <see cref="TenantedTimestampedAuditedRecordStatedCustomIdEntityBase{TId}"/> is enabled.
//        /// </summary>
//        /// <value>
//        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
//        /// </value>
//        public virtual  bool Enabled { get; set; }

//        /// <summary>
//        /// Gets or sets the text.
//        /// </summary>
//        /// <value>
//        /// The text.
//        /// </value>
//        public virtual string Text { get; set; }

//        /// <summary>
//        /// Gets or sets the display order hint.
//        /// </summary>
//        /// <value>
//        /// The display order hint.
//        /// </value>
//        public virtual int DisplayOrderHint { get; set; }

//        // A convention based hint as to display (could be a class name, or icon, CSV or both, DSL, etc.)
//        public virtual string DisplayStyleHint { get; set; }
//    }
//}