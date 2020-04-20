using Byt3.OpenFL.Common.DataObjects.SerializableDataObjects;

namespace Byt3.OpenFL.Common.Buffers.BufferCreators.BuiltIn.WFC
{
    public class SerializableWaveFunctionCollapseFLBufferCreator : ASerializableBufferCreator
    {
        public override SerializableFLBuffer CreateBuffer(string name, string[] args)
        {
            return WFCParameterObject.CreateBuffer(name, args, false);
        }



        public override bool IsCorrectBuffer(string bufferKey)
        {
            return bufferKey == "wfc";
        }
    }
}