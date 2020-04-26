using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.Events
{
    internal struct EventStorageInitialCollisionDetected
    {
        internal CollidablePairHandler pair;
        internal Collidable other;

        internal EventStorageInitialCollisionDetected(Collidable other, CollidablePairHandler pair)
        {
            this.pair = pair;
            this.other = other;
        }
    }
}