using System;
using System.Collections.Generic;

namespace App.Modules.Sys.Application.Domains.Settings.Models;

/// <summary>
/// DTO for setting value with metadata.
/// Used for individual setting operations.
/// </summary>
public record SettingDto
{
    /// <summary>
    /// Setting key (e.g., "theme", "language").
    /// </summary>
    public string Key { get; init; } = null!;

    /// <summary>
    /// Setting value (string representation).
    /// </summary>
    public string Value { get; init; } = null!;

    /// <summary>
    /// Value type hint (e.g., "string", "int", "bool", "json").
    /// </summary>
    public string? ValueType { get; init; }

    /// <summary>
    /// Scope level (System, Workspace, User).
    /// </summary>
    public string Scope { get; init; } = null!;

    /// <summary>
    /// Whether this setting is locked (prevents overrides).
    /// </summary>
    public bool IsLocked { get; init; }

    /// <summary>
    /// Optional description/help text.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Optional category for grouping.
    /// </summary>
    public string? Category { get; init; }

    /// <summary>
    /// When this setting was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// DTO for collection of settings (key-value pairs).
/// Used for bulk operations and effective settings resolution.
/// </summary>
public record SettingsCollectionDto
{
    /// <summary>
    /// Dictionary of setting key-value pairs.
    /// </summary>
    public IReadOnlyDictionary<string, string> Settings { get; init; } = new Dictionary<string, string>();

    /// <summary>
    /// Metadata about each setting (type, lock status, etc.).
    /// Optional - only included when requested.
    /// </summary>
    public IReadOnlyDictionary<string, SettingDto>? Metadata { get; init; }
}

/// <summary>
/// DTO for updating a setting value.
/// </summary>
public record UpdateSettingDto
{
    /// <summary>
    /// New value for the setting.
    /// </summary>
    public string Value { get; init; } = null!;

    /// <summary>
    /// Optional value type hint.
    /// </summary>
    public string? ValueType { get; init; }

    /// <summary>
    /// Whether to lock this setting (admin only).
    /// Only applicable for System and Workspace scopes.
    /// </summary>
    public bool? IsLocked { get; init; }
}
