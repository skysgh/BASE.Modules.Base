namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums
{
    /// <summary>
    /// <para>
    /// TODO: Enums are evil (offset issue of Interface Localization)
    /// </para>
    /// </summary>
    public enum NZDataClassification
    {
        //An error state:
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 0,

        //Policy and Privacy:
        /// <summary>
        /// Policy and Privacy - Unspecified
        /// </summary>
        Unspecified = 1,

        /// <summary>
        /// Policy and Privacy - Unclassified
        /// </summary>
        Unclassified = 2,
        /// <summary>
        /// Policy and Privacy - InConfidence
        /// </summary>
        InConfidence = 3,
        /// <summary>
        /// Policy and Privacy - Sensitive
        /// </summary>
        Sensitive = 4,

        //National Security:
        /// <summary>
        /// National Security - Restricted
        /// </summary>
        Restricted = 5,
        /// <summary>
        /// National Security - Confidential
        /// </summary>
        Confidential = 6,
        /// <summary>
        /// National Security - Secret
        /// </summary>
        Secret = 7,
        /// <summary>
        /// National Security - TopSecret
        /// </summary>
        TopSecret = 8
    }
}