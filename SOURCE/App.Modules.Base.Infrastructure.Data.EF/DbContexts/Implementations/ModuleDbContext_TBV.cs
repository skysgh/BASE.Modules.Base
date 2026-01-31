using App.Modules.Base.Infrastructure.Data.EF.DbContexts.Implementations.Base;
using App.Modules.Base.Infrastructure.Data.EF.Schema.Management;
using App.Modules.Base.Infrastructure.Data.EF.Schema.Implementations;
using App.Modules.Base.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using App.Modules.Base.Infrastructure.Constants;


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
        /// Constructor with dependency injection support for runtime scenarios.
        /// </summary>
        /// <param name="options">The DbContext options configured via DI.</param>
        /// <param name="modelBuilderOrchestrator">Orchestrator for model building.</param>
        /// <param name="dbContextPreCommitService">Pre-commit processing service (IDbContextPreCommitService for runtime).</param>
        /// <param name="loggerFactory">Logger factory for diagnostics.</param>
        public ModuleDbContext(
            DbContextOptions<ModuleDbContext> options,
            IModelBuilderOrchestrator? modelBuilderOrchestrator = null,
            object? dbContextPreCommitService = null,
            ILoggerFactory? loggerFactory = null)
            : base(options, modelBuilderOrchestrator, dbContextPreCommitService, loggerFactory)
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

    /// <summary>
    /// Design-time factory for EF Core migrations and tooling.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This factory is automatically discovered and used by:
    /// - <c>dotnet ef migrations add</c>
    /// - <c>dotnet ef database update</c>
    /// - PowerShell EF commands
    /// </para>
    /// <para>
    /// <b>Design Philosophy:</b>
    /// - NO ServiceProvider dependency
    /// - Minimal orchestrator (convention-based)
    /// - Null-object pattern for pre-commit service
    /// - Works standalone for migrations
    /// </para>
    /// <para>
    /// <b>Connection String Priority:</b>
    /// 1. Command-line argument (highest)
    /// 2. Environment variable "ConnectionStrings__Default"
    /// 3. LocalDB default (fallback)
    /// </para>
    /// </remarks>
    public class ModuleDbContextFactory : IDesignTimeDbContextFactory<ModuleDbContext>
    {
        /// <inheritdoc/>
        public ModuleDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ModuleDbContext>();

            // Connection string resolution for design-time
            var connectionString = args.Length > 0
                ? args[0]  // From command line
                : Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                    ?? "Server=(localdb)\\mssqllocaldb;Database=AppModuleDb;Trusted_Connection=True;MultipleActiveResultSets=true";

            optionsBuilder.UseSqlServer(connectionString);

            // Create minimal dependencies for design-time
            // Use simple convention-based orchestrator (no DI required)
            var modelBuilderOrchestrator = new DesignTimeModelBuilderOrchestrator();
            
            // Use null-object pattern for pre-commit service
            var preCommitService = NullDbContextPreCommitService.Instance;

            return new ModuleDbContext(
                optionsBuilder.Options,
                modelBuilderOrchestrator,
                preCommitService,
                loggerFactory: null);
        }
    }
}


