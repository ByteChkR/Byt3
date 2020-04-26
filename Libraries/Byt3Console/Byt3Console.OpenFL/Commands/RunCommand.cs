using System;
using System.Drawing;
using System.IO;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Threading;

namespace Byt3Console.OpenFL.Commands
{
    public class RunCommand : AbstractCommand
    {
        public RunCommand() : base(new[] {"--run", "-r"}, "Runs a FL Script", true)
        {
            CommandAction = (info, strings) => Run();
        }

        private void Run()
        {
            FLConsole.Settings.SetVerbosity();
            BufferCreator creator = new BufferCreator();
            FLConsole.Settings.BufferCreatorTypes.ForEach(x =>
                creator.AddBufferCreator((ASerializableBufferCreator) Activator.CreateInstance(x)));
            KernelDatabase db  =new KernelDatabase(CLAPI.MainThread, FLConsole.Settings.KernelFolder, DataVectorTypes.Uchar1);
            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(db);
            FLProgramCheckBuilder programCheckBuilder = new FLProgramCheckBuilder(iset, creator);
            FLConsole.Settings.ProgramCheckTypes.ForEach(x =>
                programCheckBuilder.AddProgramCheck((FLProgramCheck)Activator.CreateInstance(x)));


            FLScriptRunner runner =
                new FLScriptRunner(CLAPI.MainThread,db, creator, iset, programCheckBuilder,
                    new WorkItemRunnerSettings(FLConsole.Settings.MultiThread,
                        FLConsole.Settings.WorkSizeMultiplier));

            string[] inputFiles = SetInputFilesCommand.InputFiles;
            string[] outputFiles = SetOutputFilesCommand.OutputFiles;


            for (int i = 0; i < inputFiles.Length; i++)
            {
                string inp = inputFiles[i];
                string outp = outputFiles.Length > i
                    ? outputFiles[i]
                    : $"./{Path.GetFileNameWithoutExtension(inp)}.out.png";

                Bitmap bmp = new Bitmap(FLConsole.Settings.Resolution.X,
                    FLConsole.Settings.Resolution.Y);

                runner.Enqueue(new FlScriptExecutionContext(inp, bmp, result => OnFinishCallback(result, outp)));
            }

            runner.Process();
        }

        private void OnFinishCallback(FLProgram obj, string outputFile)
        {
            Logger.Log(LogType.Log, "Saving Output File: " + Path.GetFullPath(outputFile), 1);
            FLBuffer result = obj.GetActiveBuffer(false);
            Bitmap bmp = new Bitmap(result.Width, result.Height);
            CLAPI.UpdateBitmap(CLAPI.MainThread, bmp,
                CLAPI.ReadBuffer<byte>(CLAPI.MainThread, result.Buffer, (int) result.Size));
            bmp.Save(outputFile);
            result.Dispose();
        }
    }
}