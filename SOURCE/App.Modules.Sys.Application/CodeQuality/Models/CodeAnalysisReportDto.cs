using System;
using System.Collections.Generic;

namespace App.Modules.Sys.Application.CodeQuality.Models;

/// <summary>
/// Code quality analysis report DTO.
/// Contains results from runtime Roslyn analysis.
/// </summary>
public record CodeAnalysisReportDto
{
    /// <summary>
    /// When analysis was performed (UTC).
    /// </summary>
    public DateTime AnalyzedAt { get; init; }

    /// <summary>
    /// How long analysis took.
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// Number of assemblies analyzed.
    /// </summary>
    public int AssembliesAnalyzed { get; init; }

    /// <summary>
    /// Total number of issues found.
    /// </summary>
    public int TotalIssues { get; init; }

    /// <summary>
    /// Number of critical (error-level) issues.
    /// </summary>
    public int CriticalIssues { get; init; }

    /// <summary>
    /// Number of warning-level issues.
    /// </summary>
    public int WarningIssues { get; init; }

    /// <summary>
    /// Number of info-level issues.
    /// </summary>
    public int InfoIssues { get; init; }

    /// <summary>
    /// Issues grouped by category.
    /// </summary>
    public Dictionary<string, int> IssuesByCategory { get; init; } = new();

    /// <summary>
    /// Issues grouped by rule.
    /// </summary>
    public Dictionary<string, int> IssuesByRule { get; init; } = new();

    /// <summary>
    /// All issues found.
    /// </summary>
    public List<CodeIssueDto> Issues { get; init; } = new();

    /// <summary>
    /// Summary of analysis results.
    /// </summary>
    public CodeQualitySummaryDto Summary { get; init; } = new();

    /// <summary>
    /// Whether analysis found any critical issues.
    /// </summary>
    public bool HasCriticalIssues => CriticalIssues > 0;

    /// <summary>
    /// Overall health score (0-100).
    /// Based on issue severity and count.
    /// </summary>
    public int HealthScore { get; init; }
}

/// <summary>
/// Individual code quality issue DTO.
/// </summary>
public record CodeIssueDto
{
    /// <summary>
    /// Unique issue identifier.
    /// </summary>
    public string Id { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Issue severity (Error, Warning, Info).
    /// </summary>
    public string Severity { get; init; } = "Info";

    /// <summary>
    /// Issue category (NamingConvention, Architecture, NoShortcuts, etc.).
    /// </summary>
    public string Category { get; init; } = "General";

    /// <summary>
    /// Rule identifier (e.g., "NamingConvention.TypeReflecting").
    /// </summary>
    public string Rule { get; init; } = string.Empty;

    /// <summary>
    /// Human-readable issue message.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// File path where issue was found.
    /// </summary>
    public string? FilePath { get; init; }

    /// <summary>
    /// Line number where issue was found.
    /// </summary>
    public int? LineNumber { get; init; }

    /// <summary>
    /// Column number where issue was found.
    /// </summary>
    public int? ColumnNumber { get; init; }

    /// <summary>
    /// Code snippet showing the issue.
    /// </summary>
    public string? CodeSnippet { get; init; }

    /// <summary>
    /// Recommendation for fixing the issue.
    /// </summary>
    public string? Recommendation { get; init; }

    /// <summary>
    /// Link to documentation about this rule.
    /// </summary>
    public string? DocumentationUrl { get; init; }
}

/// <summary>
/// Summary of code quality analysis.
/// </summary>
public record CodeQualitySummaryDto
{
    /// <summary>
    /// Overall status (Excellent, Good, Fair, Poor).
    /// </summary>
    public string Status { get; init; } = "Unknown";

    /// <summary>
    /// Key findings (top issues to address).
    /// </summary>
    public List<string> KeyFindings { get; init; } = new();

    /// <summary>
    /// Recommendations for improvement.
    /// </summary>
    public List<string> Recommendations { get; init; } = new();

    /// <summary>
    /// Breakdown by category.
    /// </summary>
    public CategoryBreakdownDto CategoryBreakdown { get; init; } = new();
}

/// <summary>
/// Breakdown of issues by category.
/// </summary>
public record CategoryBreakdownDto
{
    /// <summary>
    /// Naming convention violations.
    /// </summary>
    public int NamingConventions { get; init; }

    /// <summary>
    /// Architectural rule violations.
    /// </summary>
    public int ArchitecturalRules { get; init; }

    /// <summary>
    /// "No shortcuts" violations.
    /// </summary>
    public int NoShortcuts { get; init; }

    /// <summary>
    /// Dependency flow violations.
    /// </summary>
    public int DependencyFlow { get; init; }

    /// <summary>
    /// Code quality issues.
    /// </summary>
    public int CodeQuality { get; init; }
}
