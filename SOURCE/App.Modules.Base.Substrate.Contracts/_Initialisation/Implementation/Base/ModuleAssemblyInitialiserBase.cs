using System;

namespace App.Modules.Base.Initialisation.Implementation.Base
{
    /// <summary>
    /// Optional base class for module-specific custom initialization.
    /// Override DoBeforeBuild() or DoAfterBuild() if you need custom logic.
    /// </summary>
    public abstract class ModuleAssemblyInitialiserBase : IModuleAssemblyInitialiser
    {
        /// <summary>
        /// Override to add custom logic BEFORE services are registered.
        /// Runs during Phase 1 - IServiceProvider not yet available.
        /// </summary>
        protected virtual void DoBeforeBuild() 
        { 
            // Override if needed
        }
        
        /// <summary>
        /// Override to add custom logic AFTER IServiceProvider is built.
        /// Runs during Phase 2 - can resolve services from DI.
        /// </summary>
        protected virtual void DoAfterBuild() 
        { 
            // Override if needed
        }
    }
}

