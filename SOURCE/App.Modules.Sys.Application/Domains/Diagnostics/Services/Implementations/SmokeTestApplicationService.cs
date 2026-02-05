using App.Modules.Sys.Application.Domains.Diagnostics.Models;
using App.Modules.Sys.Application.Domains.Diagnostics.SmokeTesting.Services;
using App.Modules.Sys.Application.Domains.Services.ObjectMapping;
using App.Modules.Sys.Infrastructure.Domains.Diagnostics;
using App.Modules.Sys.Infrastructure.Domains.Diagnostics.Implementations;
using App.Modules.Sys.Infrastructure.Services;

namespace App.Modules.Sys.Application.Domains.Diagnostics.Services.Implementations;

/// <summary>
/// Application service for smoke tests.
/// Facade over Infrastructure ISmokeTestRegistryService.
/// Delegates execution to Infrastructure and maps domain objects to DTOs.
/// </summary>
internal sealed class SmokeTestApplicationService : ISmokeTestApplicationService
{
    private readonly ISmokeTestRegistryService _registryService;
    private readonly IObjectMappingService _mappingService;

    /// <summary>
    /// Constructor with Infrastructure dependencies.
    /// </summary>
    public SmokeTestApplicationService(
        ISmokeTestRegistryService registryService,
        IObjectMappingService mappingService)
    {
        _registryService = registryService;
        _mappingService = mappingService;
    }

    /// <inheritdoc/>
    public IQueryable<SmokeTestResultDto> GetTestsQueryable()
    {
        // Get last results from registry
        var domainResult = _registryService.GetLastResults();
        
        // Convert to IQueryable and map each item
        // Note: This is in-memory IQueryable (tests already executed)
        return domainResult.Results
            .Select(r => _mappingService.Map<App.Modules.Sys.Infrastructure.Domains.Diagnostics.Implementations.SmokeTestResult, SmokeTestResultDto>(r))
            .AsQueryable();
    }

    /// <inheritdoc/>
    public async Task<SmokeTestSummaryDto> RunAllTestsAsync(CancellationToken cancellationToken = default)
    {
        // Delegate to Infrastructure registry service (returns domain object)
        var domainResult = await _registryService.RunAllTestsAsync(cancellationToken);
        
        // Map domain object to DTO (using discovered mappers)
        var dto = _mappingService.Map<SmokeTestExecutionResult, SmokeTestSummaryDto>(domainResult);
        
        return dto;
    }

    /// <inheritdoc/>
    public SmokeTestSummaryDto GetLastResults()
    {
        // Get domain object from registry
        var domainResult = _registryService.GetLastResults();
        
        // Map to DTO
        var dto = _mappingService.Map<SmokeTestExecutionResult, SmokeTestSummaryDto>(domainResult);
        
        return dto;
    }

    /// <inheritdoc/>
    public IReadOnlyList<SmokeTestResultDto> GetResultsByCategory(string category)
    {
        // Get domain objects from registry
        var domainResults = _registryService.GetResultsByCategory(category);
        
        // Map collection using LINQ (no collection overload in interface)
        var dtos = domainResults.Select(item => 
            _mappingService.Map<SmokeTestResult, SmokeTestResultDto>(item)).ToList();
        
        return dtos;
    }
}
