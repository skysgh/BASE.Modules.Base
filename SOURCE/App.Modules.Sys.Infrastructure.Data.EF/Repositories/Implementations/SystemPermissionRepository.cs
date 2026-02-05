using App.Modules.Sys.Domain.Domains.Permissions.Models;
using App.Modules.Sys.Domain.Domains.Permissions.Respositories;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.DbContexts.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Infrastructure.Repositories.Implementations
{
    /// <summary>
    /// EF Core implementation of ISystemPermissionRepository.
    /// Lives in Infrastructure layer - knows about DbContext.
    /// </summary>
    public class SystemPermissionRepository : ISystemPermissionRepository
    {
        private readonly ModuleDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public SystemPermissionRepository(ModuleDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SystemPermission>> GetAllAsync(
            string? category = null,
            CancellationToken ct = default)
        {
            var query = _context.SystemPermissions.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            return await query
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Key)
                .ToListAsync(ct);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<SystemPermission?> GetByKeyAsync(
            string key,
            CancellationToken ct = default)
        {
            return await _context.SystemPermissions
                .FirstOrDefaultAsync(p => p.Key == key, ct);
        }

/// <summary>
/// TODO
/// </summary>
/// <param name="ct"></param>
/// <returns></returns>
        public async Task<IEnumerable<string>> GetCategoriesAsync(
            CancellationToken ct = default)
        {
            return await _context.SystemPermissions
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync(ct);
        }
    }
}
