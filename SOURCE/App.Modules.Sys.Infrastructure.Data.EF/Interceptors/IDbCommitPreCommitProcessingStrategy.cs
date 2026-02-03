using System;
using App.Base.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Sys.Infrastructure.Data.EF.Interceptors
{
    /// <summary>
    /// Contract for implementations of 
    /// Pre-Commit operations. Generally used 
    /// to cleanup new an updated entities to be committed
    /// (add missing dates, etc.)
    /// <para>
    /// Invoked when the Request is wrapping up, 
    /// and invokes <c>IUnitOfWorkService</c>'s 
    /// commit operation, 
    /// which in turn invokes each DbContext's SaveChanges, 
    /// which are individually overridden, to in turn 
    /// invoke <c>IDbContextPreCommitService</c>
    /// which invokes 
    /// all PreCommitProcessingStrategy 
    /// implementations of this contract.
    /// </para>
    /// </summary>
    public interface IDbCommitPreCommitProcessingStrategy
    {
        /// <summary>
        /// Whether the strategy is enabled or not. 
        /// <para>
        /// Default is of course <c>True</c>
        /// </para>
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Execution order (lower runs first). Default: 100.
        /// </summary>
        int Order { get; }


        /// <summary>
        /// The *interface* an Entity has to be implementing
        /// to be manageable by this Stratgy.
        /// </summary>
        Type InterfaceType { get; }

        /// <summary>
        /// TODO: Improve documenation
        /// </summary>
        EntityState[] States { get; }

        /// <summary>
        /// Processes the specified database context,
        /// looking for Entities that have characteristics
        /// that match criteria defined by this
        /// ProcessingStrategy implementation.
        /// <para>
        /// Invoked by <see cref="IDbContextPreCommitService"/>
        /// </para>
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        void Process(DbContext dbContext);
    }
}
