using App.Modules.Sys.Application.CodeQuality.Models;
using App.Modules.Sys.Application.CodeQuality.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Interfaces.API.REST.Domains.V1.Diagnostics;

/// <summary>
/// API for code quality diagnostics using Roslyn analysis.
/// Provides runtime analysis of codebase quality.
/// </summary>
/// <remarks>
/// Available in Development/Staging only.
/// Production returns disabled status.
/// 
/// Authorization:
/// - Development: Anonymous access allowed
/// - Staging/Production: Requires authentication
/// 
/// Endpoints:
/// - GET /api/v1/diagnostics/code-quality - Get cached analysis results
/// - POST /api/v1/diagnostics/code-quality/scan - Trigger new scan
/// - GET /api/v1/diagnostics/code-quality/capabilities - Get analyzer info
/// </remarks>
[ApiController]
[Route("api/v1/diagnostics/code-quality")]
#if DEBUG
[AllowAnonymous] // Development: No auth required for diagnostics
#else
[Authorize] // Production: Require authentication
#endif
public class CodeQualityDiagnosticsController : ControllerBase
{
    private readonly ICodeQualityAnalysisService _codeQualityAnalysisService;

/// <summary>
/// Constructor
/// </summary>
/// <param name="codeQualityAnalysisService"></param>
/// <exception cref="System.ArgumentNullException"></exception>
    public CodeQualityDiagnosticsController(ICodeQualityAnalysisService codeQualityAnalysisService)
    {
        _codeQualityAnalysisService = codeQualityAnalysisService 
            ?? throw new System.ArgumentNullException(nameof(codeQualityAnalysisService));
    }

    /// <summary>
    /// Get code quality analysis results.
    /// Returns cached analysis from startup or last scan.
    /// </summary>
    /// <returns>Code quality report with all issues found.</returns>
    /// <response code="200">Analysis report returned successfully.</response>
    /// <response code="503">Analysis not available (Production environment).</response>
    [HttpGet]
    [ProducesResponseType(typeof(CodeAnalysisReportDto), 200)]
    [ProducesResponseType(503)]
    public IActionResult GetCodeQualityReport()
    {
        if (!_codeQualityAnalysisService.IsAnalysisAvailable())
        {
            return StatusCode(503, new 
            { 
                message = "Code quality analysis is disabled in Production environment",
                available = false
            });
        }

        var cachedReport = _codeQualityAnalysisService.GetCachedResults();
        
        if (cachedReport == null)
        {
            return Ok(new 
            { 
                message = "Analysis not yet run. Trigger scan with POST /api/v1/diagnostics/code-quality/scan",
                available = true,
                analyzed = false
            });
        }

        return Ok(cachedReport);
    }

    /// <summary>
    /// Trigger new code quality analysis scan.
    /// Performs full Roslyn analysis of codebase.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Fresh analysis report.</returns>
    /// <response code="200">Analysis completed successfully.</response>
    /// <response code="503">Analysis not available (Production environment).</response>
    /// <remarks>
    /// This operation takes 2-5 seconds for a full solution scan.
    /// Results are cached automatically.
    /// 
    /// Only available in Development/Staging environments.
    /// </remarks>
    [HttpPost("scan")]
    [ProducesResponseType(typeof(CodeAnalysisReportDto), 200)]
    [ProducesResponseType(503)]
    public async Task<IActionResult> TriggerCodeQualityScan(CancellationToken cancellationToken)
    {
        if (!_codeQualityAnalysisService.IsAnalysisAvailable())
        {
            return StatusCode(503, new 
            { 
                message = "Code quality analysis is disabled in Production environment",
                available = false
            });
        }

        var analysisReport = await _codeQualityAnalysisService.AnalyzeCodebaseAsync(cancellationToken);
        
        return Ok(analysisReport);
    }

    /// <summary>
    /// Get code quality analysis capabilities.
    /// Returns information about enabled analyzers and rules.
    /// </summary>
    /// <returns>Analysis capabilities and configuration.</returns>
    /// <response code="200">Capabilities returned successfully.</response>
    [HttpGet("capabilities")]
    [ProducesResponseType(typeof(AnalysisCapabilitiesDto), 200)]
    public IActionResult GetAnalysisCapabilities()
    {
        var capabilities = _codeQualityAnalysisService.GetCapabilities();
        return Ok(capabilities);
    }

    /// <summary>
    /// Get summary of code quality issues by category.
    /// Quick overview without full issue details.
    /// </summary>
    /// <returns>Summary statistics.</returns>
    /// <response code="200">Summary returned successfully.</response>
    /// <response code="404">No analysis results available.</response>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(CodeQualitySummaryDto), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetCodeQualitySummary()
    {
        var cachedReport = _codeQualityAnalysisService.GetCachedResults();
        
        if (cachedReport == null)
        {
            return NotFound(new { message = "No analysis results available" });
        }

        return Ok(cachedReport.Summary);
    }
}
