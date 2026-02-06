using App.Modules.Sys.Infrastructure.Domains.Diagnostics;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.DbContexts.Implementations;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services;
using App.Modules.Sys.Shared.Models.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Infrastructure.Repositories.Implementations.Base;

/// <summary>
/// Generic repository base providing cross-cutting query concerns and DbContext access.
/// Enforces soft-delete filtering by default with extension points for future security rules.
/// Returns IQueryable for composability (critical for OData and performance).
/// </summary>
/// <typeparam name="T">Entity type with Guid identifier.</typeparam>
/// <remarks>
/// <para>
/// <b>DbContext Access Pattern:</b>
/// - Repository is Scoped (created once per HTTP request)
/// - Injects IScopedDbContextProviderService (also Scoped)
/// - Use GetDbContext&lt;TDbContext&gt;() to resolve DbContext from request scope
/// - Same DbContext instance throughout the entire request
/// </para>
/// </remarks>
public abstract class GenericRepositoryBase<T> where T : class, IHasGuidId
{
    private readonly IScopedDbContextProviderService _dbProvider;
    
    /// <summary>
    /// Logger for diagnostic information.
    /// </summary>
    protected IAppLogger Logger { get; }

    /// <summary>
    /// Initializes a new instance of the repository base.
    /// </summary>
    /// <param name="dbProvider">Provider for scoped DbContext access.</param>
    /// <param name="logger">Logger instance.</param>
    protected GenericRepositoryBase(IScopedDbContextProviderService dbProvider, IAppLogger logger)
    {
        _dbProvider = dbProvider;
        Logger = logger;
    }

    /// <summary>
    /// Get DbContext for the current request.
    /// Returns the same DbContext instance throughout the request.
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type to retrieve</typeparam>
    /// <returns>The scoped DbContext instance</returns>
    /// <remarks>
    /// This method resolves the DbContext from the request's service provider.
    /// Since both this repository and the provider are Scoped, they share the same request lifetime.
    /// </remarks>
    protected TDbContext GetDbContext<TDbContext>() where TDbContext : DbContext
    {
        return _dbProvider.GetDbContext<TDbContext>();
    }

    /// <summary>
    /// Convenience property - returns ModuleDbContext for this module.
    /// </summary>
    protected ModuleDbContext Context => GetDbContext<ModuleDbContext>();

    /// <summary>
    /// Returns base query with cross-cutting concerns applied.
    /// Automatically filters soft-deleted records unless explicitly included.
    /// Override ApplySecurityFilters() for custom security rules.
    /// IMPORTANT: Returns IQueryable for composability - DO NOT materialize here!
    /// </summary>
    /// <param name="includeSoftDeleted">Whether to include soft-deleted records.</param>
    /// <returns>Composable IQueryable for further filtering (e.g., OData, service layer).</returns>
    protected IQueryable<T> Query(bool includeSoftDeleted = false)
    {
        var query = Context.Set<T>().AsQueryable();

        // Apply soft-delete filter if entity supports auditability
        if (!includeSoftDeleted && typeof(T).GetInterfaces().Any(i => 
            i.Name == "IHasInRecordAuditability" || i.FullName?.Contains("IHasInRecordAuditability") == true))
        {
            // Use dynamic filtering for soft-delete (DeletedOnUtc)
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
            var property = System.Linq.Expressions.Expression.Property(parameter, "DeletedOnUtc");
            var nullConstant = System.Linq.Expressions.Expression.Constant(null, typeof(DateTime?));
            var isNull = System.Linq.Expressions.Expression.Equal(property, nullConstant);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(isNull, parameter);
            
            query = query.Where(lambda);
        }

        // Extension point for security filters (workspace isolation, ACLs, etc.)
        query = ApplySecurityFilters(query);

        return query; // ✅ Still IQueryable - composable!
    }

