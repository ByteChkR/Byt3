﻿using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseSystems
{
    ///<summary>
    /// Requires that a class own a BroadPhaseEntry.
    ///</summary>
    public interface IBroadPhaseEntryOwner
    {
        ///<summary>
        /// Gets the broad phase entry associated with this object.
        ///</summary>
        BroadPhaseEntry Entry { get; }
    }
}