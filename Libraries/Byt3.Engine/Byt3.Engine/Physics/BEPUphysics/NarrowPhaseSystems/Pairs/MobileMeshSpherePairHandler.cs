using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a mobile mesh-sphere collision pair.
    ///</summary>
    public class MobileMeshSpherePairHandler : MobileMeshPairHandler
    {
        private readonly MobileMeshSphereContactManifold contactManifold = new MobileMeshSphereContactManifold();

        protected internal override MobileMeshContactManifold MeshManifold => contactManifold;
    }
}