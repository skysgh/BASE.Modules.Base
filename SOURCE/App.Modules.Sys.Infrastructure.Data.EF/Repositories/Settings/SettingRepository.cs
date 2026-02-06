using App.Modules.Sys.Domain.Settings;
using App.Modules.Sys.Domain.Settings.Repositories;
using App.Modules.Sys.Infrastructure.Domains.Diagnostics;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services;
using App.Modules.Sys.Infrastructure.Repositories.Implementations.Base;
using App.Modules.Sys.Shared.Lifecycles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Infrastructure.Data.EF.Repositories.Settings;

/// <summary>
/// EF Core implementation of hierarchical settings repository.
/// Supports cascade resolution (System → Workspace → User).
/// Inherits from GenericRepositoryBase for DbContext access and common repository operations.
/// Auto-registered via IHasScopedLifecycle marker interface.
/// </summary>
internal sealed class SettingRepository : RepositoryBase, 
ISettingRepository, IHasScopedLifecycle
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbProvider">Provider for scoped DbContext access</param>
    /// <param name="logger">Logger instance</param>
    public SettingRepository(IScopedDbContextProviderService dbProvider, IAppLogger logger)
        : base(dbProvider, logger)
    {
    }

    // ========================================
    // SYSTEM SCOPE
    // ========================================

    public async Task<IReadOnlyDictionary<string, Setting>> GetSystemSettingsAsync(CancellationToken ct = default)
    {
        var settings = await Context.Set<Setting>()
            .AsNoTracking()
            .Where(s => s.Scope == SettingScope.System)
            .ToListAsync(ct);

        return settings.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<Setting?> GetSystemSettingAsync(string key, CancellationToken ct = default)
    {
        return await Context.Set<Setting>()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Scope == SettingScope.System && s.Key == key, ct);
    }

    public async Task UpsertSystemSettingAsync(string key, string value, string? valueType = null, bool isLocked = false, CancellationToken ct = default)
    {
        var existing = await Context.Set<Setting>()
            .FirstOrDefaultAsync(s => s.Scope == SettingScope.System && s.Key == key, ct);

        if (existing != null)
        {
            existing.Value = value;
            existing.ValueType = valueType;
            existing.IsLocked = isLocked;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            Context.Set<Setting>().Add(new Setting
            {
                Id = Guid.NewGuid(),
                Key = key,
                Value = value,
                ValueType = valueType,
                Scope = SettingScope.System,
                IsLocked = isLocked,
                CreatedAt = DateTime.UtcNow
            });
        }

        await Context.SaveChangesAsync(ct);
    }

    // ========================================
    // WORKSPACE SCOPE
    // ========================================

    public async Task<IReadOnlyDictionary<string, Setting>> GetWorkspaceSettingsAsync(Guid workspaceId, CancellationToken ct = default)
    {
        var settings = await Context.Set<Setting>()
            .AsNoTracking()
            .Where(s => s.Scope == SettingScope.Workspace && s.WorkspaceId == workspaceId)
            .ToListAsync(ct);

        return settings.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<Setting?> GetWorkspaceSettingAsync(Guid workspaceId, string key, CancellationToken ct = default)
    {
        return await Context.Set<Setting>()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Scope == SettingScope.Workspace && s.WorkspaceId == workspaceId && s.Key == key, ct);
    }

    public async Task UpsertWorkspaceSettingAsync(Guid workspaceId, string key, string value, string? valueType = null, bool isLocked = false, CancellationToken ct = default)
    {
        var existing = await Context.Set<Setting>()
            .FirstOrDefaultAsync(s => s.Scope == SettingScope.Workspace && s.WorkspaceId == workspaceId && s.Key == key, ct);

        if (existing != null)
        {
            existing.Value = value;
            existing.ValueType = valueType;
            existing.IsLocked = isLocked;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            Context.Set<Setting>().Add(new Setting
            {
                Id = Guid.NewGuid(),
                Key = key,
                Value = value,
                ValueType = valueType,
                Scope = SettingScope.Workspace,
                WorkspaceId = workspaceId,
                IsLocked = isLocked,
                CreatedAt = DateTime.UtcNow
            });
        }

        await Context.SaveChangesAsync(ct);
    }

    public async Task DeleteWorkspaceSettingAsync(Guid workspaceId, string key, CancellationToken ct = default)
    {
        var setting = await Context.Set<Setting>()
            .FirstOrDefaultAsync(s => s.Scope == SettingScope.Workspace && s.WorkspaceId == workspaceId && s.Key == key, ct);

        if (setting != null)
        {
            Context.Set<Setting>().Remove(setting);
            await Context.SaveChangesAsync(ct);
        }
    }

    // ========================================
    // USER SCOPE
    // ========================================

    public async Task<IReadOnlyDictionary<string, Setting>> GetUserSettingsAsync(Guid workspaceId, Guid userId, CancellationToken ct = default)
    {
        var settings = await Context.Set<Setting>()
            .AsNoTracking()
            .Where(s => s.Scope == SettingScope.User && s.WorkspaceId == workspaceId && s.UserId == userId)
            .ToListAsync(ct);

        return settings.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<Setting?> GetUserSettingAsync(Guid workspaceId, Guid userId, string key, CancellationToken ct = default)
    {
        return await Context.Set<Setting>()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Scope == SettingScope.User && s.WorkspaceId == workspaceId && s.UserId == userId && s.Key == key, ct);
    }

    public async Task UpsertUserSettingAsync(Guid workspaceId, Guid userId, string key, string value, string? valueType = null, CancellationToken ct = default)
    {
        var existing = await Context.Set<Setting>()
            .FirstOrDefaultAsync(s => s.Scope == SettingScope.User && s.WorkspaceId == workspaceId && s.UserId == userId && s.Key == key, ct);

        if (existing != null)
        {
            existing.Value = value;
            existing.ValueType = valueType;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            Context.Set<Setting>().Add(new Setting
            {
                Id = Guid.NewGuid(),
                Key = key,
                Value = value,
                ValueType = valueType,
                Scope = SettingScope.User,
                WorkspaceId = workspaceId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });
        }

        await Context.SaveChangesAsync(ct);
    }

    public async Task DeleteUserSettingAsync(Guid workspaceId, Guid userId, string key, CancellationToken ct = default)
    {
        var setting = await Context.Set<Setting>()
            .FirstOrDefaultAsync(s => s.Scope == SettingScope.User && s.WorkspaceId == workspaceId && s.UserId == userId && s.Key == key, ct);

        if (setting != null)
        {
            Context.Set<Setting>().Remove(setting);
            await Context.SaveChangesAsync(ct);
        }
    }

    // ========================================
    // CASCADE QUERIES
    // ========================================

    public async Task<(
        IReadOnlyDictionary<string, Setting> System,
        IReadOnlyDictionary<string, Setting> Workspace,
        IReadOnlyDictionary<string, Setting> User
    )> GetCascadeSettingsAsync(Guid workspaceId, Guid userId, CancellationToken ct = default)
    {
        // Efficient: Single query fetches all three scopes
        var allSettings = await Context.Set<Setting>()
            .AsNoTracking()
            .Where(s =>
                s.Scope == SettingScope.System ||
                (s.Scope == SettingScope.Workspace && s.WorkspaceId == workspaceId) ||
                (s.Scope == SettingScope.User && s.WorkspaceId == workspaceId && s.UserId == userId))
            .ToListAsync(ct);

        var system = allSettings
            .Where(s => s.Scope == SettingScope.System)
            .ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);

        var workspace = allSettings
            .Where(s => s.Scope == SettingScope.Workspace)
            .ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);

        var user = allSettings
            .Where(s => s.Scope == SettingScope.User)
            .ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);

        return (system, workspace, user);
    }
}
