using System;
using System.Reflection;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;

namespace Byt3.OpenFL.Common
{
    public class FLRunner
    {
        public static Version CommonVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public FLInstructionSet InstructionSet { get; }

        public FLRunner(FLInstructionSet instructionSet)
        {
            InstructionSet = instructionSet;
        }

        public FLRunner() : this(new FLInstructionSet())
        {
        }

        public FLProgram Initialize(SerializableFLProgram file)
        {
            return file.Initialize(InstructionSet);
        }
    }
}