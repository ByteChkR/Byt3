using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a instanced mesh-convex collision pair.
    ///</summary>
    public class InstancedMeshSpherePairHandler : InstancedMeshPairHandler
    {
        private InstancedMeshSphereContactManifold contactManifold = new InstancedMeshSphereContactManifold();

        protected override InstancedMeshContactManifold MeshManifold => contactManifold;
    }
}