using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Modules.Base.Infrastructure.Data.EF.Schema.Management
{

    /// <summary>
    /// Contract for a Service used by a 
    /// DbContext to build up its schema Model.
    /// <para>
    /// A specialised class to define a DbContext model. 
    /// It's extracted from the DbContext itself for SOC
    /// objectives (when done by manual method, can end up with
    /// lots of models, and it gets unwieldy, making DbContext
    /// hard to grock.
    /// </para>
    /// </summary>
    public interface IModelBuilderOrchestrator
    {
        /// <summary>
        /// Invoked from within a
        /// <see cref="DbContext.OnModelCreating(ModelBuilder)"/>.
        /// <para>
        /// It's purpose is to search in assemblies for 
        /// all classes that implement a given contract
        /// (<see cref="IHasAppModuleDbContextModelBuilderInitializer"/>)
        /// to indicate it is part of a database schema.
        /// And invokes them.
        /// </para>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="assemblies"></param>
        void Initialize(ModelBuilder modelBuilder, params Assembly[] assemblies);
    }
}
