using System;
using App.Modules.Base.Substrate.Contracts.Models;

namespace App.Modules.Base.Infrastructure.Web.Initialisation
{
    /// <summary>
    /// Infrastructure.Web custom initialization stub.
    /// 
    /// ⚠️ NOTE: This class is NOW OPTIONAL!
    /// 
    /// Services are automatically discovered via reflection.
    /// 
    /// ✅ FOR CUSTOM LOGIC, USE ONE OF THESE PATTERNS:
    /// 
    /// 🔹 PATTERN 1: Static Constructor (Phase 1 - Before DI Build)
    /// <code>
    /// public class MyService : IHasSingletonLifecycle
    /// {
    ///     static MyService()
    ///     {
    ///         // Runs during type initialization, before DI container builds
    ///         DoCustomSetup();
    ///     }
    /// }
    /// </code>
    /// 
    /// 🔹 PATTERN 2: IServiceConfigurer (Phase 2 - After DI Build)
    /// <code>
    /// public class WebInfrastructureConfigurer : IServiceConfigurer, IHasSingletonLifecycle
    /// {
    ///     public string ServiceName => "WebInfrastructure";
    ///     
    ///     public void ConfigureService(IServiceProvider services, object config, object log)
    ///     {
    ///         // Phase 2 - can now resolve services from IServiceProvider
    ///         var credService = services.GetRequiredService&lt;ICredentialService&gt;();
    ///         
    ///         // Do custom web infrastructure setup
    ///         ConfigureMiddleware();
    ///         RegisterCustomServices();
    ///     }
    /// }
    /// </code>
    /// 
    /// 🔹 PATTERN 3: Service Constructor (Normal DI)
    /// <code>
    /// public class MyService : IHasScopedLifecycle
    /// {
    ///     public MyService(IOtherService dependency)
    ///     {
    ///         // Runs when service is resolved from DI
    ///         DoSetup(dependency);
    ///     }
    /// }
    /// </code>
    /// </summary>
    public class InfrastructureWebModuleAssemblyInitialiser
    {
        // This stub remains for backward compatibility.
        // Use IServiceConfigurer for custom initialization logic.
    }
}

