﻿using System;
using System.Collections.Generic;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUphysics.DataStructures;
using Byt3.Engine.Physics.BEPUutilities;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;

namespace Byt3.Engine.Physics.BEPUphysics.CollisionShapes
{
    ///<summary>
    /// The shape information used by a StaticGroup.
    /// Unlike most shapes, a StaticGroupShape cannot be shared between multiple StaticGroups;
    /// a StaticGroupShape is linked to a single StaticGroup.
    ///</summary>
    public class StaticGroupShape : CollisionShape
    {
        ///<summary>
        /// Constructs a new StaticGroupShape.
        ///</summary>
        ///<param name="collidables">List of collidables in the StaticGroup.</param>
        ///<param name="owner">StaticGroup directly associated with this shape.</param>
        public StaticGroupShape(IList<Collidable> collidables, StaticGroup owner)
        {
            StaticGroup = owner;
            CollidableTree = new BoundingBoxTree<Collidable>(collidables);
            //Rather than hooking up a bunch of ShapeChanged events here that don't capture the full capacity of change
            //in our child collidables, we will rely on the user telling the collidable tree to reformat itself directly.
        }
        //Technically, while this is superior to the 'put a bunch of stuff in the Space' approach, it is still 
        //inferior in maximum performance to an approach which allows for the direct comparison of a static tree against a nonstatic tree.
        //This would require tighter integration with the broad phase, though.  It's not clear that the performance boost is worth it.

        /// <summary>
        /// Gets the StaticGroup associated with this StaticGroupShape.  Unlike most shapes, there is a one-to-one relationship
        /// between StaticGroupShapes and StaticGroups.
        /// </summary>
        public StaticGroup StaticGroup { get; }

        /// <summary>
        /// Gets the bounding box tree associated with this shape.
        /// Contains Collidable instances as opposed to shapes.
        /// </summary>
        public BoundingBoxTree<Collidable> CollidableTree { get; }

        /// <summary>
        /// Adds a new collidable to the group.
        /// </summary>
        /// <param name="collidable">Collidable to remove.</param>
        public void Add(Collidable collidable)
        {
            CollidableTree.Add(collidable);
            OnShapeChanged();
        }

        /// <summary>
        /// Removes a collidable from the group.
        /// </summary>
        /// <param name="collidable">Collidable to remove.</param>
        public void Remove(Collidable collidable)
        {
            CollidableTree.Remove(collidable);
            OnShapeChanged();
        }


        /// <summary>
        /// Tests a ray against the collidable.
        /// </summary>
        /// <param name="ray">Ray to test.</param>
        /// <param name="maximumLength">Maximum length, in units of the ray's direction's length, to test.</param>
        /// <param name="result">Hit data, if any.</param>
        /// <returns>Whether or not the ray hit the entry.</returns>
        public bool RayCast(Ray ray, float maximumLength, out RayCastResult result)
        {
            RawList<Collidable> outputOverlappedElements = PhysicsResources.GetCollidableList();
            CollidableTree.GetOverlaps(ray, maximumLength, outputOverlappedElements);
            result = new RayCastResult();
            result.HitData.T = float.MaxValue;
            for (int i = 0; i < outputOverlappedElements.Count; ++i)
            {
                RayHit hit;
                if (outputOverlappedElements.Elements[i].RayCast(ray, maximumLength, out hit))
                {
                    if (hit.T < result.HitData.T)
                    {
                        result.HitData = hit;
                        result.HitObject = outputOverlappedElements.Elements[i];
                    }
                }
            }

            PhysicsResources.GiveBack(outputOverlappedElements);
            return result.HitData.T < float.MaxValue;
        }

