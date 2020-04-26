using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.Events
{
    internal struct EventStoragePairUpdated
    {
        internal NarrowPhasePair pair;
        internal BroadPhaseEntry other;

        internal EventStoragePairUpdated(BroadPhaseEntry other, NarrowPhasePair pair)
        {
            this.other = other;
            this.pair = pair;
        }
    }
}