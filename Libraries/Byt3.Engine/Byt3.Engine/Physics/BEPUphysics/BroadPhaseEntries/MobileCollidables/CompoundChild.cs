using Byt3.Engine.Physics.BEPUphysics.BroadPhaseSystems;
using Byt3.Engine.Physics.BEPUphysics.CollisionShapes;
using Byt3.Engine.Physics.BEPUphysics.Materials;
using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.MobileCollidables
{
    ///<summary>
    /// A collidable child of a compound.
    ///</summary>
    public class CompoundChild : IBoundingBoxOwner
    {
        private CompoundShape shape;
        internal int shapeIndex;

        internal CompoundChild(CompoundShape shape, EntityCollidable collisionInformation, Material material, int index)
        {
            this.shape = shape;
            CollisionInformation = collisionInformation;
            Material = material;
            shapeIndex = index;
        }

        internal CompoundChild(CompoundShape shape, EntityCollidable collisionInformation, int index)
        {
            this.shape = shape;
            CollisionInformation = collisionInformation;
            shapeIndex = index;
        }

        /// <summary>
        /// Gets the index of the shape used by this child in the CompoundShape's shapes list.
        /// </summary>
        public int ShapeIndex => shapeIndex;

        ///<summary>
        /// Gets the Collidable associated with the child.
        ///</summary>
        public EntityCollidable CollisionInformation { get; }

        ///<summary>
        /// Gets or sets the material associated with the child.
        ///</summary>
        public Material Material { get; set; }

        /// <summary>
        /// Gets the index of the shape associated with this child in the CompoundShape's shapes list.
        /// </summary>
        public CompoundShapeEntry Entry => shape.shapes.Elements[shapeIndex];

        /// <summary>
        /// Gets the bounding box of the child.
        /// </summary>
        public BoundingBox BoundingBox => CollisionInformation.boundingBox;
    }
}