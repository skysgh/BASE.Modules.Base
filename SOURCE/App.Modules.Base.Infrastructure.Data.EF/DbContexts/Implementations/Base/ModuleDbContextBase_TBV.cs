namespace App.Modules.Base.Infrastructure.Data.EF.DbContexts.Implementations.Base
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Modules.Base.Infrastructure.Factories;
    using App.Modules.Base.Infrastructure.Services;
    using App.Modules.Base.Infrastructure.Singletons;
    using App.Modules.Base.Infrastructure.Storage.Db.EF.Interceptors;
    using App.Modules.Base.Infrastructure.Storage.Db.EF.Schema.Management;
    using App.Modules.Base.Shared.Constants;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

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
        protected string? SchemaKey { get; set; }


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
        protected IModelBuilderOrchestrator _modelBuilderOrchestrator;


        /// <summary>
        /// The Service invoked in the Save method
        /// to orchestate the pre-commit checking/cleaning up
        /// of entities before they are persisted.
        /// </summary>
        protected IDbContextPreCommitService DbContextPreCommitService
        {
            get
            {
                return _dbContextPreCommitService;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected IDbContextPreCommitService _dbContextPreCommitService;



        /// <summary>
        /// Static Constructor
        /// <para>
        /// Only called once. 
        /// </para>
        /// <para>
        /// A key duty is to determine if 
        /// working within IIS or Powershell
        /// as that changes behaviour.
        /// </para>
        /// </summary>
        static ModuleDbContextBase()
        {
            // This static constructor is only called once.
            // So that Migrations can work, when outside of
            // runtime, ensure the following:
            PowershellServiceLocatorConfig.Initialize();

            //AppCoreDatabaseInitializerConfigurer.Configure();

            //            //Database.SetInitializer(new AppModuleDatabaseInitializer());
            //AppModuleDatabaseInitializerConfigurer.Configure();

        }


        /// <summary>
        /// Constructor invokes base with 
        /// default ConnectionString key,
        /// which is how the <c>Default</c> ConnectionString is found 
        /// within in web.config/app.settings
        /// </summary>
        protected ModuleDbContextBase() : 
            this(ModuleConstants.DbConnectionStringName)
        {
        }

        /// <summary>
        /// Constructor to use if passing in the connection string directly.
        /// <para>
        /// Note: please consider why you are not using Convention over Configuration
        /// and why passing around connectionstrings that will have username/passwords
        /// in them.
        /// </para>
        /// <para>
        /// Note that internally the method is invoking the App's ServiceLocator to build 
        /// a DbContextOptions then configure it's ConnectionString, 
        /// and then calls this class' other Constructor.
        /// </para>
        /// </summary>
        /// <param name="connectionStringOrName"></param>
        protected ModuleDbContextBase(string connectionStringOrName) :
            this(
                ServiceLocator
                .Get<DbContextOptionsBuilder<ModuleDbContextBase>>()
                .UseSqlServer(connectionStringOrName)
                .Options)
        {
            // Note that overrides of this method will be
            // invoking a DbContextObptionBuilder --but 
            // typed to the subclass dbcontext(not this one)
            // and end up at the constructor below.

            // In other words...it's probable that this 
            // constructor never gets 
            
        }

        /// <summary>
        /// A protected constructor that accepts a DbConnection
        /// <para>
        /// Note that all the other constructors end up here in the end,
        /// before the constructor on the base class  is called.
        /// </para>
        /// <para>
        /// Note: Does not yet invoke 
        /// <see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)"/>
        /// at this point.
        /// </para>
        /// </summary>
        /// <param name="options"></param>
        protected ModuleDbContextBase(DbContextOptions options) :
            base(options)
        {

            //Solve once:
            _modelBuilderOrchestrator =
                ServiceLocator.Get<IModelBuilderOrchestrator>();

            _dbContextPreCommitService =
                ServiceLocator.Get<IDbContextPreCommitService>();

            WireUpLogging();

        }

        private void WireUpLogging()
        {
            //Wire up the logging feature to the Diagnostics Trace feature
            string typeName = GetType().FullName ?? GetType().Name;
            var loggerFactory = ServiceLocator.Get<ILoggerFactory>();
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
            InternalModelCreating(modelBuilder, this.SchemaKey);
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
                schemaKey = this.SchemaKey;
            }

            Assembly assembly = this.GetType().Assembly;

            // Set the schema name first
            // so that all the following models don't
            // have to be explicit
            // about them each time.
            modelBuilder.HasDefaultSchema(schemaKey);



            // Then invoke the service used to build up
            // a full schema by reflection...
            // Then invoke it, passing it the model builder that
            // needs filling in.
            this.ModelBuilderOrchestrator.Initialize(modelBuilder, assembly);


            //Call base in case it ever does something.
            base.OnModelCreating(modelBuilder);
        }


        /// <summary>
        /// Intercept all saves in order to clean up loose ends
        /// <para>
        /// Uses a service locator to find the service (<see cref="IDbContextPreCommitService"/>) that can find
        /// all the matching tasks (that implement <see cref="IDbCommitPreCommitProcessingStrategy"/>), 
        /// and invoke them one after the other.
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
            _dbContextPreCommitService.PreProcess(this);

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        /// <summary>
        /// Intercept all saves in order to clean up loose ends
        /// <para>
        /// Uses a service locator to find the service (<see cref="IDbContextPreCommitService"/>) that can find
        /// all the matching tasks (that implement <see cref="IDbCommitPreCommitProcessingStrategy"/>), 
        /// and invoke them one after the other.
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
            _dbContextPreCommitService.PreProcess(this);

            return base.SaveChanges();
        }


        /// <summary>
        /// Intercept all saves in order to clean up loose ends
        /// <para>
        /// Uses a service locator to find the service (<see cref="IDbContextPreCommitService"/>) that can find
        /// all the matching tasks 
        /// (that implement 
        /// <see cref="IDbCommitPreCommitProcessingStrategy"/>), 
        /// and invoke them one after the other.
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
            _dbContextPreCommitService.PreProcess(this);

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        /// <summary>
        /// Intercept all saves in order to clean up loose ends
        /// <para>
        /// Uses a service locator to find the service (<see cref="IDbContextPreCommitService"/>) that can find
        /// all the matching tasks 
        /// (that implement 
        /// <see cref="IDbCommitPreCommitProcessingStrategy"/>), 
        /// and invoke them one after the other.
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
            _dbContextPreCommitService.PreProcess(this);

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
