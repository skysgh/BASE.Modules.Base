using System;
using System.Collections.Generic;
using System.Text;

namespace App.Modules.Base.Infrastructure.Identifiers
{
    /// <summary>
    /// Contract for a service to return a unique identifier.
    /// Internally, uses "GuidFactory" to generate the identifier,
    /// which is sequential, unlike the random "Guid.NewGuid()".
    /// </summary>
    public interface IUUIDService
    {
        /// <summary>
        /// Generates a unique identifier.
        /// </summary>
        /// <returns></returns>
        public Guid Generate();
    }
}
