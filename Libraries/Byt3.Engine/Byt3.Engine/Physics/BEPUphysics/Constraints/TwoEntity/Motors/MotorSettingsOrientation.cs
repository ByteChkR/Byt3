﻿namespace Byt3.Engine.Physics.BEPUphysics.Constraints.TwoEntity.Motors
{
    /// <summary>
    /// Contains settings for motors which act on two entities' relative orientation.
    /// </summary>
    public class MotorSettingsOrientation : MotorSettings
    {
        internal ServoSettingsOrientation servo;
        internal VelocityMotorSettings3D velocityMotor;

        public MotorSettingsOrientation(SolverUpdateable motor)
            : base(motor)
        {
            servo = new ServoSettingsOrientation(this);
            velocityMotor = new VelocityMotorSettings3D(this);
        }

        /// <summary>
        /// Gets the settings that govern the behavior of this motor if it is a servomechanism.
        /// </summary>
        public ServoSettingsOrientation Servo => servo;

        /// <summary>
        /// Gets the settings that govern the behavior of this motor if it is a velocity motor.
        /// </summary>
        public VelocityMotorSettings3D VelocityMotor => velocityMotor;
    }
}