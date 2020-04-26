using Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms;
using Byt3.Engine.Physics.BEPUutilities.ResourceManagement;

namespace Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds
{
    ///<summary>
    /// Manages persistent contacts between a convex and an instanced mesh.
    ///</summary>
    public class MobileMeshSphereContactManifold : MobileMeshContactManifold
    {
        private readonly UnsafeResourcePool<TriangleSpherePairTester> testerPool =
            new UnsafeResourcePool<TriangleSpherePairTester>();

        protected override void GiveBackTester(TrianglePairTester tester)
        {
            testerPool.GiveBack((TriangleSpherePairTester) tester);
        }

        protected override TrianglePairTester GetTester()
        {
            return testerPool.Take();
        }
    }
}