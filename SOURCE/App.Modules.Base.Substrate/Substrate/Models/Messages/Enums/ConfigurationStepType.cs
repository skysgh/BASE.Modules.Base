namespace App.Modules.Base.Substrate.Models.Messages.Enums
{
    /// <summary>
    /// Enumeration of ConfigurationStep types.
    /// <para>
    /// (General, Security, Performance, etc.)
    /// </para>
    /// </summary>
    public enum ConfigurationStepType
    {
        /// <summary>
        /// Undefined. 
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// ConfigurationStep is for general reasons
        /// </summary>
        General = 1,
        /// <summary>
        /// ConfigurationStep is for security reasons
        /// </summary>
        Security = 2,
        /// <summary>
        /// ConfigurationStep is for performance reasons
        /// </summary>
        Performance = 3
    }
}
