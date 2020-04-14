using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Threading;

namespace Byt3.OpenFL.CLI.Commands
{
    public class RunCommand : AbstractCommand
    {

        public RunCommand() : base(new[] { "--run", "-r" }, "Runs a FL Script", true)
        {
            CommandAction = Run;
        }

        private void Run(StartupArgumentInfo info, string[] args)
        {
            FLScriptRunner runner = new FLScriptRunner(CLAPI.MainThread, DataVectorTypes.Uchar1, Program.Settings.KernelFolder);

            string[] inputFiles = args;
            string[] outputFiles = SetOutputFilesCommand.OutputFiles;

            Console.ReadLine();

            for (int i = 0; i < inputFiles.Length; i++)
            {
                string inp = inputFiles[i];
                string outp = outputFiles.Length > i
                    ? outputFiles[i]
                    : $"./{Path.GetFileNameWithoutExtension(inp)}.out.png";

                Bitmap bmp = new Bitmap(Program.Settings.InternalResolution.X, Program.Settings.InternalResolution.Y);

                Dictionary<string, Bitmap> dic = new Dictionary<string, Bitmap>
                {
                    {"result", bmp}
                };

                runner.Enqueue(new FlScriptExecutionContext(inp, bmp, dic, obj => OnFinishCallback(obj, outp)));
            }

            runner.Process();
        }

        private void OnFinishCallback(Dictionary<Bitmap, byte[]> obj, string outputFile)
        {
            Logger.Log(LogType.Log, "Saving Output File: " + Path.GetFullPath(outputFile));
            KeyValuePair<Bitmap, byte[]> result = obj.ElementAt(0);
            CLAPI.UpdateBitmap(CLAPI.MainThread, result.Key, result.Value);

            result.Key.Save(outputFile);

        }

    }
}