# Introduction #

This page shows you how to download and compile the source code.

# Requirements #

  1. [Mercurial](http://mercurial.selenic.com/downloads/)
    * It's recommended to use TortoiseHG which is much easier to use
  1. Visual Studio 2010 Professional SP1
    * Using Visual C# 2010 Express might work too, but you won't be able to create an installer
    * You can download the express edition from [here](http://www.microsoft.com/visualstudio/en-us/products/2010-editions/visual-csharp-express)
  1. [Windows Installer XML toolset](http://wix.sourceforge.net/)
    * I used a weekly build: [3.6.2012.0](http://wix.sourceforge.net/releases/3.6.2012.0/)

# Download the source code #

## Cloning the source using Mercurial ##

You can use both the [command line application or TortoiseHG](http://mercurial.selenic.com/downloads/). The URL for cloning the repository can be found on the [project web site](http://code.google.com/p/rift-authenticator-for-windows/source/checkout).

## Downloading from the download page ##

It's also possible to download the source code from the [project download page](http://code.google.com/p/rift-authenticator-for-windows/downloads/list).

# Compiling the project #

  * Open the RiftAuthenticator.sln from the downloaded source code. It should automatically start Visual Studio 2010.
  * Change the build configuration to "Debug"
  * Open the context-menu of the "RiftAuthenticator.WPF" project
  * Set the "RiftAuthenticator.WPF" as start project
  * Open the "Build" menu
  * Click the "Build Solution" menu item