namespace App.Modules.Sys.Interfaces.API.REST.Domains.Constants;

/// <summary>
/// API route constants for consistent route building.
/// Uses base paths with {controller} templates for flexibility.
/// </summary>
/// <remarks>
/// Philosophy: Balance between type-safety and flexibility.
/// - Base paths are constants (prevents typos, enables refactoring)
/// - Controller names use {controller} template (automatic, rename-safe)
/// 
/// Two patterns supported:
/// 1. Hardcoded version: api/v1/{controller}
/// 2. API versioning attribute: api/rest/v{version:apiVersion}/{controller}
/// 
/// Usage:
/// [Route(ApiRoutes.V1.ControllerRoute)]           // Hardcoded v1
/// [Route(ApiRoutes.Versioned.ControllerRoute)]    // Attribute-based versioning
/// 
/// Benefits:
/// - ✅ Single source of truth for base paths
/// - ✅ Easy version changes (V1 → V2)
/// - ✅ Controller names derived automatically
/// - ✅ Less maintenance than full explicit routes
/// </remarks>
public static class ApiRoutes
{
    /// <summary>
    /// API base prefix (no version, no module).
    /// Value: "api"
    /// </summary>
    public const string ApiBase = "api";

    // ========================================
    // PATTERN 1: API VERSIONING ATTRIBUTE
    // ========================================

    /// <summary>
    /// API routes using [ApiVersion] attribute for versioning.
    /// Use with [ApiVersion("1.0")] on controller.
    /// </summary>
    public static class Versioned
    {
        /// <summary>
        /// Base path for versioned APIs: api/rest/v{version:apiVersion}
        /// Version determined by [ApiVersion] attribute on controller.
        /// </summary>
        public const string VersionedBase = $"{ApiBase}/rest/v{{version:apiVersion}}";

        /// <summary>
        /// Standard controller route template with API versioning.
        /// Example: api/rest/v1.0/{controller}
        /// </summary>
        public const string ControllerRoute = $"{VersionedBase}/{{controller}}";

        /// <summary>
        /// Module-specific versioned routes (Sys module).
        /// </summary>
        public static class Sys
        {
            /// <summary>
            /// Sys module base: api/sys/rest/v{version:apiVersion}
            /// </summary>
            public const string ModuleVersionedBase = $"{ApiBase}/sys/rest/v{{version:apiVersion}}";

            /// <summary>
            /// Sys module controller route with versioning.
            /// Example: api/sys/rest/v1.0/{controller}
            /// </summary>
            public const string ControllerRoute = $"{ModuleVersionedBase}/{{controller}}";
        }
    }

    // ========================================
    // PATTERN 2: HARDCODED VERSION (V1, V2, etc.)
    // ========================================

    /// <summary>
    /// Version 1 API routes (hardcoded version in path).
    /// </summary>
    public static class V1
    {
        /// <summary>
        /// Version identifier.
        /// Value: "v1"
        /// </summary>
        public const string Version = "v1";

        /// <summary>
        /// Base path for V1 APIs (api/v1).
        /// </summary>
        public const string VersionBase = $"{ApiBase}/{Version}";

        /// <summary>
        /// Standard controller route template (api/v1/{controller}).
        /// Controller name is derived automatically.
        /// </summary>
        public const string ControllerRoute = $"{VersionBase}/{{controller}}";

        // ========================================
        // GROUPED ENDPOINTS (For Organized Areas)
        // ========================================

        /// <summary>
        /// Diagnostics endpoints (api/v1/diagnostics).
        /// </summary>
        public static class Diagnostics
        {
            /// <summary>
            /// Base path for diagnostics endpoints.
            /// Value: "api/v1/diagnostics"
            /// </summary>
            public const string Base = $"{VersionBase}/diagnostics";

            /// <summary>
            /// Controller route template for diagnostics area.
            /// Example: /api/v1/diagnostics/{controller}
            /// </summary>
            public const string ControllerRoute = $"{Base}/{{controller}}";

            /// <summary>
            /// Code quality diagnostics endpoint.
            /// Value: "api/v1/diagnostics/code-quality"
            /// </summary>
            public const string CodeQuality = $"{Base}/code-quality";

