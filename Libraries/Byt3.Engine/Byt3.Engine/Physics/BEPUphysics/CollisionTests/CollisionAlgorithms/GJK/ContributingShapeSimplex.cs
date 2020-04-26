using Byt3.Engine.Physics.BEPUutilities;

namespace Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms.GJK
{
    ///<summary>
    /// List of points composing a shape's contributions to a simplex.
    ///</summary>
    public struct ContributingShapeSimplex
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;
        public Vector3 D;
    }
}