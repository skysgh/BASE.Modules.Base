using App.Modules.Base.Substrate.Models.Entities.Base;
using App.Modules.Base.Substrate.Attributes;

namespace App.Modules.Base.Substrate.Models.Entities.Demos
{
    /// <summary>
    /// A demonstration of a Reference Type Entity
    /// used to describe an instance of
    /// <see cref="ExampleBEntity"/>
    /// </summary>
    [ForDemoOnly]
    public class ExampleBReferenceTypeEntity :
        UntenantedAuditedRecordStatedTimestampedGuidIdReferenceDataEntityBase
    {

    }
}
