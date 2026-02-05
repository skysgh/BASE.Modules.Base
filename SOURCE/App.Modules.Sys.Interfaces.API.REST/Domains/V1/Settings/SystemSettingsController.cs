using App.Modules.Sys.Application.Domains.Settings.Models;
using App.Modules.Sys.Application.Domains.Settings.Services;
using App.Modules.Sys.Interfaces.API.REST.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Interfaces.Domains.V1.Settings;

/// <summary>
/// System Settings API - Global baseline settings (admin only).
/// Provides system administrators control over default values for all users.
/// </summary>
/// <remarks>
/// AUTHORIZATION: Requires SystemAdministrator role.
/// 
/// System settings:
/// - Provide baseline defaults for all workspaces and users
/// - Can be locked to prevent workspace/user overrides
/// - Changes affect all users unless overridden at workspace/user level
/// </remarks>
[Route(ApiRoutes.Rest.V1.Settings.System)]
[Authorize(Roles = "SystemAdministrator")]
public class SystemSettingsController : ControllerBase
{
    private readonly ISettingsApplicationService _service;

    /// <summary>
    /// Constructor - Inject Application Service.
    /// </summary>
    public SystemSettingsController(ISettingsApplicationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get system-level settings (global baseline).
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Returns only settings explicitly set at system level.
    /// Does not include workspace or user overrides.
    /// 
    /// Use this to see the baseline configuration for the entire system.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(SettingsCollectionDto), 200)]
    public async Task<ActionResult<SettingsCollectionDto>> GetSystemSettings(CancellationToken ct = default)
    {
        var settings = await _service.GetSystemSettingsAsync(ct);
        return Ok(settings);
    }

    /// <summary>
    /// Update system-level setting (global baseline).
    /// </summary>
    /// <param name="key">Setting key to update.</param>
    /// <param name="dto">New value and metadata.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Updates the global default value for a setting.
    /// 
    /// Lock behavior:
    /// - Set isLocked=true to prevent workspace/user overrides
    /// - Use for security-critical settings (e.g., sessionTimeout)
    /// 
    /// Example request:
    /// PUT /settings/system/sessionTimeout
    /// {
    ///   "value": "30",
    ///   "valueType": "int",
    ///   "isLocked": true
    /// }
    /// </remarks>
    [HttpPut("{key}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> UpdateSystemSetting(
        string key,
        [FromBody] UpdateSettingDto dto,
        CancellationToken ct = default)
    {
        await _service.UpdateSystemSettingAsync(key, dto, ct);
        return NoContent();
    }
}
