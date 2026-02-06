using App.Modules.Sys.Shared.Lifecycles;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Modules.Sys.Shared.Services
{
    /// <summary>
    /// Contract that Infrastructure and Domain 
    /// services must implement to be discoverable
    /// at startup via reflection.
    /// </summary>
    public interface IHasService : IHasSingletonLifecycle
    {
    }
}
