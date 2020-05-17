using System;
using System.IO;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.PackageCreator;
using Byt3.Engine.BuildTools.PackageCreator.Versions;

namespace Byt3Console.Engine.Player.Commands
{
    public class AddEngineCommand : AbstractCommand
    {
        public AddEngineCommand() : base(new[] {"--add-engine", "-a"},
            "--add-engine <<Path/To/File.engine>\nAdds an engine file to the engine cache")
        {
            CommandAction = (info, strings) => AddEngine(strings);
        }

        private static void AddEngine(string path)
        {
            try
            {
                IPackageManifest pm = Creator.ReadManifest(path);
                if (!EnginePlayerConsole.EngineVersions.Contains(pm.Version))
                {
                    Console.WriteLine("Adding Engine: " + pm);
                    EnginePlayerConsole.EngineVersions.Add(pm.Version);
                    File.Copy(path, EnginePlayerConsole.EngineDir + "/" + pm.Version + ".engine");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not add Engine to Player.");
                Console.WriteLine(e);
                throw;
            }
        }

        public static void AddEngine(string[] args)
        {
            if (args.Length == 0 || !File.Exists(args[0]) || !args[0].EndsWith(".engine"))
            {
                Console.WriteLine("Could not load Engine Path");
                return;
            }

            AddEngine(args[0]);
        }
    }
}