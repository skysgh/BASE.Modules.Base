namespace App.Modules.Base.Substrate.Contracts.Modules
{
    /// <summary>
    /// Configuration Information used to 
    /// document API endpoints.
    /// <para>
    /// </para>
    /// </summary>
    /// <remarks>
    /// <b>Development Concerns:</b><br/>
    /// Examples of use -- Used to configure Swagger.
    /// <para>
    /// See also:
    /// <see cref="IHasModuleDescription"/>
    /// </para>
    /// </remarks>
    public interface IHasModuleAPIDocumentationConfiguration
    {

        /// <summary>
        /// Unique *navigable* Key for API Endpoint documentation.
        /// <para>
        /// Default is {ModuleKey}-{ProtocolName} format 
        /// (eg: '<c>Foo-Rest</c>')</para>
        /// </summary>
        /// <remarks>
        /// <b>Development Concerns:</b><br/>
        /// Key is used as part of Urls, so 
        /// it is Case Sensitive, must not have spaces or 
        /// special characters.
        /// </remarks>
        string Key { get; set; }

    }
}