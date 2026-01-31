namespace App.Modules.Base.Substrate.Models.Messages._TOREVIEW.Entities.Enums
{
    /// <summary>
    /// <para>
    /// </para>
    /// </summary>
    public enum SubscriptionStatus
    {
        /// <summary>
        /// Undefined/Not set.
        /// </summary>
        Undefined = 0,


        /// <summary>
        /// Provisioning.
        /// Can be progressed to <see cref="Enabled"/>.
        /// </summary>
        Enabling = 1,

        /// <summary>
        /// Provisioned 
        /// Can be progressed to <see cref="Renewing"/>.
        /// Can be progressed to <see cref="Closing"/>.
        /// </summary>
        Enabled = 2,

        /// <summary>
        /// About to Renew Subscription
        /// Will progress to Enabled
        /// Can be progressed to <see cref="Enabled"/>.
        /// Can be progressed to <see cref="RenewalFailed"/>.
        /// </summary>
        Renewing = 3,

        /// <summary>
        /// Still functioning, but
        /// will proceed to Suspending 
        /// if information is not provided.
        /// Can be progressed to <see cref="Renewing"/>.
        /// Can be progressed to <see cref="Suspending"/>.
        /// </summary>
        RenewalFailed = 4,

        /// <summary>
        /// Suspending after x time after 
        /// Renewal Failed, and account is not updated.
        /// Can be progressed to <see cref="Enabled"/>.
        /// Can be progressed to <see cref="Closed"/>.
        /// </summary>
        Suspending = 6,

        /// <summary>
        /// Disabling Account.
        /// Can be progressed to <see cref="Enabled"/>.
        /// Can be progressed to <see cref="Closing"/>.
        /// </summary>
        Suspended = 7,

        /// <summary>
        /// Closing (eg: after 6 months Suspension, showing disinterest)
        /// Can be progressed to <see cref="Enabled"/>.
        /// Can be progressed to <see cref="Closed"/>.
        /// </summary>
        Closing = 8,

        /// <summary>
        /// Account is intentionally closed 
        /// or closed due to neglect (eg: Suspension over 6 months)
        /// and cannot be reopened
        /// without manual intervention.
        /// </summary>
        Closed = 9,
    }
}
