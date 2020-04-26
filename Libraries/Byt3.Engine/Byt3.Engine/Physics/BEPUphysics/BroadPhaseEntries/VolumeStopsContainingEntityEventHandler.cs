using Byt3.Engine.Physics.BEPUphysics.Entities;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries
{
    /// <summary>
    /// Handles any special logic to perform when an entry stops being contained by a detector volume.
    /// Runs within an update loop for updateables; modifying the updateable listing during the event is disallowed.
    /// </summary>
    /// <param name="volume">DetectorVolume no longer containing the entry.</param>
    /// <param name="entity">Entity no longer contained by the volume.</param>
    public delegate void VolumeStopsContainingEntityEventHandler(DetectorVolume volume, Entity entity);
}