using App.Modules.Sys.Application.Domains.Workspace.Services;
using App.Modules.Sys.Infrastructure.Domains.Diagnostics;
using App.Modules.Sys.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace App.Modules.Sys.Infrastructure.Web.Routing
{
    /// <summary>
    /// Middleware to resolve workspace from URL.
    /// Uses YOUR IAppLogger abstraction.
    /// </summary>
    public class WorkspaceResolutionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WorkspaceRoutingOptions _options;
        private readonly IAppLogger<WorkspaceResolutionMiddleware> _logger;

        public WorkspaceResolutionMiddleware(
            RequestDelegate next,
            IOptions<WorkspaceRoutingOptions> options,
            IAppLogger<WorkspaceResolutionMiddleware> logger)
        {
            _next = next;
            _options = options.Value;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IWorkspaceValidationService? workspaceValidator = null)
        {
            // SKIP workspace resolution for system/diagnostic paths
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";
            
            if (ShouldSkipWorkspaceResolution(path))
            {
                _logger.LogDebug($"Skipping workspace resolution for system path: {path}");
                await _next(context);
                return;
            }
            
            var workspace = await ResolveWorkspaceAsync(context, workspaceValidator);
            
            context.Items[ContextKeys.WorkspaceId] = workspace;
            context.Request.RouteValues[ContextKeys.WorkspaceId] = workspace;

            _logger.LogDebug($"Resolved workspace: {workspace}");

            await _next(context);
        }

        /// <summary>
        /// Check if request should skip workspace resolution.
        /// System paths like diagnostics, swagger, health checks don't need workspaces.
        /// </summary>
        private bool ShouldSkipWorkspaceResolution(string path)
        {
            // Swagger and OpenAPI
            if (path.StartsWith("/swagger")) return true;
            if (path.Contains("/swagger.json")) return true;
            
            // Diagnostics endpoints
            if (path.StartsWith("/api/v1/diagnostics")) return true;
            
            // Health checks
            if (path.StartsWith("/health")) return true;
            if (path.StartsWith("/api/health")) return true;
            
            // Development tools
            if (path.StartsWith("/_framework")) return true;
            if (path.StartsWith("/_vs")) return true;
            
            // System endpoints that don't need workspaces
            if (path.StartsWith("/api/v1/system")) return true;
            
            return false;
        }

        /// <summary>
        /// Resolve workspace using database lookup.
        /// </summary>
        private async Task<string> ResolveWorkspaceAsync(
            HttpContext context,
            IWorkspaceValidationService? workspaceValidator)
        {
            // STRATEGY 1: Subdomain detection (database-driven)
            if (_options.AllowSubdomainWorkspaces)
            {
                var subdomain = ExtractSubdomain(context.Request.Host.Host);
                if (!string.IsNullOrEmpty(subdomain) && !_options.IsReserved(subdomain))
                {
                    // Check if workspace exists in database
                    if (workspaceValidator != null && 
                        await workspaceValidator.WorkspaceExistsAsync(subdomain))
                    {
                        return subdomain;
                    }
                }
            }

            // STRATEGY 2: Path-based detection (database-driven)
            if (_options.AllowPathWorkspaces)
            {
                var pathWorkspace = ExtractWorkspaceFromPath(context.Request.Path);
                if (!string.IsNullOrEmpty(pathWorkspace) && !_options.IsReserved(pathWorkspace))
                {
                    // Check if workspace exists in database
                    if (workspaceValidator != null && 
                        await workspaceValidator.WorkspaceExistsAsync(pathWorkspace))
                    {
                        return pathWorkspace;
                    }
                }
            }

            // STRATEGY 3: Default workspace
            return _options.DefaultWorkspace;
        }

        /// <summary>
        /// Extract FIRST segment from host (potential subdomain).
        /// NO domain knowledge needed - just get first segment!
        /// </summary>
        /// <remarks>
        /// Examples:
        /// - ibm.someservice.com ? "ibm"
        /// - ibm.someservice.co.nz ? "ibm"
        /// - api.ibm.someservice.com ? "api"
        /// - localhost ? null
        /// </remarks>
        private string? ExtractSubdomain(string host)
        {
            // Remove port if present
            host = host.Split(':')[0];

            // Ignore localhost, IPs
            if (host == "localhost" || 
                host.All(c => char.IsDigit(c) || c == '.'))
            {
                return null;
            }

            // Get first segment
            var segments = host.Split('.');
            
            if (segments.Length < 2)
                return null;  // No subdomain possible

            var firstSegment = segments[0].ToLowerInvariant();

            // Ignore common non-workspace subdomains
            if (firstSegment == "www" || firstSegment == "mail")
                return null;

            return firstSegment;
        }

        /// <summary>
        /// Extract workspace from path if first segment is NOT reserved.
        /// Example: /ibm/api/rest/v1/... ? "ibm"
        /// Example: /api/rest/v1/... ? null (reserved word)
        /// </summary>
        private string? ExtractWorkspaceFromPath(PathString path)
        {
            if (!path.HasValue)
                return null;

            var segments = path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);
            
            if (segments.Length == 0)
                return null;

            var firstSegment = segments[0].ToLowerInvariant();

            // Check if first segment is reserved word
            if (_options.IsReserved(firstSegment))
            {
                return null;  // It's a reserved word, not a workspace
            }

            // Return first segment for database validation
            return firstSegment;
        }
    }
}

