using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a mobile mesh-convex collision pair.
    ///</summary>
    public class MobileMeshTrianglePairHandler : MobileMeshPairHandler
    {
        private readonly MobileMeshTriangleContactManifold contactManifold = new MobileMeshTriangleContactManifold();

        protected internal override MobileMeshContactManifold MeshManifold => contactManifold;
    }
}