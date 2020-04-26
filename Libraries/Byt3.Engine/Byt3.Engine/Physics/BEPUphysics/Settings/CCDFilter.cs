using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;

namespace Byt3.Engine.Physics.BEPUphysics.Settings
{
    /// <summary>
    /// Delegate which determines if a given pair should be allowed to run continuous collision detection.
    /// This is only called for entities which are continuous and colliding with other objects.
    /// </summary>
    public delegate bool CCDFilter(CollidablePairHandler pair);
}