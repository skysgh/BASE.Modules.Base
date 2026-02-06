using App.Modules.Sys.Application.Domains.Diagnostics.Models;
using App.Modules.Sys.Shared.Services;

namespace App.Modules.Sys.Application.Domains.Diagnostics.SmokeTesting.Services;

/// <summary>
/// Application service for smoke tests (facade over Infrastructure registry).
/// Returns DTOs for API consumption.
/// Stateless scoped service - inherits from IHasService (no need for additional lifecycle attributes).
/// </summary>
public interface ISmokeTestApplicationService : IHasService
{
    /// <summary>
    /// Gets test results as IQueryable for OData support.
    /// Supports flexible client-side filtering and sorting via OData syntax.
    /// </summary>
    /// <returns>IQueryable of test result DTOs.</returns>
    IQueryable<SmokeTestResultDto> GetTestsQueryable();

    /// <summary>
    /// Runs all smoke tests in dependency order.
    /// Delegates to Infrastructure registry service and maps results to DTOs.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>DTO summary of test execution.</returns>
    Task<SmokeTestSummaryDto> RunAllTestsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets cached results from last execution.
    /// </summary>
    /// <returns>DTO summary of last test execution.</returns>
    SmokeTestSummaryDto GetLastResults();

    /// <summary>
    /// Gets test results filtered by category.
    /// </summary>
    /// <param name="category">Category to filter by (supports hierarchy: "Connection/Database").</param>
    /// <returns>List of test result DTOs.</returns>
    IReadOnlyList<SmokeTestResultDto> GetResultsByCategory(string category);
}
