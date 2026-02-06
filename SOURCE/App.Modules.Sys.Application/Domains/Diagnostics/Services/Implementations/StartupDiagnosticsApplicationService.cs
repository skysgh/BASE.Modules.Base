using App.Modules.Sys.Application.Domains.Diagnostics.Models;
using App.Modules.Sys.Application.Domains.Services.ObjectMapping;
using App.Modules.Sys.Infrastructure.Services;
using App.Modules.Sys.Shared.Models.Implementations;

namespace App.Modules.Sys.Application.Domains.Diagnostics.Services.Implementations;

/// <summary>
/// Application service for startup diagnostics.
/// Facade over Infrastructure IStartupDiagnosticsRegistryService.
/// Delegates to Infrastructure and maps domain objects to DTOs.
/// </summary>
internal sealed class StartupDiagnosticsApplicationService : IStartupDiagnosticsApplicationService
{
    private readonly IStartupDiagnosticsRegistryService _registryService;
    private readonly IObjectMappingService _mappingService;

    /// <summary>
    /// Constructor with Infrastructure dependencies.
    /// </summary>
    public StartupDiagnosticsApplicationService(
        IStartupDiagnosticsRegistryService registryService,
        IObjectMappingService mappingService)
    {
        _registryService = registryService;
        _mappingService = mappingService;
    }

    /// <inheritdoc/>
    public IReadOnlyList<StartupLogEntryDto> GetAllEntries()
    {
        // Get domain objects from registry
        var domainEntries = _registryService.GetAllEntries();
        
        // Map collection using LINQ
        var dtos = domainEntries.Select(item => 
            _mappingService.Map<StartupLogEntry, StartupLogEntryDto>(item)).ToList();
        
        return dtos;
    }

    /// <inheritdoc/>
    public IReadOnlyList<StartupLogEntryDto> GetEntriesByTags(params string[] tags)
    {
        // Get filtered domain objects
        var domainEntries = _registryService.GetEntriesByTags(tags);
        
        // Map to DTOs using LINQ
        var dtos = domainEntries.Select(item => 
            _mappingService.Map<StartupLogEntry, StartupLogEntryDto>(item)).ToList();
        
        return dtos;
    }

    /// <inheritdoc/>
    public StartupDiagnosticsSummaryDto GetSummary(bool includeEntries = false)
    {
        // Get domain snapshot from registry
        var snapshot = _registryService.GetSnapshot();
        
        // Map to DTO
        var dto = _mappingService.Map<StartupDiagnosticsSnapshot, StartupDiagnosticsSummaryDto>(snapshot);
        
        // Optionally exclude entries for performance
        if (!includeEntries)
        {
            dto = dto with { Entries = null };
        }
        
        return dto;
    }
}
