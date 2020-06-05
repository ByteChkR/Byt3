﻿using System.Diagnostics;
using System.IO;
using Byt3.ADL;
using Byt3.CommandRunner;

namespace Byt3Console.VersionHelper.Commands
{
    public class GetVersionCommand : AbstractCommand
    {
        public GetVersionCommand() : base(new[] {"--get-version", "-get"},
            "Returns the File Version of the specified file")
        {
            CommandAction = (info, strings) => GetVersion(strings);
        }

        private void GetVersion(string[] args)
        {
            string v = GetVersion(args[0]);
            Logger.Log(LogType.Log, v, 1);
            if (ToFileCommand.File != null)
            {
                File.WriteAllText(ToFileCommand.File, v);
            }
        }

        private string GetVersion(string file)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(file);
            return fvi.FileVersion;
        }
    }
}