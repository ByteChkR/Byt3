using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.CommandRunner;
using Byt3.CommandRunner.SetSettings;
using Byt3.ExtPP.Base;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Benchmarking;
using Byt3.OpenFL.Common;
using Byt3.Utilities.ConsoleInternals;
using Byt3.Utilities.ManifestIO;
using Byt3Console.OpenFL.ScriptGenerator.Commands;

namespace Byt3Console.OpenFL.ScriptGenerator
{
    public class GenerateFLScriptsConsole : AConsole
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(new ProjectDebugConfig("FLScriptGen", -1, 3,
                PrefixLookupSettings.AddPrefixIfAvailable));

        public static FLScriptGeneratorSettings Settings = FLScriptGeneratorSettings.Default;
        public override string ConsoleKey => "flgen";
        public override string ConsoleTitle => "FLScript Generator";
        internal static bool DoExecute = true;

        private class HelpCommand : AbstractCommand
        {
            private DefaultHelpCommand cmd;
            internal HelpCommand(DefaultHelpCommand command) : base(command.CommandKeys, command.HelpText,
                command.DefaultCommand)
            {
                cmd = command;
                CommandAction = RunHelpCommand;
            }

            private void RunHelpCommand(StartupArgumentInfo info, string[] args)
            {
                DoExecute = false;
                cmd.CommandAction(info, args);
            }
        }

        public override bool Run(string[] args)
        {
            EmbeddedFileIOManager.Initialize();
            ManifestReader.RegisterAssembly(typeof(FLScriptGenerator).Assembly);
            ManifestReader.PrepareManifestFiles(false);

            Runner.AddCommand( new HelpCommand(new DefaultHelpCommand(true)));
            Runner.AddCommand(SetSettingsCommand.CreateSettingsCommand("Settings", Settings));

            Runner.RunCommands(args);

            if (!DoExecute) return true;

            Debug.DefaultInitialization();

            ManifestReader.RegisterAssembly(Assembly.GetExecutingAssembly());
            ManifestReader.PrepareManifestFiles(false);

            ExtPPDebugConfig.Settings.MinSeverity = Verbosity.Silent;
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            OpenCLDebugConfig.Settings.MinSeverity = Verbosity.Silent;

            Directory.CreateDirectory(Settings.OutputFolder);
            Random rnd = new Random();

            for (int i = 0; i < Settings.Amount; i++)
            {
                string file = Path.Combine(Settings.OutputFolder, "genscript." + i + ".fl");
                Logger.Log(LogType.Log, "Generating Script...", 1);
                string script = FLScriptGenerator.GenerateRandomScript(
                    rnd.Next(Settings.Functions.Min, Settings.Functions.Max),
                    rnd.Next(Settings.Buffers.Min, Settings.Buffers.Max),
                    rnd.Next(Settings.Additional.Min, Settings.Additional.Max),
                    rnd.Next(Settings.AdditionalFunctions.Min, Settings.AdditionalFunctions.Max));
                Logger.Log(LogType.Log, "Finished Script. Lines: " + script.Count(x => x == '\n'), 1);
                File.WriteAllText(file, script);
            }

            return true;
        }
    }
}