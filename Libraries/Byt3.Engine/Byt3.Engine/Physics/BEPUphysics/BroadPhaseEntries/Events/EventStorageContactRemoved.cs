using Byt3.Engine.Physics.BEPUphysics.CollisionTests;
using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.Events
{
    internal struct EventStorageContactRemoved
    {
        internal CollidablePairHandler pair;
        internal ContactData contactData;
        internal Collidable other;

        internal EventStorageContactRemoved(Collidable other, CollidablePairHandler pair, ref ContactData contactData)
        {
            this.other = other;
            this.pair = pair;
            this.contactData = contactData;
        }
    }
}