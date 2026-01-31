using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;

// using System.Collections.ObjectModel;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    ///  System entity (not exposed to the system's exterior) for
    /// A Plan Definition
    /// ('Free', 'Small' etc.) made up of zero or more
    /// <see cref="ServiceDefinition"/>s.
    /// A Tenancy can be associated to one or more 
    /// <see cref="TenantServiceProfileServicePlanAllocation"/>.
    /// <para>
    /// Think of the <see cref="ServicePlanDefinition"/> as the global
    /// definition, whereas a <see cref="TenantServiceProfileServicePlanAllocation"/>
    /// is a single instance of the <see cref="ServicePlanDefinition"/>,
    /// specific to a single <see cref="TenantServiceProfile"/>
    /// </para>
    /// </summary>
    public class ServicePlanDefinition : UntenantedAuditedRecordStatedTimestampedGuidIdReferenceDataEntityBase, IHasKey
    {
        /// <summary>
        /// The (indexed) unique Key for this ServicePlan
        /// </summary>
        public virtual string Key { get; set; }


        /// <summary>
        /// The max number of Principals allowed in this Service plan.
        /// </summary>
        public int PrincipalLimit { get; set; }

        /// <summary>
        /// Price/Month
        /// <para>
        /// Note that billing every month leads to more
        /// cancellations than year billing.
        /// And come with 12 bank charges.
        /// </para>
        /// </summary>
        public virtual decimal CostPerMonth { get; set; }

        /// <summary>
        /// Price/Year if a year is signed up for.
        /// <para>
        /// Note that although year signups create cash boosts,
        /// they come with other issues: they hide disatisfaction
        /// with the service... as well as come with 
        /// Reversal charges (then again
        /// all Cancellations do).
        /// </para>
        /// </summary>
        public virtual decimal CostPerYear { get; set; }

        /// <summary>
        /// The collection of Services that are part of this 
        /// Plan ('Free', 'Small', 'Enterprise', etc.)
        /// </summary>
        public virtual ICollection<ServiceOfferingDefinition> ServiceAllocations => _services ??= [];

        private ICollection<ServiceOfferingDefinition>? _services;


    }

}
