using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class NoHaltCommand : AbstractCommand
    {
        private static void NoHalt()
        {
            EnginePlayerConsole.ReadLine = false;
        }


        public NoHaltCommand() : base( new[] {"--no-halt", "-nH"}, "Does not wait for user input before exiting",
            false)
        {
            CommandAction = (info, strings) => NoHalt();
        }
    }
}