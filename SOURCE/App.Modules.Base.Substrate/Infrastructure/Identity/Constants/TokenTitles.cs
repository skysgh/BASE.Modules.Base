namespace App.Modules.Base.Infrastructure.Identity.Constants
{
    /// <summary>
    /// Class of const strings related to working
    /// with identity tokens
    /// </summary>
    public static class TokenTitles
    {

        // kinda annoying they do this actually
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable ConstFieldDocumentationHeader // The field must have a documentation header.
        public const string IdpIdentifierId = "http://schemas.microsoft.com/identity/claims/identityprovider";


        public const string SubjectIdentifierId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";


        public const string ExpiryId = "exp";

        public const string IssuedAtId = "iat";
#pragma warning restore ConstFieldDocumentationHeader // The field must have a documentation header.
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
