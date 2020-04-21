using Byt3.CommandRunner;

namespace Byt3.Engine.Player.Console.Commands
{
    public class SetEnginePathCommand : AbstractCommand
    {
        private static void SetEnginePath(StartupArgumentInfo info, string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("No engine path specified");
            }
            else
            {
                System.Console.WriteLine("Overriding Engine Path: " + args[0]);
                EnginePlayer.EngineVersion = "path:" + args[0];
            }
        }

        public SetEnginePathCommand() : base(SetEnginePath, new[] { "--engine-path", "-eP" }, "--engine-path <Path/To/File.game>\nSpecify a manual path to a .engine file", false)
        {

        }
    }
}