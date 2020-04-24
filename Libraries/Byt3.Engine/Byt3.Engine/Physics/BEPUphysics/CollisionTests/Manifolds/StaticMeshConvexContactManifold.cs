using Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms;
using Byt3.Engine.Physics.BEPUutilities.ResourceManagement;

namespace Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds
{
    ///<summary>
    /// Manages persistent contacts between a static mesh and a convex.
    ///</summary>
    public class StaticMeshConvexContactManifold : StaticMeshContactManifold
    {
        private static LockingResourcePool<TriangleConvexPairTester> testerPool =
            new LockingResourcePool<TriangleConvexPairTester>();

        protected override void GiveBackTester(TrianglePairTester tester)
        {
            testerPool.GiveBack((TriangleConvexPairTester) tester);
        }

        protected override TrianglePairTester GetTester()
        {
            return testerPool.Take();
        }
    }
}