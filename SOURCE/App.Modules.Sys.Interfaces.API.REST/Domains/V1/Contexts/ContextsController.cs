using App.Modules.Sys.Application.Domains.Context;
using App.Modules.Sys.Application.Domains.Context.Models.Implementations;
using App.Modules.Sys.Interfaces.API.REST.Domains.Constants;
using App.Modules.Sys.Interfaces.Domains.V1.Implementations.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Modules.Sys.Interfaces.Domains.V1.Contexts;

/// <summary>
/// Contexts API - Provides complete application context for frontend rendering.
/// Returns system, workspace, user, and computed settings in a single call.
/// </summary>
/// <remarks>
/// This endpoint replaces multiple frontend API calls with a single aggregated response.
/// Supports both authenticated and anonymous users.
/// </remarks>
[Route("api/rest/sys/v1/contexts")]
[AllowAnonymous] // Allow anonymous access - returns limited context for anonymous users
public class ContextsController : SysApiControllerBase
{
    private readonly IApplicationContextService _contextService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public ContextController(IApplicationContextService contextService)
    {
        _contextService = contextService;
    }

    /// <summary>
    /// Get complete application context.
    /// </summary>
    /// <remarks>
    /// Returns hierarchical context data:
    /// - System: Platform information, features, branding
    /// - Workspace: Tenant-specific settings and branding (if applicable)
    /// - User: User profile, roles, permissions, notifications (if authenticated)
    /// - Settings: Computed/merged settings after hierarchy resolution
    /// 
    /// Anonymous users receive limited context (system + default settings).
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <response code="200">Context retrieved successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApplicationContextDto), 200)]
    public async Task<ActionResult<ApplicationContextDto>> GetContext(CancellationToken cancellationToken = default)
    {
        var context = await _contextService.GetApplicationContextAsync(cancellationToken);
        return Ok(context);
    }
}
