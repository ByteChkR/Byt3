using System.Runtime.InteropServices;

namespace Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms
{
    /// <summary>
    /// Basic storage structure for contact data.
    /// Designed for performance critical code and pointer access.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BoxContactDataCache
    {
        public BoxContactData D1;
        public BoxContactData D2;
        public BoxContactData D3;
        public BoxContactData D4;

        public BoxContactData D5;
        public BoxContactData D6;
        public BoxContactData D7;
        public BoxContactData D8;


        /// <summary>
        /// Number of elements in the cache.
        /// </summary>
        public byte Count;

#if ALLOWUNSAFE
        /// <summary>
        /// Removes an item at the given index.
        /// </summary>
        /// <param name="index">Index to remove.</param>
        public unsafe void RemoveAt(int index)
        {
            BoxContactDataCache copy = this;
            BoxContactData* pointer = &copy.D1;
            pointer[index] = pointer[Count - 1];
            this = copy;
            Count--;
        }
#endif
    }
}