using System.IO;
using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class DefaultCommand : AbstractCommand
    {
        private static void Default(StartupArgumentInfo info, string[] args)
        {
            if (args.Length == 0 || !File.Exists(args[0]))
            {
                System.Console.WriteLine("Drag a file onto the executable, or specify the path in the command line.");
            }
            else if (args[0].EndsWith(".engine"))
            {
                AddEngineCommand.AddEngine( args);
            }
            else if (args[0].EndsWith(".game"))
            {
                RunCommand.Run( args);
            }
        }

        public DefaultCommand() : base( new[] {"--default"},
            "Default command.\nExecutes the --run Command when a game is passed\nExecutes the --add-engine command when an engine is passed",
            true)
        {
            CommandAction = Default;
        }
    }
}