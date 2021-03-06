﻿using Byt3.Engine.Physics.BEPUphysics.Constraints;
using Byt3.Engine.Physics.BEPUphysics.Entities;
using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.Vehicle
{
    /// <summary>
    /// Handles a wheel's driving force for a vehicle.
    /// </summary>
    public class WheelDrivingMotor : ISolverSettings
    {
        internal float accumulatedImpulse;

        //float linearBX, linearBY, linearBZ;
        internal float angularAX, angularAY, angularAZ;
        internal float angularBX, angularBY, angularBZ;
        internal Vector3 forceAxis;
        private float gripFriction;
        internal bool isActive = true;
        internal float linearAX, linearAY, linearAZ;
        private float maxMotorForceDt;
        internal int numIterationsAtZeroImpulse;
        internal SolverSettings solverSettings = new SolverSettings();
        private bool supportIsDynamic;
        private Entity vehicleEntity, supportEntity;

        //Inverse effective mass matrix
        internal float velocityToImpulse;

        /// <summary>
        /// Constructs a new wheel motor.
        /// </summary>
        /// <param name="gripFriction">Friction coefficient of the wheel.  Blended with the ground's friction coefficient and normal force to determine a maximum force.</param>
        /// <param name="maximumForwardForce">Maximum force that the wheel motor can apply when driving forward (a target speed greater than zero).</param>
        /// <param name="maximumBackwardForce">Maximum force that the wheel motor can apply when driving backward (a target speed less than zero).</param>
        public WheelDrivingMotor(float gripFriction, float maximumForwardForce, float maximumBackwardForce)
        {
            GripFriction = gripFriction;
            MaximumForwardForce = maximumForwardForce;
            MaximumBackwardForce = maximumBackwardForce;
        }

        internal WheelDrivingMotor(Wheel wheel)
        {
            Wheel = wheel;
        }

        /// <summary>
        /// Gets the coefficient of grip friction between the wheel and support.
        /// This coefficient is the blended result of the supporting entity's friction and the wheel's friction.
        /// </summary>
        public float BlendedCoefficient { get; private set; }

        /// <summary>
        /// Gets the axis along which the driving forces are applied.
        /// </summary>
        public Vector3 ForceAxis => forceAxis;

        /// <summary>
        /// Gets or sets the coefficient of forward-backward gripping friction for this wheel.
        /// This coefficient and the supporting entity's coefficient of friction will be 
        /// taken into account to determine the used coefficient at any given time.
        /// </summary>
        public float GripFriction
        {
            get => gripFriction;
            set => gripFriction = MathHelper.Max(value, 0);
        }

        /// <summary>
        /// Gets or sets the function used to blend the supporting entity's friction and the wheel's friction.
        /// </summary>
        public WheelFrictionBlender GripFrictionBlender { get; set; } = DefaultGripFrictionBlender;

        /// <summary>
        /// Gets or sets the maximum force that the wheel motor can apply when driving backward (a target speed less than zero).
        /// </summary>
        public float MaximumBackwardForce { get; set; } = float.MaxValue;

        /// <summary>
        /// Gets or sets the maximum force that the wheel motor can apply when driving forward (a target speed greater than zero).
        /// </summary>
        public float MaximumForwardForce { get; set; } = float.MaxValue;

        /// <summary>
        /// Gets or sets the target speed of this wheel.
        /// </summary>
        public float TargetSpeed { get; set; }

        /// <summary>
        /// Gets the force this wheel's motor is applying.
        /// </summary>
        public float TotalImpulse => accumulatedImpulse;

        /// <summary>
        /// Gets the wheel that this motor applies to.
        /// </summary>
        public Wheel Wheel { get; internal set; }

        /// <summary>
        /// Gets the relative velocity between the ground and wheel.
        /// </summary>
        /// <returns>Relative velocity between the ground and wheel.</returns>
        public float RelativeVelocity
        {
            get
            {
                float velocity = 0;
                if (vehicleEntity != null)
                {
                    velocity += vehicleEntity.linearVelocity.X * linearAX + vehicleEntity.linearVelocity.Y * linearAY +
                                vehicleEntity.linearVelocity.Z * linearAZ +
                                vehicleEntity.angularVelocity.X * angularAX +
                                vehicleEntity.angularVelocity.Y * angularAY +
                                vehicleEntity.angularVelocity.Z * angularAZ;
                }

                if (supportEntity != null)
                {
                    velocity += -supportEntity.linearVelocity.X * linearAX - supportEntity.linearVelocity.Y * linearAY -
                                supportEntity.linearVelocity.Z * linearAZ +
                                supportEntity.angularVelocity.X * angularBX +
                                supportEntity.angularVelocity.Y * angularBY +
                                supportEntity.angularVelocity.Z * angularBZ;
                }

                return velocity;
            }
        }

        #region ISolverSettings Members

        /// <summary>
        /// Gets the solver settings used by this wheel constraint.
        /// </summary>
        public SolverSettings SolverSettings => solverSettings;

        #endregion

        internal float ApplyImpulse()
        {
            //Compute relative velocity
            float lambda = (RelativeVelocity
                            - TargetSpeed) //Add in the extra goal speed
                           * velocityToImpulse; //convert to impulse


            //Clamp accumulated impulse
            float previousAccumulatedImpulse = accumulatedImpulse;
            accumulatedImpulse += lambda;
            //Don't brake, and take into account the motor's maximum force.
            if (TargetSpeed > 0)
            {
                accumulatedImpulse =
                    MathHelper.Clamp(accumulatedImpulse, 0,
                        maxMotorForceDt);
            }
            else if (TargetSpeed < 0)
            {
                accumulatedImpulse =
                    MathHelper.Clamp(accumulatedImpulse, maxMotorForceDt,
                        0);
            }
            else
            {
                accumulatedImpulse = 0;
            }

            //Friction
            float maxForce = BlendedCoefficient * Wheel.suspension.accumulatedImpulse;
            accumulatedImpulse = MathHelper.Clamp(accumulatedImpulse, maxForce, -maxForce);
            lambda = accumulatedImpulse - previousAccumulatedImpulse;


            //Apply the impulse
            Vector3 linear = new Vector3();
            Vector3 angular = new Vector3();
            linear.X = lambda * linearAX;
            linear.Y = lambda * linearAY;
            linear.Z = lambda * linearAZ;
            if (vehicleEntity.isDynamic)
            {
                angular.X = lambda * angularAX;
                angular.Y = lambda * angularAY;
                angular.Z = lambda * angularAZ;
                vehicleEntity.ApplyLinearImpulse(ref linear);
                vehicleEntity.ApplyAngularImpulse(ref angular);
            }

            if (supportIsDynamic)
            {
                linear.X = -linear.X;
                linear.Y = -linear.Y;
                linear.Z = -linear.Z;
                angular.X = lambda * angularBX;
                angular.Y = lambda * angularBY;
                angular.Z = lambda * angularBZ;
                supportEntity.ApplyLinearImpulse(ref linear);
                supportEntity.ApplyAngularImpulse(ref angular);
            }

            return lambda;
        }

        internal void PreStep(float dt)
        {
            vehicleEntity = Wheel.Vehicle.Body;
            supportEntity = Wheel.SupportingEntity;
            supportIsDynamic = supportEntity != null && supportEntity.isDynamic;

            Vector3.Cross(ref Wheel.normal, ref Wheel.slidingFriction.slidingFrictionAxis, out forceAxis);
            forceAxis.Normalize();
            //Do not need to check for normalize safety because normal and sliding friction axis must be perpendicular.

            linearAX = forceAxis.X;
            linearAY = forceAxis.Y;
            linearAZ = forceAxis.Z;

            //angular A = Ra x N
            angularAX = Wheel.ra.Y * linearAZ - Wheel.ra.Z * linearAY;
            angularAY = Wheel.ra.Z * linearAX - Wheel.ra.X * linearAZ;
            angularAZ = Wheel.ra.X * linearAY - Wheel.ra.Y * linearAX;

            //Angular B = N x Rb
            angularBX = linearAY * Wheel.rb.Z - linearAZ * Wheel.rb.Y;
            angularBY = linearAZ * Wheel.rb.X - linearAX * Wheel.rb.Z;
            angularBZ = linearAX * Wheel.rb.Y - linearAY * Wheel.rb.X;

            //Compute inverse effective mass matrix
            float entryA, entryB;

            //these are the transformed coordinates
            float tX, tY, tZ;
            if (vehicleEntity.isDynamic)
            {
                tX = angularAX * vehicleEntity.inertiaTensorInverse.M11 +
                     angularAY * vehicleEntity.inertiaTensorInverse.M21 +
                     angularAZ * vehicleEntity.inertiaTensorInverse.M31;
                tY = angularAX * vehicleEntity.inertiaTensorInverse.M12 +
                     angularAY * vehicleEntity.inertiaTensorInverse.M22 +
                     angularAZ * vehicleEntity.inertiaTensorInverse.M32;
                tZ = angularAX * vehicleEntity.inertiaTensorInverse.M13 +
                     angularAY * vehicleEntity.inertiaTensorInverse.M23 +
                     angularAZ * vehicleEntity.inertiaTensorInverse.M33;
                entryA = tX * angularAX + tY * angularAY + tZ * angularAZ + vehicleEntity.inverseMass;
            }
            else
            {
                entryA = 0;
            }

            if (supportIsDynamic)
            {
                tX = angularBX * supportEntity.inertiaTensorInverse.M11 +
                     angularBY * supportEntity.inertiaTensorInverse.M21 +
                     angularBZ * supportEntity.inertiaTensorInverse.M31;
                tY = angularBX * supportEntity.inertiaTensorInverse.M12 +
                     angularBY * supportEntity.inertiaTensorInverse.M22 +
                     angularBZ * supportEntity.inertiaTensorInverse.M32;
                tZ = angularBX * supportEntity.inertiaTensorInverse.M13 +
                     angularBY * supportEntity.inertiaTensorInverse.M23 +
                     angularBZ * supportEntity.inertiaTensorInverse.M33;
                entryB = tX * angularBX + tY * angularBY + tZ * angularBZ + supportEntity.inverseMass;
            }
            else
            {
                entryB = 0;
            }

            velocityToImpulse = -1 / (entryA + entryB); //Softness?

            BlendedCoefficient =
                GripFrictionBlender(gripFriction, Wheel.supportMaterial.kineticFriction, true, Wheel);

            //Compute the maximum force
            if (TargetSpeed > 0)
            {
                maxMotorForceDt = MaximumForwardForce * dt;
            }
            else
            {
                maxMotorForceDt = -MaximumBackwardForce * dt;
            }
        }

        internal void ExclusiveUpdate()
        {
            //Warm starting
            Vector3 linear = new Vector3();
            Vector3 angular = new Vector3();
            linear.X = accumulatedImpulse * linearAX;
            linear.Y = accumulatedImpulse * linearAY;
            linear.Z = accumulatedImpulse * linearAZ;
            if (vehicleEntity.isDynamic)
            {
                angular.X = accumulatedImpulse * angularAX;
                angular.Y = accumulatedImpulse * angularAY;
                angular.Z = accumulatedImpulse * angularAZ;
                vehicleEntity.ApplyLinearImpulse(ref linear);
                vehicleEntity.ApplyAngularImpulse(ref angular);
            }

            if (supportIsDynamic)
            {
                linear.X = -linear.X;
                linear.Y = -linear.Y;
                linear.Z = -linear.Z;
                angular.X = accumulatedImpulse * angularBX;
                angular.Y = accumulatedImpulse * angularBY;
                angular.Z = accumulatedImpulse * angularBZ;
                supportEntity.ApplyLinearImpulse(ref linear);
                supportEntity.ApplyAngularImpulse(ref angular);
            }
        }

        #region Static Stuff

        /// <summary>
        /// Default blender used by WheelSlidingFriction constraints.
        /// </summary>
        public static WheelFrictionBlender DefaultGripFrictionBlender;

        static WheelDrivingMotor()
        {
            DefaultGripFrictionBlender = BlendFriction;
        }

        /// <summary>
        /// Function which takes the friction values from a wheel and a supporting material and computes the blended friction.
        /// </summary>
        /// <param name="wheelFriction">Friction coefficient associated with the wheel.</param>
        /// <param name="materialFriction">Friction coefficient associated with the support material.</param>
        /// <param name="usingKineticFriction">True if the friction coefficients passed into the blender are kinetic coefficients, false otherwise.</param>
        /// <param name="wheel">Wheel being blended.</param>
        /// <returns>Blended friction coefficient.</returns>
        public static float BlendFriction(float wheelFriction, float materialFriction, bool usingKineticFriction,
            Wheel wheel)
        {
            return wheelFriction * materialFriction;
        }

        #endregion
    }
}