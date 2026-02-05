using App.Modules.Sys.Infrastructure.Domains.Constants;
using App.Modules.Sys.Substrate.Infrastructure.Constants;

namespace App.Modules.Sys.Interfaces.API.REST.Domains.Constants;

/// <summary>
/// Sys module API route constants.
/// Builds on shared ApiConstants from Substrate.
/// Organized by: {root}/{api-type}/{module}/{version}/{path}
/// </summary>
/// <remarks>
/// This module provides "sys" module-specific routes.
/// General constants (api, rest, v1, v2) come from ApiConstants in Substrate.
/// 
/// Hierarchy:
/// - ApiConstants.Root = "api" (from Substrate)
/// - ApiConstants.RestType = "rest" (from Substrate)
/// - ModuleId = "sys" (module-specific)
/// - ApiConstants.Versions.V1 = "v1" (from Substrate)
/// - Path = controller/explicit (module-specific)
/// 
/// Example: api/rest/sys/v1/diagnostics/code-quality
/// 
/// Benefits:
/// - ✅ Shared constants reusable across modules
/// - ✅ Module-specific structure remains local
/// - ✅ Clear hierarchy via IntelliSense nesting
/// - ✅ Easy to add new modules (use same ApiConstants)
/// </remarks>
public static class ApiRoutes
{
    /// <summary>
    /// Sys module identifier.
    /// Value: "sys"
    /// </summary>
    private const string ModuleId = ModuleConstants.Key;

    // Build module base inline (const-compatible)
    private const string RestModuleBase = $"{ApiConstants.Root}/{ApiConstants.RestType}/{ModuleId}";

    // ========================================
    // REST API ROUTES
    // ========================================

    /// <summary>
    /// REST API routes for Sys module.
    /// Pattern: api/rest/sys
    /// </summary>
    public static class Rest
    {
        /// <summary>
        /// Version 1 of Sys module REST APIs.
        /// </summary>
        public static class V1
        {
            // Build version base inline (const-compatible)
            private const string VersionBase = $"{RestModuleBase}/{ApiConstants.Versions.V1}";

            /// <summary>
            /// Standard controller route template.
            /// Controller name derived automatically from class name.
            /// Value: "api/rest/sys/v1/{controller}"
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
            private const string VersionBase = $"{RestModuleBase}/{ApiConstants.Versions.V2}";

            /// <summary>
            /// Standard controller route template for V2.
            /// Value: "api/rest/sys/v2/{controller}"
            /// </summary>
            public const string ControllerRoute = $"{VersionBase}/{{controller}}";

            // V2 areas defined here when needed...
        }
    }

    // ========================================
    // ODATA API ROUTES (future)
    // ========================================

    /// <summary>
    /// OData API routes for Sys module (future).
    /// Pattern: api/odata/sys
    /// </summary>
    public static class OData
    {
        private const string ODataModuleBase = $"{ApiConstants.Root}/{ApiConstants.ODataType}/{ModuleId}";

        /// <summary>
        /// OData base path.
        /// Value: "api/odata/sys"
        /// </summary>
        public const string ModuleBase = ODataModuleBase;

        // OData versions and entities defined here when needed...
    }

    // ========================================
    // GRAPHQL API ROUTES (future)
    // ========================================

    /// <summary>
    /// GraphQL API endpoint for Sys module (future).
    /// Pattern: api/graphql
    /// </summary>
    /// <remarks>
    /// GraphQL typically uses a single endpoint, not module-specific.
    /// </remarks>
    public static class GraphQL
    {
        /// <summary>
        /// GraphQL endpoint.
        /// Value: "api/graphql"
        /// </summary>
        public const string Endpoint = $"{ApiConstants.Root}/{ApiConstants.GraphQLType}";
    }
}





