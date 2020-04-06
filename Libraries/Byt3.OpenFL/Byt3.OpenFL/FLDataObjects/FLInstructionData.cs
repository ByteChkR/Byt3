using System.Collections.Generic;

namespace Byt3.OpenFL.FLDataObjects
{
    /// <summary>
    /// Contains information on a single FL Instruction
    /// </summary>
    public struct FlInstructionData
    {
        public object Instruction;
        public FlInstructionType InstructionType;
        public List<FlArgumentData> Arguments;
    }
}