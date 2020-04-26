using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.Character
{
    /// <summary>
    /// Result of a ray cast which acts as a support for the character controller.
    /// </summary>
    public struct SupportRayData
    {
        /// <summary>
        /// Ray hit information of the support.
        /// </summary>
        public RayHit HitData;

        /// <summary>
        /// Object hit by the ray.
        /// </summary>
        public Collidable HitObject;

        /// <summary>
        /// Whether or not the support has traction.
        /// </summary>
        public bool HasTraction;
    }
}