using System;
using Byt3.ADL;

namespace Byt3.CommandRunner
{
    public class DefaultHelpCommand : AbstractCommand
    {


        public DefaultHelpCommand() : base(new[] { "--help", "-h", "-?" }, "Prints this help text")
        {
            CommandAction = DefaultHelp;
        }

        private void DefaultHelp( StartupInfo info, string[] args)
        {
            for (int i = 0; i < Runner.CommandCount; i++)
            {
                Logger.Log(LogType.Log, "__________________________________________________________");
                Logger.Log(LogType.Log, "");
                Logger.Log(LogType.Log, Runner.GetCommandAt(i).ToString());
            }
        }

    }
}