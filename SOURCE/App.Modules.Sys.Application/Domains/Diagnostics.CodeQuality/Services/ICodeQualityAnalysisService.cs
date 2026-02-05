using App.Modules.Sys.Application.Domains.Diagnostics.CodeQuality.Models;
using App.Modules.Sys.Shared.Services;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.Domains.Diagnostics.CodeQuality.Services;

/// <summary>
/// Service for runtime code quality analysis using Roslyn.
/// Analyzes codebase for naming conventions, architectural rules, shortcuts, etc.
/// </summary>
/// <remarks>
/// Usage:
/// - Development: Runs on startup (cached for lifetime)
/// - Staging: Runs on startup or on-demand
/// - Production: Disabled (compile-time analysis only)
/// 
/// Integration:
/// - Integrates with StartupDiagnosticsHelper
/// - Results accessible via /api/v1/diagnostics/code-quality
/// - Background scanning available
/// </remarks>
public interface ICodeQualityAnalysisService : IHasService
{
    /// <summary>
    /// Analyze entire codebase for quality issues.
    /// Performs full Roslyn analysis of all application assemblies.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Complete analysis report with all issues.</returns>
    /// <remarks>
    /// Performance: ~2-5 seconds for full solution scan.
    /// Results are cached automatically.
    /// </remarks>
    Task<CodeAnalysisReportDto> AnalyzeCodebaseAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get cached analysis results (if available).
    /// Returns last analysis without re-scanning.
    /// </summary>
    /// <returns>Cached report or null if never analyzed.</returns>
    CodeAnalysisReportDto? GetCachedResults();

    /// <summary>
    /// Check if analysis is available (Development/Staging only).
    /// Returns false in Production.
    /// </summary>
    /// <returns>True if analysis can be performed.</returns>
    bool IsAnalysisAvailable();

    /// <summary>
    /// Get analysis configuration/capabilities.
    /// Returns which analyzers are enabled and their rules.
    /// </summary>
    /// <returns>Configuration summary.</returns>
    AnalysisCapabilitiesDto GetCapabilities();
}

