

## Development Perspective ##

* The filenames are prefixed with numbers to make them easier to spot 
  in the Solution Explorer.
  * But that means you have to be careful that your names don't pick 
    up the names later when moving files around and Visual Studio tries
    to be 'helpful'...
* There are three main groupings of entities:
  1. Base system entities that don't require auditing, timestamping, etc.  (ie, almost never ever used)
  2. Untenanted base classes for cross-tenancy records. Examples might include:
    * `DiagnosticTrace`
    * `ErrorReport`
    * `User`
    * etc.
  3. Base class for *Tenanted* entities. 
    * These have the same shapes as the above *Untenanted* entities - 
      just with an extra `IHasTenantFK` to keep them isolated from other Tenants.
    * Note that whereas Tenant based architectures used to be the rage at the turn of the century,
      it's now a legacy pattern to some extent, in a world where Users Group themselves together 
      in more adhoc ways than silo'ed organisations and enterprises.
    * That said, there are clients that have legal or other obligations to ensure their data remains
      silo'ed off from others. 
    * The soluton is to keep most users and groups as members of a Tenancy with `Id=Guid.Empty`, with
      the ability to silo off legacy clients if/as need be.
