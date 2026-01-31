using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    ///// <summary>
    ///// The optional limits associated to a
    ///// <see cref="ServiceOfferingDefinition"/>.
    ///// If no Limits found in either category, System Logic is to assume nothing can be developed
    ///// (ie, you always need an upper limit, even if it is equal to decimal.Max.
    ///// <para>
    ///// I'm going to wonder for a while whether the effort of creating
    ///// an external limit was worth it...
    ///// </para>
    ///// </summary>
    //public class ServiceOfferingDefinitionLimit : UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase
    //{
    //    public virtual int Type { get; set; }
    //    /// <summary>
    //    /// The maximum number of Principal/Users that can use be associated to this Service,
    //    /// or Resources be developed as part of this Tenancy.
    //    /// </summary>
    //    public virtual int Max { get; set; }
    //}

    ///// <summary>
    ///// The enumeration of the 
    ///// <see cref="ServiceOfferingDefinitionLimit"/>
    ///// Type.
    ///// </summary>
    //public enum ServiceOfferingDefinitionLimitType
    //{
    //    Undefined=0,
    //    Principal=1,
    //    Resource=2
    //}

    ///// <summary>
    ///// This is never used as the underlying Many2Many Join object
    ///// is not made visible by the ORM, and there is no need to attach Properties to it.
    ///// </summary>
    //public class ServiceOfferingDefinition_To_ServiceOffering { }


    /// <summary>
    /// The individual Services offered on this platform.
    /// Makes up the Service Catalogue.
    /// </summary>
    public class ServiceDefinition : UntenantedAuditedRecordStatedTimestampedGuidIdReferenceDataEntityBase, IHasKey
    {

        //There isn't much that can be said about a service.

        /// <summary>
        /// The unique Key for this Service Definition.
        /// </summary>
        public virtual string Key { get; set; } = string.Empty;

        //The underlying fields provide a means to render a Title, Description  and Icon.

        // Note that this Definition class does not 
        // define any constraints/limits. 
        // That's defined within 
        // ServicePlanServiceOfferingDefinition

    }

}
