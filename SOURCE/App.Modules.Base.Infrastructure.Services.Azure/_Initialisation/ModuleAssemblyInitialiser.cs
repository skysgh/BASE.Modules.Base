using App.Modules.Base.Initialisation.Implementation.Base;

namespace App.Modules.Base.Infrastructure.Services.Azure.Initialisation
{
    /// <summary>
    /// Optional custom initializer for Azure infrastructure services.
    /// Override DoBeforeBuild() or DoAfterBuild() if needed.
    /// </summary>
    public class InfrastructureAzureModuleAssemblyInitialiser : ModuleAssemblyInitialiserBase
    {
        protected override void DoBeforeBuild()
        {
            // Custom logic before DI builds
        }
        
        protected override void DoAfterBuild()
        {
            // Custom logic after DI builds (has IServiceProvider)
        }
    }
}



