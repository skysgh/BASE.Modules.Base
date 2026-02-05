using App;
using App.Modules.Sys.Domain.Domains.Sessions.Models;
using App.Modules.Sys.Domain.Domains.Sessions.Repositories;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.DbContexts.Implementations;
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
    /// Lives in Infrastructure layer - knows about DbContext.
    /// </summary>
    public class SessionRepository : ISessionRepository
    {
        private readonly ModuleDbContext _context;

        /// <summary>
        /// Initializes a new instance of the SessionRepository class.
        /// </summary>
        /// <param name="context">The database context for session data access.</param>
        public SessionRepository(ModuleDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Session>> GetSessionsAsync(
            int skip,
            int take,
            bool activeOnly,
            CancellationToken ct = default)
        {
            var query = _context.Sessions
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
            return await _context.Sessions
                .Include(s => s.Operations)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Sessions.AnyAsync(s => s.Id == id, ct);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SessionOperation>> GetOperationsAsync(
            Guid sessionId,
            int skip,
            int take,
            CancellationToken ct = default)
        {
            return await _context.SessionOperations
                .Where(o => o.SessionId == sessionId)
                .OrderByDescending(o => o.Timestamp)
                .Skip(skip)
                .Take(take)
                .ToListAsync(ct);
        }
    }
}
