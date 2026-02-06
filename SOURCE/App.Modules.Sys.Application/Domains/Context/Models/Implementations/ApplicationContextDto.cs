namespace App.Modules.Sys.Application.Domains.Context.Models.Implementations;

/// <summary>
/// Complete application context (aggregates all context levels).
/// This is the root DTO returned by /api/sys/v1/context endpoint.
/// </summary>
public record ApplicationContextDto
{
    /// <summary>
    /// System-level context (platform information).
    /// </summary>
    public SystemContextDto System { get; init; } = new();

    /// <summary>
    /// Workspace/tenant-level context (null if not in workspace context).
    /// </summary>
    public WorkspaceContextDto? Workspace { get; init; }

    /// <summary>
    /// User-level context (null for anonymous users).
    /// </summary>
    public UserContextDto? User { get; init; }

    /// <summary>
    /// Current session information.
    /// </summary>
    public SessionContextDto Session { get; init; } = new();

    /// <summary>
    /// Navigation context (current location, breadcrumbs, menu structure).
    /// </summary>
    public NavigationContextDto Navigation { get; init; } = new();

    /// <summary>
    /// Computed/merged settings (after hierarchy resolution).
    /// </summary>
    public ComputedSettingsDto Settings { get; init; } = new();

    /// <summary>
    /// Server timestamp when context was generated.
    /// </summary>
    public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
}
