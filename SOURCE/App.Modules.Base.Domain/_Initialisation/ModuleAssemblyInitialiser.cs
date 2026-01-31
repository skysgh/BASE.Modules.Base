using App.Modules.Base.Initialisation;
using App.Modules.Base.Initialisation.Implementation.Base;
using App.Modules.Base.Initialisation.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Modules.Base.Domain.Initialisation
{
    /// <summary>
    /// Assembly specific implementation of
    /// <see cref="IModuleAssemblyInitialiser"/>
    /// </summary>
    public class DomainModuleAssemblyInitialiser : ModuleAssemblyIntialiserBase
    {
        /// <inheritdoc/>
        public DomainModuleAssemblyInitialiser() : base()
        {
            this.Initialisers.Push(
                new ModuleAssemblyInitialiser());
        }
    }
}
