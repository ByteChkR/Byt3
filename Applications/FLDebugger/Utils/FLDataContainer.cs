using System;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenCL.Wrapper.TypeEnums;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using FLDebugger.Forms;

namespace FLDebugger.Utils
{
    internal class FLDataContainer
    {
        public readonly BufferCreator BufferCreator;
        public readonly FLProgramCheckBuilder CheckBuilder;
        public readonly CLAPI Instance;
        public readonly FLInstructionSet InstructionSet;
        public readonly KernelDatabase KernelDB;
        public readonly FLParser Parser;
        public SerializableFLProgram SerializedProgram;

        public FLDataContainer()
        {
            Instance = CLAPI.GetInstance();


            bool crash = false;
            do
            {
                try
                {
                    crash = false;
                    KernelDB = new KernelDatabase(Instance, "resources/kernel", DataVectorTypes.Uchar1);
                }
                catch (Exception exception)
                {
                    crash = true;
                    if (exception is CLBuildException buildException
                    ) //Display the Compile errors in a more convenient dialog
                    {
                        BuildExceptionViewer bev = new BuildExceptionViewer(buildException);
                        bev.ShowDialog();
                    }
                    else
                    {
                        throw exception; //Let the Exception Viewer Catch that
                    }
                }
            } while (crash);


            InstructionSet = FLInstructionSet.CreateWithBuiltInTypes(KernelDB);
            BufferCreator = BufferCreator.CreateWithBuiltInTypes();
            Parser = new FLParser(InstructionSet, BufferCreator, new WorkItemRunnerSettings(true, 2));
            CheckBuilder = new FLProgramCheckBuilder(InstructionSet, BufferCreator);
        }
    }
}