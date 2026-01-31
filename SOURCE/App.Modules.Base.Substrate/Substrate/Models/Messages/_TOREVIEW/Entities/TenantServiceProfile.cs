using App.Modules.Base.Substrate.Models.Entities.Base;

// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
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
    public class TenantServiceProfile : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase
    {


        // Not needed: Already part of the base class:
        ////public virtual Guid TenantFK { get; set; }

        /// <summary>
        /// TODO: Improve documentation
        /// </summary>
        public virtual Tenant Tenant { get; set; }


        /// <summary>
        /// A <see cref="TenantServiceProfile"/> most often 
        /// will be signed up to one <c>ServiceProfile</c>.
        /// <para>
        /// But there are cases where the Tenancy is signed 
        /// up to multiple.
        /// </para>
        /// </summary>
        public virtual ICollection<TenantServiceProfileServicePlanAllocation> ServicePlans => _servicePlans ??= [];

        private ICollection<TenantServiceProfileServicePlanAllocation>? _servicePlans;


        /// <summary>
        /// The collection of direct Service Allocations.
        /// <para>
        /// Consider this as overrides of the default list of services 
        /// defined within the <see cref="ServicePlanDefinition"/>
        /// associated to this <see cref="TenantServiceProfile"/>.
        /// </para>
        /// </summary>
        public virtual ICollection<TenantServiceProfileServiceOfferingAllocation> Services => _services ??= [];

        private ICollection<TenantServiceProfileServiceOfferingAllocation>? _services;

    }

}
