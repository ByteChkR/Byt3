using Byt3.Utilities.Versioning;

namespace Byt3.ExtPP.Console
{
    public class ConsoleEntry
    {
        public string ConsoleKey => "extpp";

        public void Run(string[] args)
        {
            VersionAccumulatorManager.SearchForAssemblies();
            CLI.StartConsole(args);
        }
    }
}