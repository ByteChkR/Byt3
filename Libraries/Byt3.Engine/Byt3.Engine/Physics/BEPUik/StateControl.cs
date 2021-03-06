﻿namespace Byt3.Engine.Physics.BEPUik
{
    /// <summary>
    /// Constrains an individual bone in an attempt to reach some position and orientation goal.
    /// </summary>
    public class StateControl : Control
    {
        public StateControl()
        {
            LinearMotor = new SingleBoneLinearMotor();
            AngularMotor = new SingleBoneAngularMotor();
            LinearMotor.Rigidity = 1;
            AngularMotor.Rigidity = 1;
        }

        /// <summary>
        /// Gets or sets the controlled bone.
        /// </summary>
        public override Bone TargetBone
        {
            get => LinearMotor.TargetBone;
            set
            {
                LinearMotor.TargetBone = value;
                AngularMotor.TargetBone = value;
                if (value != null)
                {
                    AngularMotor.TargetOrientation = value.Orientation;
                }
            }
        }

        /// <summary>
        /// Gets the linear motor used by the control.
        /// </summary>
        public SingleBoneLinearMotor LinearMotor { get; }

        /// <summary>
        /// Gets the angular motor used by the control.
        /// </summary>
        public SingleBoneAngularMotor AngularMotor { get; }

        public override float MaximumForce
        {
            get => LinearMotor.MaximumForce;
            set
            {
                LinearMotor.MaximumForce = value;
                AngularMotor.MaximumForce = value;
            }
        }

        protected internal override void Preupdate(float dt, float updateRate)
        {
            LinearMotor.Preupdate(dt, updateRate);
            AngularMotor.Preupdate(dt, updateRate);
        }


        protected internal override void UpdateJacobiansAndVelocityBias()
        {
            LinearMotor.UpdateJacobiansAndVelocityBias();
            AngularMotor.UpdateJacobiansAndVelocityBias();
        }

        protected internal override void ComputeEffectiveMass()
        {
            LinearMotor.ComputeEffectiveMass();
            AngularMotor.ComputeEffectiveMass();
        }

        protected internal override void WarmStart()
        {
            LinearMotor.WarmStart();
            AngularMotor.WarmStart();
        }

        protected internal override void SolveVelocityIteration()
        {
            LinearMotor.SolveVelocityIteration();
            AngularMotor.SolveVelocityIteration();
        }

        protected internal override void ClearAccumulatedImpulses()
        {
            LinearMotor.ClearAccumulatedImpulses();
            AngularMotor.ClearAccumulatedImpulses();
        }
    }
}