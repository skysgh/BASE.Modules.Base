// using System;
// using System.Collections.Generic;
using System.Globalization;
using App.Modules.Base.Substrate.Contracts.Models;


// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace App.Modules.Base.Infrastructure.Models.Entities
{
    /// <summary>
    /// Encapsulation of a diagnostics log message.
    /// </summary>
    /// <seealso cref="Substrate.Contracts.Models.IHasTitle" />
    /// <seealso cref="Substrate.Contracts.Models.IHasLogLevel" />
    /// <seealso cref="Substrate.Contracts.Models.IHasException" />
    public class DiagnosticsLogEntry : IHasTitle, IHasLogLevel, IHasException
    {

        /// <inheritdoc/>
        public LogLevel Level { get; set; }

        /// <inheritdoc/>
        public Exception? Exception { get; set; }

        /// <inheritdoc/>
        public string Title { get; set; } = string.Empty;


        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsLogEntry"/> class.
        /// </summary>
        public DiagnosticsLogEntry() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsLogEntry"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public DiagnosticsLogEntry(string message, params object?[] args)
        {
            this.Title = string.Format(CultureInfo.InvariantCulture, message, args);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsLogEntry"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
#pragma warning disable IDE0060 // Remove unused parameter
        public DiagnosticsLogEntry(Exception exception, string message, params object?[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            // TODO: do something with the Exception
            this.Title = string.Format(CultureInfo.InvariantCulture, message, args);
        }
    }
}
