using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;
using Byt3.OpenFL.Common.ElementModifiers;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.WFC
{
    public class SerializableWaveFunctionCollapseFLBufferCreator : ASerializableBufferCreator
    {
        public override SerializableFLBuffer CreateBuffer(string name, string[] args, FLBufferModifiers modifiers,
            int arraySize)
        {
            return WFCParameterObject.CreateBuffer(name, args, false, modifiers, arraySize);
        }


        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "wfc";
        }
    }
}