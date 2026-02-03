using App.Modules.Sys.Application.Services.Authorization;
using App.Modules.Sys.Interfaces.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Interfaces.API.REST.Controllers.V1
{
    /// <summary>
    /// System Permissions API - View available system permissions.
    /// Thin HTTP adapter - delegates to ISystemPermissionService.
    /// Read-only - permissions are defined in code (ReferenceDataSeeder).
    /// </summary>
    [Route("[controller]")]
    public class SystemPermissionsController : SysApiControllerBase
    {
        private readonly ISystemPermissionService _permissionService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="permissionService"></param>
        public SystemPermissionsController(ISystemPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// Get all system permissions.
        /// </summary>
        /// <param name="category">Filter by category (optional)</param>
        /// <param name="ct">Cancellation token</param>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SystemPermissionDto>), 200)]
        public async Task<ActionResult<IEnumerable<SystemPermissionDto>>> GetPermissions(
            [FromQuery] string? category = null,
            CancellationToken ct = default)
        {
            var permissions = await _permissionService.GetPermissionsAsync(category, ct);
            return Ok(permissions);
        }

        /// <summary>
        /// Get permission by key.
        /// </summary>
        /// <param name="key">Permission key</param>
        /// <param name="ct">Cancellation token</param>
        [HttpGet("{key}")]
        [ProducesResponseType(typeof(SystemPermissionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SystemPermissionDto>> GetPermission(
            string key,
            CancellationToken ct = default)
        {
            var permission = await _permissionService.GetPermissionByKeyAsync(key, ct);

            if (permission == null)
            {
                return NotFound();
            }

            return Ok(permission);
        }

        /// <summary>
        /// Get distinct permission categories.
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        [HttpGet("categories")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories(
            CancellationToken ct = default)
        {
            var categories = await _permissionService.GetCategoriesAsync(ct);
            return Ok(categories);
        }
    }
}
