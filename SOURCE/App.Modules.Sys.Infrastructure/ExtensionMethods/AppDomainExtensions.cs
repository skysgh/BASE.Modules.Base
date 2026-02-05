//// Extensions are always put in root namespace
//// for maximum usability from elsewhere:


// Extensions are always put in root namespace
// for maximum usability from elsewhere:

//#pragma warning disable IDE0130 // Namespace does not match folder structure
//namespace App
//#pragma warning restore IDE0130 // Namespace does not match folder structure
//{

//    using System;
//    using System.Collections.Generic;
//    using System.IO;
//    using System.Reflection;
//    using System.Security;
//    using System.Security.Policy;

//    /// <summary>
//    /// Extension methods to the <see cref="AppDomain"/> type.
//    /// </summary>
//    public static class AppDomainExtensions
//    {
//        //See: https://stackoverflow.com/questions/458699/how-do-i-implement-net-plugins-without-using-appdomains

//        ////See: https://stackoverflow.com/questions/4145713/looking-for-a-practical-approach-to-sandboxing-net-plugins

//        /// <summary>
//        /// Gets all derived instantiable types, instantiates them 
//        /// (using <see cref="Activator"/> - *not* <c>AppDependencyLocator</c>!)
//        /// then runs the new instance through the provided action.
//        /// <para>
//        /// Invoked at least when scanning for StructureMap scanners
//        /// in all assemblies.
//        /// </para>
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="appDomain">The application domain.</param>
//        /// <param name="func">The function.</param>
//        public static void InvokeImplementing<T>(this AppDomain appDomain, Action<T> func)
//        {
//            foreach (var foundType in appDomain.GetInstantiableTypesImplementing(typeof(T)))
//            {
//#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
//                var tmp = (T)Activator.CreateInstance(foundType);
//#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

//#pragma warning disable CS8604 // Possible null reference argument.
//                func(tmp);
//#pragma warning restore CS8604 // Possible null reference argument.
//            }
//        }


//        /// <summary>
//        /// Gets all derived instantiable types, within the domain.
//        /// <para>
//        /// An example use case would be to find all API Controllers
//        /// in order to associate them with a specific version.
//        /// </para>
//        /// </summary>
//        /// <param name="appDomain">The application domain.</param>
//        /// <returns>the found types.</returns>
//        public static IEnumerable<Type> GetInstantiableTypesImplementing<T>(this AppDomain appDomain)
//        {
//            return appDomain.GetInstantiableTypesImplementing(typeof(T));
//        }



//        /// <summary>
//        /// Gets all derived instantiable types, within the domain.
//        /// <para>
//        /// An example use case would be to find all API Controllers
//        /// in order to associate them with a specific version.
//        /// </para>
//        /// </summary>
//        /// <param name="appDomain">The application domain.</param>
//        /// <param name="type">The type.</param>
//        /// <returns>the found types.</returns>
//        public static IEnumerable<Type> GetInstantiableTypesImplementing(this AppDomain appDomain, Type type)
//        {
//            return type.GetInstantiableDescendentTypes(appDomain.GetAssemblies());
//        }

//        ///// <summary>
//        ///// 
//        ///// </summary>
//        //public static void LoadAssembliesFromApplicationBaseDirectory(this AppDomain appDomain)
//        //{
//        //    appDomain.LoadAssembliesFromApplicationBaseDirectory(
//        //        (Assembly a) => true
//        //    );
//        //}

//        ///// <summary>
//        ///// 
//        ///// </summary>
//        ///// <param name="appDomain"></param>
//        ///// <param name="assemblyFilter"></param>
//        //public static void LoadAssembliesFromApplicationBaseDirectory(
//        //    this AppDomain appDomain,
//        //    Func<Assembly, bool> assemblyFilter)
//        //{
//        //    foreach (Assembly item in AssemblyFinder.FindAssemblies(
//        //        assemblyFilter))
//        //    {
//        //        Assembly.Load(item.GetName());
//        //    }
//        //}

//        ///// <summary>
//        ///// 
//        ///// </summary>
//        ///// <param name="appDomain"></param>
//        ///// <param name="path"></param>
//        ///// <param name="assemblyFilter"></param>
//        //public static void LoadAssembliesAndExecutablesFromPath(
//        //    this AppDomain appDomain,
//        //    string path,
//        //    Func<Assembly, bool> assemblyFilter)
//        //{
//        //    foreach (Assembly item in AssemblyFinder.FindAssemblies(assemblyFilter, path,
//        //        delegate (string txt)
//        //    {
//        //        Console.WriteLine("Lamar could not load assembly from file " + txt);
//        //    }, includeExeFiles: true))
//        //    {
//        //        Assembly.Load(item.GetName());
//        //    }
//        //}


