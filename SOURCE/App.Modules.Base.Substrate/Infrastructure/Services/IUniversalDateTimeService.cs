// using System;

namespace App.Modules.Base.Infrastructure.Services
{
    /// <summary>
    /// Contract for an infrastructure service to
    /// return UTC based DateTimeOffset (not just UTC DateTime, 
    /// and certainly not Local DateTime!)
    /// to all services that need to coordinate datetime in a
    /// geo-capable manner (which you certainly need to do when
    /// you are hosting some infrastructure in one timezone, 
    /// and other pieces in another).
    /// </summary>
    /// <seealso cref="IHasAppInfrastructureService" />
    public interface IUniversalDateTimeService : IHasAppInfrastructureService
    {
        /// <summary>
        /// Return the DateTime, in UTC.
        /// </summary>
        DateTimeOffset NowUtc();
    }
}