using System;
using System.IO;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.CommandRunner;
using Byt3.OpenFL.Console.Commands;
using Byt3.Utilities.Console.Internals;
using Byt3.Utilities.Versioning;

namespace Byt3.OpenFL.Console
{




    public class ConsoleEntry : AConsole
    {
        public static Version ConsoleVersion => Assembly.GetExecutingAssembly().GetName().Version;
        internal static ProjectDebugConfig ConsoleConfig = new ProjectDebugConfig("OpenFL.Console", -1, 4, PrefixLookupSettings.AddPrefixIfAvailable);
        private ADLLogger<LogType> Logger = new ADLLogger<LogType>(ConsoleConfig);
        internal static readonly PreProcessorSettings Settings = PreProcessorSettings.GetDefault();

        public static string KernelPath
        {
            get
            {
                string path = Path.Combine(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath, "kernel");
                return path;
            }
        }
        public override string ConsoleKey => "fl";
        public override string ConsoleTitle => "OpenFL Console Runner";
        public override bool Run(string[] args)
        {
            VersionAccumulatorManager.SearchForAssemblies();
            Debug.DefaultInitialization();


            Runner.AddCommand(new DefaultHelpCommand());
            Runner.AddCommand(new SetOutputFilesCommand());
            Runner.AddCommand(new SetInputFilesCommand());
            Runner.AddCommand(
                new SetSettingsCommand(SetSettingsCommand.Create(SetSettingsCommand.Create("Settings", Settings))));
            Runner.AddCommand(new RunCommand());
            bool ret = Runner.RunCommands(args);
#if DEBUG
            System.Console.WriteLine("Press Enter to Exit.");
            System.Console.ReadLine();
#endif
            return ret;
        }
    }
}