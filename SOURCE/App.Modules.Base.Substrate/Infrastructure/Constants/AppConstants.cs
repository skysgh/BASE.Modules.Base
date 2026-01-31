namespace App.Modules.Base.Infrastructure.Constants
{
    /// <summary>
    /// Collection of constants related to
    /// the whole app in general.
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// Prefix of Assemblies 
        /// (i.e.: "<c>App.</c>")
        /// <para>
        /// Note:
        /// Used by the default assemblyScanner convention
        /// to isolate which assemblies to look within 
        /// for interfaces and implementations.
        /// </para>
        /// </summary>
        public const string AssemblyPrefix = $"{DefaultConstants.App}.";


        /// <summary>
        /// The default database connection string name
        /// </summary>
        public const string DbConnectionStringName = DefaultConstants.Default;
    }
}
