using App.Modules.Sys.Application.CodeQuality.Models;
using App.Modules.Sys.Application.CodeQuality.Services;
using App.Modules.Sys.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.CodeQuality.Services.Implementations;

/// <summary>
/// STUB implementation of code quality analysis service.
/// Returns placeholder data until Roslyn project is implemented.
/// </summary>
/// <remarks>
/// TODO: Replace with real RoslynCodeAnalysisService from Infrastructure.Roslyn project.
/// This stub allows endpoints to work without crashing.
/// </remarks>
internal sealed class StubCodeQualityAnalysisService : ICodeQualityAnalysisService
{
    private readonly IEnvironmentService _environmentService;

    public StubCodeQualityAnalysisService(IEnvironmentService environmentService)
    {
        _environmentService = environmentService;
    }

    public Task<CodeAnalysisReportDto> AnalyzeCodebaseAsync(CancellationToken cancellationToken = default)
    {
        var stubReport = new CodeAnalysisReportDto
        {
            AnalyzedAt = System.DateTime.UtcNow,
            Duration = System.TimeSpan.FromSeconds(0),
            AssembliesAnalyzed = 0,
            TotalIssues = 0,
            CriticalIssues = 0,
            WarningIssues = 0,
            InfoIssues = 0,
            HealthScore = 100,
            Summary = new CodeQualitySummaryDto
            {
                Status = "Not Implemented",
                KeyFindings = new List<string>
                {
                    "Code quality analysis is not yet implemented",
                    "Create Infrastructure.Roslyn project to enable analysis"
                },
                Recommendations = new List<string>
                {
                    "See INFRASTRUCTURE-ROSLYN-IMPLEMENTATION-GUIDE.md for implementation steps"
                }
            }
        };

        return Task.FromResult(stubReport);
    }

    public CodeAnalysisReportDto? GetCachedResults()
    {
        return null; // No cached results in stub
    }

    public bool IsAnalysisAvailable()
    {
        return false; // Stub is never available for real analysis
    }

    public AnalysisCapabilitiesDto GetCapabilities()
    {
        return new AnalysisCapabilitiesDto
        {
            IsEnabled = false,
            Environment = _environmentService.EnvironmentName,
            EnabledAnalyzers = new List<string>
            {
                "STUB: No analyzers implemented yet"
            },
            Rules = new List<AnalysisRuleDto>
            {
                new AnalysisRuleDto
                {
                    Id = "STUB",
                    Name = "Stub Implementation",
                    Description = "This is a stub. Create Infrastructure.Roslyn project for real implementation.",
                    Category = "System",
                    Severity = "Info"
                }
            }
        };
    }
}