    /// <summary>
    /// Override in derived classes to add security filtering (workspace isolation, ACLs, etc.).
    /// Default: no filtering (suitable for system-level entities).
    /// MUST return IQueryable - do not materialize!
    /// </summary>
    /// <param name="query">Base query to filter.</param>
    /// <returns>Filtered query (still IQueryable).</returns>
    protected virtual IQueryable<T> ApplySecurityFilters(IQueryable<T> query)
    {
        // Extension point - override when security model is finalized
        return query;
    }

    /// <summary>
    /// Gets entity by ID with all filters applied.
    /// </summary>
    protected async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await Query()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    /// <summary>
    /// Checks if entity exists with all filters applied.
    /// </summary>
    protected async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        return await Query()
            .AnyAsync(x => x.Id == id, ct);
    }

    /// <summary>
    /// Adds entity with audit trail using IHasInRecordAuditability.
    /// </summary>
    protected async Task AddAsync(T entity, CancellationToken ct = default)
    {
        // Apply audit info using existing Substrate contract
        if (entity is IHasInRecordAuditability auditEntity)
        {
            auditEntity.CreatedOnUtc = DateTime.UtcNow;
            auditEntity.LastModifiedOnUtc = DateTime.UtcNow;
            // TODO: auditEntity.CreatedByPrincipalId = CurrentUserId when ICurrentUserService is available
            // TODO: auditEntity.LastModifiedByPrincipalId = CurrentUserId
        }

        await Context.Set<T>().AddAsync(entity, ct);
        await Context.SaveChangesAsync(ct);
        
        Logger.LogInformation($"Added {typeof(T).Name} with ID {entity.Id}");
    }

    /// <summary>
    /// Updates entity with audit trail using IHasInRecordAuditability.
    /// </summary>
    protected async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        // Apply audit info using existing Substrate contract
        if (entity is IHasInRecordAuditability auditEntity)
        {
            auditEntity.LastModifiedOnUtc = DateTime.UtcNow;
            // TODO: auditEntity.LastModifiedByPrincipalId = CurrentUserId when ICurrentUserService is available
        }

        Context.Set<T>().Update(entity);
        await Context.SaveChangesAsync(ct);
        
        Logger.LogInformation($"Updated {typeof(T).Name} with ID {entity.Id}");
    }

    /// <summary>
    /// Soft-delete entity (preferred over hard delete).
    /// Uses IHasInRecordAuditability.DeletedOnUtc.
    /// </summary>
    protected async Task SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await Context.Set<T>().FindAsync(new object[] { id }, ct);
        
        if (entity == null)
        {
            Logger.LogWarning($"{typeof(T).Name} with ID {id} not found for soft delete");
            return;
        }

        // Apply soft-delete using existing Substrate contract
        if (entity is IHasInRecordAuditability auditEntity)
        {
            auditEntity.DeletedOnUtc = DateTime.UtcNow;
            auditEntity.LastModifiedOnUtc = DateTime.UtcNow;
            // TODO: auditEntity.DeletedByPrincipalId = CurrentUserId when ICurrentUserService is available
            // TODO: auditEntity.LastModifiedByPrincipalId = CurrentUserId
            
            await Context.SaveChangesAsync(ct);
            Logger.LogInformation($"Soft-deleted {typeof(T).Name} with ID {id}");
        }
        else
        {
            Logger.LogWarning($"{typeof(T).Name} does not support soft-delete (missing IHasInRecordAuditability)");
        }
    }

    /// <summary>
    /// Hard-delete entity (use sparingly - prefer soft-delete).
    /// </summary>
    protected async Task HardDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await Context.Set<T>().FindAsync(new object[] { id }, ct);
        
        if (entity == null)
        {
            Logger.LogWarning($"{typeof(T).Name} with ID {id} not found for hard delete");
            return;
        }

        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync(ct);
        
        Logger.LogWarning($"HARD-DELETED {typeof(T).Name} with ID {id}");
    }
}

