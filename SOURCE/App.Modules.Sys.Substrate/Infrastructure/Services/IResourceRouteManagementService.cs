// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

using App.Modules.Sys.Infrastructure.Services;

namespace App.Modules.Sys.Infrastructure.Services
{
    /// <summary>
    /// Contract for handling end user Routes 
    /// and rewriting them as necessary.
    /// </summary>
    public interface IResourceRouteManagementService : IHasAppInfrastructureService
    {
        /// <summary>
        /// Rewrites the provided request if/as required.
        /// </summary>
        /// <param name="resourceRoute"></param>
        /// <returns></returns>
        string? Rewrite(string? resourceRoute);
    }
}
