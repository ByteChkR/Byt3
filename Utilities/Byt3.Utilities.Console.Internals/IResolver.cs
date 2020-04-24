using System;

namespace Byt3.Utilities.ConsoleInternals
{
    public interface IResolver : IDisposable
    {
        string FileExtension { get; }

        string ResolveLibrary(string libraryFile);
    }
}