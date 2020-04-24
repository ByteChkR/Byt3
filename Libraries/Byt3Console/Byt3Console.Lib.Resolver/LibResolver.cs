using Byt3.Utilities.ConsoleInternals;

namespace Byt3Console.Lib.Resolver
{
    public class LibResolver : IResolver
    {
        public string FileExtension => ".dll";

        public string ResolveLibrary(string libraryFile)
        {
            return libraryFile;
        }

        public void Dispose()
        {
        }
    }
}