using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Arguments
{
    public abstract class SerializeArrayElementArgument : SerializableFLInstructionArgument
    {
        public override InstructionArgumentCategory ArgumentCategory => InstructionArgumentCategory.Value;




        public static bool TryParse(IEnumerable<string> arrayBufferNames, string line, out SerializeArrayElementArgument arg)
        {
            int openBracket = line.IndexOf('[');
            int closeBracket = line.LastIndexOf(']');
            if (openBracket == -1 && closeBracket == -1)
            {
                arg = null;
                return false;
            }


            if (openBracket == -1 || closeBracket == -1)
            {
                throw new InvalidOperationException("Wrong use of array Accessor: " + line);
            }

            string name;
            string index;

            if (arrayBufferNames.Contains(name = line.Remove(openBracket, closeBracket - openBracket + 1)))
            {
                index = line.Substring(openBracket + 1, closeBracket - openBracket - 1);
                if (int.TryParse(index,
                    out int id))
                {
                    arg = new SerializeArrayElementArgumentValueIndex(name, id);
                    return true;
                }
                if(TryParse(arrayBufferNames, index, out SerializeArrayElementArgument innerArgument))
                {
                    arg = new SerializeArrayElementArgumentEnclosedIndex(name, innerArgument);
                    //Allows setting variables as index.
                    return true;
                }
                else
                {
                    arg = new SerializeArrayElementArgumentVariableIndex(name, index);
                    return true;
                }
            }
            else
            {
                throw new InvalidOperationException("Wrong use of array Accessor: " + line);
            }

        }

    }
}