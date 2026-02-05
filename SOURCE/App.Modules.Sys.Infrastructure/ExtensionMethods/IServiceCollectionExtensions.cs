//// using Lamar.Scanning.Conventions;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection.Extensions;
// using System;
// using System.Collections.Generic;
// using System.Linq;
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
//    /// Extension Methods for
//    /// <see cref="IServiceCollection"/>
//    /// </summary>
//    public static partial class IServiceCollectionExtensions
//    {
//        /// <summary>
//        /// Get a list of implementation Types 
//        /// of the given contract/interface Type
//        /// </summary>
//        /// <typeparam name="TContract"></typeparam>
//        /// <param name="serviceCollection"></param>
//        /// <returns></returns>
//        public static IEnumerable<Type> GetImplementationServiceTypes<TContract>(this IServiceCollection serviceCollection)
//        {
//            Type contractType = typeof(TContract);

//            return serviceCollection
//                    .Where(x => (x.ServiceType == contractType) && (x.ImplementationType != null))
//                    .Select(x => x.ImplementationType!);
//        }


//        /// <summary>
//        /// Register all implementations of the specified
//        /// contract.
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="serviceCollection"></param>
//        /// <param name="serviceLifetime"></param>
//        /// <param name="assemblyLoadContext"></param>
//        public static void RegisterAllImplementationsOf<T>(
//            this IServiceCollection serviceCollection,
//            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton,
//            AssemblyLoadContext? assemblyLoadContext = null)
//        {
//            if (assemblyLoadContext == null) { assemblyLoadContext = AssemblyLoadContext.Default; }
//            var types = assemblyLoadContext.GetInstantiableTypesImplementing<T>();

//            foreach (var type in types)
//            {
//                var serviceDescriptor = new
//                    ServiceDescriptor(typeof(T), type, serviceLifetime);

//                serviceCollection
//                    .Add(serviceDescriptor);
//            }

//        }


//        //        using ICSharpCode.Decompiler.Metadata;
//        // using Microsoft.Extensions.DependencyInjection.Extensions;

//        //namespace App
//        //{
//        //    public static class IServiceCollectionExtensions
//        //    {
//        //        public static IServiceCollection CloneNaively(this IServiceCollection source)
//        //        {
//        //            var result = new ServiceCollection();

//        //            foreach (var serv in source)
//        //            {
//        //                result.Add(serv);
//        //            }
//        //            return result;
//        //        }

//        //        public static IServiceCollection CloneIntelligently(this IServiceCollection source, IServiceProvider serviceProvider)
//        //        {
//        //            var childServiceCollection = new ServiceCollection();
//        //            foreach (ServiceDescriptor service in source)
//        //            {
//        //                if (service.Lifetime == ServiceLifetime.Singleton && !service.ServiceType.IsGenericTypeDefinition)
//        //                {
//        //                    object? serviceInstance = serviceProvider.GetService(service.ServiceType);
//        //                    if (serviceInstance != null)
//        //                    {
//        //                        childServiceCollection.AddSingleton(service.ServiceType, serviceInstance);
//        //                        continue;
//        //                    }
//        //                }

//        //                childServiceCollection.Add(service);
//        //            }

//        //            return childServiceCollection;
//        //        }


//        //            public static IServiceCollection CloneNaively(this IServiceCollection source)
//        //            {
//        //                var result = new ServiceCollection();

//        //                foreach (var serv in source)
//        //                {
//        //                    result.Add(serv);
//        //                }
//        //                return result;
//        //            }

//        //            public static IServiceCollection CloneIntelligently(this IServiceCollection source, IServiceProvider serviceProvider)
//        //            {
//        //                var childServiceCollection = new ServiceCollection();
//        //                foreach (ServiceDescriptor service in source)
//        //                {
//        //                    if (service.Lifetime == ServiceLifetime.Singleton && !service.ServiceType.IsGenericTypeDefinition)
//        //                    {
//        //                        object? serviceInstance = serviceProvider.GetService(service.ServiceType);
//        //                        if (serviceInstance != null)
//        //                        {
//        //                            childServiceCollection.AddSingleton(service.ServiceType, serviceInstance);
//        //                            continue;
//        //                        }
//        //                    }

//        //                    childServiceCollection.Add(service);
//        //                }

//        //                return childServiceCollection;
//        //            }

//        //        }
//        //    }

//        //    }
//    }
//}
