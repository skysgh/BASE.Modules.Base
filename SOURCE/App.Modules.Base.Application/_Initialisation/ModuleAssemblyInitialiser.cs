using App.Modules.Base.Domain.Initialisation;
using App.Modules.Base.Initialisation;
using App.Modules.Base.Initialisation.Implementation.Base;
using App.Modules.Base.Substrate.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Modules.Base.Application.Initialisation
{
    /// <summary>
    /// Assembly specific implementation of
    /// <see cref="IModuleAssemblyInitialiser"/>
    /// </summary>
    public class ModuleAssemblyInitialiser : IModuleAssemblyInitialiser
    {
        /// <inheritdoc/>
        public void DoAfterBuild()
        {
        }

        ///<inheritdoc/>
        public void DoBeforeBuild()
        {
        }
    }

}
