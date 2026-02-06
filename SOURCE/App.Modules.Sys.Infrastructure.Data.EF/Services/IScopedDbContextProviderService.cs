using System;
using App.Modules.Sys.Shared.Services;

namespace App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services
{
    /// <summary>
    /// Provides scoped DbContext access for singleton services.
    /// Registered as SCOPED (one per request) - resolves any DbContext type from the request's scope.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Problem:</b> Singleton services cannot directly inject scoped DbContext.
    /// </para>
    /// <para>
    /// <b>Solution:</b> 
    /// - This provider is Scoped (created once per HTTP request)
    /// - It holds the request's service provider
    /// - Generic method allows resolving ANY DbContext type
    /// - Returns the request's scoped DbContext instance
    /// </para>
    /// <para>
    /// <b>Usage Pattern:</b>
    /// <code>
    /// public class MySingletonService : IHasService
    /// {
    ///     private readonly IHttpContextAccessor _httpContextAccessor;
    ///     
    ///     public MySingletonService(IHttpContextAccessor httpContextAccessor)
    ///     {
    ///         _httpContextAccessor = httpContextAccessor;
    ///     }
    ///     
    ///     private IScopedDbContextProvider DbProvider =>
    ///         _httpContextAccessor.HttpContext.RequestServices
    ///             .GetRequiredService&lt;IScopedDbContextProvider&gt;();
    ///     
    ///     public async Task&lt;MyData&gt; GetDataAsync()
    ///     {
    ///         var dbContext = DbProvider.GetDbContext&lt;ModuleDbContext&gt;();
    ///         return await dbContext.MyEntities.FirstOrDefaultAsync();
    ///     }
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public interface IScopedDbContextProviderService : IHasProviderService
    {
        /// <summary>
        /// Get the DbContext of the specified type for the current request scope.
        /// Returns the same instance for the entire request.
        /// </summary>
        /// <typeparam name="TDbContext">The DbContext type to resolve</typeparam>
        /// <returns>The scoped DbContext instance</returns>
        TDbContext GetDbContext<TDbContext>() where TDbContext : class;
    }
}


