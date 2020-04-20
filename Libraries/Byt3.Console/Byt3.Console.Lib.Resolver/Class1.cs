using System;
using Byt3.Utilities.Console.Internals;

namespace Byt3.Console.Lib.Resolver
{
    public class LibResolver:IResolver
    {
        public string FileExtension => ".dll";

       public string ResolveLibrary(string libraryFile)
       {
           return libraryFile;
       }

       public void Dispose() { }
    }
}
