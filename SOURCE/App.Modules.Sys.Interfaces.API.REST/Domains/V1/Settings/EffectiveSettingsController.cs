using App.Modules.Sys.Application.Domains.Settings.Models;
using App.Modules.Sys.Application.Domains.Settings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Interfaces.Domains.V1.Settings;

/// <summary>
/// Effective Settings API - Read-only resolved settings (User → Workspace → System cascade).
/// Returns the actual values that apply to the current user.
/// </summary>
/// <remarks>
/// This is the PRIMARY endpoint for frontend applications.
/// Use this to get "what settings apply to me right now?"
/// 
/// For modifying settings, use:
/// - UserSettingsController (default write target)
/// - WorkspaceSettingsController (workspace admin)
/// - SystemSettingsController (system admin)
/// </remarks>
[Route("api/sys/rest/v{version:apiVersion}/settings")]
[Authorize]
public class EffectiveSettingsController : ControllerBase
{
    private readonly ISettingsApplicationService _service;

    /// <summary>
    /// Constructor - Inject Application Service.
    /// </summary>
    public EffectiveSettingsController(ISettingsApplicationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get effective settings for current user (resolved cascade).
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Returns all settings with cascade resolution applied:
    /// - User overrides take precedence
    /// - Falls back to workspace settings
    /// - Falls back to system defaults
    /// - Locked settings are enforced
    /// 
    /// Example response:
    /// {
    ///   "settings": {
    ///     "theme": "dark",           // User override
    ///     "language": "en",          // System default
    ///     "pageSize": "20"           // Workspace override
    ///   }
    /// }
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(SettingsCollectionDto), 200)]
    public async Task<ActionResult<SettingsCollectionDto>> GetEffective(CancellationToken ct = default)
    {
        var settings = await _service.GetEffectiveSettingsAsync(ct);
        return Ok(settings);
    }

    /// <summary>
    /// Get effective value for specific setting key (resolved cascade).
    /// </summary>
    /// <param name="key">Setting key (e.g., "theme", "language").</param>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Returns the effective value with metadata (scope, lock status, etc.).
    /// Shows which level (System/Workspace/User) the value comes from.
    /// 
    /// Example response:
    /// {
    ///   "key": "theme",
    ///   "value": "dark",
    ///   "scope": "User",
    ///   "isLocked": false
    /// }
    /// </remarks>
    [HttpGet("{key}")]
    [ProducesResponseType(typeof(SettingDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<SettingDto>> GetEffectiveValue(string key, CancellationToken ct = default)
    {
        var setting = await _service.GetEffectiveValueAsync(key, ct);
        
        if (setting == null)
        {
            return NotFound(new { message = $"Setting '{key}' not found." });
        }

        return Ok(setting);
    }
}
