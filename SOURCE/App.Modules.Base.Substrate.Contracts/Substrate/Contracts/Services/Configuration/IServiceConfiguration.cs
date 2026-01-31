using System;
using System.Collections.Generic;
using System.Text;

namespace App.Modules.Base.Substrate.Contracts.Services.Configuration
{
    /// <summary>
    /// Interface for configration objects
    /// to be injected into Servics.
    /// <para>
    /// Configuration objects often are reliant 
    /// on injected services to retrieve 
    /// settings from external sources, such as
    /// environment variables or key vaults.
    /// </para>
    /// </summary>
    public interface IServiceConfiguration
    {
        
    }
}
