using App.Modules.Sys.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Infrastructure.Services.Configuration
{
    /// <summary>
    /// Implementation of hierarchical settings service.
    /// Works with OR without database (graceful degradation):
    /// - No DB: Read-only from appsettings.json + SettingsSchema.yml
    /// - With DB: Full persistence + hierarchy (System/Workspace/User)
    /// </summary>
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository? _repository;
        private readonly SettingsDefaultsLoader _defaultsLoader;
        private Dictionary<string, SettingDefinition>? _inMemoryDefaults;

        public SettingsService(
            SettingsDefaultsLoader defaultsLoader,
            ISettingsRepository? repository = null)
        {
            _defaultsLoader = defaultsLoader;
            _repository = repository;
        }

        public async Task<T?> GetValueAsync<T>(
            string key, 
            Guid? workspaceId = null, 
            Guid? userId = null, 
            T? defaultValue = default, 
            CancellationToken ct = default)
        {
            // With repository: Use full hierarchy
            if (_repository != null)
            {
                return await GetFromDatabaseAsync<T>(key, workspaceId, userId, defaultValue, ct);
            }

            // Without repository: Use in-memory defaults (read-only)
            return await GetFromDefaultsAsync<T>(key, defaultValue, ct);
        }

        private async Task<T?> GetFromDatabaseAsync<T>(
            string key,
            Guid? workspaceId,
            Guid? userId,
            T? defaultValue,
            CancellationToken ct)
        {
            // Walk up hierarchy: Person ? Workspace ? System ? Defaults
            SettingValue? setting = null;

            if (userId.HasValue && workspaceId.HasValue)
            {
                setting = await _repository!.GetAsync(key, workspaceId.Value.ToString(), userId.Value.ToString(), ct);
            }

            if (setting == null && workspaceId.HasValue)
            {
                setting = await _repository!.GetAsync(key, workspaceId.Value.ToString(), "*", ct);
            }

            if (setting == null)
            {
                setting = await _repository!.GetAsync(key, "*", "*", ct);
            }

            if (setting == null || string.IsNullOrEmpty(setting.SerializedTypeValue))
            {
                return defaultValue;
            }

            return JsonSerializer.Deserialize<T>(setting.SerializedTypeValue);
        }

        private async Task<T?> GetFromDefaultsAsync<T>(
            string key,
            T? defaultValue,
            CancellationToken ct)
        {
            _inMemoryDefaults ??= await _defaultsLoader.LoadDefaultsAsync(ct);

            if (!_inMemoryDefaults.TryGetValue(key, out var definition))
            {
                return defaultValue;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(definition.SerializedTypeValue ?? string.Empty);
            }
            catch
            {
                return defaultValue;
            }
        }

        public Task SetValueAsync<T>(
            string key, 
            T value, 
            Guid? workspaceId = null, 
            Guid? userId = null, 
            string? modifiedBy = null, 
            CancellationToken ct = default)
        {
            if (_repository == null)
            {
                throw new InvalidOperationException(
                    "Cannot persist settings without database. System is in read-only mode.");
            }

            var setting = new SettingValue
            {
                WorkspaceId = workspaceId?.ToString() ?? "*",
                UserId = userId?.ToString() ?? "*",
                Key = key,
                SerializedTypeName = typeof(T).FullName ?? typeof(T).Name,
                SerializedTypeValue = JsonSerializer.Serialize(value),
                IsLocked = false,
                LastModified = DateTime.UtcNow,
                ModifiedBy = modifiedBy
            };

            return _repository.SetAsync(setting, ct);
        }

        public Task ResetAsync(
            string key, 
            Guid? workspaceId = null, 
            Guid? userId = null, 
            bool resetChildren = false, 
            CancellationToken ct = default)
        {
            if (_repository == null)
            {
                throw new InvalidOperationException("Cannot reset without database.");
            }

            var workspaceIdStr = workspaceId?.ToString() ?? "*";
            var userIdStr = userId?.ToString() ?? "*";

            return resetChildren
                ? _repository.DeleteByPatternAsync(key, workspaceIdStr, userIdStr, ct)
                : _repository.DeleteAsync(key, workspaceIdStr, userIdStr, ct);
        }

        public Task SetLockAsync(
            string key, 
            Guid? workspaceId, 
            bool locked, 
            string? modifiedBy = null, 
            CancellationToken ct = default)
        {
            if (_repository == null)
            {
                throw new InvalidOperationException("Cannot set locks without database.");
            }

            var workspaceIdStr = workspaceId?.ToString() ?? "*";
            return _repository.SetLockAsync(key, workspaceIdStr, "*", locked, modifiedBy, ct);
        }

        public async Task<bool> IsLockedAsync(
            string key, 
            Guid? workspaceId = null, 
            Guid? userId = null, 
            CancellationToken ct = default)
        {
            if (_repository == null)
            {
                // No DB: Check schema defaults
                _inMemoryDefaults ??= await _defaultsLoader.LoadDefaultsAsync(ct);
                return _inMemoryDefaults.TryGetValue(key, out var def) && def.Locked;
            }

            // With DB: Check hierarchy
            var keyParts = key.Split('/');
            var currentPath = "";

            foreach (var part in keyParts)
            {
                currentPath = string.IsNullOrEmpty(currentPath) ? part : $"{currentPath}/{part}";

                if (await _repository.IsLockedAsync(currentPath, "*", "*", ct))
                {
                    return true;
                }

                if (workspaceId.HasValue && await _repository.IsLockedAsync(currentPath, workspaceId.Value.ToString(), "*", ct))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<IEnumerable<SettingValue>> GetAllAsync(
            SettingLevel level, 
            Guid? workspaceId = null, 
            Guid? userId = null, 
            CancellationToken ct = default)
        {
            if (_repository == null)
            {
                _inMemoryDefaults ??= await _defaultsLoader.LoadDefaultsAsync(ct);
                
                return _inMemoryDefaults.Values.Select(def => new SettingValue
                {
                    WorkspaceId = "*",
                    UserId = "*",
                    Key = def.Key,
                    SerializedTypeName = def.SerializedTypeName,
                    SerializedTypeValue = def.SerializedTypeValue,
                    IsLocked = def.Locked,
                    LastModified = DateTime.UtcNow,
                    ModifiedBy = "DEFAULTS"
                });
            }

            var workspaceIdStr = level == SettingLevel.System ? "*" : workspaceId?.ToString() ?? "*";
            var userIdStr = level == SettingLevel.Person ? userId?.ToString() ?? "*" : "*";

            return await _repository.GetAllAsync(workspaceIdStr, userIdStr, ct);
        }

        public async Task<IEnumerable<SettingValue>> GetChildrenAsync(
            string parentKey, 
            Guid? workspaceId = null, 
            Guid? userId = null, 
            CancellationToken ct = default)
        {
            if (_repository == null)
            {
                _inMemoryDefaults ??= await _defaultsLoader.LoadDefaultsAsync(ct);
                
                return _inMemoryDefaults.Values
                    .Where(def => def.Key.StartsWith(parentKey + "/", StringComparison.OrdinalIgnoreCase))
                    .Select(def => new SettingValue
                    {
                        WorkspaceId = "*",
                        UserId = "*",
                        Key = def.Key,
                        SerializedTypeName = def.SerializedTypeName,
                        SerializedTypeValue = def.SerializedTypeValue,
                        IsLocked = def.Locked,
                        LastModified = DateTime.UtcNow,
                        ModifiedBy = "DEFAULTS"
                    });
            }

            var workspaceIdStr = workspaceId?.ToString() ?? "*";
            var userIdStr = userId?.ToString() ?? "*";

            return await _repository.GetChildrenAsync(parentKey, workspaceIdStr, userIdStr, ct);
        }
    }

    /// <summary>
    /// Repository interface for settings persistence.
    /// OPTIONAL: System works without it (read-only mode).
    /// </summary>
    public interface ISettingsRepository
    {
        Task<SettingValue?> GetAsync(string key, string workspaceId, string userId, CancellationToken ct);
        Task SetAsync(SettingValue setting, CancellationToken ct);
        Task DeleteAsync(string key, string workspaceId, string userId, CancellationToken ct);
        Task DeleteByPatternAsync(string keyPattern, string workspaceId, string userId, CancellationToken ct);
        Task SetLockAsync(string key, string workspaceId, string userId, bool locked, string? modifiedBy, CancellationToken ct);
        Task<bool> IsLockedAsync(string key, string workspaceId, string userId, CancellationToken ct);
        Task<IEnumerable<SettingValue>> GetAllAsync(string workspaceId, string userId, CancellationToken ct);
        Task<IEnumerable<SettingValue>> GetChildrenAsync(string parentKey, string workspaceId, string userId, CancellationToken ct);
    }
}
