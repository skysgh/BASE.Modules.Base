// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace App.Modules.Base.Substrate.Infrastructure.Outcomes
{
    /// <summary>
    /// TODO: Define
    /// </summary>
    public enum SystemInUseQualities
    {

        /// <summary>
        /// No value set.
        /// </summary>
        Undefined,

        /// <summary>
        /// Set, but Unknown
        /// </summary>
        Unknown,


        /// <summary>
        /// accuracy and completeness with which users achieve specified goals
        /// </summary>
        ISO_0000_Effectiveness,

        /// <summary>
        /// resources expended in relation to the accuracy and completeness with which users achieve goals
        /// </summary>
        ISO_0000_Efficiency,

        /// <summary>
        /// degree to which user needs are satisfied when a product or system is used in a specified context of use
        /// </summary>
        ISO_0000_Satisfaction,

        /// <summary>
        /// degree to which a user is satisfied with their perceived achievement of pragmatic goals, including the results of use and the consequences of use
        /// </summary>
        ISO_0000_Satisfaction_Usefulness,

        /// <summary>
        /// degree to which a user or other stakeholder has confidence that a product or system will behave as intended
        /// </summary>
        ISO_0000_Satisfaction_Trust,

        /// <summary>
        /// degree to which a user obtains pleasure from fulfilling their personal needs
        /// </summary>
        ISO_0000_Satisfaction_Pleasure,

        /// <summary>
        /// degree to which the user is satisfied with physical comfort
        /// </summary>
        ISO_0000_Satisfaction_Comfort,

        /// <summary>
        /// degree to which a product or system mitigates the potential risk to economic status, human life, health, or the environment
        /// </summary>
        ISO_0000_FreedomFromRisk,

        /// <summary>
        /// degree to which a product or system mitigates the potential risk to economic status, human life, health, or the environment
        /// </summary>
        ISO_0000_FreedomFromRisk_EconomicRiskMitigation,

        /// <summary>
        /// degree to which a product or system mitigates the potential risk to people in the intended contexts of use
        /// </summary>
        ISO_0000_FreedomFromRisk_HealthAndSafetyRiskMitigation,

        /// <summary>
        /// degree to which a product or system mitigates the potential risk to property or the environment in the intended contexts of use
        /// </summary>
        ISO_0000_FreedomFromRisk_EnvironmentalRiskMitigation,

        /// <summary>
        /// degree to which a product or system can be used with effectiveness, efficiency, freedom from risk and satisfaction in both specified contexts of use and in contexts beyond those initially explicitly identified
        /// </summary>
        /// <remarks>
        /// Note 1 to entry: Context of use is relevant to both quality in use and some product quality (sub)characteristics (where it is referred to as “specified conditions”).
        /// </remarks>
        ISO_0000_ContextCoverage,

        /// <summary>
        /// degree to which a product or system can be used with effectiveness, efficiency, freedom from risk and satisfaction in all the specified contexts of use
        /// </summary>
        /// <remarks>
        /// Note 1 to entry: Context completeness can be specified or measured either as the degree to which a product can be used by specified users to achieve specified goals with effectiveness, efficiency, freedom from risk and satisfaction in all the intended contexts of use, or by the presence of product properties that support use in all the intended contexts of use.
        /// EXAMPLE:The extent to which software is usable using a small screen, with low network bandwidth, by a non-expert user; and in a fault-tolerant mode(e.g.no network connectivity).
        /// </remarks>
        ISO_0000_ContextCoverage_ContextCompleteness,

        /// <summary>
        /// degree to which a product or system can be used with effectiveness, efficiency, freedom from risk and satisfaction in contexts beyond those initially specified in the requirements
        /// </summary>
        /// <remarks>
        /// Note 1 to entry: Flexibility can be achieved by adapting a product (see 4.2.8.1) for additional user groups, tasks and cultures.
        /// Note 2 to entry: Flexibility enables products to take account of circumstances, opportunities and individual preferences that had not been anticipated in advance.
        /// Note 3 to entry: If a product is not designed for flexibility, it might not be safe to use the product in unintended contexts.
        /// Note 4 to entry: Flexibility can be measured either as the extent to which a product can be used by additional types of users to achieve additional types of goals with effectiveness, efficiency, freedom from risk and satisfaction in additional types of contexts of use, or by a capability to be modified to support adaptation for new types of users, tasks and environments, and suitability for individualization as defined in ISO 9241-110.
        /// </remarks>
        ISO_0000_ContextCoverage_Flexibility,
    }

}