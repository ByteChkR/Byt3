using System.Drawing;
using System.IO;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Serialization;
using Byt3.Utilities.ProgressFeedback;

namespace Byt3.OpenFL.ResourceManagement
{
    public class FLC2TexUnpacker : ResourceTypeUnpacker
    {
        private FLRunner runner;
        public FLC2TexUnpacker(CLAPI instance, FLInstructionSet iset, BufferCreator bc)
        {
            runner = new FLRunner(instance, iset, bc, FLProgramCheckBuilder.CreateDefaultCheckBuilder(iset, bc, FLProgramCheckType.InputValidationOptimized));
        }

        public override string UnpackerName => "flc2tex";
        public override void Unpack(string targetDir, string name, Stream stream, IProgressIndicator progressIndicator)
        {
            progressIndicator.SetProgress($"[{UnpackerName}]Loading FL Program: {name}", 1, 3);
            SerializableFLProgram prog = FLSerializer.LoadProgram(stream, runner.InstructionSet);

            progressIndicator.SetProgress($"[{UnpackerName}]Running FL Program: {name}", 2, 3);
            FLProgram p = runner.Run(prog, 512, 512, 1);

            string filePath = Path.Combine(targetDir, name.Replace("/", "\\").StartsWith("\\") ? name.Replace("/", "\\").Substring(1) : name.Replace("/", "\\"));
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            filePath = filePath.Replace(".flc", ".png");

            progressIndicator.SetProgress($"[{UnpackerName}]Writing FL Program Output: {Path.GetFileNameWithoutExtension(name)}", 3, 3);
            Bitmap bmp = new Bitmap(512, 512);
            CLAPI.UpdateBitmap(runner.Instance, bmp, p.GetActiveBuffer(false).Buffer);
            bmp.Save(filePath);
            stream.Close();
            p.FreeResources();
            progressIndicator.Dispose();
        }
    }
}