using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.Engine.Core;
using Byt3.Engine.Debug;
using Byt3.Engine.Tutorials.Tutorials;
using Byt3.Engine.Tutorials.Tutorials.Components;
using Byt3.OpenCL;
using Byt3.Utilities.Exceptions;
using Byt3.Utilities.ManifestIO;

namespace TestingProjectConsole.Commands
{
    public class EngineSceneRunCommand : AbstractCommand
    {
        internal static bool AttachTimeout = false;
        internal static float TimeoutTime = 15f;
        private readonly GameEngine ge;

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

        public EngineSceneRunCommand() : base(new[] {"--engine", "-e"},
            "Start Engine Test Shell.")
        {
            CommandAction = (info, strings) => EngineTest(strings);

            EmbeddedFileIOManager.Initialize();

            ManifestReader.RegisterAssembly(Assembly
                .GetExecutingAssembly()); //Register this assembly(where the files will be embedded in)
            ManifestReader.PrepareManifestFiles(false); //First Read Assembly files
            ManifestReader
                .PrepareManifestFiles(true); //Replace Any Loaded assembly files with files on the file system.

            ge = new GameEngine(EngineSettings.DefaultSettings);
        }

        private void EngineTest(string[] args)
        {
            if (args.Length != 0)
            {
                RunCommand(args);
            }

            while (true)
            {
                Console.Write("root/engine>");
                string format = Console.ReadLine();
                if (format == "exit")
                {
                    break;
                }

                string[] arg = format.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (format == "all")
                {
                    arg = scenes.Keys.ToArray();
                }

                RunCommand(arg);
            }
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
                        throw new Byt3Exception("Can not find scene with name: " + args[i]);
                    }
                }
            }
            else
            {
                Logger.Log(LogType.Error,
                    "Invalid Scene Name: " + (args.Length == 0 ? "No Argument Provided" : args[0]), 1);

                Logger.Log(LogType.Log, "Valid Scene Names: ", 1);
                foreach (KeyValuePair<string, Type> keyValuePair in scenes)
                {
                    Logger.Log(LogType.Log, "\tScene Name: " + keyValuePair.Key, 1);
                }
            }
        }


        private void StartScene(Type t)
        {
            ge.Initialize();
            ge.InitializeScene(t);

            if (AttachTimeout)
            {
                ge.SetDebugComponents(new AbstractComponent[] {new TimeoutComponent(TimeoutTime),});
            }

            ge.Run();

            if (!Directory.Exists("./logs"))
            {
                Directory.CreateDirectory("./logs");
            }

            EngineStatisticsManager.WriteStatistics("./logs/" + t.Name + ".statistics.log");

            EngineStatisticsManager.DisposeAllHandles();
            HandleBase.DisposeAllHandles();
        }
    }
}