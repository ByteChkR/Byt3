using System.Collections.Generic;

namespace Byt3.OpenFL.FLDataObjects
{
    /// <summary>
    /// Contains information on a single FL Instruction
    /// </summary>
    public struct FLInstructionData
    {
        public object Instruction;
        public FLInstructionType InstructionType;
        public List<FLArgumentData> Arguments;
    }
}