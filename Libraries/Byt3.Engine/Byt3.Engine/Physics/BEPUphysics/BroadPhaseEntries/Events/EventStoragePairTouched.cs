using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.Events
{
    internal struct EventStoragePairTouched
    {
        internal CollidablePairHandler pair;
        internal Collidable other;

        internal EventStoragePairTouched(Collidable other, CollidablePairHandler pair)
        {
            this.other = other;
            this.pair = pair;
        }
    }
}