using App.Modules.Base.Substrate.Models.Entities.Base;
// using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities;

// using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
{



    /// <summary>
    /// The Service Profile (ie Subscription)
    /// to which a <c>Plan</c> plan is associated.
    /// <para>
    /// We are not associating a <c>ServicePlan</c> 
    /// to a <see cref="Principal"/> directly
    /// to the 
    /// </para>
    /// </summary>
    public class PrincipalServiceProfile : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase
    {



        // Not needed: Already part of the base class:
        ////public virtual Guid TenantFK { get; set; }

        /// <summary>
        /// The Tenant this record belongs to.
        /// </summary>
        public virtual Tenant? Tenant { get; set; }


        /// <summary>
        /// A <see cref="PrincipalServiceProfile"/> most often 
        /// will be signed up to one <c>ServiceProfile</c>.
        /// <para>
        /// But there are cases where the Tenancy is signed 
        /// up to multiple.
        /// </para>
        /// </summary>
        public virtual ICollection<PrincipalServiceProfileServicePlanAllocation> ServicePlans => _servicePlans ??= [];

        private ICollection<PrincipalServiceProfileServicePlanAllocation>? _servicePlans;


        /// <summary>
        /// The collection of direct Service Allocations.
        /// <para>
        /// Consider this as overrides of the default list of services 
        /// defined within the <see cref="ServicePlanDefinition"/>
        /// associated to this <see cref="PrincipalServiceProfile"/>.
        /// </para>
        /// </summary>
        public virtual ICollection<PrincipalServiceProfileServiceOfferingAllocation> Services => _services
                    ??= [];

        private ICollection<PrincipalServiceProfileServiceOfferingAllocation>? _services;

    }

}
