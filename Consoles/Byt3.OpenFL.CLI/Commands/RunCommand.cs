using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Threading;

namespace Byt3.OpenFL.CLI.Commands
{
    public class RunCommand : AbstractCommand
    {
        public RunCommand() : base(new[] {"--run", "-r"}, "Runs a FL Script", true)
        {
            CommandAction = (info, strings) => Run(strings);
        }

        private void Run(string[] args)
        {
            FLScriptRunner runner =
                new FLScriptRunner(CLAPI.MainThread, DataVectorTypes.Uchar1, Program.Settings.KernelFolder);

            string[] inputFiles = args;
            string[] outputFiles = SetOutputFilesCommand.OutputFiles;


            for (int i = 0; i < inputFiles.Length; i++)
            {
                string inp = inputFiles[i];
                string outp = outputFiles.Length > i
                    ? outputFiles[i]
                    : $"./{Path.GetFileNameWithoutExtension(inp)}.out.png";

                Bitmap bmp = new Bitmap(Program.Settings.InternalResolution.X, Program.Settings.InternalResolution.Y);

                runner.Enqueue(new FlScriptExecutionContext(inp, bmp, result => OnFinishCallback(result, outp)));
            }

            runner.Process();
        }

        private void OnFinishCallback(FLProgram obj, string outputFile)
        {
            Logger.Log(LogType.Log, "Saving Output File: " + Path.GetFullPath(outputFile), 1);
            FLBuffer result = obj.ActiveBuffer;
            Bitmap bmp = new Bitmap(result.Width, result.Height);
            CLAPI.UpdateBitmap(CLAPI.MainThread, bmp,
                CLAPI.ReadBuffer<byte>(CLAPI.MainThread, result.Buffer, (int) result.Size));
            bmp.Save(outputFile);
        }
    }
}