﻿using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.Constraints.TwoEntity.Motors
{
    /// <summary>
    /// Defines the behavior of a servo that works on three degrees of freedom.
    /// Used when the MotorSettings' motorType is set to servomechanism.
    /// </summary>
    public class ServoSettings3D : ServoSettings
    {
        internal Vector3 goal;

        internal ServoSettings3D(MotorSettings motorSettings)
            : base(motorSettings)
        {
        }

        /// <summary>
        /// Gets or sets the goal position of the servo.
        /// </summary>
        public Vector3 Goal
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