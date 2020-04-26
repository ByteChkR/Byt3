using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class HelpCommand : AbstractCommand
    {

        public static void Help(StartupArgumentInfo info, string[] args)
        {
            System.Console.WriteLine("Commands:");
            for (int i = 0; i < Runner.CommandCount; i++)
            {
                System.Console.WriteLine(Runner.GetCommandAt(i));
            }
        }

        public HelpCommand() : base(Help, new[] { "--help", "-h", "-?" }, "Display this help message", false)
        {

        }
    }
}