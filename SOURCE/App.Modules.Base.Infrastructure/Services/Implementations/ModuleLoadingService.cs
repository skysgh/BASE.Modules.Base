// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using App.Base.Infrastructure.ModuleManagement;
// using System.Runtime.Loader;
// using App.Modules.Base.Infrastructure.NewFolder.Services;
// using App.Modules.Base.Infrastructure.NewFolder.Services.Configuration;

//namespace App.Modules.Base.Infrastructure.Services.Implementations
//{
//    /// <summary>
//    /// IMplementation of the 
//    /// <see cref="IModuleLoadingService"/>.
//    /// </summary>
//    public class ModuleLoadingService : IModuleLoadingService
//    {

//        private readonly IServiceProvider _serviceProvider;

//        /// <summary>
//        /// The service used to static analysis the code 
//        /// of the assembly to load
//        /// </summary>
//        private readonly IPluginValidationService _pluginValidationService;

//        private readonly IHostApplicationLifetime _applicationLifetime;

//        public ControllerTypeToScopeDictionary Scopes
//        {
//            get
//            {
//                return ControllerTypeToScopeDictionary.Instance;
//            }
//        }

//        /// <summary>
//        /// Contructor
//        /// </summary>
//        /// <param name="serviceProvider"></param>
//        /// <param name="pluginValidationService"></param>
//        /// <param name="applicationLifetime"></param>
//        /// <param name="lifetimeScope"></param>
//        public ModuleLoadingService(
//            IServiceProvider serviceProvider,
//            IPluginValidationService pluginValidationService,
//            IHostApplicationLifetime applicationLifetime)
//        {
//            _serviceProvider = serviceProvider;
//            _pluginValidationService = pluginValidationService;
//            _applicationLifetime = applicationLifetime;
//        }


//        /// <inheritdoc/>
//        public MessResult? Load(string assemblyFilePath, string? assemblyResolutionBaseDirectoryPath = null)
//        {
//            // If not given, 
//            // use same directory of provided assembly:
//            if (string.IsNullOrEmpty(assemblyResolutionBaseDirectoryPath))
//            {
//                assemblyResolutionBaseDirectoryPath =
//                    Path.GetDirectoryName(assemblyFilePath);
//            }

//            // Before loading anything, 
//            // try to check code contents for security reasons:
//            if (!StaticCheckAssembly(assemblyFilePath))
//            {
//                return null;
//            }



//            // Passed static analysis, 
//            // safe or not,
//            // we're going to be loading it.
//            // So we prepare a app custom AssemblyLoadContext,
//            // pointing at its dependency resolution directory:

//            MessResult result = new()
//            {
//                AssemblyLoadContext =
//                new AppModuleLoadContext(assemblyResolutionBaseDirectoryPath!)
//            };

//            //The load it:
//            //var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
//            result.Assembly = result.AssemblyLoadContext.LoadFromAssemblyPath(assemblyFilePath);

//            // Didn't load. So no assembly to return:
//            if (result.Assembly == null)
//            {
//                return null;
//            }

//            //Note that this is not working:
//            // As it can't see basic assemblies, 
//            // such as ones holding File types....
//            // :-(
//            // Maybe another AssemblyLoader is required...
//            // to iterate over assemblies and then dump?

//            return result;
//        }

//        /// <summary>
//        /// Statics the check assembly.
//        /// </summary>
//        /// <param name="assemblyFilePath">The assembly file path.</param>
//        /// <returns></returns>
//        private bool StaticCheckAssembly(string assemblyFilePath)
//        {
//            //Static scan the assembly:
//            ValidationConstraintConfiguration validationConstraintConfiguration =
//                new();

//            validationConstraintConfiguration
//                .Excluded
//                .Words
//                .AddRange(["Activator", "FileStream", "FileReader", "FileWriter", "File"]);

//            if (!_pluginValidationService.ValidateAssembly(assemblyFilePath,
//                validationConstraintConfiguration))
//            {
//                return false;
//            }
//            return true;
//        }

//    }//~class
//}//~ns

