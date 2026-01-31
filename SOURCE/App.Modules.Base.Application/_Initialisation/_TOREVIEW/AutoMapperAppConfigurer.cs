using App.Modules.Core.Infrastructure.Configuration;
using App.Modules.Core.Infrastructure.Services;
using App.Modules.Core.Infrastructure.Singletons;
using App.Modules.Core.Shared.Initialisation;
using App.Modules.Core.Shared.Models.Messages;
using App.Modules.Core.Infrastructure.NewFolder.Services;
using App.Modules.Core.Shared.Initialisation;
using AutoMapper;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Core.Application.Initialisation._TOREVIEW
{
    /// <summary>
    /// An invoked class to configure
    /// Automapper using ServiceLocator discovered implementations
    /// of <see cref="IHasAutomapperInitializer"/>.
    /// </summary>
    public class AutoMapperAppConfigurer : IAppConfigurer
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDiagnosticsTracingService _diagnosticsTracingService;
        private readonly IObjectMappingService objectMappingService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="diagnosticsTracingService"></param>
        /// <param name="objectMappingService"></param>
        public AutoMapperAppConfigurer(
            IServiceProvider serviceProvider,
            IDiagnosticsTracingService diagnosticsTracingService,
            IObjectMappingService objectMappingService)
        {
            this.serviceProvider = serviceProvider;
            _diagnosticsTracingService = diagnosticsTracingService;
            this.objectMappingService = objectMappingService;
        }

        /// <summary>
        /// Configures the specified application builder.
        /// <para>
        /// Invoked from 
        /// TODO
        /// </para>
        /// </summary>
        public void Configure()
        {
            // DbContext Initializer (ie Automigrations onstartup) 
            // can be hard coded, as follows, or done solely via web.config as per bottom of
            // https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/migrations-and-deployment-with-the-entity-framework-in-an-asp-net-mvc-application

            using (var elapsedTime = new ElapsedTime())
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    // You can initialize Map descriptions manually or by Convention over Configuration 
                    // using a combination of common interface and reflection.
                    var autoMapperInitialisers = serviceProvider.GetServices<IHasAutomapperInitializer>();

                    autoMapperInitialisers.ForEach(x => x.Initialize(cfg));

                    // Or if convention/reflection/magic is not your cup of tea, you can do it the old way, creating lots of maps (
                    // one map for each direction for each model):
                    //ObjectMap_Example_ExampleDto.Initialize(cfg);
                    // etc... (more maps)
                });

                try
                {
                    mapperConfiguration.AssertConfigurationIsValid();

                    //Make it go faster:
                    mapperConfiguration.CompileMappings();
                }
                catch (Exception e)
                {
                    AppInformation.StartupLog.Exceptions.Add(e);
                    string instructions = e.Message;
                }

                objectMappingService.SetConfiguration(mapperConfiguration);

                //serviceProvider.GetService<IConfigurationStepService>()
                //    .Register(
                //        ConfigurationStepType.General,
                //        ConfigurationStepStatus.White,
                //        "Automapper",
                //        $"Maps have been installed. Took {elapsedTime.ElapsedText}.");


            }


        }
    }
}