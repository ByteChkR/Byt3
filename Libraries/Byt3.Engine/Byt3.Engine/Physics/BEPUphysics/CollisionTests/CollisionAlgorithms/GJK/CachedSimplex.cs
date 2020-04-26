namespace Byt3.Engine.Physics.BEPUphysics.CollisionTests.CollisionAlgorithms.GJK
{
    ///<summary>
    /// Stored simplex used to warmstart closest point GJK runs.
    ///</summary>
    public struct CachedSimplex
    {
        //public CachedSimplex(ConvexShape shapeA, ConvexShape shapeB, ref RigidTransform transformA, ref RigidTransform transformB)
        //{
        //    RigidTransform localTransformB;
        //    MinkowskiToolbox.GetLocalTransform(ref transformA, ref transformB, out localTransformB);
        //    LocalSimplexA = new ContributingShapeSimplex();
        //    LocalSimplexB = new ContributingShapeSimplex();

        //    State = SimplexState.Point;
        //    return;
        //    shapeA.GetLocalExtremePointWithoutMargin(ref localTransformB.Position, out LocalSimplexA.A);
        //    Vector3 direction;
        //    Vector3.Negate(ref localTransformB.Position, out direction);
        //    Quaternion conjugate;
        //    Quaternion.Conjugate(ref localTransformB.Orientation, out conjugate);
        //    Vector3.Transform(ref direction, ref conjugate, out direction);
        //    shapeB.GetLocalExtremePointWithoutMargin(ref direction, out LocalSimplexB.A);
        //}

        //public CachedSimplex(ConvexShape shapeA, ConvexShape shapeB, ref RigidTransform localTransformB)
        //{
        //    LocalSimplexA = new ContributingShapeSimplex();
        //    LocalSimplexB = new ContributingShapeSimplex();

        //    State = SimplexState.Point;
        //    return;
        //    shapeA.GetLocalExtremePointWithoutMargin(ref localTransformB.Position, out LocalSimplexA.A);
        //    Vector3 direction;
        //    Vector3.Negate(ref localTransformB.Position, out direction);
        //    Quaternion conjugate;
        //    Quaternion.Conjugate(ref localTransformB.Orientation, out conjugate);
        //    Vector3.Transform(ref direction, ref conjugate, out direction);
        //    shapeB.GetLocalExtremePointWithoutMargin(ref direction, out LocalSimplexB.A);
        //}


        ///<summary>
        /// Simplex in the local space of shape A.
        ///</summary>
        public ContributingShapeSimplex LocalSimplexA;

        ///<summary>
        /// Simplex in the local space of shape B.
        ///</summary>
        public ContributingShapeSimplex LocalSimplexB;

        /// <summary>
        /// State of the simplex at the termination of the last GJK run.
        /// </summary>
        public SimplexState State;
    }
}