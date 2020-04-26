using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.Character
{
    /// <summary>
    /// Description of a support for a character controller.
    /// </summary>
    public struct SupportData
    {
        /// <summary>
        /// Position of the support.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Normal of the support.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Depth of the supporting location.
        /// Can be negative in the case of raycast supports.
        /// </summary>
        public float Depth;

        /// <summary>
        /// The object which the character is standing on.
        /// </summary>
        public Collidable SupportObject;
    }
}