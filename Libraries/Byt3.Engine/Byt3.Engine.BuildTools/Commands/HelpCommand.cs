using System;
using Byt3.CommandRunner;

namespace Byt3.Engine.BuildTools.Commands
{
    public class HelpCommand : AbstractCommand
    {
        private static void Help(StartupArgumentInfo info, string[] args)
        {
            Console.WriteLine("Commands:");
            for (int i = 0; i < Runner.CommandCount; i++)
            {
                Console.WriteLine(Runner.GetCommandAt(i));
            }
        }

        public HelpCommand() : base(Help, new[] {"--help", "-h", "-?"}, "Display this help message", false)
        {
        }
    }
}