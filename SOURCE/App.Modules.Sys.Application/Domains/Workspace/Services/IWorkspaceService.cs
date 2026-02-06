using App.Modules.Sys.Application.Domains.Context.Models.Implementations;
using App.Modules.Sys.Shared.Lifecycles;
using App.Modules.Sys.Shared.Services;

namespace App.Modules.Sys.Application.Domains.Workspace.Services;

/// <summary>
/// Service for workspace management and user workspace membership.
/// Auto-registered via IHasScopedLifecycle.
/// </summary>
public interface IWorkspaceService : IHasService
{
    /// <summary>
    /// Gets all workspaces where the specified user is a member.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of workspace summaries.</returns>
    Task<List<WorkspaceSummaryDto>> GetUserWorkspacesAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the detailed information for a specific workspace.
    /// </summary>
    /// <param name="workspaceId">Workspace identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Workspace details, or null if not found.</returns>
    Task<WorkspaceDetailsDto?> GetWorkspaceDetailsAsync(string workspaceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the user's default workspace.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Default workspace details, or null if none set.</returns>
    Task<WorkspaceDetailsDto?> GetDefaultWorkspaceAsync(string userId, CancellationToken cancellationToken = default);
}
