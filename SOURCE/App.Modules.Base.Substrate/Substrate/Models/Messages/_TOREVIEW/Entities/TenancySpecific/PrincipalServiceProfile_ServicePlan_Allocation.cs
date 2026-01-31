// using System;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Models.Entities.Base;
// using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities;

// using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
{
    /// <summary>
    /// A Complex Joint Object between a <see cref="PrincipalServiceProfile"/>
    /// and <see cref="ServicePlanDefinition"/>s.
    /// <para>
    /// Think of a <see cref="PrincipalServiceProfileServicePlanAllocation"/> as an individual 
    /// Instance of a global <see cref="ServicePlanDefinition"/>.
    /// </para>
    /// <para>
    /// Plans can be 
    /// * Membership based (monthly: $cost * $interval, year: $cost * $interval * 10/12 year discount)
    /// * Plans (with more or less features. As shown below).
    /// * Limited Supply (shown below using MaxResources)
    /// * BUild your own plan (not covered here).
    /// * Addons (add Addon Services to existing plan).
    /// * Pay as you go. (Not covered)
    /// * Resource Utilization (not covered)
    /// 
    /// </para>
    /// </summary>
    public class PrincipalServiceProfileServicePlanAllocation : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase, IHasEnabled, IHasParentFK
    {


        /// <summary>
        /// An allocation can be disabled (eg: Payment not received).
        /// </summary>
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// Date (UTC) as to when Plan comes into effect.
        /// <para>
        /// Most of the time, the Start date is now (null).
        /// </para>
        /// </summary>
        public virtual DateTimeOffset? Start { get; set; }


        /// <summary>
        /// Reference to the parent <see cref="PrincipalServiceProfile"/>
        /// (no updward Navigation required).
        /// </summary>
        public virtual Guid ParentFK { get; set; }

        /// <summary>
        /// The FK of the child <see cref="ServicePlanDefinition"/> associated to the 
        /// parent <see cref="PrincipalServiceProfile"/>.
        /// </summary>
        public virtual Guid ServicePlanFK { get; set; }

        /// <summary>
        /// The <see cref="ServicePlanDefinition"/> associated to the 
        /// parent <see cref="PrincipalServiceProfile"/>.
        /// </summary>
        public virtual ServicePlanDefinition? ServicePlan { get; set; }


        /// <summary>
        /// The number of <see cref="ServicePlanDefinition"/>'s.
        /// <para>
        /// Most times a <see cref="PrincipalServiceProfile"/> 
        /// will only be allocated one <see cref="PrincipalServiceProfileServicePlanAllocation"/>.
        /// Although you can 
        /// * Assign multiple instances of a single <see cref="PrincipalServiceProfileServicePlanAllocation"/>,
        /// * assign a collection of <see cref="PrincipalServiceProfileServicePlanAllocation"/>
        /// * or a mix, if you really want to go nuts.
        /// </para>
        /// </summary>
        public virtual int ServicePlanQuantity { get; set; }








        /// <summary>
        /// Constructor of the Joint object
        /// </summary>
        public PrincipalServiceProfileServicePlanAllocation()
        {
            Enabled = true;
            ServicePlanQuantity = 1;
        }
    }

}
