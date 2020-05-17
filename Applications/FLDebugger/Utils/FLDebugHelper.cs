using System.Collections.Generic;
using System.Text;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace FLDebugger.Utils
{
    public static class FLDebugHelper
    {
        public static Dictionary<IParsedObject, int> ToString(this FLProgram prog, out string s)
        {
            Dictionary<IParsedObject, int> ret =
                new Dictionary<IParsedObject, int>();
            StringBuilder sb = new StringBuilder();

            int lineCount = 0;

            foreach (KeyValuePair<string, FLBuffer> definedBuffer in prog.DefinedBuffers)
            {
                string f = definedBuffer.Value.ToString();
                ret.Add(definedBuffer.Value, lineCount);
                sb.AppendLine(f);
                lineCount++;
            }


            foreach (KeyValuePair<string, IFunction> externalFlFunction in prog.DefinedScripts)
            {
                string f = externalFlFunction.Value.ToString();
                ret.Add(externalFlFunction.Value, lineCount);
                sb.AppendLine(f);
                lineCount++;
            }


            foreach (KeyValuePair<string, IFunction> keyValuePair in prog.FlFunctions)
            {
                ret.Add(keyValuePair.Value, lineCount);
                sb.AppendLine(keyValuePair.Key + ":");
                lineCount++;
                FLFunction func = keyValuePair.Value as FLFunction;
                foreach (FLInstruction valueInstruction in func.Instructions)
                {
                    string f = valueInstruction.ToString();
                    f = "\t" + f;
                    ret.Add(valueInstruction, lineCount);
                    sb.AppendLine(f);
                    lineCount++;
                }

                sb.AppendLine();
                lineCount++;
            }

            s = sb.ToString();
            return ret;
        }
    }
}