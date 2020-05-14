using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.WFC
{
    public class SerializableForceWaveFunctionCollapseFLBufferCreator : ASerializableBufferCreator
    {
        public override SerializableFLBuffer CreateBuffer(string name, string[] args, bool isArray, int arraySize)
        {
            return WFCParameterObject.CreateBuffer(name, args, true, isArray, arraySize);
        }


        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "wfcf";
        }
    }
}