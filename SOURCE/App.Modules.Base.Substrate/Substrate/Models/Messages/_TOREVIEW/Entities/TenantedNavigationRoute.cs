// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Collections.ObjectModel;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{

    /// <summary>
    /// A single element within a Navigation Map used by user interfaces.
    /// <see cref="NavigationRoute"/>.
    /// </summary>
    public class TenantedNavigationRoute
        : TenantFKAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasGuidId,
        IHasOwnerFK,
        IHasTitleAndDescriptionAndImageUrlAndDisplayHints
    {

        /// <summary>
        /// Whether the route is enabled.
        /// </summary>
        public bool Enabled { get; set; }
        // Class Not even used not sure what this was supposed to be

        /// <summary>
        /// The FK of the Parent Route.
        /// </summary>
        public Guid OwnerFK { get; set; }

        /// <summary>
        /// The display title of the Route.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A description of the Route.
        /// </summary>
        public string Description { get; set; }
        /// <inheritdoc/>


        public string ImageUrl { get; set; }

        /// <summary>
        /// A hint as to the order to display the Route.
        /// </summary>
        public int DisplayOrderHint { get; set; }

        /// <summary>
        /// A hint as to the style in which to display the Route.
        /// </summary>
        public string? DisplayStyleHint { get; set; }

        /// <summary>
        /// Child/nested routes.
        /// </summary>
        public ICollection<TenantedNavigationRoute> Chilldren => _children ??= [];// new Collection<TenantedNavigationRoute>();

        private ICollection<TenantedNavigationRoute>? _children;

        /// <summary>
        /// Get the FK of the parent Navigation Route.
        /// </summary>
        /// <returns></returns>
        public Guid GetOwnerFk()
        {
            return OwnerFK;
        }
    }
}
