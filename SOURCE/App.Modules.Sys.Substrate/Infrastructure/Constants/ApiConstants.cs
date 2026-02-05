namespace App.Modules.Sys.Substrate.Infrastructure.Constants;

/// <summary>
/// Shared API route building blocks.
/// These constants are used across all modules for consistent API structure.
/// </summary>
/// <remarks>
/// Pattern: {Root}/{ApiType}/{ModuleId}/{Version}/{Path}
/// 
/// This class provides the general parts:
/// - Root: "api"
/// - ApiType: "rest", "odata", "graphql"
/// - Version: "v1", "v2", etc.
/// 
/// Each module provides its own ModuleId (e.g., "sys", "core", "accounts").
/// </remarks>
public static class ApiConstants
{
    /// <summary>
    /// API root prefix for all API endpoints.
    /// Value: "api"
    /// </summary>
    /// <remarks>
    /// All API endpoints start with this prefix.
    /// Example: api/rest/sys/v1/...
    /// </remarks>
    public const string Root = "api";

    // ========================================
    // API TYPES
    // ========================================

    /// <summary>
    /// REST API type identifier.
    /// Value: "rest"
    /// </summary>
    /// <remarks>
    /// Used for standard RESTful HTTP APIs.
    /// Example: api/rest/sys/v1/...
    /// </remarks>
    public const string RestType = "rest";

    /// <summary>
    /// OData API type identifier.
    /// Value: "odata"
    /// </summary>
    /// <remarks>
    /// Used for OData query protocol APIs.
    /// Example: api/odata/sys/v1/...
    /// </remarks>
    public const string ODataType = "odata";

    /// <summary>
    /// GraphQL API type identifier.
    /// Value: "graphql"
    /// </summary>
    /// <remarks>
    /// Used for GraphQL query APIs.
    /// Example: api/graphql
    /// </remarks>
    public const string GraphQLType = "graphql";

    // ========================================
    // VERSION IDENTIFIERS
    // ========================================

    /// <summary>
    /// API version identifiers.
    /// Standard version naming across all modules.
    /// </summary>
    public static class Versions
    {
        /// <summary>
        /// Version 1 identifier.
        /// Value: "v1"
        /// </summary>
        public const string V1 = "v1";

        /// <summary>
        /// Version 2 identifier.
        /// Value: "v2"
        /// </summary>
        public const string V2 = "v2";

        /// <summary>
        /// Version 3 identifier (future).
        /// Value: "v3"
        /// </summary>
        public const string V3 = "v3";
    }

    // ========================================
    // HELPER METHODS FOR BUILDING PATHS
    // ========================================

    /// <summary>
    /// Builds a REST API module base path.
    /// Pattern: api/rest/{moduleId}
    /// </summary>
    /// <param name="moduleId">Module identifier (e.g., "sys", "core", "accounts")</param>
    /// <returns>Module base path</returns>
    /// <example>
    /// BuildRestModuleBase("sys") → "api/rest/sys"
    /// </example>
    public static string BuildRestModuleBase(string moduleId)
        => $"{Root}/{RestType}/{moduleId}";

    /// <summary>
    /// Builds a REST API versioned module base path.
    /// Pattern: api/rest/{moduleId}/{version}
    /// </summary>
    /// <param name="moduleId">Module identifier</param>
    /// <param name="version">Version identifier</param>
    /// <returns>Versioned module base path</returns>
    /// <example>
    /// BuildRestVersionBase("sys", Versions.V1) → "api/rest/sys/v1"
    /// </example>
    public static string BuildRestVersionBase(string moduleId, string version)
        => $"{BuildRestModuleBase(moduleId)}/{version}";

    /// <summary>
    /// Builds an OData API module base path.
    /// Pattern: api/odata/{moduleId}
    /// </summary>
    /// <param name="moduleId">Module identifier</param>
    /// <returns>OData module base path</returns>
    public static string BuildODataModuleBase(string moduleId)
        => $"{Root}/{ODataType}/{moduleId}";
}
