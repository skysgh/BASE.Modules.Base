## Description ##

### Implementation Details ###

* If the Model is expected to be transitioned across an API
  it should have it's matching API DTO.
  * I really know this is a pain in two ways 
    (needs a DTO, needs a two way
    object mapping map), but it's the *only* way to protect
    API consumers from breaking changes if you update your
    internal service messages.  
    Even if the only consumer for these messages is *expected* to be
    the system's own UX and not a third party client.
    * It's that *"expected"* term that gets you every time
      when you're a bit lazy or in a hurry...

### Notes ###
Of special interest include the following:
* `ConfigurationStep` is because configuring a new app is 
   often hard. Especially when relying on remote cloud services that
   need accounts, credentials, etc. So building up a picture of what's
   working or not is essential.
* `SearchResponseItem` -- which is a single item in an array making
   up a response to a Search request. Notice how when we search
   we are not returning the records themselves -- but the Search Response
   Summary Item...which summarises key aspects of the found item.
* `MediaScanResult` is because all apps pretty quickly get into 
   a need for uploading Media. If only to provide an avatar for a user.
   But all media that is uploaded *MUST* be scanned for malware before
   it is persisted, for redistribution. Not only that, but media should
   be periodically scanned afterwards (as malware detection improves
   and what was not caught previously can maybe be now).

