namespace App.Modules.Base.Substrate.Infrastructure.Outcomes
{
    /// <summary>
    /// The quality of a system is the degree to which the system satisfies the stated and implied needs of its various stakeholders, and thus provides value. 
    /// </summary>
    public enum SystemQualities
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
        /// This characteristic represents the degree to which a product or system provides functions that meet stated and implied needs when used under specified conditions. 
        /// </summary>
        ISO_25010_FunctionalitySuitability,
        /// <summary>
        ///  Degree to which the set of functions covers all the specified tasks and intended users' objectives.
        /// </summary>
        ISO_25010_FunctionalitySuitability_FunctionalCompleteness,

        /// <summary>
        /// Degree to which a product or system provides accurate results when used by intended users.
        /// </summary>
        ISO_25010_FunctionalitySuitability_FunctionalCorrectness,

        /// <summary>
        /// Degree to which the functions facilitate the accomplishment of specified tasks and objectives.
        /// </summary>
        ISO_25010_FunctionalitySuitability_Appropriateness,

        /// <summary>
        /// This characteristic represents the degree to which a product performs its functions within specified time and throughput parameters and is efficient in the use of resources (such as CPU, memory, storage, network devices, energy, materials...) under specified conditions.
        /// </summary>
        ISO_25010_PerformanceEfficiency,

        /// <summary>
        /// Degree to which the response time and throughput rates of a product or system, when performing its functions, meet requirements.
        /// </summary>
        ISO_25010_PerformanceEfficiency_TimeBehaviour,

        /// <summary>
        /// Degree to which the amounts and types of resources used by a product or system, when performing its functions, meet requirements.
        /// </summary>
        ISO_25010_PerformanceEfficiency_ResourceUtilisation,

        /// <summary>
        /// Degree to which the maximum limits of a product or system parameter meet requirements.
        /// </summary>
        ISO_25010_PerformanceEfficiency_Capacity,

        /// <summary>
        /// Degree to which a product, system or component can exchange information with other products, systems or components, and/or perform its required functions while sharing the same common environment and resources
        /// </summary>
        ISO_25010_Compatibility,

        /// <summary>
        /// Degree to which a product can perform its required functions efficiently while sharing a common environment and resources with other products, without detrimental impact on any other product.
        /// </summary>
        ISO_25010_Compatibility_CoExistence,

        /// <summary>
        /// Degree to which a system, product or component can exchange information with other products and mutually use the information that has been exchanged.
        /// </summary>
        ISO_25010_Compatibility_Interoperability,

        /// <summary>
        /// Retired. See ISO_25010_InteractionCapability.
        /// </summary>
        ISO_25010_Usability,

        /// <summary>
        /// Degree to which a product or system can be interacted with by specified users to exchange information ia the user interfaceto complete specific tasks in a variety of contexts of use. 
        /// </summary>
        ISO_25010_InteractionCapability,

        /// <summary>
        /// Degree to which users can recognize whether a product or system is appropriate for their needs.
        /// </summary>
        ISO_25010_InteractionCapability_AppropriatenessRecognisability,

        /// <summary>
        /// Degree to which the functions of a product or system can be learnt to be used by specified users within a specified amount of time.
        /// </summary>
        ISO_25010_InteractionCapability_Learnability,

        /// <summary>
        /// Degree to which a product or system has attributes that make it easy to operate and control.
        /// </summary>
        ISO_25010_InteractionCapability_Operability,

        /// <summary>
        /// Degree to which a system prevents users against operation errors.
        /// </summary>
        ISO_25010_InteractionCapability_UserErrorProtection,


        /// <summary>
        /// Degree to which a user interface presents functions and information in an inviting and motivating manner encouraging continued interaction.
        /// </summary>
        ISO_25010_InteractionCapability_UserEngagement,

        /// <summary>
        /// Degree to which a product or system can be used by people of various backgrounds (such as people of various ages, abilities, cultures, ethnicities, languages, genders, economic situations, etc.).
        /// </summary>
        ISO_25010_InteractionCapability_Inclusivity,

        /// <summary>
        /// Degree to which a product can be used by people with the widest range of characteristics and capabilities to achieve specified goals in a specified context of use.
        /// </summary>
        ISO_25010_InteractionCapability_UserAssistance,

        /// <summary>
        /// Degree to which a product presents appropriate information, where needed by the user, to make its capabilities and use immediately obvious to the user without excessive interactions with a product or other resources (such as user documentation, help desks or other users).
        /// </summary>
        ISO_25010_InteractionCapability_SelfDescriptivness,

        /// <summary>
        /// Retired.
        /// </summary>
        ISO_25010_Usability_UserInterfaceAesthetics,

        /// <summary>
        /// Retired. See ISO_InteractionCapability_Inclusivity.
        /// </summary>
        ISO_25010_Usability_Accessibility,

        /// <summary>
        /// Degree to which a system, product or component performs specified functions under specified conditions for a specified period of time.
        /// </summary>
        ISO_25010_Reliability,

        /// <summary>
        /// Retired.
        /// </summary>
        ISO_25010_Reliability_Maturity,

        /// <summary>
        /// Degree to which a system, product or component is operational and accessible when required for use.
        /// </summary>
        ISO_25010_Reliability_Availability,

