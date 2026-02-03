using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Modules.Sys.Shared.Models.Enums;
using App.Modules.Sys.Shared.Models.Persistence;

namespace App.Modules.Sys.Shared.Models.Implementations
{
    /// <summary>
    /// A single entry in the startup log.
    /// </summary>
    public class StartupLogEntry : UniversalDisplayItem, IHasStartAndEndUtcNullable, IHasException
    {

        /// <inheritdoc/>
        public DateTime? StartUtc { get; set; }
        /// <inheritdoc/>
        public DateTime? EndUtc { get; set; }
        

        /// <summary>
        /// the Calculated duration of the operation.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                if (StartUtc == null)
                {
                    return TimeSpan.Zero;
                }
                if (EndUtc == null)
                {
                    return TimeSpan.Zero;
                }
                return StartUtc.Value - EndUtc.Value;
            }
        }


        /// <inheritdoc/>
        public Exception? Exception { get; set; }


        /// <summary>
        /// Start the entry.
        /// </summary>
        public void Start(string title, string? description=null)
        {
            Title = title;
            Description = description ?? string.Empty;

            StartUtc = DateTime.UtcNow;
        }
        /// <summary>
        /// Finalize the entry.
        /// </summary>
        public void FinalizeEntry()
        {
            EndUtc = DateTime.UtcNow;

            Metadata["Duration"] = $"{Duration.TotalMilliseconds:F0}ms";
            if (Exception != null)
            {
                Metadata["ExceptionType"] = Exception.GetType().Name;
                Metadata["ExceptionMessage"] = Exception.Message;
            }
        }
    }
}