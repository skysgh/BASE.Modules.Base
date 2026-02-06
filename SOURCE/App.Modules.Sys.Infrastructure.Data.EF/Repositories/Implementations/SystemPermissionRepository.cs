using App.Modules.Sys.Domain.Domains.Permissions.Models;
using App.Modules.Sys.Domain.Domains.Permissions.Respositories;
using App.Modules.Sys.Infrastructure.Domains.Diagnostics;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services;
using App.Modules.Sys.Infrastructure.Repositories.Implementations.Base;
using App.Modules.Sys.Shared.Lifecycles;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Infrastructure.Repositories.Implementations
{
    /// <summary>
    /// EF Core implementation of ISystemPermissionRepository.
    /// Inherits from GenericRepositoryBase for DbContext access and common repository operations.
    /// Auto-registered via IHasScopedLifecycle marker interface.
    /// </summary>
    public class SystemPermissionRepository : RepositoryBase, 
        ISystemPermissionRepository, IHasScopedLifecycle
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbProvider">Provider for scoped DbContext access</param>
        /// <param name="logger">Logger instance</param>
        public SystemPermissionRepository(IScopedDbContextProviderService dbProvider, IAppLogger logger)
            : base(dbProvider, logger)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SystemPermission>> GetAllAsync(
            string? category = null,
            CancellationToken ct = default)
        {
            var query = Context.SystemPermissions.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            return await query
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Key)
                .ToListAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<SystemPermission?> GetByKeyAsync(
            string key,
            CancellationToken ct = default)
        {
            return await Context.SystemPermissions
                .FirstOrDefaultAsync(p => p.Key == key, ct);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetCategoriesAsync(
            CancellationToken ct = default)
        {
            return await Context.SystemPermissions
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync(ct);
        }
    }
}

