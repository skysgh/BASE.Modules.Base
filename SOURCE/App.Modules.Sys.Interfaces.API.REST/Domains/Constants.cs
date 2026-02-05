namespace App.Modules.Sys.Interfaces.API.REST.Domains.Constants;

/// <summary>
/// API route constants for consistent route building.
/// Organized by: {root}/{api-type}/{module}/{version}/{path}
/// </summary>
/// <remarks>
/// Hierarchy:
/// - API Root (api)
///   - API Type (rest, odata, graphql)
///     - Module (sys, core, accounts)
///       - Version (v1, v2)
///         - Path (controller template or explicit)
/// 
/// Example: api/rest/sys/v1/diagnostics/code-quality
/// 
/// Benefits:
/// - ✅ Clear hierarchy
/// - ✅ Easy to add new modules
/// - ✅ Easy to add new API types (OData, GraphQL)
/// - ✅ Consistent versioning per module
/// </remarks>
public static class ApiRoutes
{
    /// <summary>
    /// API root prefix.
    /// Value: "api"
    /// </summary>
    public const string Root = "api";

    // ========================================
    // API TYPES
    // ========================================

    /// <summary>
    /// REST API routes (api/rest).
    /// Standard RESTful HTTP APIs.
    /// </summary>
    public static class Rest
    {
        /// <summary>
        /// REST API base path.
        /// Value: "api/rest"
        /// </summary>
        public const string Base = $"{Root}/rest";

        /// <summary>
        /// Sys module REST APIs (api/rest/sys).
        /// System infrastructure, diagnostics, and configuration.
        /// </summary>
        public static class Sys
        {
            /// <summary>
            /// Sys module base path.
            /// Value: "api/rest/sys"
            /// </summary>
            public const string ModuleBase = $"{Base}/sys";

            /// <summary>
            /// Version 1 of Sys module REST APIs.
            /// </summary>
            public static class V1
            {
                /// <summary>
                /// V1 base path.
                /// Value: "api/rest/sys/v1"
                /// </summary>
                public const string VersionBase = $"{ModuleBase}/v1";

                /// <summary>
                /// Standard controller route template.
                /// Controller name derived automatically from class name.
                /// Example: api/rest/sys/v1/{controller}
                /// </summary>
                public const string ControllerRoute = $"{VersionBase}/{{controller}}";

                // ========================================
                // DIAGNOSTICS
                // ========================================

                /// <summary>
                /// Diagnostics endpoints (api/rest/sys/v1/diagnostics).
                /// </summary>
                public static class Diagnostics
                {
                    /// <summary>
                    /// Diagnostics base path.
                    /// Value: "api/rest/sys/v1/diagnostics"
                    /// </summary>
                    public const string Base = $"{VersionBase}/diagnostics";

                    /// <summary>
                    /// Code quality diagnostics endpoint.
                    /// Value: "api/rest/sys/v1/diagnostics/code-quality"
                    /// </summary>
                    public const string CodeQuality = $"{Base}/code-quality";

                    /// <summary>
                    /// Startup diagnostics endpoint.
                    /// Value: "api/rest/sys/v1/diagnostics/startup"
                    /// </summary>
                    public const string Startup = $"{Base}/startup";

                    /// <summary>
                    /// Smoke tests diagnostics endpoint.
                    /// Value: "api/rest/sys/v1/diagnostics/smoketests"
                    /// </summary>
                    public const string SmokeTests = $"{Base}/smoketests";
                }

                // ========================================
                // PERMISSIONS
                // ========================================

                /// <summary>
                /// Permissions endpoints (api/rest/sys/v1/permissions).
                /// </summary>
                public static class Permissions
                {
                    /// <summary>
                    /// Permissions base path.
                    /// Value: "api/rest/sys/v1/permissions"
                    /// </summary>
                    public const string Base = $"{VersionBase}/permissions";
                }

                // ========================================
                // SETTINGS
                // ========================================

                /// <summary>
                /// Settings endpoints (api/rest/sys/v1/settings).
                /// </summary>
                public static class Settings
                {
                    /// <summary>
                    /// Settings base path.
                    /// Value: "api/rest/sys/v1/settings"
                    /// </summary>
                    public const string Base = $"{VersionBase}/settings";

