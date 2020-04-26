using System;
using System.Collections.Generic;
using System.Reflection;
using Byt3.ADL;
using Byt3.ADL.Configs;
using Byt3.Engine.Core;
using Byt3.Engine.Debug;
using Byt3.Engine.Tutorials.Tutorials;
using Byt3.OpenCL;
using Byt3.Utilities.ConsoleInternals;
using Byt3.Utilities.ManifestIO;

namespace Byt3.Engine.Tutorials
{
    public class TutorialConsoleStarter : AConsole
    {
        private static readonly ADLLogger<LogType> Logger =
            new ADLLogger<LogType>(new ProjectDebugConfig("Engine Demos", -1, 4,
                PrefixLookupSettings.AddPrefixIfAvailable));

        private GameEngine ge;

        private readonly Dictionary<string, Type> scenes = new Dictionary<string, Type>
        {
            {"AI", typeof(AIScene)},
            {"Audio", typeof(AudioScene)},
            {"CL", typeof(CLScene)},
            {"FL", typeof(FLScene)},
            {"FLRunner", typeof(FLRunnerScene)},
            {"GettingStarted", typeof(GettingStartedScene)},
            {"Physics", typeof(PhysicsScene)},
            {"RayCasting", typeof(RayCastingScene)},
            {"RenderTargets", typeof(RenderTargetsScene)},
        };


        public override string ConsoleKey => "tutorials";
        public override string ConsoleTitle => "Minor Engine Tutorials";

        public override bool Run(string[] args)
        {
            ADL.Debug.DefaultInitialization();
            EmbeddedFileIOManager.Initialize();

            ManifestReader.RegisterAssembly(Assembly
                .GetExecutingAssembly()); //Register this assembly(where the files will be embedded in)
            ManifestReader.PrepareManifestFiles(false); //First Read Assembly files
            ManifestReader
                .PrepareManifestFiles(true); //Replace Any Loaded assembly files with files on the file system.

            ge = new GameEngine(EngineSettings.DefaultSettings);


            Logger.Log(LogType.Log, "\"exit\" closes the Console.", 1);
            Logger.Log(LogType.Log, "\"nameofdemo\" Opens the selected Demo.", 1);
            ListSceneNames();
            if (args.Length != 0)
            {
                RunCommand(args);
            }

            while (true)
            {
                Console.Write("engine/demos>");
                string format = Console.ReadLine();
                if (format == "exit")
                {
                    break;
                }

                string[] arg = format.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                RunCommand(arg);
            }

            return true;
        }


        private void RunCommand(string[] args)
        {
            if (args.Length != 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (scenes.TryGetValue(args[i], out Type sceneType))
                    {
                        StartScene(sceneType);
                    }
                    else
                    {
                        Logger.Log(LogType.Error, $"Invalid Scene Name: {args[0]}", 1);
                    }
                }
            }
            else
            {
                Logger.Log(LogType.Error,
                    "Invalid Scene Name: " + (args.Length == 0 ? "No Argument Provided" : args[0]), 1);
            }
        }

        private void ListSceneNames()
        {
            Logger.Log(LogType.Log, "Valid Scene Names: ", 1);
            foreach (KeyValuePair<string, Type> keyValuePair in scenes)
            {
                Logger.Log(LogType.Log, "\tScene Name: " + keyValuePair.Key, 1);
            }
        }


        private void StartScene(Type t)
        {
            ge.Initialize();
            ge.InitializeScene(t);
            ge.Run();

            //Clean up our mess
            EngineStatisticsManager.DisposeAllHandles(); //Dispose GL Handles
            HandleBase.DisposeAllHandles(); //Dispose CL Handles
        }
    }
}