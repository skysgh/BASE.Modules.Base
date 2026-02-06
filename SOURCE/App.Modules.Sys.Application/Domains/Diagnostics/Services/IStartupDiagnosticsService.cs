using App.Modules.Sys.Application.Domains.Diagnostics.Models;
using App.Modules.Sys.Shared.Models.Implementations;
using App.Modules.Sys.Shared.Services;

namespace App.Modules.Sys.Application.Domains.Diagnostics.Services
{

    /// <summary>
    /// Application service for startup diagnostics (facade over Infrastructure registry).
    /// Returns DTOs for API consumption.
    /// Stateless scoped service - inherits from IHasService (no need for additional lifecycle attributes).
    /// </summary>
    public interface IStartupDiagnosticsApplicationService : IHasService
    {
        /// <summary>
        /// Gets all startup log entries as DTOs.
        /// Delegates to Infrastructure registry service and maps results.
        /// </summary>
        /// <returns>List of startup log entry DTOs.</returns>
        IReadOnlyList<StartupLogEntryDto> GetAllEntries();

        /// <summary>
        /// Gets startup log entries filtered by tags as DTOs.
        /// </summary>
        /// <param name="tags">Tags to filter by (OR logic).</param>
        /// <returns>List of filtered startup log entry DTOs.</returns>
        IReadOnlyList<StartupLogEntryDto> GetEntriesByTags(params string[] tags);

        /// <summary>
        /// Gets diagnostic summary with optional entry details.
        /// </summary>
        /// <param name="includeEntries">Whether to include individual entries (default: false for performance).</param>
        /// <returns>Startup diagnostics summary DTO.</returns>
        StartupDiagnosticsSummaryDto GetSummary(bool includeEntries = false);


        ///// <summary>
        ///// Begins a new startup log scope.
        ///// Scope auto-finalizes and logs entry on dispose.
        ///// RECOMMENDED: Use with 'using' statement for automatic cleanup.
        ///// </summary>
        ///// <param name="title">Entry title.</param>
        ///// <param name="description">Optional description.</param>
        ///// <param name="tags">Optional tags for categorization (supports hierarchy via "Category/Subcategory").</param>
        ///// <returns>Disposable scope that finalizes on dispose.</returns>
        //IStartupLogScope BeginScope(string title, string? description = null, params string[] tags);

        ///// <summary>
        ///// Manually adds a pre-built log entry (for advanced scenarios).
        ///// Most code should use BeginScope() instead.
        ///// </summary>
        //void LogEntry(StartupLogEntry entry);

        ///// <summary>
        ///// Gets total startup duration (from first entry to last).
        ///// </summary>
        //TimeSpan GetTotalStartupDuration();

        ///// <summary>
        ///// Gets summary statistics (total entries, errors, by tag).
        ///// </summary>
        //StartupDiagnosticsSummary GetSummary();
    }

}