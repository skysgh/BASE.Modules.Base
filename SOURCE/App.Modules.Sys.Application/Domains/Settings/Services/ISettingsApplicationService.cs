using App.Modules.Sys.Application.Domains.Settings.Models;
using App.Modules.Sys.Shared.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.Domains.Settings.Services;

/// <summary>
/// Application service for hierarchical settings (System → Workspace → User).
/// Handles cascade resolution and scope-specific operations.
/// </summary>
public interface ISettingsApplicationService : IHasService
{
    // ========================================
    // EFFECTIVE SETTINGS (Resolved Cascade)
    // ========================================

    /// <summary>
    /// Get effective settings for current user (resolved cascade: User → Workspace → System).
    /// This is what the user actually sees - all overrides applied.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Effective settings collection with resolved values.</returns>
    Task<SettingsCollectionDto> GetEffectiveSettingsAsync(CancellationToken ct = default);

    /// <summary>
    /// Get effective value for specific setting key (resolved cascade).
    /// </summary>
    /// <param name="key">Setting key.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Resolved setting value, or null if not found.</returns>
    Task<SettingDto?> GetEffectiveValueAsync(string key, CancellationToken ct = default);

    // ========================================
    // SYSTEM SCOPE (Admin Only)
    // ========================================

    /// <summary>
    /// Get system-level settings (global baseline).
    /// </summary>
    Task<SettingsCollectionDto> GetSystemSettingsAsync(CancellationToken ct = default);

    /// <summary>
    /// Update system-level setting (global baseline).
    /// </summary>
    Task UpdateSystemSettingAsync(string key, UpdateSettingDto dto, CancellationToken ct = default);

    // ========================================
    // WORKSPACE SCOPE (Workspace Admin)
    // ========================================

    /// <summary>
    /// Get workspace-level settings for current workspace (overrides only).
    /// Does not include inherited system settings.
    /// </summary>
    Task<SettingsCollectionDto> GetWorkspaceSettingsAsync(CancellationToken ct = default);

    /// <summary>
    /// Get effective settings for current workspace (Workspace → System cascade).
    /// Shows what workspace sees without user overrides.
    /// </summary>
    Task<SettingsCollectionDto> GetWorkspaceEffectiveSettingsAsync(CancellationToken ct = default);

    /// <summary>
    /// Update workspace-level setting (overrides system for this workspace).
    /// </summary>
    Task UpdateWorkspaceSettingAsync(string key, UpdateSettingDto dto, CancellationToken ct = default);

    /// <summary>
    /// Delete workspace-level setting (revert to system default).
    /// </summary>
    Task DeleteWorkspaceSettingAsync(string key, CancellationToken ct = default);

    // ========================================
    // USER SCOPE (Current User)
    // ========================================

    /// <summary>
    /// Get user-level settings for current user (overrides only).
    /// Does not include inherited workspace/system settings.
    /// </summary>
    Task<SettingsCollectionDto> GetUserSettingsAsync(CancellationToken ct = default);

    /// <summary>
    /// Update user-level setting (personal preference).
    /// DEFAULT TARGET for setting writes if scope not specified.
    /// </summary>
    Task UpdateUserSettingAsync(string key, UpdateSettingDto dto, CancellationToken ct = default);

    /// <summary>
    /// Delete user-level setting (revert to workspace/system default).
    /// </summary>
    Task DeleteUserSettingAsync(string key, CancellationToken ct = default);
}
