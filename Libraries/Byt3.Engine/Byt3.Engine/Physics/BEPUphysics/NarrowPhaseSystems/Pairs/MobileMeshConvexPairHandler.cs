using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a mobile mesh-convex collision pair.
    ///</summary>
    public class MobileMeshConvexPairHandler : MobileMeshPairHandler
    {
        private readonly MobileMeshConvexContactManifold contactManifold = new MobileMeshConvexContactManifold();

        protected internal override MobileMeshContactManifold MeshManifold => contactManifold;
    }
}