// using System;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;
using App.Modules.Base.Substrate.Models.Entities.Base;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    ///  System entity (not exposed to the system's exterior) for
    /// A single operation in a <see cref="Session"/>.
    /// <para>
    /// Usually, one Operation per Request.
    /// </para>
    /// </summary>
    public class SessionOperation :
        UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase,
        IHasOwnerFK
    {
        /// <summary>
        /// The FK to the owning <see cref="Session"/>
        /// <para>
        /// A Session Operation is bound to a Session
        /// Which is bound to a Principal, but not a Tenant
        /// </para>
        /// </summary>
        public virtual Guid? SessionFK { get; set; }

        /// <summary>
        /// The Ip of the Client when this Operation was invoked.
        /// <para>
        /// Note: that cell phone operations issue different IPs as cells travel between towers.
        /// </para>
        /// </summary>
        public virtual string? ClientIp { get; set; }

        /// <summary>
        /// The full url they requested
        /// </summary>
        public virtual string Url { get; set; }
        /// <summary>
        /// The Tenant Identifier.
        /// <para>
        /// TODO: Check documentation is correct.
        /// </para>
        /// </summary>
        public virtual string? ResourceTenantKey { get; set; }
        /// <summary>
        /// Name of the Controller assigned to handling request.
        /// </summary>
        public virtual string? ControllerName { get; set; }
        /// <summary>
        /// Name of Controller Action handling the request.
        /// </summary>
        public virtual string? ActionName { get; set; }
        /// <summary>
        /// Arguments provided to Controller Action.
        /// </summary>
        public virtual string? ActionArguments { get; set; }
        /// <summary>
        /// DateTime the Operation Request started.
        /// </summary>
        public virtual DateTimeOffset BeginDateTimeUtc { get; set; }
        /// <summary>
        /// DateTime the Operation Request completed (bar wrapping up writing these records).
        /// </summary>
        public virtual DateTimeOffset EndDateTimeUtc { get; set; }
        /// <summary>
        /// The duration of the Request (End - Start)
        /// </summary>
        public virtual TimeSpan Duration { get; set; }
        /// <summary>
        /// The HTTP response code provided back (200, else)
        /// </summary>
        public virtual string ResponseCode { get; set; }


        /// <summary>
        /// Gets the FK of the parent <see cref="Session"/> object.
        /// <para>
        /// (not all nested records will provide a navigation Property up to its parent, or even externally visible FK)
        /// </para>
        /// </summary>
        /// <returns></returns>
        public Guid GetOwnerFk()
        {
            return SessionFK ?? new Guid();
        }
    }
}