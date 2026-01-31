using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Contracts.Models.Enums;

namespace App.Modules.Base.Substrate.Models.Messages
{
        public class StartupLogEntry : IUniversalDisplayItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = "info-circle";
        public DisplayStatus Status { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime? EndTimestamp { get; set; }
        public TimeSpan Duration { get; set; }
        public int Order { get; set; }
        public string InitializerTypeName { get; set; } = string.Empty;
        public bool ContinueOnFailure { get; set; }
        public Exception? Exception { get; set; }
        public IEnumerable<IUniversalDisplayItemDisplayAction> AvailableActions { get; set; } = Array.Empty<DisplayAction>();
        public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        public void FinalizeEntry()
        {
            Metadata["Duration"] = $"{Duration.TotalMilliseconds:F0}ms";
            Metadata["Timestamp"] = StartTimestamp.ToString("yyyy-MM-dd HH:mm:ss");
            Metadata["Order"] = Order.ToString();
            if (Exception != null)
            {
                Metadata["ExceptionType"] = Exception.GetType().Name;
                Metadata["ExceptionMessage"] = Exception.Message;
            }
        }
    }
}