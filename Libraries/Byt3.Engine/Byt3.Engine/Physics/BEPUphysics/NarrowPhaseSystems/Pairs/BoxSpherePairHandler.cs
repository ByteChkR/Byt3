﻿using System;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using Byt3.Engine.Physics.BEPUphysics.CollisionShapes.ConvexShapes;
using Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds;
using Byt3.Engine.Physics.BEPUphysics.Constraints.Collision;
using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a box and sphere in a collision.
    ///</summary>
    public class BoxSpherePairHandler : ConvexPairHandler
    {
        private ConvexCollidable<BoxShape> box;
        private NonConvexContactManifoldConstraint contactConstraint;

        //Using a non-convex one since they have slightly lower overhead than their Convex friends when dealing with a single contact point.
        private BoxSphereContactManifold contactManifold = new BoxSphereContactManifold();
        private ConvexCollidable<SphereShape> sphere;

        public BoxSpherePairHandler()
        {
            contactConstraint = new NonConvexContactManifoldConstraint(this);
        }

        public override Collidable CollidableA => box;

        public override Collidable CollidableB => sphere;

        /// <summary>
        /// Gets the contact constraint used by the pair handler.
        /// </summary>
        public override ContactManifoldConstraint ContactConstraint => contactConstraint;

        /// <summary>
        /// Gets the contact manifold used by the pair handler.
        /// </summary>
        public override ContactManifold ContactManifold => contactManifold;

        public override Entities.Entity EntityA => box.entity;

        public override Entities.Entity EntityB => sphere.entity;

        ///<summary>
        /// Initializes the pair handler.
        ///</summary>
        ///<param name="entryA">First entry in the pair.</param>
        ///<param name="entryB">Second entry in the pair.</param>
        public override void Initialize(BroadPhaseEntry entryA, BroadPhaseEntry entryB)
        {
            box = entryA as ConvexCollidable<BoxShape>;
            sphere = entryB as ConvexCollidable<SphereShape>;

            if (box == null || sphere == null)
            {
                box = entryB as ConvexCollidable<BoxShape>;
                sphere = entryA as ConvexCollidable<SphereShape>;
                if (box == null || sphere == null)
                {
                    throw new ArgumentException("Inappropriate types used to initialize pair.");
                }
            }

            //Reorder the entries so that the guarantee that the normal points from A to B is satisfied.
            broadPhaseOverlap.entryA = box;
            broadPhaseOverlap.entryB = sphere;

            base.Initialize(entryA, entryB);
        }


        ///<summary>
        /// Cleans up the pair handler.
        ///</summary>
        public override void CleanUp()
        {
            base.CleanUp();

            box = null;
            sphere = null;
        }


        protected internal override void GetContactInformation(int index, out ContactInformation info)
        {
            info.Contact = ContactManifold.contacts.Elements[index];
            //Find the contact's force.
            info.FrictionImpulse = 0;
            info.NormalImpulse = 0;
            for (int i = 0; i < contactConstraint.frictionConstraints.Count; i++)
            {
                if (contactConstraint.frictionConstraints.Elements[i].PenetrationConstraint.contact == info.Contact)
                {
                    info.FrictionImpulse = contactConstraint.frictionConstraints.Elements[i].accumulatedImpulse;
                    info.NormalImpulse = contactConstraint.frictionConstraints.Elements[i].PenetrationConstraint
                        .accumulatedImpulse;
                    break;
                }
            }

            //Compute relative velocity
            Vector3 velocity;

            if (EntityA != null)
            {
                Vector3.Subtract(ref info.Contact.Position, ref EntityA.position, out velocity);
                Vector3.Cross(ref EntityA.angularVelocity, ref velocity, out velocity);
                Vector3.Add(ref velocity, ref EntityA.linearVelocity, out info.RelativeVelocity);
            }
            else
            {
                info.RelativeVelocity = new Vector3();
            }

            if (EntityB != null)
            {
                Vector3.Subtract(ref info.Contact.Position, ref EntityB.position, out velocity);
                Vector3.Cross(ref EntityB.angularVelocity, ref velocity, out velocity);
                Vector3.Add(ref velocity, ref EntityB.linearVelocity, out velocity);
                Vector3.Subtract(ref info.RelativeVelocity, ref velocity, out info.RelativeVelocity);
            }

            info.Pair = this;
        }
    }
}