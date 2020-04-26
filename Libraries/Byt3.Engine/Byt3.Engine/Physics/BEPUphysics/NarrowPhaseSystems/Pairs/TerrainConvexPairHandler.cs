using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a terrain-convex collision pair.
    ///</summary>
    public sealed class TerrainConvexPairHandler : TerrainPairHandler
    {
        private readonly TerrainConvexContactManifold contactManifold = new TerrainConvexContactManifold();

        protected override TerrainContactManifold TerrainManifold => contactManifold;
    }
}