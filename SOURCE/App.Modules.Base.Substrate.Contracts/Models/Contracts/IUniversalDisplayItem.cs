using App.Modules.Base.Substrate.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Modules.Base.Substrate.Contracts.Models.Contracts
{
    /// <summary>
    /// Universal contract for items displayed consistently across the UI.
    /// </summary>
    public interface IUniversalDisplayItem : IHasTitleAndDescription
    {
        string Icon { get; }
        DisplayStatus Status { get; }
        IEnumerable<DisplayAction> AvailableActions { get; }
        IDictionary<string, string> Metadata { get; }
    }

    public enum DisplayStatus
    {
        Success = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    public class DisplayAction
    {
        public string Label { get; set; } = string.Empty;
        public string ActionKey { get; set; } = string.Empty;
        public string? Icon { get; set; }
    }
}
