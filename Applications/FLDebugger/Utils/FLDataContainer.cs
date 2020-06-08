using System;
using System.Windows.Forms;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using FLDebugger.Forms;

namespace FLDebugger.Utils
{
    public class FLDataContainer : IDisposable
    {
        public readonly BufferCreator BufferCreator;
        public readonly FLProgramCheckBuilder CheckBuilder;
        public readonly CLAPI Instance;
        public readonly FLInstructionSet InstructionSet;
        public readonly KernelDatabase KernelDB;
        public readonly FLParser Parser;
        public SerializableFLProgram SerializedProgram;

        public FLDataContainer(string path)
        {
            Instance = CLAPI.GetInstance();


            KernelLoader kl = new KernelLoader(Instance, path);

            if (kl.ShowDialog() == DialogResult.Abort)
            {
                Application.Exit();
                return;
            }

            KernelDB = kl.Database;
            InstructionSet = FLInstructionSet.CreateWithBuiltInTypes(KernelDB);
            BufferCreator = BufferCreator.CreateWithBuiltInTypes();
            Parser = new FLParser(InstructionSet, BufferCreator, new WorkItemRunnerSettings(true, 2));
            CheckBuilder = new FLProgramCheckBuilder(InstructionSet, BufferCreator);
        }

        public void Dispose()
        {
            KernelDB.Dispose();
            InstructionSet.Dispose();
            Instance.Dispose();
        }
    }
}