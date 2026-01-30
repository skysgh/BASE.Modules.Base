using Microsoft.Extensions.Logging;
using App.Modules.Base.Substrate.Contracts.Models.Contracts.Enums;

namespace App.Modules.Base.Substrate.Models.Messages
{
    /// <summary>
    /// Running Log of what has been instantiated 
    /// during Startup.
    /// </summary>
    public class StartupLog
    {
        private static readonly StartupLog startupLog = new();

        /// <summary>
        /// Log a message in <see cref="Journal"/>.
        /// <para>
        /// Use ONLY during startup!
        /// </para>
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void Log(LogLevel level, string message)
        {
            Journal.Add($"{level}: {message}");
        }
        /// <summary>
        /// Singleton instance
        /// </summary>
        public static StartupLog Instance { get; set; } = startupLog;

        /// <summary>
        /// Journal of messages developed during startup
        /// </summary>
        public List<string> Journal { get; set; } = [];

        /// <summary>
        /// Exceptions occuring durin startup.
        /// </summary>
        public List<string> Notes { get; set; } = [];

        /// <summary>
        /// Exceptions occuring durin startup.
        /// </summary>
        public List<Exception> Exceptions { get; set; } = [];


        /// <summary>
        /// Services found
        /// </summary>
        public List<ImplementationDetails> Services { get; set; } = [];

        /// <summary>
        /// ObjectMapping Maps
        /// </summary>
        public List<ImplementationDetails> ObjectMapDefinitions { get; set; } = [];

        /// <summary>
        /// OData Model Definitions
        /// </summary>
        public List<ImplementationDetails> ODataModelDefinitions { get; set; } = [];

        /// <summary>
        /// DbContexts
        /// </summary>
        public List<ImplementationDetails> DbContexts { get; set; } = [];

        /// <summary>
        /// Handlers for DbContexts
        /// </summary>
        public List<ImplementationDetails> DbContextSaveHandlers { get; set; } = [];

        /// <summary>
        /// Controllers found
        /// </summary>
        public List<ImplementationDetails> Controllers { get; set; } = [];


        /// <summary>
        /// Lamar Log
        /// </summary>
        public string WhatDidIScan { get; set; }
        /// <summary>
        /// Lamar Log
        /// </summary>
        public string WhatDoIHave { get; set; }



        /// <summary>
        /// Structured history entries for initializer execution.
        /// </summary>
        public List<StartupHistoryEntry> History { get; set; } = [];

        public IEnumerable<StartupHistoryEntry> GetFailures() =>
            History.Where(x => x.Status == DisplayStatus.Error);

        public IEnumerable<StartupHistoryEntry> GetWarnings() =>
            History.Where(x => x.Status == DisplayStatus.Warning);

        public DateTime? GetLastStartupTime() =>
            History.OrderByDescending(x => x.StartTimestamp).FirstOrDefault()?.StartTimestamp;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public class ImplementationDetails
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Type Interface { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Type Implementation { get; set; }
    //}
}
