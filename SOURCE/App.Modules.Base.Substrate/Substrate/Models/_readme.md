## Description ##
Each Logical Module has a set of models 
specific to it.

Except for `Base` models -- which 
are is intended to be reusable by all Logical Modules -- 
a Logical Module's models are generally only intended 
to be available to it's own Services, and not 
shared across modules.

## Development Details ##

There are only a couple of kinds of object based models
used in a system:

### System models ###
System modesl are either:

* internal *only* (!) to the system,

And either:

* Non-persistable
* Persitable

#### Non-Persistable System Models ####
Non-Persisted System Models are used for:

* Generic Configuration Settings
* Singleton Configuration objects for stateless Services.
* In-System messages.

#### Persistable System Models ####
Persistable System models, on the other hand, :
* have an `Id` because they are persisted to a DB, 
  and `Id`'s are a practically always a requirement 
  for EF to manage them.
* The Id's are a Guid -- rather than an Int because
* When generated system side (as oppossed to in a Db)
  are faster than incremental counters (which rely on a lock)
* But must be *sequential* rather than completely random
  in order to not fragment the indexes and instead append
  to the end.
* Hence, in their constructors, they use a `GuidFactory`
  rather than the `Guid.New()`.

### Message Models ###
Message models:

* are used *externally* from the app
* are DTOs for APIs
* need to be mapped from System entities to DTOs (and back again)
  by a service invoked via the Application Services.
  * (altough admittedly its easier to do it at the 
  Presentation layer).

