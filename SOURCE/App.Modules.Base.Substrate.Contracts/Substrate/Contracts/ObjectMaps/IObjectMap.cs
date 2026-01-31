using System;
using System.Collections.Generic;
using System.Text;

namespace App.Modules.Base.Substrate.Contracts.Maps
{
    /// <summary>
    /// Contract for an object map object mapping definition.
    /// </summary>
    public interface IObjectMap
    {
        /// <summary>
        /// The type to map from.
        /// </summary>
        Type From { get; set; }
#pragma warning disable CA1716 // Identifiers should not match keywords
        /// <summary>
        /// The type to map to.
        /// </summary>
        Type To { get; set; }
#pragma warning restore CA1716 // Identifiers should not match keywords
    }
    public interface IObjectMap<TFrom, TTo> : IObjectMap
    {
    }

}
