using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.ExtPP.Base;

namespace Byt3Console.OpenFL.Commands
{
    public class ListSettingsCommand : AbstractCommand
    {
        private SetSettingsCommand ss;

        public ListSettingsCommand(SetSettingsCommand cmd) : base(new[] {"--list-settings", "-ls"},
            "Lists all settings that can be changed with the command --set-settings")
        {
            CommandAction = SetExtraSteps;
            ss = cmd;
        }

        private void SetExtraSteps(StartupArgumentInfo arg1, string[] arg2)
        {
            Logger.Log(LogType.Log, "Available Settings: \n" + ss.AllPaths.Unpack("\n"), 1);
        }
    }
}