﻿using System;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using Byt3.Engine.Physics.BEPUphysics.CollisionShapes.ConvexShapes;
using Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;

namespace Byt3.Engine.Physics.BEPUphysics.CollisionTests.Manifolds
{
    ///<summary>
    /// Manages persistent contact data between two boxes.
    ///</summary>
    public class SphereContactManifold : ContactManifold
    {
        private readonly Contact contact = new Contact();
        private bool previouslyColliding;
        protected ConvexCollidable<SphereShape> sphereA;
        protected ConvexCollidable<SphereShape> sphereB;

        ///<summary>
        /// Constructs a new manifold.
        ///</summary>
        public SphereContactManifold()
        {
            contacts = new RawList<Contact>(1);
        }

        ///<summary>
        /// Gets the first collidable in the pair.
        ///</summary>
        public ConvexCollidable<SphereShape> CollidableA => sphereA;

        /// <summary>
        /// Gets the second collidable in the pair.
        /// </summary>
        public ConvexCollidable<SphereShape> CollidableB => sphereB;

        ///<summary>
        /// Updates the manifold.
        ///</summary>
        ///<param name="dt">Timestep duration.</param>
        public override void Update(float dt)
        {
            ContactData contactData;
            bool colliding = false;
            if (SphereTester.AreSpheresColliding(sphereA.Shape, sphereB.Shape, ref sphereA.worldTransform.Position,
                ref sphereB.worldTransform.Position, out contactData))
            {
                if (!previouslyColliding && contactData.PenetrationDepth >= 0
                ) //Don't use the contact if it's an initial contact and the depth is negative.  Why not? Bounciness and InitialCollisionDetected.
                {
                    Add(ref contactData);
                    colliding = true;
                }
                else if (previouslyColliding)
                {
                    contactData.Validate();
                    contact.Normal = contactData.Normal;
                    contact.PenetrationDepth = contactData.PenetrationDepth;
                    contact.Position = contactData.Position;
                    colliding = true;
                }
            }
            else
            {
                if (previouslyColliding)
                {
                    Remove(0);
                }
            }

            previouslyColliding = colliding;
        }

        protected override void Add(ref ContactData contactCandidate)
        {
            contactCandidate.Validate();
            contact.Normal = contactCandidate.Normal;
            contact.PenetrationDepth = contactCandidate.PenetrationDepth;
            contact.Position = contactCandidate.Position;

            contacts.Add(contact);
            OnAdded(contact);
        }

        protected override void Remove(int index)
        {
            contacts.RemoveAt(index);
            OnRemoved(contact);
        }


        ///<summary>
        /// Initializes the manifold.
        ///</summary>
        ///<param name="newCollidableA">First collidable.</param>
        ///<param name="newCollidableB">Second collidable.</param>
        ///<exception cref="Exception">Thrown when the collidables being used are not of the proper type.</exception>
        public override void Initialize(Collidable newCollidableA, Collidable newCollidableB)
        {
            sphereA = (ConvexCollidable<SphereShape>) newCollidableA;
            sphereB = (ConvexCollidable<SphereShape>) newCollidableB;

            if (sphereA == null || sphereB == null)
            {
                throw new ArgumentException("Inappropriate types used to initialize pair.");
            }
        }

        ///<summary>
        /// Cleans up the manifold.
        ///</summary>
        public override void CleanUp()
        {
            sphereA = null;
            sphereB = null;
            previouslyColliding = false;
            //We don't have to worry about losing a reference to our contact- we keep it local!
            contacts.Clear();
        }

        /// <summary>
        /// Clears the contacts associated with this manifold.
        /// </summary>
        public override void ClearContacts()
        {
            previouslyColliding = false;
            base.ClearContacts();
        }
    }
}