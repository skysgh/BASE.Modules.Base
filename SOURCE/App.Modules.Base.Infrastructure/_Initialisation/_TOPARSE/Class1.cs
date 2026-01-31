using App.Base.Infrastructure.IoC;
using App.Modules.DevSpikes.Infrastructure.Services.Implementations;
using App.Modules.DevSpikes.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Modules.DevSpikes.Infrastructure.IoC.Initialisation
{
    /// <summary>
    /// Implementation of 
    /// <see cref="IAppModuleIoCInitialiser"/>
    /// to initialise the provided
    /// <see cref="IServiceCollection"/>
    /// </summary>
    public class AppModuleInitialiser : IAppModuleIoCInitialiser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppModuleInitialiser()
        {

        }
        /// <inheritdoc/>
        public void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IFooService, FooService>();
            serviceCollection.AddSingleton(typeof(IBarService<>), typeof(BarService<>));
        }
    }
}