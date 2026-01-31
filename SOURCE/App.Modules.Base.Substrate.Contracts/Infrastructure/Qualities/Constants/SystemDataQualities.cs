namespace App.Modules.Base.Infrastructure.Qualities.Constants
{
    /// <summary>
    /// Qualities of the data managed by a system.
    /// The Quality of a Data Product may be understood as the degree to which data satisfy the requirements defined by the product-owner organization
    /// </summary>
    public enum SystemDataQualities
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
        /// The degree to which data has attributes that correctly represent the true value of the intended attribute of a concept or event in a specific context of use.
        /// </summary>
        /// <remarks>
        /// It has two main aspects:
        /// Syntactic Accuracy: Syntactic accuracy is defined as the closeness of the data values to a set of values defined in a domain considered syntactically correct.
        /// Semantic Accuracy: Semantic accuracy is defined as the closeness of the data values to a set of values defined in a domain considered semantically correct.
        /// </remarks>
        ISO_25012_Accuracy,

        /// <summary>
        /// The degree to which subject data associated with an entity has values for all expected attributes and related entity instances in a specific context of use.
        /// </summary>
        ISO_25012_Completeness,

        /// <summary>
        /// The degree to which data has attributes that are free from contradiction and are coherent with other data in a specific context of use. It can be either or both among data regarding one entity and across similar data for comparable entities.
        /// </summary>
        ISO_25012_Consistency,

        /// <summary>
        /// The degree to which data has attributes that are regarded as true and believable by users in a specific context of use. Credibility includes the concept of authenticity (the truthfulness of origins, attributions, commitments).
        /// </summary>
        ISO_25012_Credibility,

        /// <summary>
        /// The degree to which data has attributes that are of the right age in a specific context of use.
        /// </summary>
        ISO_25012_Currentness,

        /// <summary>
        /// The degree to which data can be accessed in a specific context of use, particularly by people who need supporting technology or special configuration because of some disability.
        /// </summary>
        ISO_25012_Accessibility,

        /// <summary>
        /// The degree to which data has attributes that adhere to standards, conventions or regulations in force and similar rules relating to data quality in a specific context of use.
        /// </summary>
        ISO_25012_Compliance,

        /// <summary>
        /// of use. Confidentiality is an aspect of information security (together with availability, integrity) as defined in ISO/IEC 13335-1:2004.
        /// </summary>
        ISO_25012_Confidentiality,

        /// <summary>
        /// The degree to which data has attributes that can be processed and provide the expected levels of performance by using the appropriate amounts and types of resources in a specific context of use.
        /// </summary>
        ISO_25012_Efficiency,

        /// <summary>
        /// The degree to which data has attributes that are exact or that provide discrimination in a specific context of use.
        /// </summary>
        ISO_25012_Precision,

        /// <summary>
        /// The degree to which data has attributes that provide an audit trail of access to the data and of any changes made to the data in a specific context of use.
        /// </summary>
        ISO_25012_Traceability,

        /// <summary>
        /// The degree to which data has attributes that enable it to be read and interpreted by users, and are expressed in appropriate languages, symbols and units in a specific context of use.
        /// Some information about data understandability are provided by metadata.
        /// </summary>
        ISO_25012_Understandability,

        /// <summary>
        /// The degree to which data has attributes that enable it to be retrieved by authorized users and/or applications in a specific context of use.
        /// </summary>
        ISO_25012_Availability,

        /// <summary>
        /// The degree to which data has attributes that enable it to be installed, replaced or moved from one system to another preserving the existing quality in a specific context of use.
        /// </summary>
        ISO_25012_Portability,

        /// <summary>
        /// The degree to which data has attributes that enable it to maintain and preserve a specified level of operations and quality, even in the event of failure, in a specific context of use.
        /// </summary>
        ISO_25012_Recoverability

    }

}