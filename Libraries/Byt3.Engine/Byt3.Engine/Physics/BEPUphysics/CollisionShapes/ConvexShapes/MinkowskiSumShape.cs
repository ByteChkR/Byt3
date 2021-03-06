﻿using System;
using System.Collections.Generic;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using Byt3.Engine.Physics.BEPUutilities;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;
using Byt3.Engine.Physics.BEPUutilities.ResourceManagement;

namespace Byt3.Engine.Physics.BEPUphysics.CollisionShapes.ConvexShapes
{
    ///<summary>
    /// A shape composed of the pointwise summation of all points in child shapes.
    /// For example, the minkowski sum of two spheres would be a sphere with the radius of both spheres combined.
    /// The minkowski sum of a box and a sphere would be a rounded box.
    ///</summary>
    public class MinkowskiSumShape : ConvexShape
    {
        //Local offset is needed to ensure that the minkowski sum is centered on the local origin.
        private Vector3 localOffset;

        /// <summary>
        /// Constructs a minkowski sum shape.
        /// A minkowski sum can be created from more than two objects; use the other constructors.
        /// The sum will be recentered on its local origin.  The computed center is outputted by the other constructor.
        /// </summary>
        /// <param name="firstShape">First entry in the sum.</param>
        /// <param name="secondShape">Second entry in the sum.</param>
        public MinkowskiSumShape(OrientedConvexShapeEntry firstShape, OrientedConvexShapeEntry secondShape)
        {
            Shapes.Add(firstShape);
            Shapes.Add(secondShape);
            UpdateConvexShapeInfo();
            Shapes.Changed += ShapesChanged;
        }


        /// <summary>
        /// Constructs a minkowski sum shape.
        /// A minkowski sum can be created from more than two objects; use the other constructors.
        /// The sum will be recentered on its local origin.
        /// </summary>
        /// <param name="firstShape">First entry in the sum.</param>
        /// <param name="secondShape">Second entry in the sum.</param>
        /// <param name="center">Center of the minkowski sum computed pre-recentering.</param>
        public MinkowskiSumShape(OrientedConvexShapeEntry firstShape, OrientedConvexShapeEntry secondShape,
            out Vector3 center)
            : this(firstShape, secondShape)
        {
            center = -localOffset;
        }

        /// <summary>
        /// Constructs a minkowski sum shape.
        /// The sum will be recentered on its local origin.  The computed center is outputted by the other constructor.
        /// </summary>
        /// <param name="shapeEntries">Entries composing the minkowski sum.</param>
        public MinkowskiSumShape(IList<OrientedConvexShapeEntry> shapeEntries)
        {
            if (shapeEntries.Count == 0)
            {
                throw new ArgumentException("Cannot create a wrapped shape with no contained shapes.");
            }

            for (int i = 0; i < shapeEntries.Count; i++)
            {
                Shapes.Add(shapeEntries[i]);
            }

            UpdateConvexShapeInfo();
            Shapes.Changed += ShapesChanged;
        }

        /// <summary>
        /// Constructs a minkowski sum shape.
        /// The sum will be recentered on its local origin.
        /// </summary>
        /// <param name="shapeEntries">Entries composing the minkowski sum.</param>
        /// <param name="center">Center of the minkowski sum computed pre-recentering.</param>
        public MinkowskiSumShape(IList<OrientedConvexShapeEntry> shapeEntries, out Vector3 center)
            : this(shapeEntries)
        {
            center = -localOffset;
        }

        /// <summary>
        /// Constructs a minkowski sum shape from cached data.
        /// </summary>
        /// <param name="shapeEntries">Entries composing the minkowski sum.</param>
        /// <param name="localOffset">Local offset of the elements in the minkowski sum.</param>
        /// <param name="description">Cached information about the shape. Assumed to be correct; no extra processing or validation is performed.</param>
        public MinkowskiSumShape(IList<OrientedConvexShapeEntry> shapeEntries, Vector3 localOffset,
            ConvexShapeDescription description)
        {
            for (int i = 0; i < shapeEntries.Count; i++)
            {
                Shapes.Add(shapeEntries[i]);
            }

            this.localOffset = localOffset;
            UpdateConvexShapeInfo(description);
            Shapes.Changed += ShapesChanged;
        }

