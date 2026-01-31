using App.Modules.Base.Substrate.Attributes;
// using System.Xml.Linq;
using App.Modules.Base.Substrate.Models.Configuration;
using App.Modules.Base.Infrastructure.Constants;
using App.Modules.Base.Substrate.Contracts.Models;

namespace App.Base.Infrastructure.Configuration
{
    /// <summary>
    /// Configuration object to host
    /// all Base App Settings.
    /// <para>
    /// Stored as a Singleton within
    /// <see cref="AppInformation.Configuration"/>
    /// </para>
    /// </summary>
    [Alias(Name)]
    public class AppConfiguration : IConfigurationObject
    {

        /// <summary>
        /// Singleton Instance
        /// <para>
        ///  TODO: HACK.
        /// </para>
        /// </summary>
        public static AppConfiguration Instance
        { get; private set; }
            = new AppConfiguration();


        /// <summary>
        /// Configuration Section Name.
        /// </summary>
        public const string Name = "App";

        /// <summary>
        /// Configuration Section Path.
        /// </summary>
        public const string Path = Name;




        /// <summary>
        /// Child collection of hosting Settings
        /// </summary>
        public Hosting Hosting { get; set; } = new Hosting();

        /// <summary>
        /// Child collection of Environment Settings
        /// </summary>
        public Environments Environments { get; set; } = new Environments();

        /// <summary>
        /// Sub Directories associated to this application
        /// where to look for Components
        /// </summary>
        public Directories Directories => _directories ??= new Directories();

        private Directories? _directories;

        /// <summary>
        /// Child collection of Application Settings
        /// (title, description)
        /// </summary>
        public Application Application { get; set; } = new Application();


        /// <summary>
        /// Child collection of Media Routing Settings
        /// </summary>
        public Medias Medias { get; set; } = new Medias();

        /// <summary>
        /// Child collection of Media Routing Settings
        /// </summary>
        public Components Components { get; set; } = new Components();

        /// <summary>
        /// Child collection of System Capability Settings
        /// </summary>
        public Capabilities Capabilities { get; set; } = new Capabilities();


        /// <summary>
        /// Call *after* Binding to
        /// fill in defaults if missing.
        /// </summary>
        public void Initialise()
        {
            if (Environments.Defaults.Length == 0)
            {
                Environments.Defaults =
                [
                    "bt","dt","st","ut","ct","tr","pp",Environments.ProductionEnvironmentId
                ];
            }
            if (Medias.Defaults.Length == 0)
            {
                Medias.Defaults = [
                    Medias.WWWMediaId, "media"
                ];
            }
        }
    }

    /// <summary>
    /// Configuration object
    /// for detailing Hosting Settings.
    /// <para>
    /// A sub object of 
    /// <see cref="AppConfiguration"/>
    /// </para>
    /// </summary>
    [Alias(Name)]
    public class Hosting : IConfigurationObject
    {
        /// <summary>
        /// Configuration Section Name.
        /// </summary>
        public const string Name = "Hosting";

        /// <summary>
        /// Configuration Section Path.
        /// </summary>
        public const string Path = AppConfiguration.Path + Name;

        /// <summary>
        /// TEST
        /// </summary>
        public string? Something { get; set; }
    }
    /// <summary>
    /// Application settings
    /// (title, description, etc.)
    /// </summary>
    [Alias(Name)]
    public class Application : IConfigurationObject
    {
        /// <summary>
        /// Configuration Section Name.
        /// </summary>
        public const string Name = "Application";

        /// <summary>
        /// Application Title
        /// </summary>
        public string? Title { get; set; } = "B.A.S.E.";

        /// <summary>
        /// Application Description
        /// </summary>
        public string? Description { get; set; } = "Base API Support Environment.";

    }

    /// <summary>
    /// Configuration Object 
    /// for detailing AppConfiguration
    /// specific to Environments
    /// <para>
    /// A subobject of <see cref="AppConfiguration"/>
    /// </para>
    /// </summary>
    [Alias(Name)]
    public class Environments : IConfigurationObject
    {
        /// <summary>
        /// Configuration Section Name.
        /// </summary>
        public const string Name = "Environments";

        /// <summary>
        /// Configuration Section Path.
        /// </summary>
        public const string Path = AppConfiguration.Path + Name;
        /// <summary>
        /// TEST
        /// </summary>
        public string? Something { get; set; }


        /// <summary>
        /// The Environment Id for Production.
        /// </summary>
        public string ProductionEnvironmentId { get; set; } = "prod";

        /// <summary>
        /// Strip off <see cref="ProductionEnvironmentId"/>
        /// if it is present.
        /// </summary>
        public bool StripProdEnvironmentId { get; set; } = true;

        /// <summary>
        /// List of default environment Ids to parse
        /// for in Host subdomains
        /// (eg: <c>'st.www.sometenant.someservice.tld'</c>)
        /// </summary>
        public string[] Defaults { get; set; } = [];
    }



    /// <summary>
    /// Collection of settings regarding
    /// routing media
    /// </summary>
    [Alias(Name)]
    public class Medias : IConfigurationObject
    {

