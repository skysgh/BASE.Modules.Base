using App.Modules.Sys.Domain.Configuration;
using App.Modules.Sys.Shared;
using App.Modules.Sys.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace App.Modules.Sys.Infrastructure.Services.Configuration
{
    /// <summary>
    /// Loads settings from two sources:
    /// 1. SettingsSchema.yml (metadata: title, description, type, locked, validation)
    /// 2. appsettings.json (values: actual setting values)
    /// 
    /// Merges them together to create complete setting definitions.
    /// </summary>
    public class SettingsDefaultsLoader
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SettingsDefaultsLoader>? _logger;
        private readonly string _schemaPath;

        /// <summary>
        /// Initializes a new instance of SettingsDefaultsLoader.
        /// </summary>
        /// <param name="configuration">Application configuration (for reading appsettings.json)</param>
        /// <param name="schemaPath">Path to settings schema file (defaults to Configuration/SettingsSchema.yml)</param>
        /// <param name="logger">Optional logger</param>
        public SettingsDefaultsLoader(
            IConfiguration configuration,
            string? schemaPath = null, 
            ILogger<SettingsDefaultsLoader>? logger = null)
        {
            _configuration = configuration;
            _schemaPath = schemaPath ?? Path.Combine(AppContext.BaseDirectory, "Configuration", "SettingsSchema.yml");
            _logger = logger;
        }

        /// <summary>
        /// Load complete setting definitions by merging schema and values.
        /// </summary>
        /// <summary>
        /// Load complete setting definitions by merging ALL sources:
        /// 1. Import ALL appsettings.json as locked system settings
        /// 2. Enrich with metadata from SettingsSchema.yml (optional)
        /// 3. Allow schema to define additional user-configurable settings
        /// 
        /// This unifies IConfiguration (standard ASP.NET Core) with YOUR rich settings system.
        /// </summary>
        public async Task<Dictionary<string, SettingDefinition>> LoadDefaultsAsync(CancellationToken ct = default)
        {
            var result = new Dictionary<string, SettingDefinition>();
            
            // STEP 1: Import ALL appsettings.json entries as LOCKED system settings
            // This ensures connection strings, logging config, etc. are locked by default
            _logger?.LogInformation("Importing ALL appsettings.json as locked system settings...");
            ImportAllConfigurationAsLocked(result);
            
            // STEP 2: Load SettingsSchema.yml (optional - just adds metadata)
            var schema = await LoadSchemaAsync(ct);
            
            // STEP 3: Enrich existing settings with schema metadata (if schema exists)
            // OR add new user-configurable settings from schema
            _logger?.LogInformation("Enriching with metadata from SettingsSchema.yml...");
            EnrichWithSchema(result, schema);
            
            _logger?.LogInformation("Loaded {Count} unified settings ({Locked} locked, {Unlocked} unlocked)", 
                result.Count, 
                result.Count(r => r.Value.Locked),
                result.Count(r => !r.Value.Locked));
            
            return result;
        }

        /// <summary>
        /// Import ALL appsettings.json entries as locked system settings.
        /// Creates slash-separated keys from IConfiguration hierarchy.
        /// Accepts BOTH colon and slash separators for consistency.
        /// </summary>
        private void ImportAllConfigurationAsLocked(Dictionary<string, SettingDefinition> settings)
        {
            foreach (var kvp in _configuration.AsEnumerable())
            {
                // Skip null/empty keys and values
                if (string.IsNullOrEmpty(kvp.Key) || kvp.Value == null)
                {
                    continue;
                }
                
                // Normalize path: accept colons (:), slashes (/), and double underscores (__)
                var key = NormalizePath(kvp.Key);
                
                settings[key] = new SettingDefinition
                {
                    Key = key,
                    Title = GetFriendlyTitle(key),
                    Description = $"System configuration setting from appsettings.json",
                    SerializedTypeValue = kvp.Value,
                    SerializedTypeName = InferType(kvp.Value),
                    Locked = true,  // ? All appsettings are locked!
                    Category = InferCategory(key),
                    Order = 0
                };
                
                _logger?.LogTrace("Imported locked setting: {Key} = {Value}", key, kvp.Value);
            }
        }

        /// <summary>
        /// Enrich settings with metadata from schema, or add new user-configurable settings.
        /// Schema entries override locked status if explicitly set to unlocked.
        /// Normalizes all schema keys to use consistent slash separators.
        /// </summary>
        private void EnrichWithSchema(
            Dictionary<string, SettingDefinition> settings, 
            Dictionary<string, SettingSchema> schema)
        {
            foreach (var (schemaKey, schemaEntry) in schema)
            {
                // Normalize schema key (allow both : and / in YAML)
                var key = NormalizePath(schemaKey);
                
                if (settings.TryGetValue(key, out var setting))
                {
                    // Setting exists from appsettings - ENRICH with metadata
                    setting.Title = schemaEntry.Title ?? setting.Title;
                    setting.Description = schemaEntry.Description ?? setting.Description;
                    setting.Category = schemaEntry.Category ?? setting.Category;
                    setting.Order = schemaEntry.Order ?? setting.Order;
                    
                    // Schema can override locked status (useful for making some config unlockable)
                    if (schemaEntry.Locked.HasValue)
                    {
                        setting.Locked = schemaEntry.Locked.Value;
                    }
                    
                    // Add validation rules from schema
                    setting.Validation = schemaEntry.Validation;
                    
                    _logger?.LogTrace("Enriched setting: {Key} with schema metadata", key);
                }
                else
                {
                    // Setting NOT in appsettings - add from schema (user-configurable)
                    settings[key] = new SettingDefinition
                    {
                        Key = key,
                        Title = schemaEntry.Title ?? key.Split('/').Last(),
                        Description = schemaEntry.Description ?? string.Empty,
                        SerializedTypeValue = schemaEntry.Default?.ToString() ?? string.Empty,
                        SerializedTypeName = schemaEntry.Type ?? "System.String",
                        Locked = schemaEntry.Locked ?? false,  // Defaults to unlocked
                        Category = schemaEntry.Category ?? "User Settings",
                        Order = schemaEntry.Order ?? 999,
                        Validation = schemaEntry.Validation
                    };
                    
                    _logger?.LogTrace("Added schema-defined setting: {Key}", key);
                }
            }
        }

        /// <summary>
        /// Normalize configuration path to use consistent slash separators.
        /// Accepts ALL common separators for flexibility:
        /// - Colons (:) - Microsoft IConfiguration standard
        /// - Slashes (/) - Our hierarchical standard  
        /// - Double underscores (__) - Environment variable syntax
        /// </summary>
        /// <param name="path">Path with any separator</param>
        /// <returns>Normalized path with slash separators</returns>
        /// <example>
        /// "Settings:System:Security" ? "Settings/System/Security"
        /// "Settings/System/Security" ? "Settings/System/Security"
        /// "Settings__System__Security" ? "Settings/System/Security"
        /// </example>
        private static string NormalizePath(string path)
        {
            return path.Replace(":", "/").Replace("__", "/");
        }

        /// <summary>
        /// Infer type name from string value.
        /// </summary>
        private string InferType(string value)
        {
            if (bool.TryParse(value, out _))
            {
                return "System.Boolean";
            }
            if (int.TryParse(value, out _))
            {
                return "System.Int32";
            }
            if (double.TryParse(value, out _))
            {
                return "System.Double";
            }
            return "System.String";
        }

        /// <summary>
        /// Get friendly title from key.
        /// </summary>
        private string GetFriendlyTitle(string key)
        {
            var lastPart = key.Split('/').Last();
            return lastPart.Replace("_", " ").Replace("-", " ");
        }

        /// <summary>
        /// Infer category from key path.
        /// </summary>
        private string InferCategory(string key)
        {
            var parts = key.Split('/');
            return parts.Length > 1 ? parts[0] : "System";
        }

        /// <summary>
        /// Seed settings repository with defaults (only if not already present).
        /// </summary>
        /// <param name="repository">Settings repository</param>
        /// <param name="ct">Cancellation token</param>
        public async Task SeedDefaultsAsync(ISettingsRepository repository, CancellationToken ct = default)
        {
            var definitions = await LoadDefaultsAsync(ct);
            var seededCount = 0;

            foreach (var (key, definition) in definitions)
            {
                // Check if setting already exists at System level
                var existing = await repository.GetAsync(key, "*", "*", ct);
                if (existing == null)
                {
                    var setting = new SettingValue
                    {
                        WorkspaceId = "*",
                        UserId = "*",
                        Key = key,
                        SerializedTypeName = definition.SerializedTypeName,
                        SerializedTypeValue = definition.SerializedTypeValue,
                        IsLocked = definition.Locked,
                        LastModified = DateTime.UtcNow,
                        ModifiedBy = "SYSTEM_SEED"
                    };

                    await repository.SetAsync(setting, ct);
                    seededCount++;
                    
                    _logger?.LogDebug("Seeded setting: {Key} = {Value}", key, definition.SerializedTypeValue);
                }
            }

            _logger?.LogInformation("Seeded {Count} default settings", seededCount);
        }

        private async Task<Dictionary<string, SettingSchema>> LoadSchemaAsync(CancellationToken ct)
        {
            if (!File.Exists(_schemaPath))
            {
                _logger?.LogWarning("Settings schema not found: {Path}", _schemaPath);
                return new Dictionary<string, SettingSchema>();
            }

            _logger?.LogInformation("Loading settings schema from: {Path}", _schemaPath);
            
            var yaml = await File.ReadAllTextAsync(_schemaPath, ct);
            
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var schema = deserializer.Deserialize<Dictionary<string, SettingSchema>>(yaml);
            return schema ?? new Dictionary<string, SettingSchema>();
        }

        /// <summary>
        /// Setting schema from YAML (metadata only)
        /// </summary>
        private sealed class SettingSchema
        {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? Type { get; set; }
            public bool? Locked { get; set; }
            public string? Category { get; set; }
            public int? Order { get; set; }
            public object? Default { get; set; }
            public ValidationRules? Validation { get; set; }
        }
    }

    /// <summary>
    /// Complete setting definition (schema + value).
    /// Implements IHasTitleAndDescription, IHasKey, and IHasSerializedTypeValueNullable.
    /// </summary>
    public class SettingDefinition : IHasTitleAndDescription, IHasKey, IHasSerializedTypeValueNullable
    {
        /// <inheritdoc/>
        public string Key { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string Title { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string Description { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string SerializedTypeName { get; set; } = "System.String";

        /// <inheritdoc/>
        public string? SerializedTypeValue { get; set; } = string.Empty;

        /// <summary>
        /// Whether this setting is locked at this level
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Category for grouping in UI
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Display order within category
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Validation rules
        /// </summary>
        public ValidationRules? Validation { get; set; }
    }

    /// <summary>
    /// Validation rules for settings
    /// </summary>
    public class ValidationRules
    {
        public object? Min { get; set; }
        public object? Max { get; set; }
        public string? Pattern { get; set; }
        public List<string>? AllowedValues { get; set; }
    }
}

