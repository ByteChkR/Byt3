using Byt3.ADL;

namespace Byt3.CommandRunner
{
    public class DefaultHelpCommand : AbstractCommand
    {
        public DefaultHelpCommand() : base(new[] { "--help", "-h", "-?" }, "Prints this help text")
        {
            CommandAction = (info, strings) => DefaultHelp();
        }

        private void DefaultHelp()
        {
            for (int i = 0; i < Runner.CommandCount; i++)
            {
                Logger.Log(LogType.Log, "__________________________________________________________", MIN_COMMAND_SEVERITY);
                Logger.Log(LogType.Log, "", MIN_COMMAND_SEVERITY);
                Logger.Log(LogType.Log, Runner.GetCommandAt(i).ToString(), MIN_COMMAND_SEVERITY);
            }
        }
    }
}