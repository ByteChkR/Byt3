﻿using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.Events
{
    /// <summary>
    /// Handles any special logic when two bodies initally collide and generate a contact point.
    /// </summary>
    /// <param name="sender">Entry sending the event.</param>
    /// <param name="other">Other entry within the pair opposing the monitored entry.</param>
    /// <param name="pair">Pair presiding over the interaction of the two involved bodies.
    /// This reference cannot be safely kept outside of the scope of the handler; pairs can quickly return to the resource pool.</param>
    public delegate void
        InitialCollisionDetectedEventHandler<T>(T sender, Collidable other, CollidablePairHandler pair);
}