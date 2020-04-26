using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a static mesh-convex collision pair.
    ///</summary>
    public class StaticMeshConvexPairHandler : StaticMeshPairHandler
    {
        private readonly StaticMeshConvexContactManifold contactManifold = new StaticMeshConvexContactManifold();

        protected override StaticMeshContactManifold MeshManifold => contactManifold;
    }
}