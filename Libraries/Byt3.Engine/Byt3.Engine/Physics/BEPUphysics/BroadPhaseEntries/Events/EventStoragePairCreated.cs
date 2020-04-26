using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.Events
{
    //TODO: Contravariance isn't supported on all platforms...

    //Storage for deferred event dispatching
    internal struct EventStoragePairCreated
    {
        internal NarrowPhasePair pair;
        internal BroadPhaseEntry other;

        internal EventStoragePairCreated(BroadPhaseEntry other, NarrowPhasePair pair)
        {
            this.other = other;
            this.pair = pair;
        }
    }
}