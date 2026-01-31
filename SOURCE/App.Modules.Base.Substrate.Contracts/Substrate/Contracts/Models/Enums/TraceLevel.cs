namespace App.Modules.Base.Substrate.Contracts.Models.Enums
{
    /// <summary>
    /// Enumeration of Trace Level.
    /// <para>
    /// Used by <c>ExceptionRecord</c>.
    /// </para>
    /// <para>
    /// TODO: Has taken a long time to recognise that 
    /// Enums are evil (offset issue of Interface Localization)
    /// </para>
    /// </summary>
    public enum TraceLevel
    {
        /// <summary>
        /// The message is due to a error critical to the system (taking it down).
        /// </summary>
        Critical = 0,
        /// <summary>
        /// The message is due to an error. 
        /// Will cause incomplete handling of the Request.
        /// </summary>
        Error = 1,
        /// <summary>
        /// The message is to report an warning.
        /// Will probably not disrupt Request handling. 
        /// But worth keeping an eye on.
        /// </summary>
        Warn = 2,
        /// <summary>
        /// The message is informative of normal flow.
        /// </summary>
        Info = 3,
        /// <summary>
        /// The message is informative for a deep diagnostics analysis.
        /// <para>
        /// TODO: Is Debug more/less than Verbose? More right?
        /// </para>
        /// </summary>
        Verbose = 4,
        /// <summary>
        /// The message is informative for a deep diagnostics analysis
        /// only recorded when compiled in Debug Mode.
        /// <para>
        /// TODO: Is Debug more/less than Verbose? More right?
        /// </para>
        /// </summary>
        Debug = 5
    }
}