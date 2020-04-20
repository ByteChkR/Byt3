using System;

namespace Byt3.Utilities.Console.Internals
{
    public interface IResolver : IDisposable
    {
        string FileExtension { get; }

        string ResolveLibrary(string libraryFile);
    }
}