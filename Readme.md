[![Build Status](https://travis-ci.com/ByteChkR/Byt3.svg?branch=master)](https://travis-ci.com/ByteChkR/Byt3)[![Codacy Badge](https://api.codacy.com/project/badge/Grade/d6d25e60d31a46d4ab73f505ebdda9e3)](https://www.codacy.com/manual/ByteChkR/Byt3?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=ByteChkR/Byt3&amp;utm_campaign=Badge_Grade)[![codecov](https://codecov.io/gh/ByteChkR/Byt3/branch/master/graph/badge.svg)](https://codecov.io/gh/ByteChkR/Byt3)


# Byt3 Library Collection

## Libraries

### Byt3.ADL
A Debugging framework that is used througout the solution

### Byt3.AssemblyGenerator
Library that acts as a poor mans precompiler "weaver".

### Byt3Console
Console Runner implementation that is mostly modular and allows for "drop-in" additions of consoles.

#### Included Consoles
- Assembly Generator Console
- Engine Build Tools Console
- Engine Player Console
- External Text Preprocessor Console
- OpenFL Console
- OpenFL Benchmarking Console
- OpenFL Script Generator Console
- Engine Demo Console -> Located in `Libraries/Byt3.Engine`
- Engine Tutorial Console -> Located in `Libraries/Byt3.Engine`

### Byt3.Engine
Multipurpose OpenGL game engine written in pure C#.
Also includes demos and tutorials.

### Byt3.Engine.BuildTools
Build tools for the engine, including asset packaging and creating an executable package for the engine player.

### Byt3.ExtPP
External Text Processor

### Byt3.ObjectPipeline
Pipeline System. (Input/Output Oriented Modular Processing Pipeline)

### Byt3.OpenCL
OpenCL .NET Implementation and a Wrapper that implements more easy to use interface.

### Byt3.OpenFL
OpenFL is an Interpreted OpenCL pseudo language that can be used for generating Images from Noise

### Byt3.Collections
Collection of Algorithms and other Smoothing functions.

### Byt3.CommandRunner
Library that parses command line input and uses custom commands to run application logic.

### Byt3.CommandRunner.SetSettings
Command Runner Command that uses reflection to populate a settings classes.

### Byt3.MAssert
Small lightweight WIP Assert library.

### Byt3.PackageHandling
Handling System that takes a System.Object as input and passes it to the right handler.

### Byt3.Serialization
System to implement serialization of any types.

### Byt3.Threading
Implementation of Thread Worker


## Utilities

### Byt3.Callbacks
Small Helper Library that exposes a IOCallback class that can be used to "reroute" IO Calls.

### Byt3.DisposableManagement
Helper Library that associates any deriving class with a handle identifier to help with finding memory leaks.

### Byt3.ConsoleInternals
Library that implements the base classes used to load different Console and File Resolver modules.

### Byt3.Utilities.DotNet
Library containing Implementation for Loading Assemblies and Recursively Resolving .csproj references.

### Byt3.Utilities.Exceptions
Base Exceptions for the Solution

### Byt3.Utilities.FastString
Collection of algorithms that are performance optimized.

### Byt3.Utilities.IL
WIP Library containing different functions that could alleviate the use of reflection in some usecases.

### Byt3.Utilities.ManifestIO
Implements a Filesystem built on top of Byt3.Callbacks that enables treating embedded resources like files and directories.

### Byt3.Utilities.Serialization
Small Library that is containing Serialization Utilities.

### Byt3.Utilities.Threading
Implements ProcessRunner class that can run Cmd or Bash Scripts and commands.

## Unit Tests
*  Byt3.ADL.Tests -> 19 Tests
*  Engine.BuildTools.Tests -> 1
*  Byt3.ExtPP.Tests -> 13 Tests
*  Byt3.ObjectPipeline.AssetLoaderFramework.Tests -> 1 Test
*  Byt3.ObjectPipeline.Tests -> 3 Tests
*  Byt3.OpenCL.Tests -> 4 Tests
*  Byt3.OpenFL.Tests -> 20 Tests
*  Byt3.PackageHandling.Tests -> 3 Tests
*  Byt3.Serialization.Tests -> 1 Test