using Microsoft.AspNetCore.Mvc;
using System;

namespace App.Modules.Sys.Interfaces.API.REST.Controllers.V1
{
    /// <summary>
    /// Base controller for Sys module API v1.
    /// Provides common functionality for all controllers.
    /// </summary>
    [ApiController]
    // TODO: Add [ApiVersion("1.0")] when Asp.Versioning.Mvc package is added
    [Route("api/v{version:apiVersion}/workspaces/{workspaceId}")]
    public abstract class SysApiControllerBase : ControllerBase
    {
        /// <summary>
        /// Get workspace ID from route.
        /// </summary>
        protected string WorkspaceId => RouteData.Values["workspaceId"]?.ToString() ?? "default";

        /// <summary>
        /// Get current user ID from claims (if authenticated).
        /// </summary>
        protected Guid? CurrentUserId
        {
            get
            {
                var userIdClaim = User?.FindFirst("sub")?.Value 
                    ?? User?.FindFirst("userId")?.Value;
                
                if (Guid.TryParse(userIdClaim, out var userId))
                {
                    return userId;
                }
                
                return null;
            }
        }

        /// <summary>
        /// Whether current request is authenticated.
        /// </summary>
        protected bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;
    }
}
