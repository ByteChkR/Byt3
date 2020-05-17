using System.IO;
using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class RemoveEngineCommand : AbstractCommand
    {
        public RemoveEngineCommand() : base(new[] {"--remove-engine", "-r"},
            "--remove-engine <Version>\nRemoves an engine Version from the engine cache")
        {
            CommandAction = (info, strings) => RemoveEngine(strings);
        }

        private static void RemoveEngine(string[] args)
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
    }
}