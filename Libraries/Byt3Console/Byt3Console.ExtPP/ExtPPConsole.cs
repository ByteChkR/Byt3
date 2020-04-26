using Byt3.Utilities.ConsoleInternals;

namespace Byt3Console.ExtPP
{
    public class ExtPPConsole : AConsole
    {
        public override string ConsoleKey => "extpp";
        public override string ConsoleTitle => "External Text Pre Processor";

        public override bool Run(string[] args)
        {
            CLI.StartConsole(args);
            return true;
        }
    }
}