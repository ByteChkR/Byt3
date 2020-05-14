using System;
using System.Reflection;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.CommandRunner.SetSettings;
using Byt3.OpenFL.Common.Instructions;
using Byt3.Utilities.ConsoleInternals;
using Byt3.Utilities.ManifestIO;
using Byt3Console.OpenFL.Commands;

namespace Byt3Console.OpenFL
{
    public class FLConsole : AConsole
    {
        public static Version ConsoleVersion => Assembly.GetExecutingAssembly().GetName().Version;


        internal static readonly FLConsoleSettings Settings = FLConsoleSettings.Default;

        public override string ConsoleKey => "fl";
        public override string ConsoleTitle => "OpenFL Console Runner";

        public override bool Run(string[] args)
        {
            Debug.DefaultInitialization();



            EmbeddedFileIOManager.Initialize();
            ManifestReader.RegisterAssembly(typeof(KernelFLInstruction).Assembly); //Register Built In Kernels
            ManifestReader.PrepareManifestFiles(false); //First Read Assembly files
            ManifestReader.PrepareManifestFiles(true); //Replace Any Loaded assembly files with files on the file system.

            Runner.AddCommand(new DefaultHelpCommand());
            Runner.AddCommand(new SetWorkingDirCommand());
            Runner.AddCommand(new SetOutputFilesCommand());
            Runner.AddCommand(new SetInputFilesCommand());
            Runner.AddCommand(new ExtraStepCommand());
            SetSettingsCommand cmd = SetSettingsCommand.CreateSettingsCommand("Settings", Settings);
            Runner.AddCommand(new ListSettingsCommand(cmd));
            Runner.AddCommand(cmd);
            Runner.AddCommand(new BuildCommand());
            Runner.AddCommand(new RunCommand());
            bool ret = Runner.RunCommands(args);
//#if DEBUG
//            Console.WriteLine("Press Enter to Exit.");
//            Console.ReadLine();
//#endif
            return ret;
        }
    }
}