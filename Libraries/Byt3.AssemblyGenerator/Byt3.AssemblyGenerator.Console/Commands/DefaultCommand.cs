using System.IO;
using System.Linq;
using Byt3.ADL;
using Byt3.CommandRunner;

namespace Byt3.AssemblyGenerator.Console.Commands
{
    public class DefaultCommand : AbstractCommand
    {
        public DefaultCommand() : base(new[] {"--set-target", "-t"},
            "Sets the AssemblyModule Target for the current operation", true)
        {
            CommandAction = (info, strings) => Default(strings);
        }

        private void Default(string[] args)
        {
            string target = "";
            if (args.Length > 0)
            {
                target = args[0];
            }
            else
            {
                target = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.assemblyconfig",
                        SearchOption.TopDirectoryOnly)
                    .FirstOrDefault();
                if (target == null)
                {
                    target = "";
                }
                else
                {
                    target = Path.GetFullPath(target);
                }
            }

            Logger.Log(LogType.Log, "Targeting Config: " + target, 1);
            ConsoleEntry.Target = target;
        }
    }
}