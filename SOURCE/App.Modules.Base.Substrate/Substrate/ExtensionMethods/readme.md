## Description ##

Folder of ExtensionMethods specific to 
Models and their properties -- while not 
requiring refencing external 3rd party
libraries.

## Development Approach ##

- For the most part ExtensionMethods are best
  placed in the `*.Infrastructure` assembly.
  But these few are needed right here in the 
  `*.Shared` assembly, and it doesn't have a 
  access to `*.Infrastructure`.