using System;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.PackageCreator;

namespace Byt3.Engine.BuildTools.Commands
{
    public class PatchPermanentCommand : AbstractCommand
    {
        public PatchPermanentCommand() : base(new[] {"--patch-permanent", "-pp"},
            "--patch-permanent <targetFile> <patchFile>\nApplies the patch to the file permanently.")
        {
            CommandAction = (info, strings) => PatchPackagePermanent(strings);
        }

        private static void PatchPackagePermanent(string[] args)
        {
            if (args.Length != 2)
            {
                throw new BuilderInputException("Invalid Input");
            }

            try
            {
                Creator.PatchPackagePermanent(args[0], args[1]);
            }
            catch (Exception e)
            {
                throw new BuilderInputException("Input Error", e);
            }
        }
    }
}