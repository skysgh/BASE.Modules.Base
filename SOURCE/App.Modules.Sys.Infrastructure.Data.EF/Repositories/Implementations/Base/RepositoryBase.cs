using App.Modules.Sys.Infrastructure.Domains.Diagnostics;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.DbContexts.Implementations;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services;
using App.Modules.Sys.Shared.Lifecycles;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Sys.Infrastructure.Repositories.Implementations.Base;

/// <summary>
/// Simple repository base - provides DbContext access without generic constraints.
/// Use this for repositories whose entities don't implement IHasGuidId.
/// </summary>
public abstract class RepositoryBase : IHasSingletonLifecycle
{
    private readonly IScopedDbContextProviderService _dbProvider;
    
    /// <summary>
    /// Logger for diagnostic information.
    /// </summary>
    protected IAppLogger Logger { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbProvider">Provider for scoped DbContext access</param>
    /// <param name="logger">Logger instance</param>
    protected RepositoryBase(IScopedDbContextProviderService dbProvider, IAppLogger logger)
    {
        _dbProvider = dbProvider;
        Logger = logger;
    }

    /// <summary>
    /// Get DbContext for the current request.
    /// </summary>
    protected TDbContext GetDbContext<TDbContext>() where TDbContext : DbContext
    {
        return _dbProvider.GetDbContext<TDbContext>();
    }

    /// <summary>
    /// Convenience property - returns ModuleDbContext.
    /// </summary>
    protected ModuleDbContext Context => GetDbContext<ModuleDbContext>();
}
