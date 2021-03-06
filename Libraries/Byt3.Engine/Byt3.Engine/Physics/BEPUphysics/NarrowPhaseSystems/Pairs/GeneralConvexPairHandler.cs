﻿using System;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a convex-convex collision pair.
    ///</summary>
    public class GeneralConvexPairHandler : ConvexConstraintPairHandler
    {
        private readonly GeneralConvexContactManifold contactManifold = new GeneralConvexContactManifold();
        private ConvexCollidable convexA;
        private ConvexCollidable convexB;


        public override Collidable CollidableA => convexA;

        public override Collidable CollidableB => convexB;

        /// <summary>
        /// Gets the contact manifold used by the pair handler.
        /// </summary>
        public override ContactManifold ContactManifold => contactManifold;

        public override Entities.Entity EntityA => convexA.entity;

        public override Entities.Entity EntityB => convexB.entity;

        ///<summary>
        /// Initializes the pair handler.
        ///</summary>
        ///<param name="entryA">First entry in the pair.</param>
        ///<param name="entryB">Second entry in the pair.</param>
        public override void Initialize(BroadPhaseEntry entryA, BroadPhaseEntry entryB)
        {
            convexA = entryA as ConvexCollidable;
            convexB = entryB as ConvexCollidable;

            if (convexA == null || convexB == null)
            {
                throw new ArgumentException("Inappropriate types used to initialize pair.");
            }

            base.Initialize(entryA, entryB);
        }


        ///<summary>
        /// Cleans up the pair handler.
        ///</summary>
        public override void CleanUp()
        {
            base.CleanUp();

            convexA = null;
            convexB = null;
        }
    }
}