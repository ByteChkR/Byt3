namespace Byt3.Engine.Physics.BEPUphysics.Constraints.TwoEntity.Motors
{
    /// <summary>
    /// Contains genereal settings for motors.
    /// </summary>
    public abstract class MotorSettings
    {
        internal float maximumForce = float.MaxValue;
        internal MotorMode mode = MotorMode.VelocityMotor;
        internal SolverUpdateable motor;

        protected MotorSettings(SolverUpdateable motor)
        {
            this.motor = motor;
        }

        /// <summary>
        /// Gets and sets the maximum impulse that the constraint will attempt to apply when satisfying its requirements.
        /// This field can be used to simulate friction in a constraint.
        /// </summary>
        public float MaximumForce
        {
            get
            {
                if (maximumForce > 0)
                {
                    return maximumForce;
                }

                return 0;
            }
            set
            {
                value = value >= 0 ? value : 0;
                if (value != maximumForce)
                {
                    maximumForce = value;
                    motor.ActivateInvolvedEntities();
                }
            }
        }

        /// <summary>
        /// Gets or sets what kind of motor this is.
        /// <para>If velocityMotor is chosen, the motor will try to achieve some velocity using the VelocityMotorSettings.</para> 
        /// <para>If servomechanism is chosen, the motor will try to reach some position using the ServoSettings.</para> 
        /// </summary>
        public MotorMode Mode
        {
            get => mode;
            set
            {
                if (mode != value)
                {
                    mode = value;
                    motor.ActivateInvolvedEntities();
                }
            }
        }
    }
}