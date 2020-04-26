using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.Constraints
{
    /// <summary>
    /// Implemented by solver updateables which have a two dimensional impulse.
    /// </summary>
    public interface I2DImpulseConstraintWithError : I2DImpulseConstraint
    {
        /// <summary>
        /// Gets the current constraint error.
        /// </summary>
        Vector2 Error { get; }
    }
}