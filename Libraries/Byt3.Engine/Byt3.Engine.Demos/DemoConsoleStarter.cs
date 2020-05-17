using System.Reflection;
using Byt3.ADL;
using Byt3.Callbacks;
using Byt3.Engine.Core;
using Byt3.Engine.Debug;
using Byt3.Engine.Demos.scenes;
using Byt3.OpenCL;
using Byt3.Utilities.ConsoleInternals;
using Byt3.Utilities.ManifestIO;

namespace Byt3.Engine.Demos
{
    public class DemoConsoleStarter : AConsole
    {
        private static readonly ADLLogger<DebugChannel> Logger =
            new ADLLogger<DebugChannel>(EngineDebugConfig.Settings, "Demo Console Starter");


        public override string ConsoleKey => "demos";
        public override string ConsoleTitle => "Minor Engine Demos";

        public override bool Run(string[] args)
        {
            ADL.Debug.DefaultInitialization();
            EmbeddedFileIOManager.Initialize();

            ManifestReader.RegisterAssembly(Assembly
                .GetExecutingAssembly()); //Register this assembly(where the files will be embedded in)
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
            engine.InitializeScene<PhysicsDemoScene>();
            engine.Run();
            engine.Dispose();

            HandleBase.DisposeAllHandles();
            EngineStatisticsManager.DisposeAllHandles();

            return true;
        }
    }
}