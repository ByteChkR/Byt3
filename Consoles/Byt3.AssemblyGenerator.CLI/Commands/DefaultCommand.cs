using System;
using System.IO;
using System.Linq;
using Byt3.CommandRunner;

namespace Byt3.AssemblyGenerator.CLI.Commands
{
    public class DefaultCommand : AbstractCommand
    {
        public DefaultCommand() : base(Default, new[] { "--set-target", "-t" },
            "Sets the AssemblyModule Target for the current operation", true)
        {

        }

        private static void Default(StartupInfo info, string[] args)
        {
            string target = "";
            if (args.Length > 0)
            {
                target = args[0];
            }
            else
            {
                target= Directory.GetFiles(Directory.GetCurrentDirectory(), "*.assemblyconfig", SearchOption.TopDirectoryOnly)
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
            
            Console.WriteLine("Targeting Config: " + target);
            Program.Target = target;
        }
    }
}