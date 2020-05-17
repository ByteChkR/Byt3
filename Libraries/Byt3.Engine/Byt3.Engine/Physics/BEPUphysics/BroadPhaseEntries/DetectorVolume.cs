using System;
using System.Collections.Generic;
using Byt3.Engine.Physics.BEPUphysics.CollisionRuleManagement;
using Byt3.Engine.Physics.BEPUphysics.CollisionShapes.ConvexShapes;
using Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms;
using Byt3.Engine.Physics.BEPUphysics.DataStructures;
using Byt3.Engine.Physics.BEPUphysics.Entities;
using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;
using Byt3.Engine.Physics.BEPUphysics.OtherSpaceStages;
using Byt3.Engine.Physics.BEPUutilities;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;
using Byt3.Engine.Physics.BEPUutilities.ResourceManagement;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries
{
    /// <summary>
    /// Manages the detection of entities within an arbitrary closed triangle mesh.
    /// </summary>
    public class DetectorVolume : BroadPhaseEntry, ISpaceObject, IDeferredEventCreator
    {
        private readonly Queue<ContainmentChange> containmentChanges = new Queue<ContainmentChange>();


        /// <summary>
        /// Used to protect against containment changes coming in from multithreaded narrowphase contexts.
        /// </summary>
        private readonly SpinLock locker = new SpinLock();

        private bool innerFacingIsClockwise;

        internal Dictionary<Entity, DetectorVolumePairHandler> pairs =
            new Dictionary<Entity, DetectorVolumePairHandler>();


        private TriangleMesh triangleMesh;


        /// <summary>
        /// Creates a detector volume.
        /// </summary>
        /// <param name="triangleMesh">Closed and consistently wound mesh defining the volume.</param>
        public DetectorVolume(TriangleMesh triangleMesh)
        {
            TriangleMesh = triangleMesh;
            UpdateBoundingBox();
        }

        /// <summary>
        /// Gets the list of pairs associated with the detector volume.
        /// </summary>
        public ReadOnlyDictionary<Entity, DetectorVolumePairHandler> Pairs =>
            new ReadOnlyDictionary<Entity, DetectorVolumePairHandler>(pairs);

        /// <summary>
        /// Gets or sets the triangle mesh data and acceleration structure.  Must be a closed mesh with consistent winding.
        /// </summary>
        public TriangleMesh TriangleMesh
        {
            get => triangleMesh;
            set
            {
                triangleMesh = value;
                UpdateBoundingBox();
                Reinitialize();
            }
        }

        ///<summary>
        /// Space that owns the detector volume.
        ///</summary>
        public Space Space { get; private set; }

        /// <summary>
        /// Gets whether this collidable is associated with an active entity. True if it is, false if it's not.
        /// </summary>
        public override bool IsActive => false;


        DeferredEventDispatcher IDeferredEventCreator.DeferredEventDispatcher { get; set; }

        bool IDeferredEventCreator.IsActive
        {
            get => true;
            set => throw new NotSupportedException("Detector volumes are always active deferred event generators.");
        }

        void IDeferredEventCreator.DispatchEvents()
        {
            while (containmentChanges.Count > 0)
            {
                ContainmentChange change = containmentChanges.Dequeue();
                switch (change.Change)
                {
                    case ContainmentChangeType.BeganTouching:
                        EntityBeganTouching?.Invoke(this, change.Entity);

                        break;
                    case ContainmentChangeType.StoppedTouching:
                        EntityStoppedTouching?.Invoke(this, change.Entity);

                        break;
                    case ContainmentChangeType.BeganContaining:
                        VolumeBeganContainingEntity?.Invoke(this, change.Entity);

                        break;
                    case ContainmentChangeType.StoppedContaining:
                        VolumeStoppedContainingEntity?.Invoke(this, change.Entity);

                        break;
                }
            }
        }

        int IDeferredEventCreator.ChildDeferredEventCreators
        {
            get => 0;
            set => throw new NotSupportedException("The detector volume does not allow child deferred event creators.");
        }


        Space ISpaceObject.Space
        {
            get => Space;
            set => Space = value;
        }


        void ISpaceObject.OnAdditionToSpace(Space newSpace)
        {
        }

        void ISpaceObject.OnRemovalFromSpace(Space oldSpace)
        {
        }


        /// <summary>
        /// Fires when an entity comes into contact with the volume.
        /// </summary>
        public event EntityBeginsTouchingVolumeEventHandler EntityBeganTouching;

        /// <summary>
        /// Fires when an entity ceases to intersect the volume.
        /// </summary>
        public event EntityStopsTouchingVolumeEventHandler EntityStoppedTouching;

        /// <summary>
        /// Fires when an entity becomes fully engulfed by a volume.
        /// </summary>
        public event VolumeBeginsContainingEntityEventHandler VolumeBeganContainingEntity;

        /// <summary>
        /// Fires when an entity ceases to be fully engulfed by a volume.
        /// </summary>
        public event VolumeStopsContainingEntityEventHandler VolumeStoppedContainingEntity;

        /// <summary>
        /// Determines if a point is contained by the detector volume.
        /// </summary>
        /// <param name="point">Point to check for containment.</param>
        /// <returns>Whether or not the point is contained by the detector volume.</returns>
        public bool IsPointContained(Vector3 point)
        {
            RawList<int> triangles = CommonResources.GetIntList();
            bool contained = IsPointContained(ref point, triangles);
            CommonResources.GiveBack(triangles);
            return contained;
        }

        internal bool IsPointContained(ref Vector3 point, RawList<int> triangles)
        {
            Vector3 rayDirection;
            //Point from the approximate center of the mesh outwards.
            //This is a cheap way to reduce the number of unnecessary checks when objects are external to the mesh.
            Vector3.Add(ref boundingBox.Max, ref boundingBox.Min, out rayDirection);
            Vector3.Multiply(ref rayDirection, .5f, out rayDirection);
            Vector3.Subtract(ref point, ref rayDirection, out rayDirection);
            //If the point is right in the middle, we'll need a backup.
            if (rayDirection.LengthSquared() < .01f)
            {
                rayDirection = Vector3.Up;
            }

            Ray ray = new Ray(point, rayDirection);
            triangleMesh.Tree.GetOverlaps(ray, triangles);

            float minimumT = float.MaxValue;
            bool minimumIsClockwise = false;

            for (int i = 0; i < triangles.Count; i++)
            {
                Vector3 a, b, c;
                triangleMesh.Data.GetTriangle(triangles.Elements[i], out a, out b, out c);

                RayHit hit;
                bool hitClockwise;
                if (Toolbox.FindRayTriangleIntersection(ref ray, float.MaxValue, ref a, ref b, ref c, out hitClockwise,
                    out hit))
                {
                    if (hit.T < minimumT)
                    {
                        minimumT = hit.T;
                        minimumIsClockwise = hitClockwise;
                    }
                }
            }

            triangles.Clear();

            //If the first hit is on the inner surface, then the ray started inside the mesh.
            return minimumT < float.MaxValue && minimumIsClockwise == innerFacingIsClockwise;
        }

        protected override void CollisionRulesUpdated()
        {
            foreach (DetectorVolumePairHandler pair in pairs.Values)
            {
                pair.CollisionRule =
                    CollisionRules.CollisionRuleCalculator(pair.BroadPhaseOverlap.entryA,
                        pair.BroadPhaseOverlap.entryB);
            }
        }

        public override bool RayCast(Ray ray, float maximumLength, out RayHit rayHit)
        {
            return triangleMesh.RayCast(ray, maximumLength, TriangleSidedness.DoubleSided, out rayHit);
        }

        public override bool ConvexCast(ConvexShape castShape, ref RigidTransform startingTransform, ref Vector3 sweep,
            out RayHit hit)
        {
            hit = new RayHit();
            BoundingBox boundingBox;
            castShape.GetSweptBoundingBox(ref startingTransform, ref sweep, out boundingBox);
            TriangleShape tri = PhysicsThreadResources.GetTriangle();
            RawList<int> hitElements = CommonResources.GetIntList();
            if (triangleMesh.Tree.GetOverlaps(boundingBox, hitElements))
            {
                hit.T = float.MaxValue;
                for (int i = 0; i < hitElements.Count; i++)
                {
                    triangleMesh.Data.GetTriangle(hitElements[i], out tri.vA, out tri.vB, out tri.vC);
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

        /// <summary>
        /// Sets the bounding box of the detector volume to the current hierarchy root bounding box.  This is called automatically if the TriangleMesh property is set.
        /// </summary>
        public override void UpdateBoundingBox()
        {
            boundingBox = triangleMesh.Tree.BoundingBox;
        }

        /// <summary>
        /// Updates the detector volume's interpretation of the mesh.  This should be called when the the TriangleMesh is changed significantly.  This is called automatically if the TriangleMesh property is set.
        /// </summary>
        public void Reinitialize()
        {
            //Pick a point that is known to be outside the mesh as the origin.
            Vector3 origin = (triangleMesh.Tree.BoundingBox.Max - triangleMesh.Tree.BoundingBox.Min) * 1.5f +
                             triangleMesh.Tree.BoundingBox.Min;

            //Pick a direction which will definitely hit the mesh.
            Vector3 a, b, c;
            triangleMesh.Data.GetTriangle(0, out a, out b, out c);
            Vector3 direction = (a + b + c) / 3 - origin;

            Ray ray = new Ray(origin, direction);
            RawList<int> triangles = CommonResources.GetIntList();
            triangleMesh.Tree.GetOverlaps(ray, triangles);

            float minimumT = float.MaxValue;

            for (int i = 0; i < triangles.Count; i++)
            {
                triangleMesh.Data.GetTriangle(triangles.Elements[i], out a, out b, out c);

                RayHit hit;
                bool hitClockwise;
                if (Toolbox.FindRayTriangleIntersection(ref ray, float.MaxValue, ref a, ref b, ref c, out hitClockwise,
                    out hit))
                {
                    if (hit.T < minimumT)
                    {
                        minimumT = hit.T;
                        innerFacingIsClockwise = !hitClockwise;
                    }
                }
            }

            CommonResources.GiveBack(triangles);
        }

        internal void BeganTouching(DetectorVolumePairHandler pair)
        {
            locker.Enter();
            containmentChanges.Enqueue(new ContainmentChange
            {
                Change = ContainmentChangeType.BeganTouching,
                Entity = pair.Collidable.entity
            });
            locker.Exit();
        }

        internal void StoppedTouching(DetectorVolumePairHandler pair)
        {
            locker.Enter();
            containmentChanges.Enqueue(new ContainmentChange
            {
                Change = ContainmentChangeType.StoppedTouching,
                Entity = pair.Collidable.entity
            });
            locker.Exit();
        }

        internal void BeganContaining(DetectorVolumePairHandler pair)
        {
            locker.Enter();
            containmentChanges.Enqueue(new ContainmentChange
            {
                Change = ContainmentChangeType.BeganContaining,
                Entity = pair.Collidable.entity
            });
            locker.Exit();
        }

        internal void StoppedContaining(DetectorVolumePairHandler pair)
        {
            locker.Enter();
            containmentChanges.Enqueue(new ContainmentChange
            {
                Change = ContainmentChangeType.StoppedContaining,
                Entity = pair.Collidable.entity
            });
            locker.Exit();
        }

        private struct ContainmentChange
        {
            public Entity Entity;
            public ContainmentChangeType Change;
        }

        private enum ContainmentChangeType : byte
        {
            BeganTouching,
            StoppedTouching,
            BeganContaining,
            StoppedContaining
        }
    }
}