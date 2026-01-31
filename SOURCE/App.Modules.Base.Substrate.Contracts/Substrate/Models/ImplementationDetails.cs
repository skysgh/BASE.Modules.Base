using System;

namespace App.Modules.Base.Substrate.Models.Messages
{
    /// <summary>
    /// Details about a discovered implementation (service, mapper, schema, etc.)
    /// Used by StartupLog to track what was registered during initialization.
    /// </summary>
    public class ImplementationDetails
    {
        /// <summary>
        /// The interface implemented (if any).
        /// </summary>
        public Type? Interface { get; set; }
        
        /// <summary>
        /// The concrete implementation type.
        /// </summary>
        public Type? Implementation { get; set; }
        
        /// <summary>
        /// Human-readable description of what was registered.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
