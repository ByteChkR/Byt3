using Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms;
using Byt3.Engine.Physics.BEPUutilities.ResourceManagement;

namespace Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds
{
    public class TerrainSphereContactManifold : TerrainContactManifold
    {
        private static readonly LockingResourcePool<TriangleSpherePairTester> testerPool =
            new LockingResourcePool<TriangleSpherePairTester>();

        protected override TrianglePairTester GetTester()
        {
            return testerPool.Take();
        }

        protected override void GiveBackTester(TrianglePairTester tester)
        {
            testerPool.GiveBack((TriangleSpherePairTester) tester);
        }
    }
}