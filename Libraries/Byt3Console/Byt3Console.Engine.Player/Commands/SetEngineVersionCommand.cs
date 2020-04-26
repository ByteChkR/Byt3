using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class SetEngineVersionCommand : AbstractCommand
    {


        private static void SetEngineVersion(StartupArgumentInfo info, string[] args)
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

        public SetEngineVersionCommand() : base(SetEngineVersion, new[] { "--engine", "-e" }, "--engine <Version>\nSpecify a manual version", false)
        {

        }
    }
}