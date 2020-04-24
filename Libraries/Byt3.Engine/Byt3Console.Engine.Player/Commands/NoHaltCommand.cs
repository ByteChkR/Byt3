using Byt3.CommandRunner;

namespace Byt3Console.Engine.Player.Commands
{
    public class NoHaltCommand : AbstractCommand
    {

        private static void NoHalt(StartupArgumentInfo info, string[] args)
        {
            EnginePlayer.ReadLine = false;
        }


        public NoHaltCommand() : base(NoHalt, new[] { "--no-halt", "-nH" }, "Does not wait for user input before exiting", false)
        {

        }
    }
}