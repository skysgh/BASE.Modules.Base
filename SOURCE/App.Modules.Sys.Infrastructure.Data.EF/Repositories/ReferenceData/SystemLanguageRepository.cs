using App.Modules.Sys.Domain.ReferenceData;
using App.Modules.Sys.Domain.ReferenceData.Repositories;
using App.Modules.Sys.Infrastructure.Domains.Diagnostics;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services;
using App.Modules.Sys.Infrastructure.Repositories.Implementations.Base;
using App.Modules.Sys.Shared.Lifecycles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Infrastructure.Data.EF.Repositories.ReferenceData;

/// <summary>
/// EF Core implementation of SystemLanguage repository.
/// Inherits from GenericRepositoryBase for DbContext access and common repository operations.
/// Auto-registered via IHasScopedLifecycle marker interface.
/// </summary>
internal sealed class SystemLanguageRepository : RepositoryBase, 
ISystemLanguageRepository, IHasScopedLifecycle
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbProvider">Provider for scoped DbContext access</param>
    /// <param name="logger">Logger instance</param>
    public SystemLanguageRepository(IScopedDbContextProviderService dbProvider, IAppLogger logger)
        : base(dbProvider, logger)
    {
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<SystemLanguage>> GetAllAsync(bool activeOnly = false, CancellationToken ct = default)
    {
        var query = Context.Set<SystemLanguage>().AsNoTracking();

        if (activeOnly)
        {
            query = query.Where(l => l.IsActive);
        }

        return await query
            .OrderBy(l => l.SortOrder)
            .ThenBy(l => l.Name)
            .ToListAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<SystemLanguage?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return null;
        }

        return await Context.Set<SystemLanguage>()
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Code.Equals(code, StringComparison.OrdinalIgnoreCase), ct);
    }

    /// <inheritdoc/>
    public async Task<SystemLanguage> GetDefaultAsync(CancellationToken ct = default)
    {
        var defaultLang = await Context.Set<SystemLanguage>()
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.IsDefault, ct);

        if (defaultLang == null)
        {
            throw new InvalidOperationException("No default language configured in system. Database seed data missing.");
        }

        return defaultLang;
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string code, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return false;
        }

        return await Context.Set<SystemLanguage>()
            .AsNoTracking()
            .AnyAsync(l => l.Code.Equals(code, StringComparison.OrdinalIgnoreCase), ct);
    }
}

