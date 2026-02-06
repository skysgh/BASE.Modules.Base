using App.Modules.Sys.Domain.Domains.Identity;
using App.Modules.Sys.Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.Domains.Identity.Services
{
    /// <summary>
    /// Application service for managing users.
    /// Orchestrates Domain + Infrastructure concerns.
    /// </summary>
    public interface IUserManagementService : IHasService
    {
        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="email">User email (required for authentication)</param>
        /// <param name="isSystemAdmin">Whether user is system administrator</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created user</returns>
        Task<User> CreateUserAsync(string email, bool isSystemAdmin = false, CancellationToken ct = default);

        /// <summary>
        /// Get user by ID.
        /// </summary>
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Get user by email.
        /// </summary>
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

        /// <summary>
        /// Get all users (paginated).
        /// </summary>
        Task<IEnumerable<User>> GetAllAsync(int skip = 0, int take = 100, CancellationToken ct = default);

        /// <summary>
        /// Update user.
        /// </summary>
        Task UpdateAsync(User user, CancellationToken ct = default);

        /// <summary>
        /// Activate user (allow login).
        /// </summary>
        Task ActivateAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Deactivate user (prevent login).
        /// </summary>
        Task DeactivateAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Delete user (soft or hard delete based on implementation).
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Check if email is already in use.
        /// </summary>
        Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
    }
}