        /// <summary>
        /// Configuration Section Name.
        /// </summary>
        public const string Name = "Medias";

        /// <summary>
        /// Configuration Section Path.
        /// </summary>
        public const string Path = AppConfiguration.Path + Name;

        /// <summary>
        /// TEST
        /// </summary>
        public string? Something { get; set; }


        /// <summary>
        /// The Environment Id for Production.
        /// </summary>
        /// <remarks>
        /// Don't confuse purpose with <see cref="Directories.WwwRootDirectory"/></remarks>
        public string WWWMediaId { get; set; } = "www";

        /// <summary>
        /// Strip off <see cref="WWWMediaId"/>
        /// if it is present.
        /// </summary>
        public bool EnforceWWWMediaId { get; set; }


        /// <summary>
        /// List of default SubDomains to resolve 
        /// Host settings for (eg: 'www.sometenancy.someservice.tld')
        /// </summary>
        public string[] Defaults { get; set; } = [];
    }


    /// <summary>
    /// Collection of settings regarding
    /// component handling
    /// </summary>
    [Alias(Medias.Name)]
    public class Components : IConfigurationObject
    {
        /// <summary>
        /// Configuration Section Name.
        /// </summary>
        public const string Name = "Components";

        /// <summary>
        /// Configuration Section Path.
        /// </summary>
        public const string Path = AppConfiguration.Path + Name;

        /// <summary>
        /// The Prefix (ie, "App.") used for all application 
        /// Assemblies, to more easily distinguish them from 
        /// 3rd party or "System."
        /// libraries.
        /// </summary>
        public string AppPrefix { get; set; } = AppConstants.AssemblyPrefix;


        ///// <summary>
        ///// Method to set AppPrefix
        ///// </summary>
        ///// <param name="value"></param>
        //public void SetAppPrefix(string value) { AppPrefix = value; }


        /// <summary>
        /// Array of Keys of Components to load by reflection
        /// </summary>
        public string[] ComponentKeys { get; set; } = [];

        /// <summary>
        /// Array of Modules to load by reflection
        /// </summary>
        public string[] ModuleKeys { get; set; } = [];


    }

    /// <summary>
    /// Collection of Settings regarding
    /// System Capabilities
    /// </summary>
    [Alias(Name)]
    public class Capabilities : IConfigurationObject
    {
        /// <summary>
        /// Configuration Section Name.
        /// </summary>
        public const string Name = "Capabilities";

        /// <summary>
        /// Configuration Section Path.
        /// </summary>
        public const string Path = AppConfiguration.Path + Name;

        /// <summary>
        /// TEST
        /// </summary>
        public string? Something { get; set; }
        /// <summary>
        /// Collection of Multi
        /// </summary>
        public MultiTenancies MultiTenancies { get; set; }
            = new MultiTenancies();

    }

    /// <summary>
    /// 
    /// </summary>
    [Alias(Name)]
    public class MultiTenancies : IConfigurationObject, IHasEnabled
    {
        /// <summary>
        /// Configuration Section Name.
        /// </summary>
        public const string Name = "MultiTenancies";

        /// <summary>
        /// Configuration Section Path.
        /// </summary>
        public const string Path = Capabilities.Path + Name;

        /// <inheritdoc/>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// The Id of the default/fallback tenant 
        /// </summary>
        public string DefaultTenantId { get; set; } = "Default";
    }





    ///// <summary>
    ///// Collection of settings specific to 
    ///// Module loading
    ///// </summary>
    //[Alias(Modules.Name)]
    //public class Modules : IConfigurationObject, IHasEnabled
    //{
    //    /// <summary>
    //    /// Configuration Section Name.
    //    /// </summary>
    //    public const string Name = "Modules";

    //    /// <summary>
    //    /// Configuration Section Name.
    //    /// </summary>
    //    public const string Path = Capabilities.Path + Name;

    //    /// <inheritdoc/>
    //    public bool Enabled { get; set; }



    //}


    /// <summary>
    /// 
    /// </summary>
    [Alias(Name)]
    public class Directories : IConfigurationObject
    {


        /// <summary>
        /// Configuration Section Name.
        /// </summary>
        public const string Name = "Directories";

        /// <summary>
        /// Configuration Section Path.
        /// </summary>
        public const string Path = Components.Path + Name;


        /// <summary>
        /// The Content Directory under root 
        /// <para>
        /// Default is <c>"wwwroot"</c>.
        /// </para>
        /// </summary>
        public string WwwRootDirectory { get; set; } = "wwwroot";

        /// <summary>
        /// The MODULES Directory
        /// <para>
        /// Default is <c>"MODULES"</c>.
        /// </para>
        /// </summary>
        public string ModulesSubDirectory { get; set; } = "MODULES";

        /// <summary>
        /// The MODULES Directory
        /// <para>
        /// Default is <c>"COMPONENTS"</c>.
        /// </para>
        /// </summary>
        public string ComponentsSubDirectory { get; set; } = "COMPONENTS";
    }
}