//        ///// <summary>
//        ///// Extension method to load all assemblies in the Bin directory.
//        ///// This functionality is important at startup: it is so that 
//        ///// they are available by reflection even 
//        ///// if not directly Referenced.
//        ///// <para>
//        ///// Note: Expected to be invoked with AppDomain.CurrentDomain.
//        ///// </para>
//        ///// </summary>
//        ///// <param name="appDomain"></param>
//        ///// <param name="excludeNameFilter"></param>
//        //public static void LoadAllBinDirectoryAssemblies(this AppDomain appDomain, Predicate<string>? excludeNameFilter = null)
//        //{
//        //    throw new Exception("DO NOT USE. Loads assemblies as asked." +
//        //        "But in ASP.NET assemblies are actually coming from a " +
//        //        "different location (hot compile?) and causes assemblies " +
//        //        "to be duplicated...so you get at least 2 controllers of " +
//        //        "everything...Berk.");

//        //    //var binPath =
//        //    //    Path.Combine(appDomain.BaseDirectory,
//        //    //        "bin"); // note: don't use CurrentEntryAssembly or anything like that.

//        //    //if (!Directory.Exists(binPath))
//        //    //{
//        //    //    binPath = appDomain.BaseDirectory;
//        //    //}

//        //    //binPath = LoadAllAssembliesInDir(appDomain, binPath, excludeNameFilter);
//        //}



//        ///// <summary>
//        ///// Returns a new <see cref="AppDomain"/> according to the specified criteria.
//        ///// </summary>
//        ///// <param name="name">The name to be assigned to the new instance.</param>
//        ///// <param name="path">The root folder path in which assemblies will be resolved.</param>
//        ///// <param name="zone">A <see cref="SecurityZone"/> that determines the permission set to be assigned to this instance.</param>
//        ///// <returns></returns>
//        //public static AppDomain CreateSandboxDomain(
//        //    Type type, 
//        //    string name,
//        //    string path,
//        //    SecurityZone zone)
//        //{
//        //    var setup = AppDomainSetup.();

//        //    //    ApplicationBase =
//        //    //    Path.GetFullPath(path)
//        //    //};

//        //    var evidence = new Evidence();
//        //    evidence.AddHostEvidence(new Zone(zone));

//        //    var permissions = SecurityManager.GetStandardSandbox(evidence);

//        //    var strongName = type.Assembly.Evidence.GetHostEvidence<StrongName>();

//        //    AppDomain result =
//        //        AppDomain.CreateDomain(
//        //            name, 
//        //            null, 
//        //            setup, 
//        //            permissions, 
//        //            strongName);
//        //}



//        //        /// <summary>
//        //        /// 
//        //        /// </summary>
//        //        /// <param name="appDomain"></param>
//        //        /// <param name="probDir"></param>
//        //        /// <param name="excludeNameFilter"></param>
//        //        /// <returns></returns>
//        //        public static string LoadAllAssembliesInDir(this AppDomain appDomain, string probDir, 
//        //            Predicate<string>? excludeNameFilter = null)
//        //        {
//        //            var filenamePaths = Directory.GetFiles(probDir, "*.dll", SearchOption.AllDirectories);

//        //            foreach (var filePath in filenamePaths)
//        //            {
//        //                if (excludeNameFilter != null)
//        //                {
//        //                    if (excludeNameFilter.Invoke(filePath))
//        //                    {
//        //                        continue;
//        //                    }
//        //                }
//        //                string message;
//        //                try
//        //                {
//        //                    var loadedAssembly = Assembly.LoadFile(filePath);
//        //                }
//        //#pragma warning disable CS0168 // Variable is declared but never used
//        //                catch (FileLoadException loadEx)
//        //#pragma warning restore CS0168 // Variable is declared but never used
//        //                {
//        //                    message = loadEx.Message;
//        //                } // The Assembly has already been loaded.
//        //#pragma warning disable 168
//        //                catch (BadImageFormatException imgEx)
//        //#pragma warning restore 168
//        //                {
//        //                    message = imgEx.Message;
//        //                } // If a BadImageFormatException exception is thrown, the file is not an assembly.
//        //                catch (Exception e)
//        //                {
//        //                    message = e.Message;
//        //                }
//        //            } // foreach dll

//        //            return probDir;
//        //        }
//    }
//}