using App.Modules.Sys.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.Domains.Settings.Repositories;

/// <summary>
/// Repository interface for hierarchical settings (System → Workspace → User).
/// Application layer depends on this interface.
/// </summary>
public interface ISettingRepository
{
    // ========================================
    // SYSTEM SCOPE (Global Baseline)
    // ========================================

    /// <summary>
    /// Get all system-level settings.
    /// </summary>
    Task<IReadOnlyDictionary<string, Setting>> GetSystemSettingsAsync(CancellationToken ct = default);

    /// <summary>
    /// Get specific system setting by key.
    /// </summary>
    Task<Setting?> GetSystemSettingAsync(string key, CancellationToken ct = default);

    /// <summary>
    /// Update or create system setting.
    /// </summary>
    Task UpsertSystemSettingAsync(string key, string value, string? valueType = null, bool isLocked = false, CancellationToken ct = default);

    // ========================================
    // WORKSPACE SCOPE (Workspace Overrides)
    // ========================================

    /// <summary>
    /// Get all workspace-level settings for specific workspace.
    /// </summary>
    Task<IReadOnlyDictionary<string, Setting>> GetWorkspaceSettingsAsync(Guid workspaceId, CancellationToken ct = default);

    /// <summary>
    /// Get specific workspace setting by key.
    /// </summary>
    Task<Setting?> GetWorkspaceSettingAsync(Guid workspaceId, string key, CancellationToken ct = default);

    /// <summary>
    /// Update or create workspace setting.
    /// </summary>
    Task UpsertWorkspaceSettingAsync(Guid workspaceId, string key, string value, string? valueType = null, bool isLocked = false, CancellationToken ct = default);

    /// <summary>
    /// Delete workspace setting (revert to system default).
    /// </summary>
    Task DeleteWorkspaceSettingAsync(Guid workspaceId, string key, CancellationToken ct = default);

    // ========================================
    // USER SCOPE (Personal Preferences)
    // ========================================

    /// <summary>
    /// Get all user-level settings for specific user in workspace.
    /// </summary>
    Task<IReadOnlyDictionary<string, Setting>> GetUserSettingsAsync(Guid workspaceId, Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Get specific user setting by key.
    /// </summary>
    Task<Setting?> GetUserSettingAsync(Guid workspaceId, Guid userId, string key, CancellationToken ct = default);

    /// <summary>
    /// Update or create user setting.
    /// </summary>
    Task UpsertUserSettingAsync(Guid workspaceId, Guid userId, string key, string value, string? valueType = null, CancellationToken ct = default);

    /// <summary>
    /// Delete user setting (revert to workspace/system default).
    /// </summary>
    Task DeleteUserSettingAsync(Guid workspaceId, Guid userId, string key, CancellationToken ct = default);

    // ========================================
    // CASCADE QUERIES (for resolution)
    // ========================================

    /// <summary>
    /// Get all settings for cascade resolution (System + Workspace + User).
    /// Used by application service to compute effective settings.
    /// </summary>
    Task<(
        IReadOnlyDictionary<string, Setting> System,
        IReadOnlyDictionary<string, Setting> Workspace,
        IReadOnlyDictionary<string, Setting> User
    )> GetCascadeSettingsAsync(Guid workspaceId, Guid userId, CancellationToken ct = default);
}