        /// <summary>
        /// Degree to which a system, product or component operates as intended despite the presence of hardware or software faults.
        /// </summary>
        ISO_25010_Reliability_FaultTolerance,

        /// <summary>
        /// Degree to which a system, product or component performs specified functions without fault under normal operation.
        /// </summary>
        ISO_25010_Reliability_Faultlessness,

        /// <summary>
        /// Degree to which, in the event of an interruption or a failure, a product or system can recover the data directly affected and re-establish the desired state of the system.
        /// </summary>
        ISO_25010_Reliability_Recoverability,

        /// <summary>
        /// Degree to which a product or system defends against attack patterns by malicious actos and protects information and data so that persons or other products or systems have the degree of data access appropriate to their types and levels of authorization. 
        /// </summary>
        ISO_25010_Security,

        /// <summary>
        /// Degree to which a product or system ensures that data are accessible only to those authorized to have access.
        /// </summary>
        ISO_25010_Security_Confidentiality,

        /// <summary>
        /// Degree to which a system, product or component ensures that the state of its system and data are protected from unauthorized modification or deletion either by malicious action or computer error.
        /// </summary>
        ISO_25010_Security_Integrity,

        /// <summary>
        /// Degree to which actions or events can be proven to have taken place so that the events or actions cannot be repudiated later.
        /// </summary>
        ISO_25010_Security_NonRepudiation,

        /// <summary>
        /// Degree to which the identity of a subject or resource can be proved to be the one claimed.
        /// </summary>
        ISO_25010_Security_Authenticity,

        /// <summary>
        /// Degree to which the actions of an entity can be traced uniquely to the entity.
        /// </summary>
        ISO_25010_Security_Accountability,

        /// <summary>
        /// This characteristic represents the degree of effectiveness and efficiency with which a product or system can be modified to improve it, correct it or adapt it to changes in environment, and in requirements. 
        /// </summary>
        ISO_25010_Maintainability,

        /// <summary>
        /// Degree to which a system or computer program is composed of discrete components such that a change to one component has minimal impact on other components.
        /// </summary>
        ISO_25010_Maintainability_Modularity,

        /// <summary>
        /// Degree to which a product can be used as an asset in more than one system, or in building other assets.
        /// </summary>
        ISO_25010_Maintainability_Reusability,

        /// <summary>
        /// Degree of effectiveness and efficiency with which it is possible to assess the impact on a product or system of an intended change to one or more of its parts, to diagnose a product for deficiencies or causes of failures, or to identify parts to be modified.
        /// </summary>
        ISO_25010_Maintainability_Analysability,

        /// <summary>
        /// Degree to which a product or system can be effectively and efficiently modified without introducing defects or degrading existing product quality.
        /// </summary>
        ISO_25010_Maintainability_Modifyability,

        /// <summary>
        /// Degree of effectiveness and efficiency with which test criteria can be established for a system, product or component and tests can be performed to determine whether those criteria have been met.
        /// </summary>
        ISO_25010_Maintainability_Testability,

        /// <summary>
        /// Degree to which a product can be adapted to changes in its requirements, contexts of use or sys tem environment.
        /// </summary>
        ISO_25010_Flexibility,

        /// <summary>
        /// </summary>
        ISO_25010_Flexibility_Adaptability,

        /// <summary>
        /// </summary>
        ISO_25010_Flexibility_Scalability,

        /// <summary>
        /// </summary>
        ISO_25010_Flexibility_Installability,

        /// <summary>
        /// </summary>
        ISO_25010_Flexibility_Replaceability,

        /// <summary>
        /// Retired.
        /// </summary>
        ISO_25010_Portability,

        /// <summary>
        /// Retired.
        /// </summary>
        ISO_25010_Portability_Adaptability,

        /// <summary>
        /// Retired.
        /// </summary>
        ISO_25010_Portability_Installability,

        /// <summary>
        /// Retired.
        /// </summary>
        ISO_25010_Portability_Replaceability,

        /// <summary>
        /// This characteristic represents the degree to which a product under defined conditions to avoid a state in which human life, health, property, or the environment is endangered.
        /// </summary>
        ISO_25010_Safety,

        /// <summary>
        /// Degree to which a product or system constrains its operation to within safe parameters or states when encountering operational hazard.
        /// </summary>
        ISO_25010_Safety_OperationalConstraint,

        /// <summary>
        /// Degree to which a product can identify a course of events or operations that can expose life, property or environment to unacceptable risk.
        /// </summary>
        ISO_25010_Safety_RiskIdentification,

        /// <summary>
        /// Degree to which a product can automatically place itself in a safe operating mode, or to revert to a safe condition in the event of a failure.
        /// </summary>
        ISO_25010_Safety_FailSafe,

        /// <summary>
        /// Degree to which a product or system provides warnings of unacceptable risks to operations or internal controls so that they can react in sufficient time to sustain safe operations.
        /// </summary>
        ISO_25010_Safety_HazardWarning,

        /// <summary>
        /// Degree to which a product can maintain safety during and after integration with one or more components.
        /// </summary>
        ISO_25010_Safety_SafeIntegation,
    }

}