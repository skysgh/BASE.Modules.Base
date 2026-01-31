using App.Modules.Base.Substrate.Contracts.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Universal contract for items displayed consistently across the UI.
    /// </summary>
    public interface IUniversalDisplayItem : IHasTitleAndDescription
    {
        string Icon { get; }
        DisplayStatus Status { get; }
        IEnumerable<IUniversalDisplayItemDisplayAction> AvailableActions { get; }
        IDictionary<string, string> Metadata { get; }
    }
}
