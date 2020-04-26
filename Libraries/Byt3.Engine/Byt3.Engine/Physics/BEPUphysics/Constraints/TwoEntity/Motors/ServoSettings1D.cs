namespace Byt3.Engine.Physics.BEPUphysics.Constraints.TwoEntity.Motors
{
    /// <summary>
    /// Defines the behavior of a servo that works on one degree of freedom.
    /// Used when the MotorSettings' motorType is set to servomechanism.
    /// </summary>
    public class ServoSettings1D : ServoSettings
    {
        internal float goal;

        internal ServoSettings1D(MotorSettings motorSettings)
            : base(motorSettings)
        {
        }

        /// <summary>
        /// Gets or sets the goal position of the servo.
        /// </summary>
        public float Goal
        {
            get => goal;
            set
            {
                if (goal != value)
                {
                    goal = value;
                    motorSettings.motor.ActivateInvolvedEntities();
                }
            }
        }
    }
}