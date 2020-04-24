using Byt3.Engine.BuildTools;
using Byt3.Utilities.ConsoleInternals;

namespace Byt3Console.Engine.BuildTools
{
    public class BuilderConsole : AConsole
    {
        public override string ConsoleKey => "ebuild";
        public override string ConsoleTitle => "Engine Builder";
        public override bool Run(string[] args)
        {
            return Builder.RunCommand(args);
        }
    }
}
