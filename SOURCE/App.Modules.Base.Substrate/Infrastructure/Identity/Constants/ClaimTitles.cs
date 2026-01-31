namespace App.Modules.Base.Infrastructure.Identity.Constants
{
    /// <summary>
    /// Class of static strings used to 
    /// work with ID Tokens
    /// </summary>
    public static class ClaimTitles
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string ScopeElementId = "http://schemas.microsoft.com/identity/claims/scope";

        public const string RoleElementId = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public const string CultureElementId = "http://schemas.org.tld/identity/claims/culture";

        public const string PrincipalKeyElementId = "http://schemas.org.tld/identity/claims/tenant";

        public const string ObjectIdElementId = "http://schemas.microsoft.com/identity/claims/objectidentifier";


        public const string UserIdentifier = "UserId";

        public const string SessionIdentifier = "SessionId";

        public const string UniqueSessionIdentifier = "UniqueSessionId";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}
