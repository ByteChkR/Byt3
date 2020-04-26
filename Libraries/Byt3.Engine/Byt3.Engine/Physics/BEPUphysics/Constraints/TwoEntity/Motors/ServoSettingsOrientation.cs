using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.Constraints.TwoEntity.Motors
{
    /// <summary>
    /// Defines the behavior of a servo that works on the relative orientation of two entities.
    /// Used when the MotorSettings' motorType is set to servomechanism.
    /// </summary>
    public class ServoSettingsOrientation : ServoSettings
    {
        internal Quaternion goal;

        internal ServoSettingsOrientation(MotorSettings motorSettings)
            : base(motorSettings)
        {
        }

        /// <summary>
        /// Gets or sets the goal orientation of the servo.
        /// </summary>
        public Quaternion Goal
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