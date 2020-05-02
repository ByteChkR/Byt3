using System.Collections.Generic;
using System.Text;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public class SerializableFLFunction : SerializableNamedObject
    {
        public List<SerializableFLInstruction> Instructions { get; }

        public SerializableFLFunction(string name, List<SerializableFLInstruction> instructions) : base(name)
        {
            Instructions = instructions;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Name);
            sb.AppendLine(":");

            for (int i = 0; i < Instructions.Count; i++)
            {
                sb.AppendLine("\t" + Instructions[i]);
            }

            sb.AppendLine();
            return sb.ToString();
        }
    }
}