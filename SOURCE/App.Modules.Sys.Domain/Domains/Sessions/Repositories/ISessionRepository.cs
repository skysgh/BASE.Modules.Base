using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Modules.Sys.Domain.Domains.Sessions.Models;
using App.Modules.Sys.Shared.Lifecycles;

namespace App.Modules.Sys.Domain.Domains.Sessions.Repositories
{
    /// <summary>
    /// Repository interface for Session aggregate.
    /// Application layer depends on this interface, not implementation.
    /// </summary>
    public interface ISessionRepository: IHasSingletonLifecycle
    {
        /// <summary>
        /// Get sessions with filtering and pagination.
        /// </summary>
        Task<IEnumerable<Session>> GetSessionsAsync(
            int skip,
            int take,
            bool activeOnly,
            CancellationToken ct = default);

        /// <summary>
        /// Get session by ID.
        /// </summary>
        Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Check if session exists.
        /// </summary>
        Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Get operations for a session.
        /// </summary>
        Task<IEnumerable<SessionOperation>> GetOperationsAsync(
            Guid sessionId,
            int skip,
            int take,
            CancellationToken ct = default);
    }
}