        ///<summary>
        /// Gets the list of shapes in the minkowski sum.
        ///</summary>
        public ObservableList<OrientedConvexShapeEntry> Shapes { get; } =
            new ObservableList<OrientedConvexShapeEntry>();

        ///<summary>
        /// Gets the local offset of the elements in the minkowski sum.
        /// This is required because convex shapes need to be centered on their local origin.
        ///</summary>
        public Vector3 LocalOffset => localOffset;


        private void ShapesChanged(ObservableList<OrientedConvexShapeEntry> list)
        {
            OnShapeChanged();
        }

        protected override void OnShapeChanged()
        {
            UpdateConvexShapeInfo();
            base.OnShapeChanged();
        }


        /// <summary>
        /// Computes and applies a convex shape description for this MinkowskiSumShape.
        /// </summary>
        /// <returns>Description required to define a convex shape.</returns>
        public void UpdateConvexShapeInfo()
        {
            //Compute the volume distribution.
            RawList<Vector3> samples = CommonResources.GetVectorList();
            if (samples.Capacity < InertiaHelper.SampleDirections.Length)
            {
                samples.Capacity = InertiaHelper.SampleDirections.Length;
            }

            samples.Count = InertiaHelper.SampleDirections.Length;
            for (int i = 0; i < InertiaHelper.SampleDirections.Length; ++i)
            {
                GetLocalExtremePoint(InertiaHelper.SampleDirections[i], out samples.Elements[i]);
            }

            RawList<int> triangles = CommonResources.GetIntList();
            ConvexHullHelper.GetConvexHull(samples, triangles);

            float volume;
            Vector3 center;
            InertiaHelper.ComputeShapeDistribution(samples, triangles, out center, out volume, out volumeDistribution);
            Volume = volume;

            //Recenter the shape.
            localOffset = -center;
            CommonResources.GiveBack(samples);
            CommonResources.GiveBack(triangles);

            //Compute the radii.
            float minRadius = 0, maxRadius = 0;
            for (int i = 0; i < Shapes.Count; i++)
            {
                minRadius += Shapes.WrappedList.Elements[i].CollisionShape.MinimumRadius;
                maxRadius += Shapes.WrappedList.Elements[i].CollisionShape.MaximumRadius;
            }

            MinimumRadius = minRadius + collisionMargin;
            MaximumRadius = maxRadius + collisionMargin;
        }


        ///<summary>
        /// Gets the extreme point of the shape in local space in a given direction.
        ///</summary>
        ///<param name="direction">Direction to find the extreme point in.</param>
        ///<param name="extremePoint">Extreme point on the shape.</param>
        public override void GetLocalExtremePointWithoutMargin(ref Vector3 direction, out Vector3 extremePoint)
        {
            RigidTransform transform = new RigidTransform {Orientation = Shapes.WrappedList.Elements[0].Orientation};
            Shapes.WrappedList.Elements[0].CollisionShape.GetExtremePoint(direction, ref transform, out extremePoint);
            for (int i = 1; i < Shapes.WrappedList.Count; i++)
            {
                Vector3 temp;
                transform.Orientation = Shapes.WrappedList.Elements[i].Orientation;
                Shapes.WrappedList.Elements[i].CollisionShape.GetExtremePoint(direction, ref transform, out temp);
                Vector3.Add(ref extremePoint, ref temp, out extremePoint);
            }

            Vector3.Add(ref extremePoint, ref localOffset, out extremePoint);
        }


        /// <summary>
        /// Retrieves an instance of an EntityCollidable that uses this EntityShape.  Mainly used by compound bodies.
        /// </summary>
        /// <returns>EntityCollidable that uses this shape.</returns>
        public override EntityCollidable GetCollidableInstance()
        {
            return new ConvexCollidable<MinkowskiSumShape>(this);
        }
    }
}