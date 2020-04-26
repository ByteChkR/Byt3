using System;
using Byt3.ADL;
using Byt3.CommandRunner;

namespace TestingProjectConsole.Commands
{
    public class VersionHelperTestCommand : AbstractCommand
    {


        public VersionHelperTestCommand() : base(new[] { "--version-helper-tests", "-vht" },
            "Start Version Helper Test Shell.")
        {
            CommandAction = (info, strings) => VersionHelperTest();
        }

        private void VersionHelperTest()
        {
            Version v = Version.Parse("1.0.0.0");
            Logger.Log(LogType.Log, "r = Reset version to 1.0.0.0", 1);
            Logger.Log(LogType.Log, "exit = Exit Version Helper Test Shell", 1);
            while (true)
            {
                System.Console.WriteLine(v);
                System.Console.Write(">");
                string format = System.Console.ReadLine();
                if (format == "exit") return;
                if (format == "r")
                {
                    v = Version.Parse("1.0.0.0");
                    continue;
                }

                v = Byt3Console.VersionHelper.VersionHelperConsole.ChangeVersion(v, format);
            }
        }

    }
}