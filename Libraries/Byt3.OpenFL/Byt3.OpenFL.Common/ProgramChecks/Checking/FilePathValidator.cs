using Byt3.Callbacks;
using Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.FromFile;
using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.Exceptions;

namespace Byt3.OpenFL.Common.ProgramChecks
{
    public class FilePathValidator : FLProgramCheck<SerializableFLProgram>
    {
        public override bool ChangesOutput => false;
        public override object Process(object o)
        {
            SerializableFLProgram input = (SerializableFLProgram) o;
            for (int i = 0; i < input.DefinedBuffers.Count; i++)
            {
                if (input.DefinedBuffers[i] is SerializableFromFileFLBuffer buf)
                {
                    if (!IOManager.FileExists(buf.File))
                    {
                        throw new FLProgramCheckException(
                            $"File: {buf.File} referenced in Defined Buffer: {buf.Name} but the file does not exist.",
                            this);
                    }
                }
            }

            return input;
        }
    }
}