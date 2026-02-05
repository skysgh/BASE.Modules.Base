using App.Modules.Sys.Application.Domains.Sessions.Services;
using App.Modules.Sys.Interfaces.API.REST.Domains.Constants;
using App.Modules.Sys.Interfaces.Domains.V1.Implementations.Base;
using App.Modules.Sys.Interfaces.Models.Session;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Interfaces.Domains.V1.Sessions
{
    /// <summary>
    /// Sessions API - Manage user sessions.
    /// Thin HTTP adapter - delegates to ISessionService.
    /// </summary>
    /// <remarks>
    /// Endpoints:
    /// - GET /api/rest/sys/v1/sessions - List sessions
    /// - GET /api/rest/sys/v1/sessions/{id} - Get session
    /// - GET /api/rest/sys/v1/sessions/{id}/operations - Get session operations
    /// </remarks>
    [Route(ApiRoutes.Rest.V1.ControllerRoute)]
    public class SessionsController : SysApiControllerBase
    {
        private readonly ISessionService _sessionService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sessionService"></param>
        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Get all sessions for workspace (paginated).
        /// </summary>
        /// <param name="skip">Number to skip</param>
        /// <param name="take">Number to take (max 100)</param>
        /// <param name="activeOnly">Only active sessions</param>
        /// <param name="ct">Cancellation token</param>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SessionDto>), 200)]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetSessions(
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            [FromQuery] bool activeOnly = false,
            CancellationToken ct = default)
        {
            var sessions = await _sessionService.GetSessionsAsync(skip, take, activeOnly, ct);
            return Ok(sessions);
        }

        /// <summary>
        /// Get session by ID.
        /// </summary>
        /// <param name="id">Session ID</param>
        /// <param name="ct">Cancellation token</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SessionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SessionDto>> GetSession(
            Guid id,
            CancellationToken ct = default)
        {
            var session = await _sessionService.GetSessionByIdAsync(id, ct);

            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        /// <summary>
        /// Get operations for a session.
        /// </summary>
        /// <param name="id">Session ID</param>
        /// <param name="skip">Number to skip</param>
        /// <param name="take">Number to take (max 100)</param>
        /// <param name="ct">Cancellation token</param>
        [HttpGet("{id}/operations")]
        [ProducesResponseType(typeof(IEnumerable<SessionOperationDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<SessionOperationDto>>> GetSessionOperations(
            Guid id,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 50,
            CancellationToken ct = default)
        {
            // Verify session exists
            if (!await _sessionService.SessionExistsAsync(id, ct))
            {
                return NotFound();
            }

            var operations = await _sessionService.GetSessionOperationsAsync(id, skip, take, ct);
            return Ok(operations);
        }
    }
}
