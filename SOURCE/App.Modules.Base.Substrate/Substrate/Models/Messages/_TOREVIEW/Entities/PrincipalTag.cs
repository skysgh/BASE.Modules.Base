using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// System entity (not exposed to the system's exterior) for
    /// a Tag associated to a <see cref="Principal"/>
    /// </summary>
    public class PrincipalTag : UntenantedAuditedRecordStatedTimestampedGuidIdReferenceDataEntityBase
    {
    }
}