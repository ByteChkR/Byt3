using Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms;
using Byt3.Engine.Physics.BEPUutilities.ResourceManagement;

namespace Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds
{
    public class TerrainConvexContactManifold : TerrainContactManifold
    {
        private static readonly LockingResourcePool<TriangleConvexPairTester> testerPool =
            new LockingResourcePool<TriangleConvexPairTester>();

        protected override TrianglePairTester GetTester()
        {
            return testerPool.Take();
        }

        protected override void GiveBackTester(TrianglePairTester tester)
        {
            testerPool.GiveBack((TriangleConvexPairTester) tester);
        }
    }
}