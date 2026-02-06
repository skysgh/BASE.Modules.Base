using App.Modules.Sys.Shared.Models.Implementations;
namespace App.Modules.Sys.Application.Domains.Diagnostics.SmokeTesting.Models
{

    /// <summary>
    /// Disposable scope for automatic startup log entry finalization.
    /// Automatically logs entry on dispose.
    /// </summary>
    public interface IStartupLogScope : IDisposable
    {
        /// <summary>
        /// The underlying log entry being tracked.
        /// </summary>
        StartupLogEntry Entry { get; }

        /// <summary>
        /// Adds metadata to the log entry.
        /// </summary>
        void AddMetadata(string key, object value);

        /// <summary>
        /// Marks entry with an exception (auto-tagged as Error).
        /// </summary>
        void SetException(Exception ex);
    }

}