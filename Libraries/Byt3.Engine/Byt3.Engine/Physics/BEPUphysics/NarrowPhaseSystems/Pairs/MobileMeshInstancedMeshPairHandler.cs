﻿using System;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using Byt3.Engine.Physics.BEPUphysics.CollisionShapes.ConvexShapes;
using Byt3.Engine.Physics.BEPUutilities;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;
using Byt3.Engine.Physics.BEPUutilities.ResourceManagement;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs
{
    ///<summary>
    /// Handles a mobile mesh-mobile mesh collision pair.
    ///</summary>
    public class MobileMeshInstancedMeshPairHandler : MobileMeshMeshPairHandler
    {
        private InstancedMesh mesh;

        public override Collidable CollidableB => mesh;

        public override Entities.Entity EntityB => null;

        protected override Materials.Material MaterialB => mesh.material;

        protected override TriangleCollidable GetOpposingCollidable(int index)
        {
            //Construct a TriangleCollidable from the static mesh.
            TriangleCollidable toReturn = PhysicsResources.GetTriangleCollidable();
            TriangleShape shape = toReturn.Shape;
            mesh.Shape.TriangleMesh.Data.GetTriangle(index, out shape.vA, out shape.vB, out shape.vC);
            Matrix3x3.Transform(ref shape.vA, ref mesh.worldTransform.LinearTransform, out shape.vA);
            Matrix3x3.Transform(ref shape.vB, ref mesh.worldTransform.LinearTransform, out shape.vB);
            Matrix3x3.Transform(ref shape.vC, ref mesh.worldTransform.LinearTransform, out shape.vC);
            Vector3 center;
            Vector3.Add(ref shape.vA, ref shape.vB, out center);
            Vector3.Add(ref center, ref shape.vC, out center);
            Vector3.Multiply(ref center, 1 / 3f, out center);
            Vector3.Subtract(ref shape.vA, ref center, out shape.vA);
            Vector3.Subtract(ref shape.vB, ref center, out shape.vB);
            Vector3.Subtract(ref shape.vC, ref center, out shape.vC);

            Vector3.Add(ref center, ref mesh.worldTransform.Translation, out center);
            //The bounding box doesn't update by itself.
            toReturn.worldTransform.Position = center;
            toReturn.worldTransform.Orientation = Quaternion.Identity;
            toReturn.UpdateBoundingBoxInternal(0);
            shape.sidedness = mesh.Sidedness;
            shape.collisionMargin = mobileMesh.Shape.MeshCollisionMargin;
            return toReturn;
        }


        protected override void ConfigureCollidable(TriangleEntry entry, float dt)
        {
        }

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


        protected override void UpdateContainedPairs(float dt)
        {
            RawList<int> overlappedElements = CommonResources.GetIntList();
            BoundingBox localBoundingBox;

            Vector3 sweep;
            Vector3.Multiply(ref mobileMesh.entity.linearVelocity, dt, out sweep);
            mobileMesh.Shape.GetSweptLocalBoundingBox(ref mobileMesh.worldTransform, ref mesh.worldTransform, ref sweep,
                out localBoundingBox);
            mesh.Shape.TriangleMesh.Tree.GetOverlaps(localBoundingBox, overlappedElements);
            for (int i = 0; i < overlappedElements.Count; i++)
            {
                TryToAdd(overlappedElements.Elements[i]);
            }

            CommonResources.GiveBack(overlappedElements);
        }
    }
}