using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.Constraints
{
    /// <summary>
    /// Implemented by solver updateables which have a three dimensional impulse.
    /// </summary>
    public interface I3DImpulseConstraintWithError : I3DImpulseConstraint
    {
        /// <summary>
        /// Gets the current constraint error.
        /// </summary>
        Vector3 Error { get; }
    }
}