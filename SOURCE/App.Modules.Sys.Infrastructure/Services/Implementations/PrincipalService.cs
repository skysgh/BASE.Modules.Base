using App.Modules.Sys.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace App.Modules.Sys.Infrastructure.Services.Implementations;

/// <summary>
/// Production implementation of principal service.
/// Provides access to current user identity and claims from HTTP context.
/// </summary>
/// <remarks>
/// Scoped lifetime - per HTTP request.
/// Wraps HttpContext.User for clean abstraction.
/// </remarks>
public sealed class PrincipalService : IPrincipalService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PrincipalService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public ClaimsPrincipal? CurrentPrincipal => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => CurrentPrincipal?.Identity?.IsAuthenticated ?? false;

    public string? UserName => CurrentPrincipal?.Identity?.Name;

    public string? AuthenticationType => CurrentPrincipal?.Identity?.AuthenticationType;

    public string? GetUserId()
    {
        if (!IsAuthenticated)
        {
            return null;
        }

        // Try common claim types for user ID
        var userIdClaim = CurrentPrincipal?.FindFirst(ClaimTypes.NameIdentifier) ??
                         CurrentPrincipal?.FindFirst("sub") ?? // OIDC standard
                         CurrentPrincipal?.FindFirst("userId");

        return userIdClaim?.Value;
    }

    public string? GetUserEmail()
    {
        if (!IsAuthenticated)
        {
            return null;
        }

        var emailClaim = CurrentPrincipal?.FindFirst(ClaimTypes.Email) ??
                        CurrentPrincipal?.FindFirst("email");

        return emailClaim?.Value;
    }

    public bool IsInRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("Role name cannot be null or whitespace", nameof(roleName));
        }

        return CurrentPrincipal?.IsInRole(roleName) ?? false;
    }

    public bool HasClaim(string claimType)
    {
        if (string.IsNullOrWhiteSpace(claimType))
        {
            throw new ArgumentException("Claim type cannot be null or whitespace", nameof(claimType));
        }

        return CurrentPrincipal?.HasClaim(c => c.Type == claimType) ?? false;
    }

    public bool HasClaim(string claimType, string claimValue)
    {
        if (string.IsNullOrWhiteSpace(claimType))
        {
            throw new ArgumentException("Claim type cannot be null or whitespace", nameof(claimType));
        }

        if (string.IsNullOrWhiteSpace(claimValue))
        {
            throw new ArgumentException("Claim value cannot be null or whitespace", nameof(claimValue));
        }

        return CurrentPrincipal?.HasClaim(claimType, claimValue) ?? false;
    }

    public string? GetClaimValue(string claimType)
    {
        if (string.IsNullOrWhiteSpace(claimType))
        {
            throw new ArgumentException("Claim type cannot be null or whitespace", nameof(claimType));
        }

        return CurrentPrincipal?.FindFirst(claimType)?.Value;
    }

    public string[] GetClaimValues(string claimType)
    {
        if (string.IsNullOrWhiteSpace(claimType))
        {
            throw new ArgumentException("Claim type cannot be null or whitespace", nameof(claimType));
        }

        if (CurrentPrincipal == null)
        {
            return Array.Empty<string>();
        }

        return CurrentPrincipal.FindAll(claimType)
            .Select(c => c.Value)
            .ToArray();
    }

    public string[] GetRoles()
    {
        if (CurrentPrincipal == null)
        {
            return Array.Empty<string>();
        }

        return CurrentPrincipal.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToArray();
    }
}
