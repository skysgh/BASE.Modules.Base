using Microsoft.Extensions.Logging;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace App.Modules.Base.Infrastructure.Services
{
    /// <summary>
    /// Contract to persist information
    /// as to startup.
    /// </summary>
    public interface IStartupLoggingService : IHasAppInfrastructureService
    {

        /// <summary>
        /// Invoke during startup.
        /// </summary>
        /// <param name="appLogger"></param>
        public void SetLogger(ILogger appLogger);

        /// <summary>
        /// Journal a single message:
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        public void LogMessage(LogLevel logLevel, string message);
    }
}
