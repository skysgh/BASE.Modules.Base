using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;
using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Contracts;
using App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
{

    /// <summary>
    /// A Tenancy-specific record of Geodata.
    /// </summary>
    public class GeoData :
        TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasTitleAndDescription,
        IHasLatitudeAndLongitude,
        IHasGenericValue<decimal?>
    {
        /// <summary>
        /// The Title
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// The Description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// The Lat
        /// </summary>
        public virtual decimal Latitude { get; set; }
        /// <summary>
        /// The long
        /// </summary>
        public virtual decimal Longitude { get; set; }

        /// <summary>
        /// The Type
        /// </summary>
        public virtual GeoDataType Type { get; set; }

        /// <summary>
        /// The value
        /// </summary>
        public virtual decimal? Value { get; set; }
        /// <summary>
        /// The Colour
        /// </summary>
        public virtual string? Color { get; set; }

        /// <summary>
        /// Is object draggable
        /// </summary>
        public virtual bool Draggable { get; set; }
    }
}