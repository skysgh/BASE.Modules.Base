using App.Modules.Sys.Application.ReferenceData.Models;
using App.Modules.Sys.Application.ReferenceData.Services;
using App.Modules.Sys.Interfaces.API.REST.Domains.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace App.Modules.Sys.Interfaces.Domains.V1.ReferenceData;

/// <summary>
/// System Languages API - Available UI languages for internationalization.
/// Provides read-only access to system language reference data.
/// </summary>
[Route(ApiRoutes.Rest.V1.ReferenceData.Languages)]
[Authorize]
public class SystemLanguagesController : ControllerBase
{
    private readonly ISystemLanguageApplicationService _service;

    /// <summary>
    /// Constructor - Inject Application Service.
    /// </summary>
    public SystemLanguagesController(ISystemLanguageApplicationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Query system languages with OData support.
    /// Supports $filter, $orderby, $top, $skip for flexible querying.
    /// </summary>
    /// <remarks>
    /// OData query examples:
    /// - ?$filter=IsActive eq true
    /// - ?$filter=contains(Code, 'en')
    /// - ?$orderby=SortOrder&amp;$top=10
    /// 
    /// Standard OData syntax for client-side filtering/sorting.
    /// </remarks>
    [HttpGet("query")]
    [EnableQuery(MaxTop = 100,
                 AllowedQueryOptions = AllowedQueryOptions.Filter |
                                      AllowedQueryOptions.OrderBy |
                                      AllowedQueryOptions.Top |
                                      AllowedQueryOptions.Skip |
                                      AllowedQueryOptions.Count)]
    [ProducesResponseType(typeof(IQueryable<SystemLanguageDto>), 200)]
    public IQueryable<SystemLanguageDto> QueryLanguages([FromQuery] bool activeOnly = false)
    {
        return _service.GetLanguagesQueryable(activeOnly);
    }

    /// <summary>
    /// Get all system languages.
    /// </summary>
    /// <param name="activeOnly">If true, only return active languages.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Returns all configured languages.
    /// Use activeOnly=true to exclude inactive/upcoming languages.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(List<SystemLanguageDto>), 200)]
    public async Task<ActionResult<IReadOnlyList<SystemLanguageDto>>> GetAll(
        [FromQuery] bool activeOnly = false,
        CancellationToken ct = default)
    {
        var languages = await _service.GetAllLanguagesAsync(activeOnly, ct);
        return Ok(languages);
    }

    /// <summary>
    /// Get language by code.
    /// </summary>
    /// <param name="code">ISO 639-1 language code (e.g., "en", "es").</param>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Returns specific language by its code.
    /// Case-insensitive matching.
    /// </remarks>
    [HttpGet("{code}")]
    [ProducesResponseType(typeof(SystemLanguageDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<SystemLanguageDto>> GetByCode(
        string code,
        CancellationToken ct = default)
    {
        var language = await _service.GetLanguageByCodeAsync(code, ct);
        
        if (language == null)
        {
            return NotFound(new { message = $"Language with code '{code}' not found." });
        }

        return Ok(language);
    }

    /// <summary>
    /// Get the default system language.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <remarks>
    /// Returns the configured default language (fallback for localization).
    /// Always returns a result (guaranteed by system seed data).
    /// </remarks>
    [HttpGet("default")]
    [ProducesResponseType(typeof(SystemLanguageDto), 200)]
    public async Task<ActionResult<SystemLanguageDto>> GetDefault(CancellationToken ct = default)
    {
        var language = await _service.GetDefaultLanguageAsync(ct);
        return Ok(language);
    }
}
