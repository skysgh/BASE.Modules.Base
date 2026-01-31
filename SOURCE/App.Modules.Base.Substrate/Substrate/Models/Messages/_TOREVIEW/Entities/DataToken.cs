using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// System entity (not exposed to the system's exterior) for
    ///     A data token 
    ///     which ensures the data is *not*
    ///     in the actual table, so if it is ever
    ///     inadvertently leaked, the data is basically
    ///     useless.
    /// </summary>
    public class DataToken : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase
    {
        /// <summary>
        /// The value of the token.
        /// </summary>
        public virtual string Value { get; set; }
    }
}