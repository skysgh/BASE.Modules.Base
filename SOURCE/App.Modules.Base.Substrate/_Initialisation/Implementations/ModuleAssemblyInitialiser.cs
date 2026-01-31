using App.Modules.Base.Initialisation.Implementation;
using App.Modules.Base.Initialisation.Implementation.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Modules.Base.Initialisation.Implementations
{

    /// <summary>
    /// Assembly specific implementation of
    /// <see cref="IModuleAssemblyInitialiser"/>
    /// </summary>
    public class ModuleAssemblyInitialiser : IModuleAssemblyInitialiser
    {
        public void DoAfterBuild()
        {
        }

        public void DoBeforeBuild()
        {
        }
    }
}
