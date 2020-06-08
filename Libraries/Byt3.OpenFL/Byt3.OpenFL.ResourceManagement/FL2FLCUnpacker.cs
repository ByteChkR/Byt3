using System.IO;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.Parsing.StageResults;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Serialization;
using Byt3.Utilities.ProgressFeedback;

namespace Byt3.OpenFL.ResourceManagement
{
    public class FL2FLCUnpacker : ResourceTypeUnpacker
    {
        private FLRunner runner;
        public FL2FLCUnpacker(CLAPI instance, FLInstructionSet iset, BufferCreator bc)
        {
            runner = new FLRunner(instance, iset, bc, FLProgramCheckBuilder.CreateDefaultCheckBuilder(iset, bc, FLProgramCheckType.InputValidationOptimized));
        }

        public override string UnpackerName => "fl2flc";
        public override void Unpack(string targetDir, string name, Stream stream, IProgressIndicator progressIndicator)
        {

            progressIndicator.SetProgress($"[{UnpackerName}]Loading FL Program: {name}", 1, 3);
            SerializableFLProgram prog = runner.Parser.Process(new FLParserInput(name));

            string filePath = Path.Combine(targetDir, name.Replace("/", "\\").StartsWith("\\") ? name.Replace("/", "\\").Substring(1) : name.Replace("/", "\\"));
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            filePath = filePath + "c";

            progressIndicator.SetProgress($"[{UnpackerName}]Writing FL Program Output: {Path.GetFileNameWithoutExtension(name)}", 3, 3);
            Stream s = File.OpenWrite(filePath);
            FLSerializer.SaveProgram(s, prog, runner.InstructionSet, new string[0]);
            s.Dispose();
            progressIndicator.Dispose();
        }
    }
}