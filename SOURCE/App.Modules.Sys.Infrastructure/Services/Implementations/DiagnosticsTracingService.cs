using System.Collections.Generic;
using System.Threading;
using App.Modules.Sys.Infrastructure.Services;
using App.Modules.Sys.Shared.Models.Enums;

namespace App.Base.Infrastructure.Services.Implementations
{
    /// <summary>
    ///     Implementation of the
    ///     <see cref="IDiagnosticsTracingService" />
    ///     Infrastructure Service Contract
    /// </summary>
    public class DiagnosticsTracingService : IDiagnosticsTracingService
    {
        private static readonly Queue<TraceEntry> _cache = new Queue<TraceEntry>();
        private static TraceLevel _flushLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsTracingService"/> class.
        /// </summary>
        public DiagnosticsTracingService()
        {
            // Needs to be wired up to Application Settings to be dynamic in order to not need a restart
            // when errors start happening.
            _flushLevel = TraceLevel.Verbose;
        }

        /// <inheritdoc/>
        public void Trace(TraceLevel traceLevel, string message, params object[] arguments)
        {
            _cache.Enqueue(new TraceEntry {TracelLevel = traceLevel, Message = message, Args = arguments});

            if (_cache.Count > 100)
            {
                lock (this)
                {
                    while (_cache.Count > 100)
                    {
                        _cache.Dequeue();
                    }
                }
            }
            if (traceLevel <= _flushLevel)
            {
                lock (this)
                {
                    while (_cache.Count > 0)
                    {
                        var x = _cache.Dequeue();
                        if (x != null)
                        {
                            DirectTrace(x.TracelLevel, x.Message, x.Args ?? Array.Empty<object>());
                        }
                    }
                }
            }
        }


        private static void DirectTrace(TraceLevel traceLevel, string message, params object[] arguments)
        {
            const string lineEnding = "\r\n";

            if (arguments != null && arguments.Length > 0)
            {
                message = string.Format(System.Globalization.CultureInfo.InvariantCulture, message, arguments);
            }

            //         var threadId = Thread.CurrentThread.Name?? Thread.CurrentThread.ManagedThreadId.ToString();
            var threadId = Thread.CurrentThread.Name ?? Environment.CurrentManagedThreadId.ToString(System.Globalization.CultureInfo.InvariantCulture);

            switch (traceLevel)
            {
                case TraceLevel.Critical:
                    System.Diagnostics.Trace.Write($"[CRITICAL] {threadId}: {message}{lineEnding}");
                    break;
                case TraceLevel.Error:
                    System.Diagnostics.Trace.Write($"[ERROR...] {threadId}: {message}{lineEnding}");
                    break;
                case TraceLevel.Warn:
                    System.Diagnostics.Trace.Write($"[WARN....] {threadId}: {message}{lineEnding}");
                    break;
                case TraceLevel.Info:
                    System.Diagnostics.Trace.Write($"[INFO....] {threadId}: {message}{lineEnding}");
                    break;
                case TraceLevel.Debug:
                    System.Diagnostics.Trace.Write($"[DEBUG...] {threadId}: {message}{lineEnding}DEBUG: {threadId}: {message}{lineEnding}");
                    break;
                case TraceLevel.Verbose:
                    System.Diagnostics.Trace.Write($"[VERBOSE ] {threadId}: {message}{lineEnding}");
                    break;
            }

        }

        /// <summary>
        /// Traces a message with the specified <typeparamref name="TContext"/> context type, trace level, and message.
        /// </summary>
        /// <typeparam name="TContext">The type representing the context for the trace entry.</typeparam>
        /// <param name="traceLevel">The severity level of the trace.</param>
        /// <param name="message">The message to trace.</param>
        /// <param name="arguments">Optional arguments to format the message.</param>
        public void Trace<TContext>(TraceLevel traceLevel, string message, params object[] arguments)
        {
            DirectTrace(traceLevel, message, arguments);
        }

        /// <summary>
        /// Traces a message with the specified context identifier, trace level, and message.
        /// </summary>
        /// <param name="contextIdentifier"></param>
        /// <param name="traceLevel"></param>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        public void Trace(string contextIdentifier, TraceLevel traceLevel, string message, params object[] arguments)
        {
            // Format the message with the provided arguments
            //string formattedMessage = string.Format(message, arguments);

            // Log the message with the context identifier and trace level
            DirectTrace(traceLevel, message, arguments);
            //Console.WriteLine($"[{traceLevel}] {contextIdentifier}: {formattedMessage}");
        }

        private sealed class TraceEntry
        {
            public object[]? Args;
            public required string Message;
            public TraceLevel TracelLevel;
        }
    }
}
