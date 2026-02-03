using App.Modules.Sys.Infrastructure.Data.EF.Interceptors;
using Microsoft.EntityFrameworkCore;
using App.Modules.Sys.Shared.Models;
using App.Modules.Sys.Shared.Services;
using App.Modules.Sys.Infrastructure.Services;

namespace App.Base.Infrastructure.Services
{
    /// <summary>
    /// Contract for an Infrastructure Service to 
    /// pre-process all new/updated/modified entities
    /// belonging to a specific DbContext, before 
    /// they are saved.
    /// <para>
    /// This service implementation is invoked because
    /// the various DbContext implementations (eg: AppDbContext)
    /// override their SaveChanges method to do so
    /// TODO: currently it's not automatically handled from the IUnitOfWorkService implementation.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Implements <see cref="IHasInfrastructureService"/>,
    /// which is a Module specific specialisation of
    /// <see cref="IHasService"/> in order for the Dependency 
    /// Injection service to easily find it at startup.
    /// </para>
    /// </remarks>
    /// <seealso cref="IHasService" />
    public interface IDbContextPreCommitService : IHasInfrastructureService, IHasInitialize
    {
        /// <summary>
        /// Pass all entities belonging to the specified DbContext
        /// through all implementations of 
        /// <see cref="IDbCommitPreCommitProcessingStrategy"/>
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        void PreProcess(DbContext dbContext);
    }
}
