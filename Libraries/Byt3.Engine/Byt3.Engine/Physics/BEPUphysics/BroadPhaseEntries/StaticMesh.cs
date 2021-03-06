﻿using System;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.Events;
using Byt3.Engine.Physics.BEPUphysics.CollisionShapes;
using Byt3.Engine.Physics.BEPUphysics.CollisionShapes.ConvexShapes;
using Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms;
using Byt3.Engine.Physics.BEPUphysics.DataStructures;
using Byt3.Engine.Physics.BEPUphysics.OtherSpaceStages;
using Byt3.Engine.Physics.BEPUutilities;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;
using Byt3.Engine.Physics.BEPUutilities.ResourceManagement;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries
{
    ///<summary>
    /// Unmoving, collidable triangle mesh.
    ///</summary>
    ///<remarks>
    /// The acceleration structure for the mesh is created individually for each
    /// StaticMesh; if you want to create many meshes of the same model, consider using the
    /// InstancedMesh.
    /// </remarks>
    public class StaticMesh : StaticCollidable
    {
        protected internal ContactEventManager<StaticMesh> events;

        internal bool improveBoundaryBehavior = true;

        internal TriangleSidedness sidedness = TriangleSidedness.DoubleSided;

        ///<summary>
        /// Constructs a new static mesh.
        ///</summary>
        ///<param name="vertices">Vertex positions of the mesh.</param>
        ///<param name="indices">Index list of the mesh.</param>
        public StaticMesh(Vector3[] vertices, int[] indices)
        {
            base.Shape = new StaticMeshShape(vertices, indices);
            Events = new ContactEventManager<StaticMesh>();
        }

        ///<summary>
        /// Constructs a new static mesh.
        ///</summary>
        ///<param name="vertices">Vertex positions of the mesh.</param>
        ///<param name="indices">Index list of the mesh.</param>
        /// <param name="worldTransform">Transform to use to create the mesh initially.</param>
        public StaticMesh(Vector3[] vertices, int[] indices, AffineTransform worldTransform)
        {
            base.Shape = new StaticMeshShape(vertices, indices, worldTransform);
            Events = new ContactEventManager<StaticMesh>();
        }

        ///<summary>
        /// Gets the TriangleMesh acceleration structure used by the StaticMesh.
        ///</summary>
        public TriangleMesh Mesh { get; private set; }

        ///<summary>
        /// Gets or sets the world transform of the mesh.
        ///</summary>
        public AffineTransform WorldTransform
        {
            get => ((TransformableMeshData) Mesh.Data).worldTransform;
            set
            {
                ((TransformableMeshData) Mesh.Data).WorldTransform = value;
                Mesh.Tree.Refit();
                UpdateBoundingBox();
            }
        }

        ///<summary>
        /// Gets the shape used by the mesh.
        ///</summary>
        public new StaticMeshShape Shape => (StaticMeshShape) shape;

        ///<summary>
        /// Gets or sets the sidedness of the mesh.  This can be used to ignore collisions and rays coming from a direction relative to the winding of the triangle.
        ///</summary>
        public TriangleSidedness Sidedness
        {
            get => sidedness;
            set => sidedness = value;
        }

        /// <summary>
        /// Gets or sets whether or not the collision system should attempt to improve contact behavior at the boundaries between triangles.
        /// This has a slight performance cost, but prevents objects sliding across a triangle boundary from 'bumping,' and otherwise improves
        /// the robustness of contacts at edges and vertices.
        /// </summary>
        public bool ImproveBoundaryBehavior
        {
            get => improveBoundaryBehavior;
            set => improveBoundaryBehavior = value;
        }

        ///<summary>
        /// Gets the event manager used by the mesh.
        ///</summary>
        public ContactEventManager<StaticMesh> Events
        {
            get => events;
            set
            {
                if (value.Owner != null && //Can't use a manager which is owned by a different entity.
                    value != events) //Stay quiet if for some reason the same event manager is being set.
                {
                    throw new ArgumentException(
                        "Event manager is already owned by a mesh; event managers cannot be shared.");
                }

                if (events != null)
                {
                    events.Owner = null;
                }

                events = value;
                if (events != null)
                {
                    events.Owner = this;
                }
            }
        }

        protected internal override IContactEventTriggerer EventTriggerer => events;

        protected override IDeferredEventCreator EventCreator => events;

        protected override void OnShapeChanged(CollisionShape collisionShape)
        {
            if (!IgnoreShapeChanges)
            {
                Mesh = new TriangleMesh(Shape.TriangleMeshData);
                UpdateBoundingBox();
            }
        }

        /// <summary>
        /// Updates the bounding box to the current state of the entry.
        /// </summary>
        public override void UpdateBoundingBox()
        {
            boundingBox = Mesh.Tree.BoundingBox;
        }

        /// <summary>
        /// Tests a ray against the entry.
        /// </summary>
        /// <param name="ray">Ray to test.</param>
        /// <param name="maximumLength">Maximum length, in units of the ray's direction's length, to test.</param>
        /// <param name="rayHit">Hit location of the ray on the entry, if any.</param>
        /// <returns>Whether or not the ray hit the entry.</returns>
        public override bool RayCast(Ray ray, float maximumLength, out RayHit rayHit)
        {
            return Mesh.RayCast(ray, maximumLength, sidedness, out rayHit);
        }

        /// <summary>
        /// Casts a convex shape against the collidable.
        /// </summary>
        /// <param name="castShape">Shape to cast.</param>
        /// <param name="startingTransform">Initial transform of the shape.</param>
        /// <param name="sweep">Sweep to apply to the shape.</param>
        /// <param name="hit">Hit data, if any.</param>
        /// <returns>Whether or not the cast hit anything.</returns>
        public override bool ConvexCast(ConvexShape castShape, ref RigidTransform startingTransform, ref Vector3 sweep,
            out RayHit hit)
        {
            hit = new RayHit();
            BoundingBox boundingBox;
            castShape.GetSweptBoundingBox(ref startingTransform, ref sweep, out boundingBox);
            TriangleShape tri = PhysicsThreadResources.GetTriangle();
            RawList<int> hitElements = CommonResources.GetIntList();
            if (Mesh.Tree.GetOverlaps(boundingBox, hitElements))
            {
                hit.T = float.MaxValue;
                for (int i = 0; i < hitElements.Count; i++)
                {
                    Mesh.Data.GetTriangle(hitElements[i], out tri.vA, out tri.vB, out tri.vC);
                    Vector3 center;
                    Vector3.Add(ref tri.vA, ref tri.vB, out center);
                    Vector3.Add(ref center, ref tri.vC, out center);
                    Vector3.Multiply(ref center, 1f / 3f, out center);
                    Vector3.Subtract(ref tri.vA, ref center, out tri.vA);
                    Vector3.Subtract(ref tri.vB, ref center, out tri.vB);
                    Vector3.Subtract(ref tri.vC, ref center, out tri.vC);
                    tri.MaximumRadius = tri.vA.LengthSquared();
                    float radius = tri.vB.LengthSquared();
                    if (tri.MaximumRadius < radius)
                    {
                        tri.MaximumRadius = radius;
                    }

                    radius = tri.vC.LengthSquared();
                    if (tri.MaximumRadius < radius)
                    {
                        tri.MaximumRadius = radius;
                    }

                    tri.MaximumRadius = (float) Math.Sqrt(tri.MaximumRadius);
                    tri.collisionMargin = 0;
                    RigidTransform triangleTransform = new RigidTransform
                        {Orientation = Quaternion.Identity, Position = center};
                    RayHit tempHit;
                    if (MPRToolbox.Sweep(castShape, tri, ref sweep, ref Toolbox.ZeroVector, ref startingTransform,
                            ref triangleTransform, out tempHit) && tempHit.T < hit.T)
                    {
                        hit = tempHit;
                    }
                }

                tri.MaximumRadius = 0;
                PhysicsThreadResources.GiveBack(tri);
                CommonResources.GiveBack(hitElements);
                return hit.T != float.MaxValue;
            }

            PhysicsThreadResources.GiveBack(tri);
            CommonResources.GiveBack(hitElements);
            return false;
        }

        ///<summary>
        /// Tests a ray against the mesh.
        ///</summary>
        ///<param name="ray">Ray to test.</param>
        ///<param name="maximumLength">Maximum length to test in units of the ray direction's length.</param>
        ///<param name="sidedness">Sidedness to use when raycasting.  Doesn't have to be the same as the mesh's own sidedness.</param>
        ///<param name="rayHit">Data about the ray's intersection with the mesh, if any.</param>
        ///<returns>Whether or not the ray hit the mesh.</returns>
        public bool RayCast(Ray ray, float maximumLength, TriangleSidedness sidedness, out RayHit rayHit)
        {
            return Mesh.RayCast(ray, maximumLength, sidedness, out rayHit);
        }
    }
}