        /// <summary>
        /// Tests a ray against the collidable.
        /// </summary>
        /// <param name="ray">Ray to test.</param>
        /// <param name="maximumLength">Maximum length, in units of the ray's direction's length, to test.</param>
        /// <param name="filter">Test to apply to the entry. If it returns true, the entry is processed, otherwise the entry is ignored. If a collidable hierarchy is present
        /// in the entry, this filter will be passed into inner ray casts.</param>
        /// <param name="result">Hit data, if any.</param>
        /// <returns>Whether or not the ray hit the entry.</returns>
        public bool RayCast(Ray ray, float maximumLength, Func<BroadPhaseEntry, bool> filter, out RayCastResult result)
        {
            RawList<Collidable> outputOverlappedElements = PhysicsResources.GetCollidableList();
            CollidableTree.GetOverlaps(ray, maximumLength, outputOverlappedElements);
            result = new RayCastResult();
            result.HitData.T = float.MaxValue;
            for (int i = 0; i < outputOverlappedElements.Count; ++i)
            {
                RayHit hit;
                if (outputOverlappedElements.Elements[i].RayCast(ray, maximumLength, filter, out hit))
                {
                    if (hit.T < result.HitData.T)
                    {
                        result.HitData = hit;
                        result.HitObject = outputOverlappedElements.Elements[i];
                    }
                }
            }

            PhysicsResources.GiveBack(outputOverlappedElements);
            return result.HitData.T < float.MaxValue;
        }


        /// <summary>
        /// Casts a convex shape against the collidable.
        /// </summary>
        /// <param name="castShape">Shape to cast.</param>
        /// <param name="startingTransform">Initial transform of the shape.</param>
        /// <param name="sweep">Sweep to apply to the shape.</param>
        /// <param name="result">Hit data, if any.</param>
        /// <returns>Whether or not the cast hit anything.</returns>
        public bool ConvexCast(ConvexShapes.ConvexShape castShape, ref RigidTransform startingTransform,
            ref Vector3 sweep, out RayCastResult result)
        {
            RawList<Collidable> outputOverlappedElements = PhysicsResources.GetCollidableList();
            BoundingBox boundingBox;
            castShape.GetSweptBoundingBox(ref startingTransform, ref sweep, out boundingBox);

            CollidableTree.GetOverlaps(boundingBox, outputOverlappedElements);
            result = new RayCastResult();
            result.HitData.T = float.MaxValue;
            for (int i = 0; i < outputOverlappedElements.Count; ++i)
            {
                RayHit hit;
                if (outputOverlappedElements.Elements[i]
                    .ConvexCast(castShape, ref startingTransform, ref sweep, out hit))
                {
                    if (hit.T < result.HitData.T)
                    {
                        result.HitData = hit;
                        result.HitObject = outputOverlappedElements.Elements[i];
                    }
                }
            }

            PhysicsResources.GiveBack(outputOverlappedElements);
            return result.HitData.T < float.MaxValue;
        }

        /// <summary>
        /// Casts a convex shape against the collidable.
        /// </summary>
        /// <param name="castShape">Shape to cast.</param>
        /// <param name="startingTransform">Initial transform of the shape.</param>
        /// <param name="sweep">Sweep to apply to the shape.</param>
        /// <param name="filter">Test to apply to the entry. If it returns true, the entry is processed, otherwise the entry is ignored. If a collidable hierarchy is present
        /// in the entry, this filter will be passed into inner ray casts.</param>
        /// <param name="result">Hit data, if any.</param>
        /// <returns>Whether or not the cast hit anything.</returns>
        public bool ConvexCast(ConvexShapes.ConvexShape castShape, ref RigidTransform startingTransform,
            ref Vector3 sweep, Func<BroadPhaseEntry, bool> filter, out RayCastResult result)
        {
            RawList<Collidable> outputOverlappedElements = PhysicsResources.GetCollidableList();
            BoundingBox boundingBox;
            castShape.GetSweptBoundingBox(ref startingTransform, ref sweep, out boundingBox);

            CollidableTree.GetOverlaps(boundingBox, outputOverlappedElements);
            result = new RayCastResult();
            result.HitData.T = float.MaxValue;
            for (int i = 0; i < outputOverlappedElements.Count; ++i)
            {
                RayHit hit;
                if (outputOverlappedElements.Elements[i]
                    .ConvexCast(castShape, ref startingTransform, ref sweep, filter, out hit))
                {
                    if (hit.T < result.HitData.T)
                    {
                        result.HitData = hit;
                        result.HitObject = outputOverlappedElements.Elements[i];
                    }
                }
            }

            PhysicsResources.GiveBack(outputOverlappedElements);
            return result.HitData.T < float.MaxValue;
        }
    }
}