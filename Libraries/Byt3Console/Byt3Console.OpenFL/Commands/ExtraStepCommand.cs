using Byt3.CommandRunner;

namespace Byt3Console.OpenFL.Commands
{
    public class ExtraStepCommand : AbstractCommand
    {
        internal static string[] extras = new string[0];

        public ExtraStepCommand() : base(new[] {"--set-extra", "-se"},
            "Can be used to perform extra operations when serializing")
        {
            CommandAction = (info, strings) => SetExtraSteps(strings);
        }

        private static void SetExtraSteps(string[] arg2)
        {
            extras = arg2;
        }
    }
}