using App.Modules.Sys.Application.Domains.Settings.Models;
using App.Modules.Sys.Application.Domains.Settings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Interfaces.Domains.V1.Settings;

/// <summary>
/// User Settings API - Personal preferences (current user).
/// Allows users to customize their personal settings within their workspace.
/// </summary>
/// <remarks>
/// AUTHORIZATION: Authenticated users can modify their own settings.
/// 
/// User settings:
/// - Personal preferences that override workspace/system defaults
/// - DEFAULT WRITE TARGET: Use this controller for general setting updates
/// - Cannot override workspace-locked or system-locked settings
/// - Scoped to current workspace (different per workspace)
/// </remarks>
[Route("api/sys/rest/v{version:apiVersion}/settings/user")]
[Authorize]
public class UserSettingsController : ControllerBase
{
    private readonly ISettingsApplicationService _service;

    /// <summary>
    /// Constructor - Inject Application Service.
    /// </summary>
    public UserSettingsController(ISettingsApplicationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get user-level settings for current user (overrides only).
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Returns only settings explicitly set at user level.
    /// Does not include inherited workspace/system settings.
    /// 
    /// Use this to see what the user has customized.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(SettingsCollectionDto), 200)]
    public async Task<ActionResult<SettingsCollectionDto>> GetUserSettings(CancellationToken ct = default)
    {
        var settings = await _service.GetUserSettingsAsync(ct);
        return Ok(settings);
    }

    /// <summary>
    /// Update user-level setting (personal preference).
    /// </summary>
    /// <param name="key">Setting key to update.</param>
    /// <param name="dto">New value and metadata.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// DEFAULT TARGET for setting writes.
    /// 
    /// Updates user's personal preference, overriding workspace/system defaults.
    /// 
    /// Restrictions:
    /// - Cannot override system-locked settings
    /// - Cannot override workspace-locked settings
    /// 
    /// Example request:
    /// PUT /settings/user/theme
    /// {
    ///   "value": "dark",
    ///   "valueType": "string"
    /// }
    /// 
    /// Frontend usage:
    /// This is the primary endpoint for user preference changes.
    /// Example: User changes theme in settings UI → PUT to this endpoint
    /// </remarks>
    [HttpPut("{key}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateUserSetting(
        string key,
        [FromBody] UpdateSettingDto dto,
        CancellationToken ct = default)
    {
        try
        {
            await _service.UpdateUserSettingAsync(key, dto, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete user-level setting (revert to workspace/system default).
    /// </summary>
    /// <param name="key">Setting key to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Removes the user override, reverting to workspace or system default.
    /// User will see the workspace value (if set) or system value.
    /// 
    /// Example:
    /// DELETE /settings/user/theme
    /// Result: User reverts to workspace theme (or system if workspace not customized)
    /// 
    /// Frontend usage:
    /// Use this for "Reset to default" functionality in user settings UI.
    /// </remarks>
    [HttpDelete("{key}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteUserSetting(string key, CancellationToken ct = default)
    {
        await _service.DeleteUserSettingAsync(key, ct);
        return NoContent();
    }
}
