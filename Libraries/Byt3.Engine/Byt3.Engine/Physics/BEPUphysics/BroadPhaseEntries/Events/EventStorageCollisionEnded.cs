using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.Events
{
    internal struct EventStorageCollisionEnded
    {
        internal CollidablePairHandler pair;
        internal Collidable other;

        internal EventStorageCollisionEnded(Collidable other, CollidablePairHandler pair)
        {
            this.other = other;
            this.pair = pair;
        }
    }
}