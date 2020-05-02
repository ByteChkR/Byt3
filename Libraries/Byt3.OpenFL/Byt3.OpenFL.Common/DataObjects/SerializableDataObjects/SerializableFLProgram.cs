using System;
using System.Collections.Generic;
using System.Text;
using Byt3.OpenFL.Common.ProgramChecks;

namespace Byt3.OpenFL.Common.DataObjects.SerializableDataObjects
{
    public class SerializableFLProgram : FLPipelineResult
    {
        public string FileName { get; }
        public List<SerializableExternalFLFunction> ExternalFunctions { get; }
        public List<SerializableFLFunction> Functions { get; }
        public List<SerializableFLBuffer> DefinedBuffers { get; }

        public SerializableFLProgram(string filename, List<SerializableExternalFLFunction> externalFunctions,
            List<SerializableFLFunction> functions, List<SerializableFLBuffer> definedBuffers)
        {
            FileName = filename;
            ExternalFunctions = externalFunctions;
            Functions = functions;
            DefinedBuffers = definedBuffers;
        }


        public Dictionary<SerializableNamedObject, Tuple<int, int>> ToString(out string s)
        {
            Dictionary<SerializableNamedObject, Tuple<int, int>> ret =
                new Dictionary<SerializableNamedObject, Tuple<int, int>>();
            StringBuilder sb = new StringBuilder();

            int lineCount = 0;

            foreach (SerializableFLBuffer serializableFlBuffer in DefinedBuffers)
            {
                string f = serializableFlBuffer.ToString();
                ret.Add(serializableFlBuffer, new Tuple<int, int>(sb.Length, sb.Length+f.Length));
                sb.AppendLine(f);
                lineCount++;
            }

            foreach (SerializableExternalFLFunction serializableExternalFlFunction in ExternalFunctions)
            {
                string f = serializableExternalFlFunction.ToString();
                ret.Add(serializableExternalFlFunction, new Tuple<int, int>(sb.Length, sb.Length + f.Length));
                sb.AppendLine(f);
                lineCount++;
            }

            foreach (SerializableFLFunction serializableFlFunction in Functions)
            {
                string f = serializableFlFunction.ToString();
                ret.Add(serializableFlFunction, new Tuple<int, int>(sb.Length, sb.Length + f.Length));
                sb.AppendLine(f);
                lineCount++;
            }

            s= sb.ToString();
            return ret;
        }

        public override string ToString()
        {
            ToString(out string r);
            return r;
        }
    }
}