namespace Byt3.Engine.Physics.BEPUphysics.Constraints.TwoEntity.Motors
{
    /// <summary>
    /// Defines the behavior of a velocity motor that works on one degree of freedom.
    /// Used when the MotorSettings' motorType is set to velocityMotor.
    /// </summary>
    public class VelocityMotorSettings1D : VelocityMotorSettings
    {
        internal float goalVelocity;

        internal VelocityMotorSettings1D(MotorSettings motorSettings)
            : base(motorSettings)
        {
        }

        /// <summary>
        /// Gets or sets the goal velocity of the motor.
        /// </summary>
        public float GoalVelocity
        {
            get => goalVelocity;
            set
            {
                if (value != goalVelocity)
                {
                    goalVelocity = value;
                    motorSettings.motor.ActivateInvolvedEntities();
                }
            }
        }
    }
}