# CBaS (Circuit Builder and Simulator)

CBaS is a tool which allows users to create circuits using traditional logic gates, and simulate and view their states using input and output controls.


## Current Features
CBaS currently supports the following features:
 - Support for all traditional logic gates
 - Input via clickable input buttons, and a square wave generator, with a selection of frequencies
 - Output via binary output display, and seven-segment display
 - Circuit data saving/loading using EEPROM chip
 - Project structure allowing for saving and importing of projects, consisting of circuits and folders (currently all stored on the user's machine as individual files

## Future Features

CBaS will aim to support the following features:

 - Better ribbon integration consisting of many more controls and options
 - Better multithreading model, including progress bar along the bottom of the screen
 - Removal of all references to Win32 libraries to allow better support on Linux and MacOS via Wine
 - Better architecture to allow for proper unit testing, and decoupling of all UI and business logic - allowing for circuits to run while not even open
 - Better internal project structure for ease of development
 - Full documentation
 - Communication across circuits, allowing users to create tidier designs
 - Change of how projects are stored - 1 file instead of a folder on the user's system - providing a safer way for users to move the location of projects
 - Support for TTL chips (74lsXX)
 - Create custom "modules" - circuits which can be reused in the form of "chips" including the ability to import/export these
 - Collaboration features so multiple users can work together on the same project at the same time
 - Version control support to view and commit any changes, with support for common VCSs such as Git
 - An "About" page which will contain the required license information as part of the MIT license attached to a few dependencies this project uses
