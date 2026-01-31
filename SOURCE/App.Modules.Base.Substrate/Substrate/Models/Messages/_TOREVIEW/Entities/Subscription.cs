// using App.Modules.Base.Substrate.Models.Entities.Enums;
// using System;

//namespace App.Modules.Base.Substrate.Models.Entities
//{
//    /// <summary>
//    /// <para>
//    /// An ApplicationAccount = A Tenancy.
//    /// An ApplicationAccount has multiple UserAccounts/Principals associated to it, via multiple Roles.
//    ///   An ApplicationAccount must have at least one UserAccount/Principal as an Owner.
//    ///   Logic must be applied that an Owner may not remove oneself as an Owner, if they are the last Owner.
//    /// An ApplicationAccount, upon Creation, is associated to a Subscription. 
//    /// An ApplicationAccount provides logical Operational Information:
//    /// * Name (for display), 
//    /// * Identifier (for routing), 
//    /// * Status (Provisioning, Enabled, Suspending, etc.),
//    /// * Analytics Type and Id (Google, other), Users.
//    /// Design wise, the most contentious is Status (see Subscription below).
//    /// 
//    /// As stated earlier, upon Create of an ApplciationAccount/Tenant, it is associated to its Owner.
//    /// But it is also associated to a Subscription.
//    /// 
//    /// A Subscription provides to the ApplicationAccount Status constraints (eg: its paid for)
//    /// A Subscription can be managed/passed to a different Principal than the Account Holder. 
//    /// A Subscription does not provide Billing information (Billing information can be linked to a Subscription).
//    /// An ApplicationAccount/Tenant can have multiple Principles Mapped to it in various Roles.
//    /// </para>
//    /// <para>
//    /// A subscription required operational constraints. 
//    /// The most common is a Beginning/End.
//    /// (Monthly, Quarterly, Yearly).
//    /// See Chron Date.
//    /// But you don't want to cut it off right away. 
//    /// So you want a Status (Active, GettingClose...OVerTime...Suspended...
//    /// </para>
//    /// <para>
//    /// A Subscription would not contain Pricing information
//    /// (that would be an Accounting issue, while this is an
//    /// Access logic object).
//    /// </para>
//    /// </summary>
//    public class Subscription : UntenantedAuditedRecordStatedTimestampedGuidIdEntityBase
//    {

//        /// <summary>
//        /// The accounting system defined Key of the Subscription (eg: SUB-123).
//        /// </summary>
//        public string Key { get; set; }

//        /// <summary>
//        /// It's not as simple as <see cref="IHasEnabled"/>
//        /// </summary>
//        public SubscriptionStatus Status { get; set; }

//        /// <summary>
//        /// A Subscription can be for a future beginning.
//        /// </summary>
//        public DateTimeOffset? Begins { get; set; }

//        /// <summary>
//        /// A subscription should have a defined end date.
//        /// </summary>
//        public DateTimeOffset? Ends { get; set; }

//        /// <summary>
//        /// A displayable name for the Subscription.
//        /// <para>Not the same as the </para>
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// The FK to the Owner Account
//        /// </summary>
//        public Guid OwnerFK { get; set; }

//        /// <summary>
//        /// The Subscription Owner Account / Principal
//        /// </summary>
//        public Principal Owner { get; set; }


//        // I am not going to add an Account/Principal Members property
//        // as it could get rather large, making it unwieldy 
//        // to pick up a Subscription object. 

//        // Then again, Accounts/Principals should not have a SusbcriptionFK
//        // as they could belong to multiple Subscriptions
//        // (the relationship is managed within an EF Map).


//    }

//}
