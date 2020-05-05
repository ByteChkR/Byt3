using System;
using System.Collections.Generic;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.Callbacks;
using Byt3.Engine.Core;
using Byt3.Engine.Debug;
using Byt3.OpenCL;
using Byt3.OpenFL.Common;
using Byt3.Utilities.ConsoleInternals;
using Byt3.Utilities.ManifestIO;
using HorrorOfBindings.scenes;
using OpenTK;
using OpenTK.Graphics;

namespace HorrorOfBindings
{
    public class HOBConsoleStarter : AConsole
    {
        private static readonly ADLLogger<DebugChannel> Logger =
            new ADLLogger<DebugChannel>(new ProjectDebugConfig("Engine Demos", -1, 4,
                PrefixLookupSettings.AddPrefixIfAvailable));


        public override string ConsoleKey => "hob";
        public override string ConsoleTitle => "Horror of Bindings Game";

        public override bool Run(string[] args)
        {
            Byt3.ADL.Debug.DefaultInitialization();
            EmbeddedFileIOManager.Initialize();

            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level3;

            ManifestReader.RegisterAssembly(Assembly
                .GetExecutingAssembly()); //Register this assembly(where the files will be embedded in)
            ManifestReader.RegisterAssembly(typeof(OpenFLDebugConfig).Assembly); //Register the OpenFL.Common Assembly as it contains the CL kernels
            ManifestReader.PrepareManifestFiles(false); //First Read Assembly files
            ManifestReader
                .PrepareManifestFiles(true); //Replace Any Loaded assembly files with files on the file system.


            if (IOManager.FileExists("assemblyList.txt")) //Alternative, load assembly list to register from text file.
            {
                Logger.Log(DebugChannel.Log, "Loading Assembly List", 1);

                ManifestReader.LoadAssemblyListFromFile("assemblyList.txt");
            }


            GameEngine engine = new GameEngine(EngineSettings.DefaultSettings);

            engine.Initialize();
            engine.InitializeScene<HoBMenuScene>();
            engine.Run();
            engine.Dispose();

            HandleBase.DisposeAllHandles();
            EngineStatisticsManager.DisposeAllHandles();

            return true;
        }
    }
}