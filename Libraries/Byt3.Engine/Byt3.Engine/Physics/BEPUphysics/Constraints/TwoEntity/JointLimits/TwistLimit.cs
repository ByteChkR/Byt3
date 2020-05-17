﻿using System;
using Byt3.Engine.Physics.BEPUphysics.Entities;
using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.Constraints.TwoEntity.JointLimits
{
    /// <summary>
    /// Prevents the connected entities from twisting relative to each other beyond given limits.
    /// </summary>
    public class TwistLimit : JointLimit, I1DImpulseConstraintWithError, I1DJacobianConstraint
    {
        private float biasVelocity;
        private Vector3 jacobianA, jacobianB;

        /// <summary>
        /// Naximum angle that entities can twist.
        /// </summary>
        protected float maximumAngle;

        /// <summary>
        /// Minimum angle that entities can twist.
        /// </summary>
        protected float minimumAngle;

        private float velocityToImpulse;

        /// <summary>
        /// Constructs a new constraint which prevents the connected entities from twisting relative to each other beyond given limits.
        /// To finish the initialization, specify the connections (ConnectionA and ConnectionB) 
        /// as well as the BasisA, BasisB and the MinimumAngle and MaximumAngle.
        /// This constructor sets the constraint's IsActive property to false by default.
        /// </summary>
        public TwistLimit()
        {
            IsActive = false;
        }

        /// <summary>
        /// Constructs a new constraint which prevents the connected entities from twisting relative to each other beyond given limits.
        /// </summary>
        /// <param name="connectionA">First connection of the pair.</param>
        /// <param name="connectionB">Second connection of the pair.</param>
        /// <param name="axisA">Twist axis attached to the first connected entity.</param>
        /// <param name="axisB">Twist axis attached to the second connected entity.</param>
        /// <param name="minimumAngle">Minimum twist angle allowed.</param>
        /// <param name="maximumAngle">Maximum twist angle allowed.</param>
        public TwistLimit(Entity connectionA, Entity connectionB, Vector3 axisA, Vector3 axisB, float minimumAngle,
            float maximumAngle)
        {
            ConnectionA = connectionA;
            ConnectionB = connectionB;
            SetupJointTransforms(axisA, axisB);
            MinimumAngle = minimumAngle;
            MaximumAngle = maximumAngle;
        }

        /// <summary>
        /// Gets the basis attached to entity A.
        /// The primary axis represents the twist axis attached to entity A.
        /// The x axis and y axis represent a plane against which entity B's attached x axis is projected to determine the twist angle.
        /// </summary>
        public JointBasis3D BasisA { get; } = new JointBasis3D();


        /// <summary>
        /// Gets the basis attached to entity B.
        /// The primary axis represents the twist axis attached to entity A.
        /// The x axis is projected onto the plane defined by localTransformA's x and y axes
        /// to get the twist angle.
        /// </summary>
        public JointBasis2D BasisB { get; } = new JointBasis2D();

        /// <summary>
        /// Gets or sets the maximum angle that entities can twist.
        /// </summary>
        public float MaximumAngle
        {
            get => maximumAngle;
            set
            {
                maximumAngle = value % MathHelper.TwoPi;
                if (minimumAngle > MathHelper.Pi)
                {
                    minimumAngle -= MathHelper.TwoPi;
                }

                if (minimumAngle <= -MathHelper.Pi)
                {
                    minimumAngle += MathHelper.TwoPi;
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum angle that entities can twist.
        /// </summary>
        public float MinimumAngle
        {
            get => minimumAngle;
            set
            {
                minimumAngle = value % MathHelper.TwoPi;
                if (minimumAngle > MathHelper.Pi)
                {
                    minimumAngle -= MathHelper.TwoPi;
                }

                if (minimumAngle <= -MathHelper.Pi)
                {
                    minimumAngle += MathHelper.TwoPi;
                }
            }
        }

        /// <summary>
        /// Sets up the joint transforms by automatically creating perpendicular vectors to complete the bases.
        /// </summary>
        /// <param name="worldTwistAxisA">Twist axis in world space to attach to entity A.</param>
        /// <param name="worldTwistAxisB">Twist axis in world space to attach to entity B.</param>
        public void SetupJointTransforms(Vector3 worldTwistAxisA, Vector3 worldTwistAxisB)
        {
            worldTwistAxisA.Normalize();
            worldTwistAxisB.Normalize();

            Vector3 worldXAxis;
            Vector3.Cross(ref worldTwistAxisA, ref Toolbox.UpVector, out worldXAxis);
            float length = worldXAxis.LengthSquared();
            if (length < Toolbox.Epsilon)
            {
                Vector3.Cross(ref worldTwistAxisA, ref Toolbox.RightVector, out worldXAxis);
            }

            worldXAxis.Normalize();

            //Complete A's basis.
            Vector3 worldYAxis;
            Vector3.Cross(ref worldTwistAxisA, ref worldXAxis, out worldYAxis);

            BasisA.rotationMatrix = connectionA.orientationMatrix;
            BasisA.SetWorldAxes(worldTwistAxisA, worldXAxis, worldYAxis);

            //Rotate the axis to B since it could be arbitrarily rotated.
            Quaternion rotation;
            Quaternion.GetQuaternionBetweenNormalizedVectors(ref worldTwistAxisA, ref worldTwistAxisB, out rotation);
            Quaternion.Transform(ref worldXAxis, ref rotation, out worldXAxis);

            BasisB.rotationMatrix = connectionB.orientationMatrix;
            BasisB.SetWorldAxes(worldTwistAxisB, worldXAxis);
        }

        /// <summary>
        /// Solves for velocity.
        /// </summary>
        public override float SolveIteration()
        {
            float velocityA, velocityB;
            //Find the velocity contribution from each connection
            Vector3.Dot(ref connectionA.angularVelocity, ref jacobianA, out velocityA);
            Vector3.Dot(ref connectionB.angularVelocity, ref jacobianB, out velocityB);
            //Add in the constraint space bias velocity
            float lambda = -(velocityA + velocityB) + biasVelocity - softness * TotalImpulse;

            //Transform to an impulse
            lambda *= velocityToImpulse;

            //Clamp accumulated impulse (can't go negative)
            float previousAccumulatedImpulse = TotalImpulse;
            TotalImpulse = MathHelper.Max(TotalImpulse + lambda, 0);
            lambda = TotalImpulse - previousAccumulatedImpulse;

            //Apply the impulse
            Vector3 impulse;
            if (connectionA.isDynamic)
            {
                Vector3.Multiply(ref jacobianA, lambda, out impulse);
                connectionA.ApplyAngularImpulse(ref impulse);
            }

            if (connectionB.isDynamic)
            {
                Vector3.Multiply(ref jacobianB, lambda, out impulse);
                connectionB.ApplyAngularImpulse(ref impulse);
            }

            return Math.Abs(lambda);
        }

        /// <summary>
        /// Do any necessary computations to prepare the constraint for this frame.
        /// </summary>
        /// <param name="dt">Simulation step length.</param>
        public override void Update(float dt)
        {
            BasisA.rotationMatrix = connectionA.orientationMatrix;
            BasisB.rotationMatrix = connectionB.orientationMatrix;
            BasisA.ComputeWorldSpaceAxes();
            BasisB.ComputeWorldSpaceAxes();

            Quaternion rotation;
            Quaternion.GetQuaternionBetweenNormalizedVectors(ref BasisB.primaryAxis, ref BasisA.primaryAxis,
                out rotation);

            //Transform b's 'Y' axis so that it is perpendicular with a's 'X' axis for measurement.
            Vector3 twistMeasureAxis;
            Quaternion.Transform(ref BasisB.xAxis, ref rotation, out twistMeasureAxis);

            //By dotting the measurement vector with a 2d plane's axes, we can get a local X and Y value.
            float y, x;
            Vector3.Dot(ref twistMeasureAxis, ref BasisA.yAxis, out y);
            Vector3.Dot(ref twistMeasureAxis, ref BasisA.xAxis, out x);
            float angle = (float) Math.Atan2(y, x);

            float distanceFromCurrent, distanceFromMaximum;
            if (IsAngleValid(angle, out distanceFromCurrent, out distanceFromMaximum))
            {
                isActiveInSolver = false;
                TotalImpulse = 0;
                Error = 0;
                isLimitActive = false;
                return;
            }

            isLimitActive = true;

            //Compute the jacobian.
            if (Error > 0)
            {
                Vector3.Add(ref BasisA.primaryAxis, ref BasisB.primaryAxis, out jacobianB);
                if (jacobianB.LengthSquared() < Toolbox.Epsilon)
                {
                    //A nasty singularity can show up if the axes are aligned perfectly.
                    //In a 'real' situation, this is impossible, so just ignore it.
                    isActiveInSolver = false;
                    return;
                }

                jacobianB.Normalize();
                jacobianA.X = -jacobianB.X;
                jacobianA.Y = -jacobianB.Y;
                jacobianA.Z = -jacobianB.Z;
            }
            else
            {
                //Reverse the jacobian so that the solver loop is easier.
                Vector3.Add(ref BasisA.primaryAxis, ref BasisB.primaryAxis, out jacobianA);
                if (jacobianA.LengthSquared() < Toolbox.Epsilon)
                {
                    //A nasty singularity can show up if the axes are aligned perfectly.
                    //In a 'real' situation, this is impossible, so just ignore it.
                    isActiveInSolver = false;
                    return;
                }

                jacobianA.Normalize();
                jacobianB.X = -jacobianA.X;
                jacobianB.Y = -jacobianA.Y;
                jacobianB.Z = -jacobianA.Z;
            }

            //****** VELOCITY BIAS ******//
            //Compute the correction velocity.
            Error = ComputeAngleError(distanceFromCurrent, distanceFromMaximum);
            float errorReduction;
            springSettings.ComputeErrorReductionAndSoftness(dt, 1 / dt, out errorReduction, out softness);


            biasVelocity = MathHelper.Min(MathHelper.Max(0, Math.Abs(Error) - margin) * errorReduction,
                maxCorrectiveVelocity);
            if (bounciness > 0)
            {
                float relativeVelocity;
                float dot;
                //Find the velocity contribution from each connection
                Vector3.Dot(ref connectionA.angularVelocity, ref jacobianA, out relativeVelocity);
                Vector3.Dot(ref connectionB.angularVelocity, ref jacobianB, out dot);
                relativeVelocity += dot;
                biasVelocity = MathHelper.Max(biasVelocity, ComputeBounceVelocity(-relativeVelocity));
            }

            //The nice thing about this approach is that the jacobian entry doesn't flip.
            //Instead, the error can be negative due to the use of Atan2.
            //This is important for limits which have a unique high and low value.


            //****** EFFECTIVE MASS MATRIX ******//
            //Connection A's contribution to the mass matrix
            float entryA;
            Vector3 transformedAxis;
            if (connectionA.isDynamic)
            {
                Matrix3x3.Transform(ref jacobianA, ref connectionA.inertiaTensorInverse, out transformedAxis);
                Vector3.Dot(ref transformedAxis, ref jacobianA, out entryA);
            }
            else
            {
                entryA = 0;
            }

            //Connection B's contribution to the mass matrix
            float entryB;
            if (connectionB.isDynamic)
            {
                Matrix3x3.Transform(ref jacobianB, ref connectionB.inertiaTensorInverse, out transformedAxis);
                Vector3.Dot(ref transformedAxis, ref jacobianB, out entryB);
            }
            else
            {
                entryB = 0;
            }

            //Compute the inverse mass matrix
            velocityToImpulse = 1 / (softness + entryA + entryB);
        }

        /// <summary>
        /// Performs any pre-solve iteration work that needs exclusive
        /// access to the members of the solver updateable.
        /// Usually, this is used for applying warmstarting impulses.
        /// </summary>
        public override void ExclusiveUpdate()
        {
            //****** WARM STARTING ******//
            //Apply accumulated impulse
            Vector3 impulse;
            if (connectionA.isDynamic)
            {
                Vector3.Multiply(ref jacobianA, TotalImpulse, out impulse);
                connectionA.ApplyAngularImpulse(ref impulse);
            }

            if (connectionB.isDynamic)
            {
                Vector3.Multiply(ref jacobianB, TotalImpulse, out impulse);
                connectionB.ApplyAngularImpulse(ref impulse);
            }
        }

        private static float ComputeAngleError(float distanceFromCurrent, float distanceFromMaximum)
        {
            float errorFromMin = MathHelper.TwoPi - distanceFromCurrent;
            float errorFromMax = distanceFromCurrent - distanceFromMaximum;
            return errorFromMax > errorFromMin ? errorFromMin : -errorFromMax;
        }

        private float GetDistanceFromMinimum(float angle)
        {
            if (minimumAngle > 0)
            {
                if (angle >= minimumAngle)
                {
                    return angle - minimumAngle;
                }

                if (angle > 0)
                {
                    return MathHelper.TwoPi - minimumAngle + angle;
                }

                return MathHelper.TwoPi - minimumAngle + angle;
            }

            if (angle < minimumAngle)
            {
                return MathHelper.TwoPi - minimumAngle + angle;
            }

            return angle - minimumAngle;
        }

        private bool IsAngleValid(float currentAngle, out float distanceFromCurrent, out float distanceFromMaximum)
        {
            distanceFromCurrent = GetDistanceFromMinimum(currentAngle);
            distanceFromMaximum = GetDistanceFromMinimum(maximumAngle);
            return distanceFromCurrent < distanceFromMaximum;
        }

        #region I1DImpulseConstraintWithError Members

        /// <summary>
        /// Gets the current relative velocity between the connected entities with respect to the constraint.
        /// </summary>
        public float RelativeVelocity
        {
            get
            {
                if (isLimitActive)
                {
                    float velocityA, velocityB;
                    //Find the velocity contribution from each connection
                    Vector3.Dot(ref connectionA.angularVelocity, ref jacobianA, out velocityA);
                    Vector3.Dot(ref connectionB.angularVelocity, ref jacobianB, out velocityB);
                    return velocityA + velocityB;
                }

                return 0;
            }
        }


        /// <summary>
        /// Gets the total impulse applied by this constraint.
        /// </summary>
        public float TotalImpulse { get; private set; }

        /// <summary>
        /// Gets the current constraint error.
        /// </summary>
        public float Error { get; private set; }

        #endregion

        #region I1DJacobianConstraint Members

        /// <summary>
        /// Gets the linear jacobian entry for the first connected entity.
        /// </summary>
        /// <param name="jacobian">Linear jacobian entry for the first connected entity.</param>
        public void GetLinearJacobianA(out Vector3 jacobian)
        {
            jacobian = Toolbox.ZeroVector;
        }

        /// <summary>
        /// Gets the linear jacobian entry for the second connected entity.
        /// </summary>
        /// <param name="jacobian">Linear jacobian entry for the second connected entity.</param>
        public void GetLinearJacobianB(out Vector3 jacobian)
        {
            jacobian = Toolbox.ZeroVector;
        }

        /// <summary>
        /// Gets the angular jacobian entry for the first connected entity.
        /// </summary>
        /// <param name="jacobian">Angular jacobian entry for the first connected entity.</param>
        public void GetAngularJacobianA(out Vector3 jacobian)
        {
            jacobian = jacobianA;
        }

        /// <summary>
        /// Gets the angular jacobian entry for the second connected entity.
        /// </summary>
        /// <param name="jacobian">Angular jacobian entry for the second connected entity.</param>
        public void GetAngularJacobianB(out Vector3 jacobian)
        {
            jacobian = jacobianB;
        }

        /// <summary>
        /// Gets the mass matrix of the constraint.
        /// </summary>
        /// <param name="outputMassMatrix">Constraint's mass matrix.</param>
        public void GetMassMatrix(out float outputMassMatrix)
        {
            outputMassMatrix = velocityToImpulse;
        }

        #endregion
    }
}