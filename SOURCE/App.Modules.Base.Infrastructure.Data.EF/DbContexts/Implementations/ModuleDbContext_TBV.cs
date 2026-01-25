using App.Modules.Base.Infrastructure.Factories;
using App.Modules.Base.Infrastructure.Storage.Db.EF.DbContexts.Implementations.Base;
using App.Modules.Base.Shared.Constants;
using Microsoft.EntityFrameworkCore;


namespace App.Modules.Base.Infrastructure.Data.EF.DbContexts.Implementations
{
    /// <summary>
    /// The Module specific DbContext (notice is has it's own Schema).
    /// <para>
    /// Inherits from the common <see cref="ModuleDbContextBase"/> 
    /// where <c>AppDbContextBase.SaveChanges</c>
    /// and <c>AppDbContextBase.SaveChangesAsync</c>
    /// intercept the save operation, 
    /// to clean up new/updated objects
    /// </para>
    /// <para>
    /// Also (and very importantly) the base class' static Constructor 
    /// ensures its migration capabilities work from the commandline.
    /// </para>
    /// </summary>
    /// <seealso cref="ModuleDbContextBase" />

    //[Alias(Constants.Db.AppCoreDbContextNames.Core)]
    public class ModuleDbContext : ModuleDbContextBase
    {
        /*
        /// <summary>
        /// Expost the Types/Tables specific to this DbContext
        /// </summary>
        public DbSet<NothingDefinedYet>? NothingDefinedYet { get; set; }
        */


        /// <summary>
        /// Constructor
        /// <para>
        /// Constructor invokes base with 
        /// Key ('AppCoreDbContext') used to find the 
        /// ConnectionString in web.config
        /// </para>
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ModuleDbContext() :
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            this(ModuleConstants.DbConnectionStringName)
        {
            // Note:
            // above is passing on to 'this' next constructor
            // and not 'base'
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <para>
        /// Note how its using a ServiceLocator in the middle
        /// to build a <c>DbContextOptions</c> to pass down.
        /// </para>
        /// <param name="connectionStringOrName"></param>
        public ModuleDbContext(string connectionStringOrName)
            : base(
                   ServiceLocator
                  .Get<DbContextOptionsBuilder>()
                  .UseSqlServer(connectionStringOrName)
                  .Options)
        {
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // You can either set this property in the constructor
            // or just before invoking the base, 
            // BUT DONT FORGET IT (or you'll be adding to the 
            // BASE schema....making it harder to remove later.
            this.SchemaKey = ModuleConstants.DbSchemaKey;

            base.OnModelCreating(modelBuilder);
        }

    }
}


