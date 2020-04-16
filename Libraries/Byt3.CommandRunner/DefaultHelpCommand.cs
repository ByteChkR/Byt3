using Byt3.ADL;

namespace Byt3.CommandRunner
{
    public class DefaultHelpCommand : AbstractCommand
    {
        public DefaultHelpCommand() : base(new[] {"--help", "-h", "-?"}, "Prints this help text")
        {
            CommandAction = DefaultHelp;
        }

        private void DefaultHelp(StartupArgumentInfo argumentInfo, string[] args)
        {
            for (int i = 0; i < Runner.CommandCount; i++)
            {
                Logger.Log(LogType.Log, "__________________________________________________________",1);
                Logger.Log(LogType.Log, "", 1);
                Logger.Log(LogType.Log, Runner.GetCommandAt(i).ToString(), 1);
            }
        }
    }
}