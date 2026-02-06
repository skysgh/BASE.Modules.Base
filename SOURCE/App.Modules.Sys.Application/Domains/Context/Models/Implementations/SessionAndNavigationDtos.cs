using System;

namespace App.Modules.Sys.Application.Domains.Context.Models.Implementations;

/// <summary>
/// Session context information.
/// </summary>
public record SessionContextDto
{
    /// <summary>
    /// Session unique identifier.
    /// </summary>
    public Guid? Id { get; init; }

    /// <summary>
    /// Whether user is authenticated.
    /// </summary>
    public bool IsAuthenticated { get; init; }

    /// <summary>
    /// When session expires (null = never expires).
    /// </summary>
    public DateTime? ExpiresAt { get; init; }

    /// <summary>
    /// Last activity timestamp.
    /// </summary>
    public DateTime? LastActivityAt { get; init; }
}

/// <summary>
/// Navigation context (where am I in the app?).
/// Represents the concentric circles: Service → WorkItem → Page → Verb.
/// </summary>
public record NavigationContextDto
{
    /// <summary>
    /// Current route/path in SPA.
    /// </summary>
    public string? CurrentRoute { get; init; }

    /// <summary>
    /// Breadcrumb trail showing navigation hierarchy.
    /// </summary>
    public List<BreadcrumbDto> Breadcrumbs { get; init; } = new();

    /// <summary>
    /// Primary navigation menu items.
    /// </summary>
    public List<NavigationItemDto> PrimaryMenu { get; init; } = new();

    /// <summary>
    /// Secondary/utility navigation items.
    /// </summary>
    public List<NavigationItemDto> SecondaryMenu { get; init; } = new();
}

/// <summary>
/// Breadcrumb navigation item.
/// </summary>
public record BreadcrumbDto
{
    /// <summary>
    /// Display label.
    /// </summary>
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// Route/path.
    /// </summary>
    public string Route { get; init; } = string.Empty;

    /// <summary>
    /// Whether this is the current (active) breadcrumb.
    /// </summary>
    public bool IsCurrent { get; init; }
}

/// <summary>
/// Navigation menu item.
/// </summary>
public record NavigationItemDto
{
    /// <summary>
    /// Unique identifier for this menu item.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Display label (localized).
    /// </summary>
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// Route/path.
    /// </summary>
    public string Route { get; init; } = string.Empty;

    /// <summary>
    /// Icon identifier (e.g., "home", "list", "settings").
    /// </summary>
    public string? Icon { get; init; }

    /// <summary>
    /// Badge count (e.g., notifications, unread items).
    /// </summary>
    public int? BadgeCount { get; init; }

    /// <summary>
    /// Whether this item is currently active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Child menu items (for hierarchical navigation).
    /// </summary>
    public List<NavigationItemDto> Children { get; init; } = new();
}
