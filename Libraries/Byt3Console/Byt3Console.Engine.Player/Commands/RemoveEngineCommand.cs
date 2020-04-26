using System.IO;
using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class RemoveEngineCommand : AbstractCommand
    {

        private static void RemoveEngine(StartupArgumentInfo info, string[] args)
        {
            if (EnginePlayerConsole.IsEngineVersionAvailable(args[0]))
            {
                System.Console.WriteLine("Deleting Version " + args[0]);
                File.Delete(EnginePlayerConsole.EngineDir + "/" + args[0] + ".engine");
            }
            else
            {
                System.Console.WriteLine("Engine Version not available. Skipping Deletion.");
            }
        }

        public RemoveEngineCommand() : base(RemoveEngine, new[] { "--remove-engine", "-r" }, "--remove-engine <Version>\nRemoves an engine Version from the engine cache", false)
        {

        }
    }
}