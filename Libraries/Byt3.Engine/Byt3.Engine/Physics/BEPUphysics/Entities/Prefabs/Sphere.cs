using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using Byt3.Engine.Physics.BEPUphysics.CollisionShapes.ConvexShapes;
using Byt3.Engine.Physics.BEPUphysics.EntityStateManagement;
using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.Entities.Prefabs
{
    /// <summary>
    /// Ball-shaped object that can collide and move.  After making an entity, add it to a Space so that the engine can manage it.
    /// </summary>
    public class Sphere : Entity<ConvexCollidable<SphereShape>>
    {
        private Sphere(float radius)
            : base(new ConvexCollidable<SphereShape>(new SphereShape(radius)))
        {
        }

        private Sphere(float radius, float mass)
            : base(new ConvexCollidable<SphereShape>(new SphereShape(radius)), mass)
        {
        }


        /// <summary>
        /// Constructs a physically simulated sphere.
        /// </summary>
        /// <param name="position">Position of the sphere.</param>
        /// <param name="radius">Radius of the sphere.</param>
        /// <param name="mass">Mass of the object.</param>
        public Sphere(Vector3 position, float radius, float mass)
            : this(radius, mass)
        {
            Position = position;
        }

        /// <summary>
        /// Constructs a nondynamic sphere.
        /// </summary>
        /// <param name="position">Position of the sphere.</param>
        /// <param name="radius">Radius of the sphere.</param>
        public Sphere(Vector3 position, float radius)
            : this(radius)
        {
            Position = position;
        }

        /// <summary>
        /// Constructs a physically simulated sphere.
        /// </summary>
        /// <param name="motionState">Motion state specifying the entity's initial state.</param>
        /// <param name="radius">Radius of the sphere.</param>
        /// <param name="mass">Mass of the object.</param>
        public Sphere(MotionState motionState, float radius, float mass)
            : this(radius, mass)
        {
            MotionState = motionState;
        }

        /// <summary>
        /// Constructs a nondynamic sphere.
        /// </summary>
        /// <param name="motionState">Motion state specifying the entity's initial state.</param>
        /// <param name="radius">Radius of the sphere.</param>
        public Sphere(MotionState motionState, float radius)
            : this(radius)
        {
            MotionState = motionState;
        }

        /// <summary>
        /// Radius of the sphere.
        /// </summary>
        public float Radius
        {
            get => CollisionInformation.Shape.Radius;
            set => CollisionInformation.Shape.Radius = value;
        }
    }
}