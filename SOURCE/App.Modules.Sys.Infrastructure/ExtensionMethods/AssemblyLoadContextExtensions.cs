// using JasperFx.TypeDiscovery;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Reflection;
// using System.Reflection.Metadata;
// using System.Runtime.Loader;
// using System.Text;
// using System.Threading.Tasks;

// Extensions are always put in root namespace
// for maximum usability from elsewhere:

//#pragma warning disable IDE0130 // Namespace does not match folder structure
//namespace App
//#pragma warning restore IDE0130 // Namespace does not match folder structure
//{
//    /// <summary>
//    /// Extension methods to the AssemblyLoadContext type.
//    /// <para>
//    /// Avoid using AppDomain which is 
//    /// Windows environment specific.
//    /// </para>
//    /// </summary>
//    public static class AssemblyLoadContextExtensions
//    {

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
//        /// <param name="assemblyLoadContext">The assemblyloadContext.</param>
//        /// <param name="func">The function.</param>
//        public static void InvokeImplementing<T>(this AssemblyLoadContext assemblyLoadContext, Action<T> func)
//            where T : class
//        {
//            foreach (var foundType in assemblyLoadContext.GetInstantiableTypesImplementing(typeof(T)))
//            {
//                T? tmp = Activator.CreateInstance(foundType) as T;

//                if (tmp != null)
//                {
//                    func(tmp);
//                }
//            }
//        }




//        /// <summary>
//        /// TODO
//        /// </summary>
//        /// <param name="assemblyLoadContext"></param>
//        /// <returns></returns>
//        public static IEnumerable<Type> GetInstantiableTypesImplementing<T>(
//            this AssemblyLoadContext assemblyLoadContext)
//        {
//            return assemblyLoadContext.GetInstantiableTypesImplementing(typeof(T));
//        }

//            /// <summary>
//            /// TODO
//            /// </summary>
//            /// <param name="assemblyLoadContext"></param>
//            /// <param name="type"></param>
//            /// <returns></returns>
//            public static IEnumerable<Type> GetInstantiableTypesImplementing(
//            this AssemblyLoadContext assemblyLoadContext, 
//            Type type)
//        {
//            return type.GetInstantiableDescendentTypes(assemblyLoadContext.Assemblies);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public static Assembly[] LoadAssembliesFromApplicationBaseDirectory(this AssemblyLoadContext assemblyLoadContext, string? path=null)
//        {
//            List<Assembly> results = [];
//            if (string.IsNullOrEmpty(path))
//            {
//                foreach (Assembly assembly in AssemblyFinder.FindAssemblies(a => true, true))
//                {
//                    results.Add(assemblyLoadContext.LoadFromAssemblyPath(assembly.Location));
//                }

//            }
//            else
//            {
//                foreach (Assembly assembly in AssemblyFinder.FindAssemblies(
//                    filter: a => true, 
//                    assemblyPath: path, 
//                    logFailure: s=> {},
//                    includeExeFiles:true))
//                {
//                    results.Add(assemblyLoadContext.LoadFromAssemblyPath(assembly.Location));
//                }
//            }
//            return results.OrderBy(x => x.GetName().Name).ToArray();
//        }

//            /// <summary>
//            /// 
//            /// </summary>
//            /// <param name="assemblyLoadContext"></param>
//            /// <param name="path"></param>
//            /// <param name="assemblyFilter"></param>
//            public static Assembly[] LoadAssembliesAndExecutablesFromPath(
//        this AssemblyLoadContext assemblyLoadContext,
//        string path,
//        Func<Assembly, bool> assemblyFilter)
//        {
//            List<Assembly> results  = [];

//            // Find assemblies:
//            foreach (Assembly assembly in AssemblyFinder.FindAssemblies(
//                filter: assemblyFilter, 
//                assemblyPath: path, 
//                logFailure:
//                 delegate (string txt)
//                {                    
//                    Console.WriteLine("ERROR: Could not retrieve assembly from file. Error: " + txt);
//                }, includeExeFiles: true))
//            {
//                results.Add(assemblyLoadContext.LoadFromAssemblyPath(assembly.Location));
//            }
//            return results.OrderBy(x=>x.GetName().Name).ToArray();
//        }
//    }
//}
