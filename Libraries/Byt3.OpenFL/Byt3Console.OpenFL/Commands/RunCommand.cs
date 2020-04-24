using System.Drawing;
using System.IO;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;
using Byt3.OpenFL.Serialization;
using Byt3.OpenFL.Threading;

namespace Byt3Console.OpenFL.Commands
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
                new FLScriptRunner(CLAPI.MainThread, DataVectorTypes.Uchar1, ConsoleEntry.Settings.KernelFolder);

            string[] inputFiles = args;
            string[] outputFiles = SetOutputFilesCommand.OutputFiles;


            for (int i = 0; i < inputFiles.Length; i++)
            {
                string inp = inputFiles[i];
                string outp = outputFiles.Length > i
                    ? outputFiles[i]
                    : $"./{Path.GetFileNameWithoutExtension(inp)}.out.png";

                Bitmap bmp = new Bitmap(ConsoleEntry.Settings.InternalResolution.X,
                    ConsoleEntry.Settings.InternalResolution.Y);

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

    public class BuildCommand : AbstractCommand
    {
        public BuildCommand() : base(new[] {"--build", "-b"}, "Builds a FL Script", true)
        {
            CommandAction = (info, strings) => Build(strings);
        }

        private void Build(string[] args)
        {
            string[] inputFiles = args;
            string[] outputFiles = SetOutputFilesCommand.OutputFiles;
            FLParser p = new FLParser(FLInstructionSet.CreateWithBuiltInTypes(CLAPI.MainThread,ConsoleEntry.KernelPath),
                BufferCreator.CreateWithBuiltInTypes());

            Logger.Log(LogType.Log, $"Building {inputFiles.Length} Files", 1);

            for (int i = 0; i < inputFiles.Length; i++)
            {
                string inp = inputFiles[i];
                Logger.Log(LogType.Log, $"Building {inp}", 2);
                string outp = outputFiles.Length > i
                    ? outputFiles[i]
                    : $"./{Path.GetFileNameWithoutExtension(inp)}.flc";

                SerializableFLProgram prog = p.Process(new FLParserInput(inp));

                Stream dst = File.Create(outp);
                FLSerializer.SaveProgram(dst, prog, ExtraStepCommand.extras);
                dst.Close();
                Logger.Log(LogType.Log, $"Output: {outp}", 2);
            }

            Logger.Log(LogType.Log, $"Finished Building {inputFiles.Length} Files", 1);
        }
    }
}