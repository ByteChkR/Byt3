namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.Events
{
    internal struct EventStoragePairRemoved
    {
        internal BroadPhaseEntry other;

        internal EventStoragePairRemoved(BroadPhaseEntry other)
        {
            this.other = other;
        }
    }
}