using System;
using Byt3.Utilities.Console.Internals;

namespace Byt3.Engine.BuildTools.Console
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
