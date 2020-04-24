using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseSystems
{
    ///<summary>
    /// Requires that a class have a BoundingBox.
    ///</summary>
    public interface IBoundingBoxOwner
    {
        ///<summary>
        /// Gets the bounding box of the object.
        ///</summary>
        BoundingBox BoundingBox { get; }
    }
}