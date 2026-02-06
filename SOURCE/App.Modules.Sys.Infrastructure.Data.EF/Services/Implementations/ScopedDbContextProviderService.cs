
using App.Modules.Sys.Infrastructure.Services;

using System;

namespace App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services.Implementations
{
    /// <summary>
    /// Implementation of IScopedDbContextProviderService.
    /// Registered as SINGLETON (via IHasProviderService) - uses IContextService to resolve DbContext from request scope.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Lifetime: SINGLETON</b> (via IHasProviderService inheritance)
    /// </para>
    /// <para>
    /// <b>How it works:</b>
    /// 1. This provider is Singleton (can be injected by singleton repositories)
    /// 2. Uses IContextService abstraction (hides HttpContext dependency)
    /// 3. When GetDbContext is called, IContextService resolves from request's service provider
    /// 4. Each request gets its own DbContext instance
    /// </para>
    /// <para>
    /// <b>Why this pattern at 150k concurrent users:</b>
    /// - Repositories are Singleton (zero per-request allocation overhead)
    /// - Provider is Singleton (injected once)
    /// - DbContext resolved at runtime from request scope (correct lifetime)
    /// - IContextService abstraction (no direct framework dependencies)
    /// </para>
    /// </remarks>
    public class ScopedDbContextProviderService : IScopedDbContextProviderService
    {
        private readonly IContextService _contextService;

        /// <summary>
        /// Constructor - Injected with IContextService abstraction
        /// </summary>
        public ScopedDbContextProviderService(IContextService contextService)
        {
            _contextService = contextService ?? throw new ArgumentNullException(nameof(contextService));
        }

        /// <inheritdoc/>
        public TDbContext GetDbContext<TDbContext>() where TDbContext : class
        {
            // Use IContextService abstraction to resolve from request scope
            return _contextService.GetService<TDbContext>();
        }
    }
}




