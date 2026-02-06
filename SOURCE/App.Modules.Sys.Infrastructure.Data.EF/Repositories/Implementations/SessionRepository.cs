using App;
using App.Modules.Sys.Domain.Domains.Sessions.Models;
using App.Modules.Sys.Domain.Domains.Sessions.Repositories;
using App.Modules.Sys.Infrastructure.Domains.Diagnostics;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.DbContexts.Implementations;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services;
using App.Modules.Sys.Infrastructure.Repositories.Implementations.Base;
using App.Modules.Sys.Shared.Lifecycles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Infrastructure.Repositories.Implementations
{
    /// <summary>
    /// EF Core implementation of ISessionRepository.
    /// Inherits from GenericRepositoryBase for DbContext access and common repository operations.
    /// Auto-registered via IHasScopedLifecycle marker interface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Lifetime: Scoped</b> - Created once per HTTP request
    /// </para>
    /// <para>
    /// <b>DbContext Access:</b>
    /// Uses GetDbContext&lt;ModuleDbContext&gt;() or Context property from base class.
    /// Base class injects IScopedDbContextProviderService which resolves DbContext from request scope.
    /// </para>
    /// </remarks>
    public class SessionRepository : RepositoryBase, ISessionRepository
    {
        /// <summary>
        /// Initializes a new instance of the SessionRepository class.
        /// </summary>
        /// <param name="dbProvider">Provider for scoped DbContext access</param>
        /// <param name="logger">Logger instance</param>
        public SessionRepository(IScopedDbContextProviderService dbProvider, IAppLogger logger) 
            : base(dbProvider, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            int skip,
            int take,
            bool activeOnly,
            CancellationToken ct = default)
        {
            var query = Context.Sessions
                .Include(s => s.Operations)
                .AsQueryable();

            if (activeOnly)
            {
                query = query.Where(s =>
                    s.TerminatedAt == null &&
                    (s.ExpiresAt == null || s.ExpiresAt > DateTime.UtcNow));
            }

            return await query
                .OrderByDescending(s => s.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<Session?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await Context.Sessions
                .Include(s => s.Operations)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        {
            return await Context.Sessions.AnyAsync(s => s.Id == id, ct);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SessionOperation>> GetOperationsAsync(
            Guid sessionId,
            int skip,
            int take,
            CancellationToken ct = default)
        {
            return await Context.SessionOperations
                .Where(o => o.SessionId == sessionId)
                .OrderByDescending(o => o.Timestamp)
                .Skip(skip)
                .Take(take)
                .ToListAsync(ct);
        }
    }
}

