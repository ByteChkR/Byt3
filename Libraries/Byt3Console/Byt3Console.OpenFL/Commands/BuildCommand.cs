using System;
using System.IO;
using Byt3.ADL;
using Byt3.CommandRunner;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;
using Byt3.OpenFL.Serialization;

namespace Byt3Console.OpenFL.Commands
{
    public class BuildCommand : AbstractCommand
    {
        public BuildCommand() : base(new[] {"--build", "-b"}, "Builds a FL Script", true)
        {
            CommandAction = (info, strings) => Build();
        }

        private void Build()
        {
            FLConsole.Settings.SetVerbosity();
            string[] inputFiles = SetInputFilesCommand.InputFiles;
            string[] outputFiles = SetOutputFilesCommand.OutputFiles;

            BufferCreator creator = new BufferCreator();
            FLConsole.Settings.BufferCreatorTypes.ForEach(x =>
                creator.AddBufferCreator((ASerializableBufferCreator)Activator.CreateInstance(x)));
            FLInstructionSet iset = FLInstructionSet.CreateWithBuiltInTypes(CLAPI.MainThread, FLConsole.Settings.KernelFolder);
            FLProgramCheckBuilder programCheckBuilder = new FLProgramCheckBuilder(iset, creator);

            FLParser p = new FLParser(iset, creator,
                new WorkItemRunnerSettings(FLConsole.Settings.MultiThread,
                    FLConsole.Settings.WorkSizeMultiplier));
            programCheckBuilder.Attach(p, true);

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