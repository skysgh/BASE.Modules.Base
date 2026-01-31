namespace App.Modules.Base.Substrate
{
    /// <summary>
    /// Constants specific to this 
    /// Logical Module
    /// </summary>
    public class ModuleConstants
    {
        /// <summary>
        /// Unique key to identify this logical module.
        /// </summary>
        public const string Key = "Core";

        /// <summary>
        /// Unique Key to use as name for DbSchema 
        /// for this Logical Module.
        /// </summary>
        public const string DbSchemaKey = Key;

        /// <summary>
        /// The name of the ConnectionString
        /// in app settings
        /// (it's the same as the Base one).
        /// </summary>
        public const string DbConnectionName =
            Base.Infrastructure.Constants.ModuleConstants.DbConnectionStringName;

    }
}
