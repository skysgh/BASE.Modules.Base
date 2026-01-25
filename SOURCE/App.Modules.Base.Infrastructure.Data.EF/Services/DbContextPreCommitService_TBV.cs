using App.Modules.Base.Infrastructure.Factories;
using App.Modules.Base.Infrastructure.Storage.Db.EF.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace App.Base.Infrastructure.Services.Implementations
{
    //using App.Modules.Base.Infrastructure.Db.DbContextFactories;
    //using App.Modules.Base.Infrastructure.Db.DbContextFactories.Implementations;
    /// <summary>
    ///     Implementation of the
    ///     <see cref="IDbContextPreCommitService" />
    ///     Infrastructure Service Contract
    /// to pre-process all new/updated/modified entities
    /// belonging to a specific DbContext, before 
    /// they are saved.
    /// <para>
    /// This service implementation is invoked because
    /// the various DbContext implementations (eg: AppDbContext)
    /// override their SaveChanges method to do so
    /// TODO: currently it's not automatically handled from the IUnitOfWorkService implementation.
    /// </para>
    /// </summary>
    /// <seealso cref="App.Base.Infrastructure.Services.IDbContextPreCommitService" />
    public class DbContextPreCommitService : IDbContextPreCommitService
    {

        IDbCommitPreCommitProcessingStrategy[] _processors;

        /// <summary>
        /// Constructor
        /// </summary>
        public DbContextPreCommitService()
        {
            _processors =
                ServiceLocator
                    .GetAll
                        <IDbCommitPreCommitProcessingStrategy>()
                        .ToArray();

        }

        /// <summary>
        /// Pass all entities belonging to the specified DbContext
        /// through all implementations of 
        /// <see cref="IDbCommitPreCommitProcessingStrategy"/>
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public void PreProcess(DbContext dbContext)
        {
            _processors.ForEach(x => x.Process(dbContext));
        }
    }
}
