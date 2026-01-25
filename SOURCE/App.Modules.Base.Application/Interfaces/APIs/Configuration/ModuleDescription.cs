using App.Modules.Base.Substrate.Contracts.Modules;

namespace App.Modules.Base.Application.Interfaces.APIs.Configuration
{
    /// <summary>
    /// Configuration object
    /// used to describe Module.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Development Concerns:</b><br/>
    /// Module developers provide default values.
    /// But as only needed for API Documentation, etc.
    /// and not Routing, are not required to be Static, 
    /// hence they are configurable if required.
    /// </para>
    /// </remarks>
    public class ModuleDescription : IHasModuleDescription
    {
        /// <summary>
        /// Public default configurable Title of the Module
        /// </summary>
        public string Title { get; set; } = "BASE Module";

        /// <summary>
        /// Public configurable Description of the Module
        /// </summary>
        public string Description { get; set; } = "Base, system independent, Module on which all other Logical Modules rely/reference.";

        /// <summary>
        /// Public configurable Url to Module Maintainer web page.
        /// </summary>
        public string OrganisationUrl { get; set; } = "TODO:BASE:Website Url";

        /// <summary>
        /// public configurable Url to Module maintainer Contact information web page.
        /// </summary>
        public string ContactUrl { get; set; } = "TODO:BASE:Contact Url";

    }
}
