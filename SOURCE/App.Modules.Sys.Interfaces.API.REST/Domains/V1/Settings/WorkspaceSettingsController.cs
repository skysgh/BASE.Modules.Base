using App.Modules.Sys.Application.Domains.Settings.Models;
using App.Modules.Sys.Application.Domains.Settings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Interfaces.Domains.V1.Settings;

/// <summary>
/// Workspace Settings API - Workspace-specific overrides (workspace admin).
/// Allows workspace administrators to customize settings for their workspace.
/// </summary>
/// <remarks>
/// AUTHORIZATION: Requires WorkspaceAdministrator role.
/// 
/// Workspace settings:
/// - Override system defaults for specific workspace
/// - Provide defaults for all users in the workspace
/// - Can be locked to prevent user overrides
/// - Cannot override system-locked settings
/// </remarks>
[Route("api/sys/rest/v{version:apiVersion}/settings/workspace")]
[Authorize(Roles = "WorkspaceAdministrator")]
public class WorkspaceSettingsController : ControllerBase
{
    private readonly ISettingsApplicationService _service;

    /// <summary>
    /// Constructor - Inject Application Service.
    /// </summary>
    public WorkspaceSettingsController(ISettingsApplicationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get workspace-level settings for current workspace (overrides only).
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Returns only settings explicitly set at workspace level.
    /// Does not include inherited system settings or user overrides.
    /// 
    /// Use this to see what's customized for this workspace.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(SettingsCollectionDto), 200)]
    public async Task<ActionResult<SettingsCollectionDto>> GetWorkspaceSettings(CancellationToken ct = default)
    {
        var settings = await _service.GetWorkspaceSettingsAsync(ct);
        return Ok(settings);
    }

    /// <summary>
    /// Get effective settings for current workspace (Workspace → System cascade).
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Returns what workspace sees (workspace overrides + system defaults).
    /// Does not include user-level overrides.
    /// 
    /// Use this to preview what workspace users will see by default.
    /// </remarks>
    [HttpGet("effective")]
    [ProducesResponseType(typeof(SettingsCollectionDto), 200)]
    public async Task<ActionResult<SettingsCollectionDto>> GetWorkspaceEffective(CancellationToken ct = default)
    {
        var settings = await _service.GetWorkspaceEffectiveSettingsAsync(ct);
        return Ok(settings);
    }

    /// <summary>
    /// Update workspace-level setting (overrides system for this workspace).
    /// </summary>
    /// <param name="key">Setting key to update.</param>
    /// <param name="dto">New value and metadata.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Customizes a setting for this workspace, overriding the system default.
    /// 
    /// Restrictions:
    /// - Cannot override system-locked settings
    /// - Set isLocked=true to prevent user overrides within workspace
    /// 
    /// Example request:
    /// PUT /settings/workspace/theme
    /// {
    ///   "value": "corporate-blue",
    ///   "valueType": "string",
    ///   "isLocked": false
    /// }
    /// </remarks>
    [HttpPut("{key}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateWorkspaceSetting(
        string key,
        [FromBody] UpdateSettingDto dto,
        CancellationToken ct = default)
    {
        try
        {
            await _service.UpdateWorkspaceSettingAsync(key, dto, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete workspace-level setting (revert to system default).
    /// </summary>
    /// <param name="key">Setting key to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Removes the workspace override, reverting to system default.
    /// Workspace users will see the system value (unless they have user overrides).
    /// 
    /// Example:
    /// DELETE /settings/workspace/theme
    /// Result: Workspace reverts to system theme setting
    /// </remarks>
    [HttpDelete("{key}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteWorkspaceSetting(string key, CancellationToken ct = default)
    {
        await _service.DeleteWorkspaceSettingAsync(key, ct);
        return NoContent();
    }
}
