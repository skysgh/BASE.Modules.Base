using App.Modules.Sys.Domain.Domains.Identity;
using App.Modules.Sys.Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.Domains.Identity.Services
{
    /// <summary>
    /// Application service for multi-provider authentication.
    /// Links users to external identity providers (Google, Microsoft, etc.)
    /// </summary>
    public interface IUserIdentityService : IHasService 
    {
        /// <summary>
        /// Link authentication provider to user.
        /// Allows user to login via this provider.
        /// </summary>
        /// <param name="userId">User to link to</param>
        /// <param name="provider">Provider name (e.g., "Google", "Microsoft", "Local")</param>
        /// <param name="providerUserId">User ID from external provider</param>
        /// <param name="passwordHash">Password hash (only for Provider="Local")</param>
        /// <param name="ct">Cancellation token</param>
        Task LinkIdentityAsync(
            Guid userId, 
            string provider, 
            string providerUserId, 
            string? passwordHash = null, 
            CancellationToken ct = default);

        /// <summary>
        /// Unlink authentication provider from user.
        /// </summary>
        Task UnlinkIdentityAsync(Guid userId, string provider, CancellationToken ct = default);

        /// <summary>
        /// Find user by external provider credentials.
        /// Used during OAuth callback to resolve User from external ID.
        /// </summary>
        Task<User?> FindByProviderAsync(string provider, string providerUserId, CancellationToken ct = default);

        /// <summary>
        /// Get all identities linked to a user.
        /// </summary>
        Task<IEnumerable<UserIdentity>> GetUserIdentitiesAsync(Guid userId, CancellationToken ct = default);

        /// <summary>
        /// Update last login timestamp.
        /// </summary>
        Task UpdateLastLoginAsync(Guid userId, CancellationToken ct = default);

        /// <summary>
        /// Check if provider is already linked to user.
        /// </summary>
        Task<bool> IsProviderLinkedAsync(Guid userId, string provider, CancellationToken ct = default);
    }
}
