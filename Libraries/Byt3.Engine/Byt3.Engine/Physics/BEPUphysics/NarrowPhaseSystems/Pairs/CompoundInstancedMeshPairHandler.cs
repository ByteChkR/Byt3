﻿using System;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a compound-instanced mesh collision pair.
    ///</summary>
    public class CompoundInstancedMeshPairHandler : CompoundGroupPairHandler
    {
        private InstancedMesh mesh;

        public override Collidable CollidableB => mesh;

        public override Entities.Entity EntityB => null;

        ///<summary>
        /// Initializes the pair handler.
        ///</summary>
        ///<param name="entryA">First entry in the pair.</param>
        ///<param name="entryB">Second entry in the pair.</param>
        public override void Initialize(BroadPhaseEntry entryA, BroadPhaseEntry entryB)
        {
            mesh = entryA as InstancedMesh;
            if (mesh == null)
            {
                mesh = entryB as InstancedMesh;
                if (mesh == null)
                {
                    throw new ArgumentException("Inappropriate types used to initialize pair.");
                }
            }

            base.Initialize(entryA, entryB);
        }


        ///<summary>
        /// Cleans up the pair handler.
        ///</summary>
        public override void CleanUp()
        {
            base.CleanUp();
            mesh = null;
        }


        protected override void UpdateContainedPairs()
        {
            //Could go other way; get triangles in mesh that overlap the compound.
            //Could be faster sometimes depending on the way it's set up.
            RawList<CompoundChild> overlappedElements = PhysicsResources.GetCompoundChildList();
            compoundInfo.hierarchy.Tree.GetOverlaps(mesh.boundingBox, overlappedElements);
            for (int i = 0; i < overlappedElements.Count; i++)
            {
                TryToAdd(overlappedElements.Elements[i].CollisionInformation, mesh,
                    overlappedElements.Elements[i].Material, mesh.material);
            }

            PhysicsResources.GiveBack(overlappedElements);
        }
    }
}