using System.Collections.Generic;
using System.Text;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;

namespace FLDebugger.Utils
{
    public static class FLDebugHelper
    {
        public static Dictionary<FLParsedObject, int> ToString(this FLProgram prog, out string s)
        {
            Dictionary<FLParsedObject, int> ret =
                new Dictionary<FLParsedObject, int>();
            StringBuilder sb = new StringBuilder();

            int lineCount = 0;

            foreach (KeyValuePair<string, FLBuffer> definedBuffer in prog.DefinedBuffers)
            {
                string f = definedBuffer.Value.ToString();
                ret.Add(definedBuffer.Value, lineCount);
                sb.AppendLine(f);
                lineCount++;
            }


            foreach (KeyValuePair<string, ExternalFlFunction> externalFlFunction in prog.DefinedScripts)
            {
                string f = externalFlFunction.Value.ToString();
                ret.Add(externalFlFunction.Value, lineCount);
                sb.AppendLine(f);
                lineCount++;
            }


            foreach (KeyValuePair<string, FLFunction> keyValuePair in prog.FlFunctions)
            {
                ret.Add(keyValuePair.Value, lineCount);
                sb.AppendLine(keyValuePair.Key + ":");
                lineCount++;
                foreach (FLInstruction valueInstruction in keyValuePair.Value.Instructions)
                {
                    string f = "\t" + valueInstruction;
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