using System.IO;
using Byt3.Utilities.ConsoleInternals;

namespace Byt3Console.Link.Resolver
{
    public class LinkResolver : IResolver
    {
        public string FileExtension => ".link";

        public string ResolveLibrary(string libraryFile)
        {
            return File.ReadAllText(libraryFile);
        }

        public void Dispose()
        {
        }
    }
}