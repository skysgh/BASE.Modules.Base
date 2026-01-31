using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.TenancySpecific
{
    /// <summary>
    /// A Permission that can be assigned directly to a Security Profile.
    /// </summary>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public class PrincipalSecurityProfilePermission : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase, IHasTitleAndDescription
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {
        /// <summary>
        /// It's title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// It's Description
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

}

