using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class SetEngineVersionCommand : AbstractCommand
    {
        public SetEngineVersionCommand() : base(new[] {"--engine", "-e"},
            "--engine <Version>\nSpecify a manual version")
        {
            CommandAction = (info, strings) => SetEngineVersion(strings);
        }

        private static void SetEngineVersion(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("No engine version specified");
            }
            else
            {
                System.Console.WriteLine("Overriding Engine Version: " + args[0]);
                EnginePlayerConsole.EngineVersion = args[0];
            }
        }
    }
}