using System;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.PackageCreator;

namespace Byt3.Engine.BuildTools.Commands
{
    public class PatchCommand : AbstractCommand
    {
        private static void PatchPackage( string[] args)
        {
            if (args.Length != 2)
            {
                throw new ApplicationException("Invalid Input");
            }

            try
            {
                Creator.PatchPackage(args[0], args[1]);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Input Error", e);
            }
        }

        public PatchCommand() : base( new[] {"--patch", "-p"},
            "--patch <targetFile> <patchFile>\nApplies the patch to the file.", false)
        {
            CommandAction = (info, strings) => PatchPackage(strings);
        }
    }
}