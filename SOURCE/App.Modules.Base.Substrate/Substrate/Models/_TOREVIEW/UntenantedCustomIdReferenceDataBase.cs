//namespace App.Modules.Base.Substrate.Models.Entities.Base
//{
//    public abstract class UntenantedCustomIdReferenceDataBase<TId> : UntenantedTimestampedAuditedRecordStatedCustomIdEntityBase<TId>, IHasEnabled, IHasDisplayOrderHint
//    {



//        public virtual bool Enabled { get; set; }

//        public virtual string Text { get; set; }

//        /// <summary>
//        /// Higher numbers are displayed at top.
//        /// </summary>
//        public virtual int DisplayOrderHint { get; set; }

//        // A convention based hint as to display (could be a class name, or icon, CSV or both, DSL, etc.)
//        public virtual string DisplayStyleHint { get; set; }

//        protected UntenantedCustomIdReferenceDataBase()
//        {
//            //REMEMBER: ID MUST BE PROVIDED IN THIS CASE...
//        }
//    }
//}