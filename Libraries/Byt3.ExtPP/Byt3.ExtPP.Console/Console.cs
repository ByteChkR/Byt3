using Byt3.Utilities.Console.Internals;
using Byt3.Utilities.Versioning;

namespace Byt3.ExtPP.Console
{
    public class ConsoleEntry : AConsole
    {
        public override string ConsoleKey => "extpp";
        public override string ConsoleTitle => "External Text Pre Processor";

        public override bool Run(string[] args)
        {
            VersionAccumulatorManager.SearchForAssemblies();
            CLI.StartConsole(args);
            return true;
        }
    }
}