using System.Threading;
using System.Threading.Tasks;
using App.Modules.Sys.Shared.Services;

namespace App.Modules.Sys.Application.Domains.Workspace.Services
{
    /// <summary>
    /// Service for validating workspace IDs.
    /// Used by routing middleware to determine if a segment is a workspace.
    /// </summary>
    public interface IWorkspaceValidationService : IHasService
    {
        /// <summary>
        /// Check if workspace ID exists.
        /// </summary>
        /// <param name="workspaceId">Potential workspace ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if workspace exists</returns>
        Task<bool> WorkspaceExistsAsync(string workspaceId, CancellationToken ct = default);

        /// <summary>
        /// Check if workspace is active.
        /// </summary>
        Task<bool> IsWorkspaceActiveAsync(string workspaceId, CancellationToken ct = default);
    }
}
