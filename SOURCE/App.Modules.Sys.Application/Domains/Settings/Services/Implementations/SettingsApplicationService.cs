using App.Modules.Sys.Application.Context.Services;
using App.Modules.Sys.Application.Domains.Settings.Models;
using App.Modules.Sys.Application.Domains.Settings.Services;
using App.Modules.Sys.Domain.Settings;
using App.Modules.Sys.Domain.Settings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.Domains.Settings.Services.Implementations;

/// <summary>
/// Application service implementation for hierarchical settings.
/// Handles cascade resolution (System → Workspace → User) with lock enforcement.
/// </summary>
internal sealed class SettingsApplicationService : ISettingsApplicationService
{
    private readonly ISettingRepository _repository;
    private readonly IUserContextService _userContext;

    public SettingsApplicationService(
        ISettingRepository repository,
        IUserContextService userContext)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }

    // ========================================
    // EFFECTIVE SETTINGS (Resolved Cascade)
    // ========================================

    /// <inheritdoc/>
    public async Task<SettingsCollectionDto> GetEffectiveSettingsAsync(CancellationToken ct = default)
    {
        var workspaceId = _userContext.CurrentWorkspaceId;
        var userId = _userContext.CurrentUserId;

        var (system, workspace, user) = await _repository.GetCascadeSettingsAsync(workspaceId, userId, ct);

        // Resolve cascade with lock enforcement
        var effective = ResolveCascade(system, workspace, user);

        return new SettingsCollectionDto
        {
            Settings = effective
        };
    }

    /// <inheritdoc/>
    public async Task<SettingDto?> GetEffectiveValueAsync(string key, CancellationToken ct = default)
    {
        var workspaceId = _userContext.CurrentWorkspaceId;
        var userId = _userContext.CurrentUserId;

        var (system, workspace, user) = await _repository.GetCascadeSettingsAsync(workspaceId, userId, ct);

        // Get effective setting with metadata
        var effectiveSetting = ResolveSettingWithMetadata(key, system, workspace, user);

        return effectiveSetting != null ? MapToDto(effectiveSetting) : null;
    }

    // ========================================
    // SYSTEM SCOPE
    // ========================================

    /// <inheritdoc/>
    public async Task<SettingsCollectionDto> GetSystemSettingsAsync(CancellationToken ct = default)
    {
        var settings = await _repository.GetSystemSettingsAsync(ct);

        return new SettingsCollectionDto
        {
            Settings = settings.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value)
        };
    }

    /// <inheritdoc/>
    public async Task UpdateSystemSettingAsync(string key, UpdateSettingDto dto, CancellationToken ct = default)
    {
        await _repository.UpsertSystemSettingAsync(
            key,
            dto.Value,
            dto.ValueType,
            dto.IsLocked ?? false,
            ct);
    }

    // ========================================
    // WORKSPACE SCOPE
    // ========================================

    /// <inheritdoc/>
    public async Task<SettingsCollectionDto> GetWorkspaceSettingsAsync(CancellationToken ct = default)
    {
        var workspaceId = _userContext.CurrentWorkspaceId;
        var settings = await _repository.GetWorkspaceSettingsAsync(workspaceId, ct);

        return new SettingsCollectionDto
        {
            Settings = settings.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value)
        };
    }

    /// <inheritdoc/>
    public async Task<SettingsCollectionDto> GetWorkspaceEffectiveSettingsAsync(CancellationToken ct = default)
    {
        var workspaceId = _userContext.CurrentWorkspaceId;

        var system = await _repository.GetSystemSettingsAsync(ct);
        var workspace = await _repository.GetWorkspaceSettingsAsync(workspaceId, ct);

        // Resolve: Workspace → System (no user overrides)
        var effective = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Start with system
        foreach (var kvp in system)
        {
            effective[kvp.Key] = kvp.Value.Value;
        }

        // Apply workspace overrides (if not locked at system level)
        foreach (var kvp in workspace)
        {
            if (system.TryGetValue(kvp.Key, out var systemSetting) && systemSetting.IsLocked)
            {
                continue; // System locked - skip workspace override
            }

            effective[kvp.Key] = kvp.Value.Value;
        }

        return new SettingsCollectionDto
        {
            Settings = effective
        };
    }

    /// <inheritdoc/>
    public async Task UpdateWorkspaceSettingAsync(string key, UpdateSettingDto dto, CancellationToken ct = default)
    {
        var workspaceId = _userContext.CurrentWorkspaceId;

        // Check if system locked
        var systemSetting = await _repository.GetSystemSettingAsync(key, ct);
        if (systemSetting?.IsLocked == true)
        {
            throw new InvalidOperationException($"Setting '{key}' is locked at system level and cannot be overridden.");
        }

        await _repository.UpsertWorkspaceSettingAsync(
            workspaceId,
            key,
            dto.Value,
            dto.ValueType,
            dto.IsLocked ?? false,
            ct);
    }

    /// <inheritdoc/>
    public async Task DeleteWorkspaceSettingAsync(string key, CancellationToken ct = default)
    {
        var workspaceId = _userContext.CurrentWorkspaceId;
        await _repository.DeleteWorkspaceSettingAsync(workspaceId, key, ct);
    }

    // ========================================
    // USER SCOPE
    // ========================================

    /// <inheritdoc/>
    public async Task<SettingsCollectionDto> GetUserSettingsAsync(CancellationToken ct = default)
    {
        var workspaceId = _userContext.CurrentWorkspaceId;
        var userId = _userContext.CurrentUserId;

        var settings = await _repository.GetUserSettingsAsync(workspaceId, userId, ct);

        return new SettingsCollectionDto
        {
            Settings = settings.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value)
        };
    }

    /// <inheritdoc/>
    public async Task UpdateUserSettingAsync(string key, UpdateSettingDto dto, CancellationToken ct = default)
    {
        var workspaceId = _userContext.CurrentWorkspaceId;
        var userId = _userContext.CurrentUserId;

        // Check if locked at higher levels
        var systemSetting = await _repository.GetSystemSettingAsync(key, ct);
        if (systemSetting?.IsLocked == true)
        {
            throw new InvalidOperationException($"Setting '{key}' is locked at system level and cannot be overridden.");
        }

        var workspaceSetting = await _repository.GetWorkspaceSettingAsync(workspaceId, key, ct);
        if (workspaceSetting?.IsLocked == true)
        {
            throw new InvalidOperationException($"Setting '{key}' is locked at workspace level and cannot be overridden.");
        }

        await _repository.UpsertUserSettingAsync(
            workspaceId,
            userId,
            key,
            dto.Value,
            dto.ValueType,
            ct);
    }

    /// <inheritdoc/>
    public async Task DeleteUserSettingAsync(string key, CancellationToken ct = default)
    {
        var workspaceId = _userContext.CurrentWorkspaceId;
        var userId = _userContext.CurrentUserId;

        await _repository.DeleteUserSettingAsync(workspaceId, userId, key, ct);
    }

    // ========================================
    // PRIVATE HELPERS
    // ========================================

    /// <summary>
    /// Resolve cascade with lock enforcement.
    /// </summary>
    private static Dictionary<string, string> ResolveCascade(
        IReadOnlyDictionary<string, Setting> system,
        IReadOnlyDictionary<string, Setting> workspace,
        IReadOnlyDictionary<string, Setting> user)
    {
        var effective = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Get all unique keys
        var allKeys = system.Keys
            .Union(workspace.Keys)
            .Union(user.Keys)
            .Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var key in allKeys)
        {
            system.TryGetValue(key, out var systemSetting);
            workspace.TryGetValue(key, out var workspaceSetting);
            user.TryGetValue(key, out var userSetting);

            // Apply lock enforcement
            if (systemSetting?.IsLocked == true)
            {
                effective[key] = systemSetting.Value; // Force system value
            }
            else if (workspaceSetting?.IsLocked == true)
            {
                effective[key] = workspaceSetting.Value; // Force workspace value
            }
            else
            {
                // Full cascade: User → Workspace → System
                effective[key] = userSetting?.Value
                              ?? workspaceSetting?.Value
                              ?? systemSetting?.Value
                              ?? string.Empty;
            }
        }

        return effective;
    }

    /// <summary>
    /// Resolve single setting with metadata.
    /// </summary>
    private static Setting? ResolveSettingWithMetadata(
        string key,
        IReadOnlyDictionary<string, Setting> system,
        IReadOnlyDictionary<string, Setting> workspace,
        IReadOnlyDictionary<string, Setting> user)
    {
        system.TryGetValue(key, out var systemSetting);
        workspace.TryGetValue(key, out var workspaceSetting);
        user.TryGetValue(key, out var userSetting);

        // Return setting from highest effective level
        if (systemSetting?.IsLocked == true)
        {
            return systemSetting;
        }
        if (workspaceSetting?.IsLocked == true)
        {
            return workspaceSetting;
        }

        return userSetting ?? workspaceSetting ?? systemSetting;
    }

    /// <summary>
    /// Map domain Setting to DTO.
    /// </summary>
    private static SettingDto MapToDto(Setting setting)
    {
        return new SettingDto
        {
            Key = setting.Key,
            Value = setting.Value,
            ValueType = setting.ValueType,
            Scope = setting.Scope.ToString(),
            IsLocked = setting.IsLocked,
            Description = setting.Description,
            Category = setting.Category,
            UpdatedAt = setting.UpdatedAt
        };
    }
}
