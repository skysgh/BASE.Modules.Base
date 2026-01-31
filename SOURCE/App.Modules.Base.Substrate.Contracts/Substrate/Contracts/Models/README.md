## Description ##

Folder of Contracts for Entities, used throughout 
the system.

For example, most entities are expected to have

- `IHasGuidId`: permits a Guid Id (as oppossed to slower a `int`-based Id )
- `IHasTimestamp`: permits not crushing someone else's changes inadvertently
- `IHasRecordState`: permits logical deleting (and recovery...)
- `IHasAuditability`: permits primitive in-record auditability

Many will also have the following field too to ensure
Tenant Separation (even if I pretty much disagree with
using Tenancies, there are sometimes legal/contractual
obligations to be met:

- `IHasTenantFK`: wires back to a specific Tenant.

Many of the above are already bundled up into higher value
contracts:

- `IHasReferenceData`: a combination of `IHasEnabled`, `IHasTitleAndDescription`

There are plenty more standard contracts that are useful
every day:

- `IHasTextAndDescription`: used for most options lists.

Note that I do point out in documentation the similarity, 
but different between the following, with recommendations
when to use which:

- `IHasKey`
- `IHasName`
- `IHasTitle`
- `IHasText`
- `IHasValue`


## Development Perspective ##

* Use Contracts where ever possible. To Excess if you can.
  * It permits later handling of entities by handlers that just
    need to match template rather than do a whole bunch of Reflection
    to find out if a Property exists on it or not. 
* Contracts start with `IHas`.
  * Admittedly, can't remember when or why I started using this convention... 
    * But there was a good reason - at the time...
* Some Contracts are marked with `[EditorBrowsable(EditorBrowsableState.Never)]`
  because although they are perfectly legal (ie, not `[Obsolete]`) one 
  should in most cases avoid using them.
    * Examples are:
      * `[IHasTag]` - prefer the Nullable version.
      * `[IHasParentFK]` - prefer the nullable version.
      * `[IHasDescrptionNullable]` - prefer expecting `Description`s be added to all List Items as it reduces Training and Tech Support costs later.
* Whereas we recommend using Contracts all over internal Entities to facilitate
  automated handling by interface matching - as oppossed to using Reflection and lots of 
  custom case management logic - experience tells us to recommend being highly cautious 
  with their application to DTOs:
    * It has happened in the past where we've updated a contract (eg: Non Nullable to Nullable)
      without being mindful of the changes to DTOs, and therefore Service clients...


