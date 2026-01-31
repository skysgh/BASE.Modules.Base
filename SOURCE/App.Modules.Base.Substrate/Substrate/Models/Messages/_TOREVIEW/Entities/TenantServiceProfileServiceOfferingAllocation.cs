// using System;
using App.Modules.Base.Substrate.Models.Entities.Base;
using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// The Complex Joint Object used to Assign ServiceOfferings to a TenantServiceIdentity, directly, 
    /// without going through a <c>ServicePlan</c>.
    /// <para>
    /// There's always an exception to any Plan 
    /// (beta, teaser, freebie to cover a stuffup, etc.)
    /// </para>
    /// </summary>
    public class TenantServiceProfileServiceOfferingAllocation : UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase
    {

        /// <summary>
        /// The Type of assignment, defining whether
        /// the Service is being added, or removed (ie
        /// maybe it's already part of a <c>ServicePlan</c>
        /// but it's being removed in this case, for some reason).
        /// </summary>
        public AssignmentType Type { get; set; }

        /// <summary>
        /// Price/Month
        /// <para>
        /// This billing only comes into effect if the
        /// Service is associated directly to a 
        /// <see cref="TenantServiceProfile"/>
        /// </para>
        /// </summary>
        public virtual decimal CostPerMonth { get; set; }

        /// <summary>
        /// Price/Year if a year is signed up for.
        /// <para>
        /// This billing only comes into effect if the
        /// Service is associated directly to a 
        /// <see cref="TenantServiceProfile"/>
        /// </para>
        /// </summary>
        public virtual decimal CostPerYear { get; set; }


        /// <summary>
        /// The FK of the <see cref="TenantServiceProfile"/>
        /// this <see cref="TenantServiceProfileServiceOfferingAllocation"/>
        /// belongs to.
        /// </summary>
        public Guid ServiceProfileFK { get; set; }
        //public TenantServiceProfile ServiceProfile {get;set;}

        /// <summary>
        /// The FK of the <see cref="ServiceOfferingDefinition"/>
        /// this <see cref="TenantServiceProfileServiceOfferingAllocation"/>
        /// is allocating to the owning
        /// <c>ServiceProfile</c>
        /// </summary>
        public Guid ServiceOfferingFK { get; set; }

        /// <summary>
        /// The single service offering.
        /// </summary>
        public ServiceOfferingDefinition ServiceOffering { get; set; }

    }

}
