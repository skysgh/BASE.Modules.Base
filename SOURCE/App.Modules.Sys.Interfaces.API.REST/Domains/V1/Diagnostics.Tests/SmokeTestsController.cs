using App.Modules.Sys.Application.Domains.Diagnostics.Models;
using App.Modules.Sys.Application.Domains.Diagnostics.SmokeTesting.Services;
using App.Modules.Sys.Interfaces.API.REST.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace App.Modules.Sys.Interfaces.Domains.V1.Diagnostics.Tests;

/// <summary>
/// Smoke Test API - System health validation and smoke tests.
/// Provides visibility into infrastructure readiness and data integrity.
/// Uses Application layer facade service (returns DTOs, not domain objects).
/// </summary>
[Route(ApiRoutes.Rest.V1.Diagnostics.SmokeTests)]
[Authorize] // Secure by default
public class SmokeTestsController : ControllerBase
{
    private readonly ISmokeTestApplicationService _smokeTestService;

    /// <summary>
    /// Constructor - Inject Application Service (not Infrastructure!).
    /// </summary>
    public SmokeTestsController(ISmokeTestApplicationService smokeTestService)
    {
        _smokeTestService = smokeTestService;
    }

    /// <summary>
    /// Query smoke test results with OData support.
    /// Supports $filter, $orderby, $top, $skip for flexible client-side querying.
    /// </summary>
    /// <remarks>
    /// OData query examples:
    /// - ?$filter=Status eq 'Failed'
    /// - ?$filter=Category eq 'Database' and CriticalFailures gt 0
    /// - ?$orderby=StartUtc desc&amp;$top=20
    /// - ?$filter=contains(Title, 'Database')&amp;$orderby=StartUtc
    /// 
    /// Standard OData syntax - no custom query parameters needed.
    /// </remarks>
    [HttpGet("query")]
    [EnableQuery(MaxTop = 100, 
                 AllowedQueryOptions = AllowedQueryOptions.Filter | 
                                      AllowedQueryOptions.OrderBy | 
                                      AllowedQueryOptions.Top | 
                                      AllowedQueryOptions.Skip |
                                      AllowedQueryOptions.Count)]
    [ProducesResponseType(typeof(IQueryable<SmokeTestResultDto>), 200)]
    public IQueryable<SmokeTestResultDto> QueryTests()
    {
        // Service returns IQueryable<Dto>
        // OData middleware handles filtering, sorting, paging
        // Materializes before HTTP response
        return _smokeTestService.GetTestsQueryable();
    }

    /// <summary>
    /// Run all smoke tests and return results.
    /// </summary>
    /// <remarks>
    /// Executes all discovered smoke tests in dependency order.
    /// Tests with failed dependencies are skipped.
    /// Use for on-demand health validation.
    /// </remarks>
    [HttpPost("run")]
    [ProducesResponseType(typeof(SmokeTestSummaryDto), 200)]
    public async Task<IActionResult> RunAllTests(CancellationToken cancellationToken)
    {
        var summary = await _smokeTestService.RunAllTestsAsync(cancellationToken);
        return Ok(summary);
    }

    /// <summary>
    /// Get last smoke test results (cached).
    /// </summary>
    /// <remarks>
    /// Returns results from the most recent test run.
    /// Does not execute tests - use POST /run to execute.
    /// Returns Unknown status if tests have never been run.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(SmokeTestSummaryDto), 200)]
    public IActionResult GetLastResults()
    {
        var summary = _smokeTestService.GetLastResults();
        return Ok(summary);
    }

    /// <summary>
    /// Get smoke test results filtered by category.
    /// </summary>
    /// <param name="category">Category to filter by (e.g., "Configuration", "Connection/Database").</param>
    /// <remarks>
    /// Returns only tests matching the specified category.
    /// Supports hierarchical categories with forward slashes.
    /// For more flexible querying, use GET /query with OData syntax.
    /// </remarks>
    [HttpGet("by-category/{category}")]
    [ProducesResponseType(typeof(List<SmokeTestResultDto>), 200)]
    public IActionResult GetByCategory(string category)
    {
        var results = _smokeTestService.GetResultsByCategory(category);
        return Ok(results);
    }

    /// <summary>
    /// Get overall system health status (for CI/CD).
    /// </summary>
    /// <remarks>
    /// Lightweight endpoint for health checks.
    /// Returns: Healthy, Degraded, Unhealthy, or Unknown.
    /// Suitable for container orchestrators and load balancers.
    /// </remarks>
    [HttpGet("health")]
    [AllowAnonymous] // Health check should be accessible
    [ProducesResponseType(typeof(HealthStatusResponse), 200)]
    public IActionResult GetHealthStatus()
    {
        var summary = _smokeTestService.GetLastResults();
        
        return Ok(new HealthStatusResponse
        {
            Status = summary.OverallStatus,
            CriticalFailures = summary.CriticalFailures,
            LastChecked = summary.LastRunAt
        });
    }
}

/// <summary>
/// Lightweight health status response for CI/CD pipelines.
/// </summary>
public record HealthStatusResponse
{
    /// <summary>
    /// Overall health status (Healthy, Degraded, Unhealthy, Unknown).
    /// </summary>
    public string Status { get; init; } = "Unknown";

    /// <summary>
    /// Number of critical test failures.
    /// </summary>
    public int CriticalFailures { get; init; }

    /// <summary>
    /// When tests were last run.
    /// </summary>
    public DateTime? LastChecked { get; init; }
}


