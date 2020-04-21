using System;
using Byt3.CommandRunner;
using Byt3.Engine.BuildTools.PackageCreator;

namespace Byt3.Engine.BuildTools.Commands
{
    public class CreatePatchDeltaCommand :AbstractCommand
    {
        private static void CreatePatchDelta(StartupArgumentInfo info, string[] args)
        {
            if (args.Length != 3)
            {
                throw new ApplicationException("Invalid Input");
            }

            try
            {
                Creator.CreatePatchFromDelta(args[0], args[1], args[2]);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Input Error", e);
            }
        }
        public CreatePatchDeltaCommand() : base(CreatePatchDelta, new[] { "--create-patch-delta", "-cdpatch" }, "--create-patch-delta <oldFile> <newFile> <destinationFile>", false)
        {

        }
    }
}