using System;
using System.IO;
using System.Linq;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.OpenFL.Benchmarking;

namespace TestingProjectConsole.Commands
{
    public class GenerateFLScriptsCommand : AbstractCommand
    {

        public GenerateFLScriptsCommand() : base(new[] { "--genfl", "-gfl" },
            "Generates FL Scripts. Default: 10, specify to override.")
        {
            CommandAction = (info, strings) => SetFlag(strings);
        }

        private void SetFlag(string[] args)
        {
            int amount = 10;
            if (args.Length != 0)
            {
                if (int.TryParse(args[0], out int newI)) amount = newI;
            }
            Directory.CreateDirectory("genscripts");
            Random rnd = new Random();

            for (int i = 0; i < amount; i++)
            {
                string file = "genscripts/genscript." + i + ".fl";
                Logger.Log(LogType.Log, "Generating Script...", 1);
                string script = FLScriptGenerator.GenerateRandomScript(rnd.Next(500, 1000), rnd.Next(150, 500), rnd.Next(15, 20), rnd.Next(5000, 10000));
                Logger.Log(LogType.Log, "Finished Script. Lines: " + script.Count(x => x == '\n'), 1);
                File.WriteAllText(file, script);
            }
        }

    }
}