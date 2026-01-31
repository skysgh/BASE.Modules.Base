// using System;
using App.Modules.Base.Substrate.Models.Entities.Base;
// using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities;
using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums;



// using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
{
    /// <summary>
    /// A descriptor of Media uploaded by end users (to a StorageAccount)
    /// <para>
    /// Note that each Tenancy's Media is kept 
    /// seperate from every other Tenant's uploaded Media.
    /// </para>
    /// </summary>
    public class MediaMetadata : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase
    {
        /// <summary>
        /// FK to Data classification.
        /// <para>
        /// Use an Enum as DataClassification is shared across Bounded DbContexts
        /// </para>
        /// </summary>
        public virtual NZDataClassification DataClassificationFK { get; set; } /* Unclassified or not determines whether external scanning services can be invoked or not */

        /// <summary>
        /// Get the Data classification
        /// </summary>
        public virtual DataClassification DataClassification { get; set; }

        /// <summary>
        /// Get when the media was uploaded.
        /// </summary>
        public virtual DateTime UploadedDateTimeUtc { get; set; }
        /// <summary>
        /// Get/Set size of media stream
        /// </summary>
        public virtual long ContentSize { get; set; } /*size of stream*/
        /// <summary>
        /// The media mime type.
        /// <para>
        /// Note: The extension is not always a correct indicator...
        /// </para>
        /// </summary>
        public virtual string MimeType { get; set; } = string.Empty;
        /// <summary>
        /// the name the file had on the uploader's machine
        /// </summary>
        public virtual string SourceFileName { get; set; } = string.Empty;
        /// <summary>
        /// unique hash of the stream for faster reference later
        /// </summary>
        public virtual string ContentHash { get; set; } = string.Empty;

        /// <summary>
        /// name in storage container
        /// </summary>
        public virtual string LocalName { get; set; } = string.Empty;

        /// <summary>
        /// Date last scanned
        /// <para>
        /// should be scanned regularly as threat detectors evolve
        /// </para>
        /// </summary>
        public virtual DateTime? LatestScanDateTimeUtc { get; set; } /**/


        /// <summary>
        /// Results of last scan
        /// </summary>
        public virtual bool? LatestScanMalwareDetetected { get; set; }
        /// <summary>
        /// Information about last negative scan
        /// </summary>
        public virtual string LatestScanResults { get; set; }
    }
}