using System.IO;
using System.Net;
using Byt3.Utilities.ConsoleInternals;

namespace Byt3Console.Url.Resolver
{
    public class UrlResolver : IResolver
    {
        public string FileExtension => ".web";
        private static readonly string TempPath = Path.Combine(Path.GetTempPath(), "Byt3.Console.Url.Resolver");

        public string ResolveLibrary(string libraryFile)
        {
            WebClient wc = new WebClient();
            string temp = Path.Combine(TempPath, Path.GetFileNameWithoutExtension(Path.GetTempFileName()),
                Path.GetTempFileName() + ".dll");

            wc.DownloadFile(File.ReadAllText(libraryFile), temp);

            return temp;
        }

        public void Dispose()
        {
            Directory.Delete(TempPath);
        }
    }
}