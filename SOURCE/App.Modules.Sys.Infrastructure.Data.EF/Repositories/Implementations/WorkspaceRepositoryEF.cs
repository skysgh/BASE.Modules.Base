using App;
using App.Modules.Sys.Domain.Domains.Workspaces.Models;
using App.Modules.Sys.Domain.Domains.Workspaces.Repositories;
using App.Modules.Sys.Infrastructure.Domains.Diagnostics;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.DbContexts.Implementations;
using App.Modules.Sys.Infrastructure.Repositories.Implementations.Base;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Sys.Infrastructure.Repositories.Implementations;

/// <summary>
/// EF Core implementation of workspace repository.
/// Inherits cross-cutting concerns from GenericRepositoryBase.
/// </summary>
internal sealed class WorkspaceRepositoryEF : GenericRepositoryBase<Workspace>, IWorkspaceRepository
{
    public WorkspaceRepositoryEF(
        ModuleDbContext context,
        IAppLogger<WorkspaceRepositoryEF> logger)
        : base(context, logger) { }

    // Override when multi-tenancy/security model is finalized
    protected override IQueryable<Workspace> ApplySecurityFilters(IQueryable<Workspace> query)
    {
        // TODO: When workspace isolation model is finalized, add:
        // Option A: Strict isolation
        //   return query.Where(w => w.Id == CurrentWorkspaceId);
        //
        // Option B: Shared-with-rights (ACL-based)
        //   return query.Where(w => w.Id == CurrentWorkspaceId 
        //                        || w.SharedWith.Any(s => s.UserId == CurrentUserId));
        //
        // For now: no filtering (system-level access during development)
        return base.ApplySecurityFilters(query);
    }

    public new async Task<Workspace?> GetByIdAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        return await Query() // ✅ Soft-delete filtered automatically
            .Include(w => w.Members)
            .FirstOrDefaultAsync(w => w.Id == workspaceId, cancellationToken);
    }

    public async Task<List<Workspace>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Query() // ✅ Soft-delete + security filters applied
            .Include(w => w.Members)
            .Where(w => w.Members.Any(m => m.UserId == userId))
            .Where(w => w.IsActive)
            .OrderBy(w => w.Title)
            .ToListAsync(cancellationToken);
    }

    public async Task<Workspace?> GetDefaultWorkspaceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Query() // ✅ Soft-delete + security filters applied
            .Include(w => w.Members)
            .Where(w => w.Members.Any(m => m.UserId == userId && m.IsDefault))
            .Where(w => w.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public new async Task AddAsync(Workspace workspace, CancellationToken cancellationToken = default)
    {
        await base.AddAsync(workspace, cancellationToken); // ✅ Audit trail applied
    }

    public new async Task UpdateAsync(Workspace workspace, CancellationToken cancellationToken = default)
    {
        workspace.UpdatedAt = DateTime.UtcNow;
        await base.UpdateAsync(workspace, cancellationToken); // ✅ Audit trail applied
    }

    public async Task DeleteAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        await SoftDeleteAsync(workspaceId, cancellationToken); // ✅ Soft-delete enforced
    }
}
