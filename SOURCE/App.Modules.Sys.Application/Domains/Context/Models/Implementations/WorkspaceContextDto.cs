namespace App.Modules.Sys.Application.Domains.Context.Models.Implementations;

/// <summary>
/// Workspace/tenant-level context information.
/// Supports multi-workspace membership.
/// </summary>
public record WorkspaceContextDto
{
    /// <summary>
    /// All workspaces the user is a member of.
    /// </summary>
    public List<WorkspaceSummaryDto> MemberOf { get; init; } = new();

    /// <summary>
    /// Current/active workspace (the one user is operating in).
    /// </summary>
    public WorkspaceDetailsDto? Current { get; init; }
}

/// <summary>
/// Summary information for a workspace (used in memberOf list).
/// </summary>
public record WorkspaceSummaryDto
{
    /// <summary>
    /// Workspace unique identifier.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Workspace display name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// User's role in this workspace.
    /// </summary>
    public string Role { get; init; } = string.Empty;

    /// <summary>
    /// Whether this is the default workspace for the user.
    /// </summary>
    public bool IsDefault { get; init; }
}

/// <summary>
/// Detailed information for the current/active workspace.
/// </summary>
public record WorkspaceDetailsDto
{
    /// <summary>
    /// Workspace unique identifier.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Workspace display name.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Workspace title (may differ from name).
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Workspace description.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Workspace-level branding (tenant customization).
    /// </summary>
    public WorkspaceBrandingDto Branding { get; init; } = new();

    /// <summary>
    /// Contact information for this workspace.
    /// </summary>
    public WorkspaceContactDto Contact { get; init; } = new();

    /// <summary>
    /// Account/subscription information.
    /// </summary>
    public AccountInfoDto Account { get; init; } = new();

    /// <summary>
    /// Resource paths (images, i18n, etc.).
    /// </summary>
    public WorkspaceResourcesDto Resources { get; init; } = new();

    /// <summary>
    /// Account-level feature capabilities (what workspace has paid for).
    /// </summary>
    public Dictionary<string, bool> AccountFeatures { get; init; } = new();

    /// <summary>
    /// UI rendering options/preferences (what user wants to see).
    /// </summary>
    public Dictionary<string, object> UIOptions { get; init; } = new();
}

/// <summary>
/// Workspace contact information.
/// </summary>
public record WorkspaceContactDto
{
    /// <summary>
    /// Contact email address.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Contact phone number.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Physical/postal address.
    /// </summary>
    public AddressDto? Address { get; init; }
}

/// <summary>
/// Physical/postal address.
/// </summary>
public record AddressDto
{
    /// <summary>
    /// Street address.
    /// </summary>
    public string? Street { get; init; }

    /// <summary>
    /// City/locality.
    /// </summary>
    public string? City { get; init; }

    /// <summary>
    /// State/region/province.
    /// </summary>
    public string? Region { get; init; }

    /// <summary>
    /// Postal/ZIP code.
    /// </summary>
    public string? PostalCode { get; init; }

    /// <summary>
    /// Country.
    /// </summary>
    public string? Country { get; init; }
}

/// <summary>
/// Workspace branding customization.
/// </summary>
public record WorkspaceBrandingDto
{
    /// <summary>
    /// Organization name.
    /// </summary>
    public string OrganizationName { get; init; } = string.Empty;

    /// <summary>
    /// Logo URL (default/light mode).
    /// </summary>
    public string? LogoUrl { get; init; }

    /// <summary>
    /// Logo URL for dark mode.
    /// </summary>
    public string? LogoDarkUrl { get; init; }

    /// <summary>
    /// Small logo URL (for compact UI).
    /// </summary>
    public string? LogoSmallUrl { get; init; }

    /// <summary>
    /// Theme colors and styling.
    /// </summary>
    public ThemeDto Theme { get; init; } = new();
}

/// <summary>
/// Theme styling (colors, layout, density).
/// </summary>
public record ThemeDto
{
    /// <summary>
    /// Primary brand color (hex).
    /// </summary>
    public string? PrimaryColor { get; init; }

    /// <summary>
    /// Secondary brand color (hex).
    /// </summary>
    public string? SecondaryColor { get; init; }

    /// <summary>
    /// Accent color (hex).
    /// </summary>
    public string? AccentColor { get; init; }

    /// <summary>
    /// Custom CSS URL (advanced customization).
    /// </summary>
    public string? CustomCssUrl { get; init; }

    // TODO: Add when designing theme entities:
    // public string? LayoutMode { get; init; }  // sidebar-left, top-nav, etc.
    // public string? Density { get; init; }  // comfortable, compact, spacious
    // public string? CornerRadius { get; init; }  // sharp, rounded, pill
    // public string? FontFamily { get; init; }
}

/// <summary>
/// Workspace resource paths.
/// </summary>
public record WorkspaceResourcesDto
{
    /// <summary>
    /// Images root path.
    /// </summary>
    public string? ImagesRoot { get; init; }

    /// <summary>
    /// "Trusted by" logos path.
    /// </summary>
    public string? TrustedByPath { get; init; }

    /// <summary>
    /// i18n translation files path.
    /// </summary>
    public string? I18nPath { get; init; }
}

/// <summary>
/// Account/subscription tier information.
/// </summary>
public record AccountInfoDto
{
    /// <summary>
    /// Subscription tier (Free, Pro, Enterprise).
    /// </summary>
    public string Tier { get; init; } = "Free";

    /// <summary>
    /// Maximum user limit for this account.
    /// </summary>
    public int? MaxUsers { get; init; }

    /// <summary>
    /// Storage limit in GB.
    /// </summary>
    public int? StorageLimitGb { get; init; }

    /// <summary>
    /// Whether this account is in good standing.
    /// </summary>
    public bool IsActive { get; init; } = true;
}
