using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.Constraints
{
    /// <summary>
    /// Contains the error reduction factor and softness of a constraint.
    /// These can be used to make the same behaviors as the stiffness and damping constants,
    /// but may provide a more intuitive representation for rigid constraints.
    /// </summary>
    public class SpringAdvancedSettings
    {
        internal float errorReductionFactor = .1f;

        internal float softness = .00001f;

        internal bool useAdvancedSettings;

        /// <summary>
        /// Gets or sets the error reduction parameter of the spring.
        /// </summary>
        public float ErrorReductionFactor
        {
            get => errorReductionFactor;
            set => errorReductionFactor = MathHelper.Clamp(value, 0, 1);
        }

        /// <summary>
        /// Gets or sets the softness of the joint.  Higher values allow the constraint to be violated more.
        /// </summary>
        public float Softness
        {
            get => softness;
            set => softness = MathHelper.Max(0, value);
        }

        /// <summary>
        /// Gets or sets whether or not to use the advanced settings.
        /// If this is set to true, the errorReductionFactor and softness will be used instead
        /// of the stiffness constant and damping constant.
        /// </summary>
        public bool UseAdvancedSettings
        {
            get => useAdvancedSettings;
            set => useAdvancedSettings = value;
        }
    }
}