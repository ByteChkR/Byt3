using System;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.PackageCreator;

namespace Byt3.Engine.BuildTools.Commands
{
    public class PatchPermanentCommand : AbstractCommand
    {
        private static void PatchPackagePermanent( string[] args)
        {
            if (args.Length != 2)
            {
                throw new ApplicationException("Invalid Input");
            }

            try
            {
                Creator.PatchPackagePermanent(args[0], args[1]);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Input Error", e);
            }
        }

        public PatchPermanentCommand() : base(new[] {"--patch-permanent", "-pp"},
            "--patch-permanent <targetFile> <patchFile>\nApplies the patch to the file permanently.", false)
        {
            CommandAction = (info, strings) => PatchPackagePermanent(strings);
        }
    }
}