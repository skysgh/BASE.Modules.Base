// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

using App.Modules.Base.Substrate.Contracts.Services.Configuration;

namespace App.Modules.Base.Infrastructure.NewFolder.Services.Configuration
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// TODO: Document
    /// </summary>
    public class ValidationConstraintConfiguration : IServiceConfiguration
    {
        /// <summary>
        /// TODO: Document
        /// </summary>
        public Permitted Permitted { get; } = new Permitted();
        /// <summary>
        /// TODO: Document
        /// </summary>
        public Excluded Excluded { get; } = new Excluded();

    }


    /// <summary>
    /// TODO: Document
    /// </summary>
    public class Permitted
    {
        /// <summary>
        /// TODO: Document
        /// </summary>
        public List<string> AssemblyNames { get; } = [];
        /// <summary>
        /// TODO: Document
        /// </summary>
        public List<string> Namespaces { get; } = [];
    }
    /// <summary>
    /// TODO: Document
    /// </summary>
    public class Excluded
    {
        /// <summary>
        /// TODO: Document
        /// </summary>
        public List<string> AssemblyNames { get; } = [];
        /// <summary>
        /// TODO: Document
        /// </summary>
        public List<string> Namespaces { get; } = [];
        /// <summary>
        /// TODO: Document
        /// </summary>
        public List<string> Words { get; } = [];
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

}
