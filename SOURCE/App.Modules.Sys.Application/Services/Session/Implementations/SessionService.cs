using App.Modules.Sys.Application.Contracts.Repositories;
using App.Modules.Sys.Application.Services.Session;
using App.Modules.Sys.Interfaces.Models.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.Services.Session.Implementations
{
    /// <summary>
    /// Implementation of session service.
    /// Uses ISessionRepository interface - no direct EF dependency.
    /// Handles DTO mapping and business logic.
    /// </summary>
    internal sealed class SessionService : ISessionService
    {
        private readonly ISessionRepository _repository;

        public SessionService(ISessionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SessionDto>> GetSessionsAsync(
            int skip = 0,
            int take = 50,
            bool activeOnly = false,
            CancellationToken ct = default)
        {
            take = Math.Min(take, 100); // Max 100

            var sessions = await _repository.GetSessionsAsync(skip, take, activeOnly, ct);

            return sessions.Select(s => new SessionDto
            {
                Id = s.Id,
                UserId = s.UserId,
                IsAuthenticated = s.UserId.HasValue,
                CreatedAt = s.CreatedAt.ToString("o"),
                LastActivityAt = s.LastActivityAt?.ToString("o"),
                ExpiresAt = s.ExpiresAt?.ToString("o"),
                IsActive = s.IsActive,
                OperationCount = s.Operations.Count
            });
        }

        public async Task<SessionDto?> GetSessionByIdAsync(
            Guid id,
            CancellationToken ct = default)
        {
            var session = await _repository.GetByIdAsync(id, ct);
            
            if (session == null)
            {
                return null;
            }

            return new SessionDto
            {
                Id = session.Id,
                UserId = session.UserId,
                IsAuthenticated = session.UserId.HasValue,
                CreatedAt = session.CreatedAt.ToString("o"),
                LastActivityAt = session.LastActivityAt?.ToString("o"),
                ExpiresAt = session.ExpiresAt?.ToString("o"),
                IsActive = session.IsActive,
                OperationCount = session.Operations.Count
            };
        }

        public async Task<IEnumerable<SessionOperationDto>> GetSessionOperationsAsync(
            Guid sessionId,
            int skip = 0,
            int take = 50,
            CancellationToken ct = default)
        {
            take = Math.Min(take, 100);

            var operations = await _repository.GetOperationsAsync(sessionId, skip, take, ct);

            return operations.Select(o => new SessionOperationDto
            {
                Id = o.Id,
                SessionId = o.SessionId,
                OperationType = o.OperationType,
                Resource = o.Resource,
                HttpMethod = o.HttpMethod,
                IpAddress = o.IpAddress,
                Timestamp = o.Timestamp.ToString("o"),
                StatusCode = o.StatusCode,
                DurationMs = o.DurationMs,
                IsSuccess = o.IsSuccess,
                ErrorMessage = o.ErrorMessage
            });
        }

        public async Task<bool> SessionExistsAsync(
            Guid id,
            CancellationToken ct = default)
        {
            return await _repository.ExistsAsync(id, ct);
        }
    }
}
