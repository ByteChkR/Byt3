using System;
using System.IO;
using System.Net;
using Byt3.Utilities.Console.Internals;

namespace Byt3.Console.Link.Resolver
{
    public class LinkResolver : IResolver
    {
        public string FileExtension => ".link";

        public string ResolveLibrary(string libraryFile)
        {
            return File.ReadAllText(libraryFile);
        }
        public void Dispose() { }
    }
}
