using System;
using System.Linq;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.ProgramChecks.Checking
{
    public class KeywordValidator : FLProgramCheck<SerializableFLProgram>
    {
        public override FLProgramCheckType CheckType => FLProgramCheckType.InputValidation;


        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;


            foreach (SerializableFLBuffer serializableFlBuffer in input.DefinedBuffers)
            {
                if (serializableFlBuffer.Name != FLKeywords.InputBufferKey && IsProtected(serializableFlBuffer.Name))
                {
                    throw new InvalidOperationException("The script defines a buffer with name: " +
                                                        serializableFlBuffer.Name +
                                                        " but this name is a protected keyword.");
                }
            }

            foreach (SerializableExternalFLFunction serializableExternalFlFunction in input.ExternalFunctions)
            {
                if (IsProtected(serializableExternalFlFunction.Name))
                {
                    throw new InvalidOperationException("The script defines a script with name: " +
                                                        serializableExternalFlFunction.Name +
                                                        " but this name is a protected keyword.");
                }
            }

            foreach (SerializableFLFunction serializableFlFunction in input.Functions)
            {
                if (serializableFlFunction.Name != FLKeywords.EntryFunctionKey &&
                    IsProtected(serializableFlFunction.Name))
                {
                    throw new InvalidOperationException("The script defines a function with name: " +
                                                        serializableFlFunction.Name +
                                                        " but this name is a protected keyword.");
                }
            }


            return input;
        }

        private bool IsProtected(string keyword)
        {
            return FLKeywords.ProtectedKeywords.Contains(keyword);
        }
    }
}