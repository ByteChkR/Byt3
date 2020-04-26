using Byt3.CommandRunner;

namespace Byt3Console.OpenFL.Commands
{
    public class ExtraStepCommand : AbstractCommand
    {
        internal static string[] extras = new string[0];

        public ExtraStepCommand() : base(SetExtraSteps, new[] {"--set-extra", "-se"},
            "Can be used to perform extra operations when serializing")
        {
        }

        private static void SetExtraSteps(StartupArgumentInfo arg1, string[] arg2)
        {
            extras = arg2;
        }
    }
}