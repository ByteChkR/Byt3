using System;

namespace Byt3.CommandRunner
{
    public class DefaultHelpCommand : AbstractCommand
    {


        public DefaultHelpCommand() : base(DefaultHelp, new[] {"--help", "-h", "-?"}, "Prints this help text")
        {

        }

        private static void DefaultHelp(StartupInfo info, string[] args)
        {
            for (int i = 0; i < Runner.CommandCount; i++)
            {
                Console.WriteLine("__________________________________________________________");
                Console.WriteLine("");
                Console.WriteLine(Runner.GetCommandAt(i).ToString());
            }
        }

    }
}