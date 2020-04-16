using System;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.CommandRunner;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.CLI.Commands;
using Byt3.OpenFL.Parsing;

namespace Byt3.OpenFL.CLI
{
    internal class Program
    {

        public static PreProcessorSettings Settings = PreProcessorSettings.GetDefault();

        private static void Main(string[] args)
        {




            Debug.DefaultInitialization();

            //OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level20;
            //OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Silent;
            //InternalADLProjectDebugConfig.Settings.MinSeverity = Verbosity.Silent;
            //CommandRunnerDebugConfig.Settings.MinSeverity = Verbosity.Silent;
            //ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Silent;


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
