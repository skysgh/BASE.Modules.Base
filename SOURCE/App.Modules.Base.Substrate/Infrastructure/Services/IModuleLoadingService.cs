// using App.Base.Infrastructure.ModuleManagement;
// using System.Reflection;
// using System.Runtime.Loader;

//namespace App.Modules.Base.Infrastructure.Services
//{

//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
//    /// <summary>
//    /// The mess result.
//    /// </summary>
//    public class MessResult : 
//    {
//        /// <summary>
//        /// Gets or sets the TODO.
//        /// </summary>
//        public AssemblyLoadContext? AssemblyLoadContext { get; set; }
//        /// <summary>
//        /// Gets or sets the assembly. TODO
//        /// </summary>
//        public Assembly? Assembly { get; set; }
//    }
//#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

//    /// <summary>
//    /// Contract for a service to 
//    /// load extension modules,
//    /// static scan (security), 
//    /// determine paths and register them,
//    /// etc.
//    /// </summary>
//    public interface IModuleLoadingService : IHasAppBaseInfrastructureService
//    {
//        /// <summary>
//        /// TODO: State should not be part of a service.
//        /// </summary>
//        ControllerTypeToScopeDictionary Scopes { get; }

//        /// <summary>
//        /// Load the assembly from an upload directory
//        /// (todo, probably should be converted to stream processing
//        /// rather than hard drive file)
//        /// 
//        /// </summary>
//        /// <param name="assemblyFilePath"></param>
//        /// <param name="assemblyResolutionBaseDirectoryPath"></param>
//        /// <returns></returns>
//        public MessResult? Load(string assemblyFilePath, string? assemblyResolutionBaseDirectoryPath = null);


//    }
//}