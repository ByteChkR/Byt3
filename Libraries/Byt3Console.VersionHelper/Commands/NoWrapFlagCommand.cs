using Byt3.CommandRunner;

namespace Byt3Console.VersionHelper.Commands
{
    public class NoWrapFlagCommand : AbstractCommand
    {
        public static bool NoWrap;

        public NoWrapFlagCommand() : base(new[] {"--no-wrap", "-nw"}, "Disables the Max Values for the Version Strings")
        {
            CommandAction = (info, strings) => NoWrapFlag();
        }

        private void NoWrapFlag()
        {
            NoWrap = true;
        }
    }
}