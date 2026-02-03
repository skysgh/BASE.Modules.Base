using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using App.Modules.Sys.Infrastructure.Data.EF.Schema.Management;
using App.Modules.Sys.Infrastructure.Data.EF.Schema.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using App.Base.Infrastructure.Services;

namespace App.Modules.Sys.Infrastructure.Data.EF.DbContexts.Implementations.Base
{

    /// <summary>
    /// This is the base class that 
    /// *all* DbContexts in the app inherit from.
    /// <para>
    /// It provides overrides of the default <c>SaveChanges()</c> method
    /// that ensure cleanup. 
    /// </para>
    /// <para>
    /// For example, the filling in of basic tracking and
    /// auditing attributes if the contract implements specific interfaces.
    /// </para>
    /// </summary>
    public abstract class ModuleDbContextBase : DbContext
    {

        /// <summary>
        /// Each DbContext manages its own 
        /// distinct schema in the database.
        /// <para>
        /// In most cases, its identical to the 
        /// <c>ModuleConstants</c> short Key.
        /// </para>
        /// <para>
        /// Note: Except for the default schema, whose name is ''.
        /// </para>
        /// </summary>
        protected string SchemaKey { get; set; } = string.Empty;


        /// <summary>
        /// The Model Builder Orchestrator that 
        /// will be used to coordinate the development
        /// of this Logical Module's DbContext schema model.
        /// </summary>
        protected IModelBuilderOrchestrator ModelBuilderOrchestrator
        {
            get { return _modelBuilderOrchestrator; }
        }
        /// <summary>
        /// 
        /// </summary>
        private readonly IModelBuilderOrchestrator _modelBuilderOrchestrator;


        /// <summary>
        /// The Service invoked in the Save method
        /// to orchestate the pre-commit checking/cleaning up
        /// of entities before they are persisted.
        /// </summary>
        /// <remarks>
        /// Can be either IDbContextPreCommitService (runtime) or the internal null service (design-time).
        /// Both implement PreProcess(DbContext) method.
        /// </remarks>
        protected IDbContextPreCommitService? DbContextPreCommitService
        {
            get
            {
                return _dbContextPreCommitService;
            }
        }
        private readonly IDbContextPreCommitService? _dbContextPreCommitService;

        /// <summary>
        /// Primary constructor with dependency injection support.
        /// </summary>
        /// <param name="options">The DbContext options.</param>
        /// <param name="modelBuilderOrchestrator">Optional orchestrator for model building. If null, creates default for design-time.</param>
        /// <param name="dbContextPreCommitService">Optional pre-commit service. If null, creates no-op for design-time. Can be IDbContextPreCommitService or internal null service.</param>
        /// <param name="loggerFactory">Optional logger factory for diagnostics.</param>
        /// <remarks>
        /// This constructor supports both runtime (with DI) and design-time (migrations) scenarios.
        /// For design-time, nullable parameters allow EF tooling to create minimal instances.
        /// </remarks>
        protected ModuleDbContextBase(
            DbContextOptions options,
            IModelBuilderOrchestrator? modelBuilderOrchestrator = null,
            IDbContextPreCommitService? dbContextPreCommitService = null,
            ILoggerFactory? loggerFactory = null) :
            base(options)
        {
            // Use provided dependencies or create defaults for design-time scenarios
            _modelBuilderOrchestrator = modelBuilderOrchestrator 
                ?? new RunTimeModelBuilderOrchestrator(Enumerable.Empty<Type>());

            _dbContextPreCommitService = dbContextPreCommitService;

            if (loggerFactory != null)
            {
                WireUpLogging(loggerFactory);
            }
        }

        private void WireUpLogging(ILoggerFactory loggerFactory)
        {
            //Wire up the logging feature to the Diagnostics Trace feature
            string typeName = GetType().FullName ?? GetType().Name;
            loggerFactory.CreateLogger(typeName);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(msg => Trace.WriteLine(msg));

            base.OnConfiguring(optionsBuilder);
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
            InternalModelCreating(modelBuilder, SchemaKey);
        }


        /// <summary>
        /// Method used to setup the model for this
        /// DbContext only.
        /// <para>
        /// Invoked by
        /// <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)"/>
        /// </para>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="schemaKey"></param>
        protected virtual void InternalModelCreating(ModelBuilder modelBuilder, string? schemaKey)
        {
            if (schemaKey == null)
            {
                schemaKey = SchemaKey;
            }

            Assembly assembly = GetType().Assembly;

            // Set the schema name first
            // so that all the following models don't
            // have to be explicit
            // about them each time.
            modelBuilder.HasDefaultSchema(schemaKey);



            // Then invoke the service used to build up
            // a full schema by reflection...
            // Then invoke it, passing it the model builder that
            // needs filling in.
            ModelBuilderOrchestrator.Initialize(modelBuilder, assembly);


            //Call base in case it ever does something.
            base.OnModelCreating(modelBuilder);
        }


        /// <summary>
        /// Intercept all saves in order to clean up loose ends
        /// <para>
        /// Invokes the pre-commit service (either runtime or design-time no-op version).
        /// </para>
        /// <para>
        /// Usual tasks are filling in auditing attributes on certain objects
        /// that implement certain interfaces, 
        /// etc.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // Use dynamic to call PreProcess - works for both IDbContextPreCommitService and null service
            if (_dbContextPreCommitService!=null)
            {
                _dbContextPreCommitService.PreProcess(this);
            }
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        /// <summary>
        /// Intercept all saves in order to clean up loose ends
        /// <para>
        /// Invokes the pre-commit service (either runtime or design-time no-op version).
        /// </para>
        /// <para>
        /// Usual tasks are filling in auditing attributes on certain objects
        /// that implement certain interfaces, 
        /// etc.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            if (_dbContextPreCommitService != null)
            {
                _dbContextPreCommitService.PreProcess(this);
            }

            return base.SaveChanges();
        }


        /// <summary>
        /// Intercept all saves in order to clean up loose ends
        /// <para>
        /// Invokes the pre-commit service (either runtime or design-time no-op version).
        /// </para>
        /// <para>
        /// Usual tasks are filling in auditing attributes on certain objects
        /// that implement certain interfaces, 
        /// etc.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (_dbContextPreCommitService != null)
            {
                _dbContextPreCommitService.PreProcess(this);
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        /// <summary>
        /// Intercept all saves in order to clean up loose ends
        /// <para>
        /// Invokes the pre-commit service (either runtime or design-time no-op version).
        /// </para>
        /// <para>
        /// Usual tasks are filling in auditing attributes on certain objects
        /// that implement certain interfaces, 
        /// etc.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_dbContextPreCommitService != null)
            {
                _dbContextPreCommitService.PreProcess(this);
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
