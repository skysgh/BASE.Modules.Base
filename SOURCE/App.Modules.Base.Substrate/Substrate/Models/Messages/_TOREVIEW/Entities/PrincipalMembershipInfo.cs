using App.Modules.Base.Substrate.Contracts.Models;
using App.Modules.Base.Substrate.Contracts.Models.Enums;
using App.Modules.Base.Substrate.Contracts.Models.StorageMetadata;

namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities
{
    /// <summary>
    /// System entity (not exposed to the system's exterior) for
    /// TODO: Describe better
    /// <para>
    /// Local Membership info is kept separate from Principal
    /// as it has nothing to do with Principal or business.
    /// Can't repeat this enough. Use an external 3rd party IdP.
    /// </para>
    /// </summary>
    public class PrincipalMembershipInfo : IHasGuidId, IHasTimestamp, IHasRecordState
    {
        // If the info in your app is valuable to someone, 
        // it's never an issue of if. It's an issue of when.
        // So if you persist not using an IdP (AAD, B2C, whatever),
        // do *NOT* save user information in a non-encrypted state.
        // ie: Email and Phone info used for authentication purposes must be encrypted.


        /// <summary>
        /// The storage id of the record
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Storage Timestamp
        /// </summary>
        public byte[] Timestamp { get; set; }

        /// <summary>
        /// Storage Record state
        /// </summary>
        public RecordPersistenceState RecordState { get; set; }

        /// <summary>
        /// Service User Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Credential used by User
        /// </summary>
        /* GOD I HATE DOING THIS WITHIN AN APP. USE AN IDP! */
        public string PasswordHash { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string EncryptedEmailAddress { get; set; }

        /// <summary>
        /// Email address
        /// <para>
        /// TODO: Confirm Type (bool?)
        /// </para>
        /// </summary>
        public string EmailConfirmed { get; set; }

        /// <summary>
        /// Two factor Authorisation enabled (or not)
        /// </summary>
        public bool TFAEnabled { get; set; }

        /// <summary>
        /// Device phone number (encrypted)
        /// </summary>
        public string TFAEncryptedDeviceNumber { get; set; }

        /// <summary>
        /// Device is confirmed
        /// </summary>
        public string TFADeviceConfirmed { get; set; }

        /// <summary>
        /// Counter to record number of attempts at entering credentials.
        /// <para>
        /// Used to count incorrect attempts, and lock out the Principal
        /// </para>
        /// </summary>
        public int IncorrectAttemptCount { get; set; }

        /// <summary>
        /// If attempts were high, system is locked out until the future
        /// </summary>
        public DateTime? LockedOutUntillUtc { get; set; }
    }
}
