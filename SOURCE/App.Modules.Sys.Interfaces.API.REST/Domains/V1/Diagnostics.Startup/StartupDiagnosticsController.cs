using App.Modules.Sys.Application.Domains.Diagnostics.Models;
using App.Modules.Sys.Application.Domains.Diagnostics.SmokeTesting.Services;
using App.Modules.Sys.Interfaces.Domains.V1.Diagnostics.Startup.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Modules.Sys.Interfaces.Domains.V1.Diagnostics.Startup;

/// <summary>
/// Diagnostics API - Startup diagnostics and system health.
/// Provides visibility into module loading, service registration, database initialization.
/// Uses Application layer facade service (returns DTOs, not domain objects).
/// </summary>
[Route("api/sys/rest/v{version:apiVersion}/diagnostics/startup")]
[Authorize] // Secure by default - only authenticated users
public class StartupDiagnosticsController : ControllerBase
{
    private readonly IStartupDiagnosticsApplicationService _diagnosticsService;

    /// <summary>
    /// Constructor - Inject Application Service (not Infrastructure!).
    /// </summary>
    public StartupDiagnosticsController(IStartupDiagnosticsApplicationService diagnosticsService)
    {
        _diagnosticsService = diagnosticsService;
    }

    /// <summary>
    /// Get complete startup log.
    /// </summary>
    /// <remarks>
    /// Returns all startup log entries with timing, tags, and metadata.
    /// Use for detailed debugging of initialization sequence.
    /// </remarks>
    [HttpGet("startup")]
    [ProducesResponseType(typeof(StartupDiagnosticsResponse), 200)]
    public IActionResult GetStartupLog()
    {
        var entries = _diagnosticsService.GetAllEntries();
        var summary = _diagnosticsService.GetSummary();

        return Ok(new StartupDiagnosticsResponse
        {
            Summary = summary,
            Entries = entries
        });
    }

    /// <summary>
    /// Get startup log summary (statistics only).
    /// </summary>
    /// <remarks>
    /// Returns high-level statistics without full entry details.
    /// Lighter-weight than full startup log.
    /// </remarks>
    [HttpGet("startup/summary")]
    [ProducesResponseType(typeof(StartupDiagnosticsSummary), 200)]
    public IActionResult GetStartupSummary()
    {
        return Ok(_diagnosticsService.GetSummary());
    }

    /// <summary>
    /// Get startup log filtered by tags.
    /// </summary>
    /// <param name="tags">Tags to filter by (comma-separated, OR logic).</param>
    /// <remarks>
    /// Examples:
    /// - ?tags=Module - Show only module discovery
    /// - ?tags=Database/Connection,Database/Migration - Show database-related entries
    /// </remarks>
    [HttpGet("startup/by-tags")]
    [ProducesResponseType(typeof(List<StartupLogEntryDto>), 200)]
    public IActionResult GetStartupLogByTags([FromQuery] string tags)
    {
        var tagArray = tags?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        var entries = _diagnosticsService.GetEntriesByTags(tagArray);
        return Ok(entries);
    }
}
