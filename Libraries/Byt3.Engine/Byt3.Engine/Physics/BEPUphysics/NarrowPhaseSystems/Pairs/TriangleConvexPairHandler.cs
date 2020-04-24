﻿using System;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseSystems;
using Byt3.Engine.Physics.BEPUphysics.CollisionShapes.ConvexShapes;
using Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms.GJK;
using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;
using Byt3.Engine.Physics.BEPUphysics.PositionUpdating;
using Byt3.Engine.Physics.BEPUphysics.Settings;
using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a triangle-convex collision pair.
    ///</summary>
    public class TriangleConvexPairHandler : ConvexConstraintPairHandler
    {
        private TriangleConvexContactManifold contactManifold = new TriangleConvexContactManifold();
        private ConvexCollidable convex;
        private ConvexCollidable<TriangleShape> triangle;

        public override Collidable CollidableA => convex;

        public override Collidable CollidableB => triangle;

        public override Entities.Entity EntityA => convex.entity;

        public override Entities.Entity EntityB => triangle.entity;

        /// <summary>
        /// Gets the contact manifold used by the pair handler.
        /// </summary>
        public override ContactManifold ContactManifold => contactManifold;


        ///<summary>
        /// Initializes the pair handler.
        ///</summary>
        ///<param name="entryA">First entry in the pair.</param>
        ///<param name="entryB">Second entry in the pair.</param>
        public override void Initialize(BroadPhaseEntry entryA, BroadPhaseEntry entryB)
        {
            triangle = entryA as ConvexCollidable<TriangleShape>;
            convex = entryB as ConvexCollidable;

            if (triangle == null || convex == null)
            {
                triangle = entryB as ConvexCollidable<TriangleShape>;
                convex = entryA as ConvexCollidable;

                if (triangle == null || convex == null)
                {
                    throw new ArgumentException("Inappropriate types used to initialize pair.");
                }
            }

            //Contact normal goes from A to B.
            broadPhaseOverlap.entryA = convex;
            broadPhaseOverlap.entryB = triangle;

            base.Initialize(entryA, entryB);
        }

        ///<summary>
        /// Cleans up the pair handler.
        ///</summary>
        public override void CleanUp()
        {
            base.CleanUp();

            triangle = null;
            convex = null;
        }


        ///<summary>
        /// Updates the time of impact for the pair.
        ///</summary>
        ///<param name="requester">Collidable requesting the update.</param>
        ///<param name="dt">Timestep duration.</param>
        public override void UpdateTimeOfImpact(Collidable requester, float dt)
        {
            BroadPhaseOverlap overlap = BroadPhaseOverlap;
            PositionUpdateMode triangleMode = triangle.entity == null
                ? PositionUpdateMode.Discrete
                : triangle.entity.PositionUpdateMode;
            PositionUpdateMode convexMode =
                convex.entity == null ? PositionUpdateMode.Discrete : convex.entity.PositionUpdateMode;
            if (
                (overlap.entryA.IsActive || overlap.entryB.IsActive) && //At least one has to be active.
                (
                    convexMode == PositionUpdateMode.Continuous && //If both are continuous, only do the process for A.
                    triangleMode == PositionUpdateMode.Continuous &&
                    overlap.entryA == requester ||
                    (convexMode == PositionUpdateMode.Continuous) ^ //If only one is continuous, then we must do it.
                    (triangleMode == PositionUpdateMode.Continuous)
                )
            )
            {
                //Only perform the test if the minimum radii are small enough relative to the size of the velocity.
                Vector3 velocity;
                if (convexMode == PositionUpdateMode.Discrete)
                    //Triangle is static for the purposes of this continuous test.
                {
                    velocity = triangle.entity.linearVelocity;
                }
                else if (triangleMode == PositionUpdateMode.Discrete)
                    //Convex is static for the purposes of this continuous test.
                {
                    Vector3.Negate(ref convex.entity.linearVelocity, out velocity);
                }
                else
                    //Both objects are moving.
                {
                    Vector3.Subtract(ref triangle.entity.linearVelocity, ref convex.entity.linearVelocity,
                        out velocity);
                }

                Vector3.Multiply(ref velocity, dt, out velocity);
                float velocitySquared = velocity.LengthSquared();

                float minimumRadiusA = convex.Shape.MinimumRadius * MotionSettings.CoreShapeScaling;
                timeOfImpact = 1;
                if (minimumRadiusA * minimumRadiusA < velocitySquared)
                {
                    //Spherecast A against B.
                    RayHit rayHit;
                    if (GJKToolbox.CCDSphereCast(new Ray(convex.worldTransform.Position, -velocity), minimumRadiusA,
                        triangle.Shape, ref triangle.worldTransform, timeOfImpact, out rayHit))
                    {
                        if (triangle.Shape.sidedness != TriangleSidedness.DoubleSided)
                        {
                            //Only perform sweep if the object is in danger of hitting the object.
                            //Triangles can be one sided, so check the impact normal against the triangle normal.
                            Vector3 AB, AC;
                            Vector3.Subtract(ref triangle.Shape.vB, ref triangle.Shape.vA, out AB);
                            Vector3.Subtract(ref triangle.Shape.vC, ref triangle.Shape.vA, out AC);
                            Vector3 normal;
                            Vector3.Cross(ref AB, ref AC, out normal);

                            float dot;
                            Vector3.Dot(ref rayHit.Normal, ref normal, out dot);
                            if (triangle.Shape.sidedness == TriangleSidedness.Counterclockwise && dot < 0 ||
                                triangle.Shape.sidedness == TriangleSidedness.Clockwise && dot > 0)
                            {
                                timeOfImpact = rayHit.T;
                            }
                        }
                        else
                        {
                            timeOfImpact = rayHit.T;
                        }
                    }
                }

                //TECHNICALLY, the triangle should be casted too.  But, given the way triangles are usually used and their tiny minimum radius, ignoring it is usually just fine.
                //var minimumRadiusB = triangle.minimumRadius * MotionSettings.CoreShapeScaling;
                //if (minimumRadiusB * minimumRadiusB < velocitySquared)
                //{
                //    //Spherecast B against A.
                //    RayHit rayHit;
                //    if (GJKToolbox.SphereCast(new Ray(triangle.entity.position, velocity), minimumRadiusB, convex.Shape, ref convex.worldTransform, 1, out rayHit) &&
                //        rayHit.T < timeOfImpact)
                //    {
                //        if (triangle.Shape.sidedness != TriangleSidedness.DoubleSided)
                //        {
                //            float dot;
                //            Vector3.Dot(ref rayHit.Normal, ref normal, out dot);
                //            if (dot > 0)
                //            {
                //                timeOfImpact = rayHit.T;
                //            }
                //        }
                //        else
                //        {
                //            timeOfImpact = rayHit.T;
                //        }
                //    }
                //}

                //If it's intersecting, throw our hands into the air and give up.
                //This is generally a perfectly acceptable thing to do, since it's either sitting
                //inside another object (no ccd makes sense) or we're still in an intersecting case
                //from a previous frame where CCD took place and a contact should have been created
                //to deal with interpenetrating velocity.  Sometimes that contact isn't sufficient,
                //but it's good enough.
                if (timeOfImpact == 0)
                {
                    timeOfImpact = 1;
                }
            }
        }
    }
}