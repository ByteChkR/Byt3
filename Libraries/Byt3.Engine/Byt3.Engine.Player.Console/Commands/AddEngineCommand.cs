using System;
using System.IO;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.PackageCreator;
using Byt3.Engine.BuildTools.PackageCreator.Versions;

namespace Byt3.Engine.Player.Console.Commands
{
    public class AddEngineCommand : AbstractCommand
    {
        private static void AddEngine(string path)
        {
            try
            {
                IPackageManifest pm = Creator.ReadManifest(path);
                if (!EnginePlayer.EngineVersions.Contains(pm.Version))
                {
                    System.Console.WriteLine("Adding Engine: " + pm);
                    EnginePlayer.EngineVersions.Add(pm.Version);
                    File.Copy(path, EnginePlayer.EngineDir + "/" + pm.Version + ".engine");
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Could not add Engine to Player.");
                System.Console.WriteLine(e);
                throw;
            }
        }
        public static void AddEngine(StartupArgumentInfo info, string[] args)
        {
            if (args.Length == 0 || !File.Exists(args[0]) || !args[0].EndsWith(".engine"))
            {
                System.Console.WriteLine("Could not load Engine Path");
                return;
            }

            AddEngine(args[0]);
        }

        public AddEngineCommand() : base(AddEngine, new[] { "--add-engine", "-a" }, "--add-engine <<Path/To/File.engine>\nAdds an engine file to the engine cache", false)
        {

        }
    }
}