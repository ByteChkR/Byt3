using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class SetEnginePathCommand : AbstractCommand
    {
        public SetEnginePathCommand() : base(new[] {"--engine-path", "-eP"},
            "--engine-path <Path/To/File.game>\nSpecify a manual path to a .engine file")
        {
            CommandAction = (info, strings) => SetEnginePath(strings);
        }

        private static void SetEnginePath(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("No engine path specified");
            }
            else
            {
                System.Console.WriteLine("Overriding Engine Path: " + args[0]);
                EnginePlayerConsole.EngineVersion = "path:" + args[0];
            }
        }
    }
}