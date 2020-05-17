using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class NoHaltCommand : AbstractCommand
    {
        public NoHaltCommand() : base(new[] {"--no-halt", "-nH"}, "Does not wait for user input before exiting")
        {
            CommandAction = (info, strings) => NoHalt();
        }

        private static void NoHalt()
        {
            EnginePlayerConsole.ReadLine = false;
        }
    }
}