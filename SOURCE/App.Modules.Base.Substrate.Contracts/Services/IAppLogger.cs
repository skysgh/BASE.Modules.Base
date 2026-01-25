namespace App.Modules.Base.Substrate.Contracts.Services
{
    /// <summary>
    /// Application-specific logger abstraction.
    /// Wraps the underlying logging framework to avoid direct dependencies.
    /// </summary>
    public interface IAppLogger
    {
        /// <summary>
        /// Logs a trace message for detailed debugging information.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogTrace(string message);
        /// <summary>
        /// Logs a debug message for diagnostic information.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogDebug(string message);
        /// <summary>
        /// Logs an informational message about the application flow.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogInformation(string message);
        /// <summary>
        /// Logs a warning message for abnormal or unexpected events that don't cause failure.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogWarning(string message);
        /// <summary>
        /// Logs an error message for failures in the current operation.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogError(string message);
        /// <summary>
        /// Logs an error message with exception details for failures in the current operation.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="message">The message to log.</param>
        void LogError(Exception exception, string message);
        /// <summary>
        /// Logs a critical message for unrecoverable failures that require immediate attention.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogCritical(string message);
        /// <summary>
        /// Logs a critical message with exception details for unrecoverable failures.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="message">The message to log.</param>
        void LogCritical(Exception exception, string message);
    }

    /// <summary>
    /// Generic logger for specific category/type.
    /// </summary>
    /// <typeparam name="T">The category type used for log scoping.</typeparam>
    public interface IAppLogger<T> : IAppLogger
    {
    }
}