using App.Modules.Base.Infrastructure.Storage.Db.EF.Schema.Definitions.Conventions;
using App.Modules.Base.Infrastructure.Storage.Db.EF.Schema.Management;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Base.Infrastructure.Data.EF.Schema.Definitions.Conventions.Implementations
{

    /// <summary>
    /// A convention to define a usable Db Table Name 
    /// name for a given entity
    /// (essentially adds an <c>s</c>, <c>ies</c>)
    /// depending on the ending of the entity name.
    /// <para>
    /// TODO: Admittedly it's pretty primitive!!!
    /// </para>
    /// <para>
    /// An implementation of 
    /// <see cref="IModelBuilderConvention"/>
    /// to make defining models easier and more predictable 
    /// when
    /// <see cref="DbContext.OnModelCreating(ModelBuilder)"/>
    /// invokes 
    /// <see cref="IModelBuilderOrchestrator"/>
    /// which inturn invokes instances of
    /// <see cref="IHasAppModuleDbContextModelBuilderInitializer"/>
    /// such as this class.
    /// </para>
    /// </summary>
    public class DefaultTableAndSchemaNamingConvention : IModelBuilderConvention
    {
        /// <summary>
        /// Given an Entity, it adds to the builder
        /// the table name of the model to be defined.
        /// <para>
        /// Generally the first Convention invoked
        /// from within an
        /// <see cref="IHasAppModuleDbContextModelBuilderInitializer"/>
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="schema"></param>
        public void Define<T>(
            ModelBuilder modelBuilder,
            string schema = Shared.Constants.ModuleConstants.DbSchemaKey)
            where T : class
        {
            string name = typeof(T).Name;

            if (name.EndsWith("y"))
            {
                name = name.Substring(0, name.Length - 1) + "ies";
            }
            else
            {
                name += "s";
            }

            modelBuilder.Entity<T>().ToTable(name, schema);

        }
    }

}
