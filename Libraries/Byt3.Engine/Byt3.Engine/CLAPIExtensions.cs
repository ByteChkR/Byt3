using Byt3.Engine.DataTypes;
using Byt3.Engine.IO;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;

namespace Byt3.Engine
{
    public static class CLAPIExtensions
    {
        public static void UpdateTexture(this Texture tex, CLAPI instance, MemoryBuffer buffer, int width, int height)
        {
            TextureLoader.Update(tex, CLAPI.ReadBuffer<byte>(instance, buffer, (int) buffer.Size), width, height);
        }
    }
}