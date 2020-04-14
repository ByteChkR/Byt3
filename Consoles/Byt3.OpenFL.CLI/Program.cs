using System;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.OpenFL.CLI.Commands;

namespace Byt3.OpenFL.CLI
{
    internal class Program
    {

        public static PreProcessorSettings Settings = PreProcessorSettings.GetDefault();

        private static void Main(string[] args)
        {

            Debug.DefaultInitialization();

            Runner.AddCommand(new DefaultHelpCommand());
            Runner.AddCommand(new SetOutputFilesCommand());
            Runner.AddCommand(new SetSettingsCommand(SetSettingsCommand.Create(SetSettingsCommand.Create("Settings", Settings))));
            Runner.AddCommand(new RunCommand());
            Runner.RunCommands(args);
#if DEBUG
            Console.WriteLine("Press Enter to Exit.");
            Console.ReadLine();
#endif
        }



    }
}
