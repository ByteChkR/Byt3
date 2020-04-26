using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a static mesh-sphere collision pair.
    ///</summary>
    public class StaticMeshSpherePairHandler : StaticMeshPairHandler
    {
        private readonly StaticMeshSphereContactManifold contactManifold = new StaticMeshSphereContactManifold();

        protected override StaticMeshContactManifold MeshManifold => contactManifold;
    }
}