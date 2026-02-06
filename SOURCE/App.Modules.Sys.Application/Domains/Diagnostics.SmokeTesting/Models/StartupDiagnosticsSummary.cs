namespace App.Modules.Sys.Application.Domains.Diagnostics.SmokeTesting.Models
{
    /// <summary>
    /// Summary statistics for startup diagnostics.
    /// </summary>
    public record StartupDiagnosticsSummary
    {
        /// <summary>
        /// Total number of log entries.
        /// </summary>
        public int TotalEntries { get; init; }

        /// <summary>
        /// Number of error-level entries.
        /// </summary>
        public int ErrorCount { get; init; }

        /// <summary>
        /// Number of warning-level entries.
        /// </summary>
        public int WarningCount { get; init; }

        /// <summary>
        /// Total startup duration.
        /// </summary>
        public TimeSpan TotalDuration { get; init; }

        /// <summary>
        /// Count of entries grouped by tag.
        /// </summary>
        public Dictionary<string, int> EntriesByTag { get; init; } = new();

        /// <summary>
        /// When startup began.
        /// </summary>
        public DateTime? StartedAt { get; init; }

        /// <summary>
        /// When startup completed.
        /// </summary>
        public DateTime? CompletedAt { get; init; }
    }

}