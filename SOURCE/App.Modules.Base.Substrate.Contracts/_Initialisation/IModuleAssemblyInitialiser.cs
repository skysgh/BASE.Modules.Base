using System;
using System.Collections.Generic;
using System.Text;

namespace App.Modules.Base.Initialisation
{
    public interface IModuleAssemblyInitialiser
    {
        void DoBeforeBuild();
        void DoAfterBuild();
    }
}