                    /// <summary>
                    /// Effective (resolved) settings endpoint.
                    /// Returns merged settings (User → Workspace → System).
                    /// Value: "api/rest/sys/v1/settings/effective"
                    /// </summary>
                    public const string Effective = $"{Base}/effective";

                    /// <summary>
                    /// System-level settings endpoint.
                    /// Requires system administrator permissions.
                    /// Value: "api/rest/sys/v1/settings/system"
                    /// </summary>
                    public const string System = $"{Base}/system";

                    /// <summary>
                    /// Workspace-level settings endpoint.
                    /// Requires workspace administrator permissions.
                    /// Value: "api/rest/sys/v1/settings/workspace"
                    /// </summary>
                    public const string Workspace = $"{Base}/workspace";

                    /// <summary>
                    /// User-level settings endpoint.
                    /// User's personal preference overrides.
                    /// Value: "api/rest/sys/v1/settings/user"
                    /// </summary>
                    public const string User = $"{Base}/user";
                }

                // ========================================
                // REFERENCE DATA
                // ========================================

                /// <summary>
                /// Reference data endpoints (api/rest/sys/v1/refdata).
                /// </summary>
                public static class ReferenceData
                {
                    /// <summary>
                    /// Reference data base path.
                    /// Value: "api/rest/sys/v1/refdata"
                    /// </summary>
                    public const string Base = $"{VersionBase}/refdata";

                    /// <summary>
                    /// System languages reference data endpoint.
                    /// Returns available UI languages.
                    /// Value: "api/rest/sys/v1/refdata/languages"
                    /// </summary>
                    public const string Languages = $"{Base}/languages";
                }

                // ========================================
                // HEALTH
                // ========================================

                /// <summary>
                /// Health check endpoints (api/rest/sys/v1/health).
                /// Used for monitoring, load balancer health checks, and readiness probes.
                /// </summary>
                public static class Health
                {
                    /// <summary>
                    /// Health check base path.
                    /// Value: "api/rest/sys/v1/health"
                    /// </summary>
                    public const string Base = $"{VersionBase}/health";

                    /// <summary>
                    /// Liveness probe endpoint.
                    /// Returns 200 if application process is running.
                    /// Value: "api/rest/sys/v1/health/live"
                    /// </summary>
                    public const string Live = $"{Base}/live";

                    /// <summary>
                    /// Readiness probe endpoint.
                    /// Returns 200 if application is ready to serve traffic.
                    /// Value: "api/rest/sys/v1/health/ready"
                    /// </summary>
                    public const string Ready = $"{Base}/ready";

                    /// <summary>
                    /// Startup probe endpoint.
                    /// Returns 200 if application has completed initialization.
                    /// Value: "api/rest/sys/v1/health/startup"
                    /// </summary>
                    public const string Startup = $"{Base}/startup";
                }
            }

            /// <summary>
            /// Version 2 of Sys module REST APIs (future).
            /// </summary>
            public static class V2
            {
                /// <summary>
                /// V2 base path.
                /// Value: "api/rest/sys/v2"
                /// </summary>
                public const string VersionBase = $"{ModuleBase}/v2";

                /// <summary>
                /// Standard controller route template for V2.
                /// Example: api/rest/sys/v2/{controller}
                /// </summary>
                public const string ControllerRoute = $"{VersionBase}/{{controller}}";

                // V2 areas defined here when needed...
            }
        }

        // ========================================
        // OTHER MODULES (when needed)
        // ========================================

        // public static class Core { ... }
        // public static class Accounts { ... }
    }

    // ========================================
    // OTHER API TYPES (when needed)
    // ========================================

    /// <summary>
    /// OData API routes (api/odata).
    /// OData query protocol for advanced filtering and querying.
    /// </summary>
    public static class OData
    {
        /// <summary>
        /// OData API base path.
        /// Value: "api/odata"
        /// </summary>
        public const string Base = $"{Root}/odata";

        // public static class Sys { ... }
    }

    /// <summary>
    /// GraphQL API routes (api/graphql).
    /// GraphQL query endpoint.
    /// </summary>
    public static class GraphQL
    {
        /// <summary>
        /// GraphQL API base path.
        /// Value: "api/graphql"
        /// </summary>
        public const string Base = $"{Root}/graphql";

        // public static class Sys { ... }
    }
}