            /// <summary>
            /// Startup diagnostics endpoint.
            /// Value: "api/v1/diagnostics/startup"
            /// </summary>
            public const string Startup = $"{Base}/startup";
        }

        /// <summary>
        /// Settings endpoints (api/v1/settings).
        /// </summary>
        public static class Settings
        {
            /// <summary>
            /// Base path for settings endpoints.
            /// Value: "api/v1/settings"
            /// </summary>
            public const string Base = $"{VersionBase}/settings";

            /// <summary>
            /// Controller route template for settings area.
            /// Example: /api/v1/settings/{controller}
            /// </summary>
            public const string ControllerRoute = $"{Base}/{{controller}}";

            /// <summary>
            /// Effective (resolved) settings endpoint.
            /// Returns merged settings (User → Workspace → System).
            /// Value: "api/v1/settings/effective"
            /// </summary>
            public const string Effective = $"{Base}/effective";

            /// <summary>
            /// System-level settings endpoint.
            /// Requires system administrator permissions.
            /// Value: "api/v1/settings/system"
            /// </summary>
            public const string System = $"{Base}/system";

            /// <summary>
            /// Workspace-level settings endpoint.
            /// Requires workspace administrator permissions.
            /// Value: "api/v1/settings/workspace"
            /// </summary>
            public const string Workspace = $"{Base}/workspace";

            /// <summary>
            /// User-level settings endpoint.
            /// User's personal preference overrides.
            /// Value: "api/v1/settings/user"
            /// </summary>
            public const string User = $"{Base}/user";
        }

        /// <summary>
        /// Reference data endpoints (api/v1/reference-data).
        /// </summary>
        public static class ReferenceData
        {
            /// <summary>
            /// Base path for reference data endpoints.
            /// Value: "api/v1/reference-data"
            /// </summary>
            public const string Base = $"{VersionBase}/reference-data";

            /// <summary>
            /// Controller route template for reference data area.
            /// Example: /api/v1/reference-data/{controller}
            /// </summary>
            public const string ControllerRoute = $"{Base}/{{controller}}";

            /// <summary>
            /// System languages reference data endpoint.
            /// Returns available UI languages.
            /// Value: "api/v1/reference-data/system-languages"
            /// </summary>
            public const string SystemLanguages = $"{Base}/system-languages";
        }

        /// <summary>
        /// Health check endpoints (api/v1/health).
        /// Used for monitoring, load balancer health checks, and readiness probes.
        /// </summary>
        public static class Health
        {
            /// <summary>
            /// Base path for health check endpoints.
            /// Value: "api/v1/health"
            /// </summary>
            public const string Base = $"{VersionBase}/health";

            /// <summary>
            /// Liveness probe endpoint.
            /// Returns 200 if application process is running.
            /// Value: "api/v1/health/live"
            /// </summary>
            public const string Live = $"{Base}/live";

            /// <summary>
            /// Readiness probe endpoint.
            /// Returns 200 if application is ready to serve traffic.
            /// Value: "api/v1/health/ready"
            /// </summary>
            public const string Ready = $"{Base}/ready";

            /// <summary>
            /// Startup probe endpoint.
            /// Returns 200 if application has completed initialization.
            /// Value: "api/v1/health/startup"
            /// </summary>
            public const string Startup = $"{Base}/startup";
        }
    }

    /// <summary>
    /// Version 2 API routes (future).
    /// Placeholder for next API version.
    /// </summary>
    public static class V2
    {
        /// <summary>
        /// Version identifier for V2.
        /// Value: "v2"
        /// </summary>
        public const string Version = "v2";

        /// <summary>
        /// Base path for V2 APIs.
        /// Value: "api/v2"
        /// </summary>
        public const string VersionBase = $"{ApiBase}/{Version}";

        /// <summary>
        /// Standard controller route template for V2.
        /// Example: api/v2/{controller}
        /// </summary>
        public const string ControllerRoute = $"{VersionBase}/{{controller}}";

        // V2 areas defined here when needed...
    }
}


