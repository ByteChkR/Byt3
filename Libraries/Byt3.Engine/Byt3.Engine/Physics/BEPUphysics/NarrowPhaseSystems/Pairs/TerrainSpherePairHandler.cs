using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a terrain-sphere collision pair.
    ///</summary>
    public sealed class TerrainSpherePairHandler : TerrainPairHandler
    {
        private readonly TerrainSphereContactManifold contactManifold = new TerrainSphereContactManifold();

        protected override TerrainContactManifold TerrainManifold => contactManifold;
    }
}