﻿using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries.MobileCollidables;
using Byt3.Engine.Physics.BEPUphysics.CollisionShapes.ConvexShapes;
using Byt3.Engine.Physics.BEPUphysics.DeactivationManagement;
using Byt3.Engine.Physics.BEPUphysics.Entities;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;
using Byt3.Engine.Physics.BEPUutilities.ResourceManagement;

namespace Byt3.Engine.Physics.BEPUphysics
{
    /// <summary>
    /// Handles allocation and management of commonly used resources.
    /// </summary>
    public static class PhysicsResources
    {
        private static LockingResourcePool<RawList<RayCastResult>> SubPoolRayCastResultList;
        private static LockingResourcePool<RawList<BroadPhaseEntry>> SubPoolBroadPhaseEntryList;
        private static LockingResourcePool<RawList<Collidable>> SubPoolCollidableList;
        private static LockingResourcePool<RawList<Entity>> SubPoolEntityRawList;
        private static LockingResourcePool<TriangleShape> SubPoolTriangleShape;
        private static LockingResourcePool<RawList<CompoundChild>> SubPoolCompoundChildList;
        private static LockingResourcePool<TriangleCollidable> SubPoolTriangleCollidables;

        private static LockingResourcePool<SimulationIslandConnection> SimulationIslandConnections;

        static PhysicsResources()
        {
            ResetPools();
        }

        public static void ResetPools()
        {
            SubPoolRayCastResultList = new LockingResourcePool<RawList<RayCastResult>>();
            SubPoolBroadPhaseEntryList = new LockingResourcePool<RawList<BroadPhaseEntry>>();
            SubPoolCollidableList = new LockingResourcePool<RawList<Collidable>>();
            SubPoolCompoundChildList = new LockingResourcePool<RawList<CompoundChild>>();

            SubPoolEntityRawList = new LockingResourcePool<RawList<Entity>>(16);
            SubPoolTriangleShape = new LockingResourcePool<TriangleShape>();
            SubPoolTriangleCollidables = new LockingResourcePool<TriangleCollidable>();
            SimulationIslandConnections = new LockingResourcePool<SimulationIslandConnection>();
        }

        //#endif
        /// <summary>
        /// Retrieves a ray cast result list from the resource pool.
        /// </summary>
        /// <returns>Empty ray cast result list.</returns>
        public static RawList<RayCastResult> GetRayCastResultList()
        {
            return SubPoolRayCastResultList.Take();
        }

        /// <summary>
        /// Returns a resource to the pool.
        /// </summary>
        /// <param name="list">List to return.</param>
        public static void GiveBack(RawList<RayCastResult> list)
        {
            list.Clear();
            SubPoolRayCastResultList.GiveBack(list);
        }

        /// <summary>
        /// Retrieves an BroadPhaseEntry list from the resource pool.
        /// </summary>
        /// <returns>Empty BroadPhaseEntry list.</returns>
        public static RawList<BroadPhaseEntry> GetBroadPhaseEntryList()
        {
            return SubPoolBroadPhaseEntryList.Take();
        }

        /// <summary>
        /// Returns a resource to the pool.
        /// </summary>
        /// <param name="list">List to return.</param>
        public static void GiveBack(RawList<BroadPhaseEntry> list)
        {
            list.Clear();
            SubPoolBroadPhaseEntryList.GiveBack(list);
        }

        /// <summary>
        /// Retrieves a Collidable list from the resource pool.
        /// </summary>
        /// <returns>Empty Collidable list.</returns>
        public static RawList<Collidable> GetCollidableList()
        {
            return SubPoolCollidableList.Take();
        }

        /// <summary>
        /// Returns a resource to the pool.
        /// </summary>
        /// <param name="list">List to return.</param>
        public static void GiveBack(RawList<Collidable> list)
        {
            list.Clear();
            SubPoolCollidableList.GiveBack(list);
        }

        /// <summary>
        /// Retrieves an CompoundChild list from the resource pool.
        /// </summary>
        /// <returns>Empty information list.</returns>
        public static RawList<CompoundChild> GetCompoundChildList()
        {
            return SubPoolCompoundChildList.Take();
        }

        /// <summary>
        /// Returns a resource to the pool.
        /// </summary>
        /// <param name="list">List to return.</param>
        public static void GiveBack(RawList<CompoundChild> list)
        {
            list.Clear();
            SubPoolCompoundChildList.GiveBack(list);
        }


        /// <summary>
        /// Retrieves an Entity RawList from the resource pool.
        /// </summary>
        /// <returns>Empty Entity raw list.</returns>
        public static RawList<Entity> GetEntityRawList()
        {
            return SubPoolEntityRawList.Take();
        }

        /// <summary>
        /// Returns a resource to the pool.
        /// </summary>
        /// <param name="list">List to return.</param>
        public static void GiveBack(RawList<Entity> list)
        {
            list.Clear();
            SubPoolEntityRawList.GiveBack(list);
        }


        /// <summary>
        /// Retrieves a TriangleCollidable from the resource pool.
        /// </summary>
        /// <returns>Initialized TriangleCollidable.</returns>
        public static TriangleCollidable GetTriangleCollidable()
        {
            return SubPoolTriangleCollidables.Take();
        }

        /// <summary>
        /// Returns a resource to the pool.
        /// </summary>
        /// <param name="triangle">Triangle collidable to return.</param>
        public static void GiveBack(TriangleCollidable triangle)
        {
            triangle.CleanUp();
            SubPoolTriangleCollidables.GiveBack(triangle);
        }

        /// <summary>
        /// Retrieves a simulation island connection from the resource pool.
        /// </summary>
        /// <returns>Uninitialized simulation island connection.</returns>
        public static SimulationIslandConnection GetSimulationIslandConnection()
        {
            return SimulationIslandConnections.Take();
        }

        /// <summary>
        /// Returns a resource to the pool.
        /// </summary>
        /// <param name="connection">Connection to return.</param>
        public static void GiveBack(SimulationIslandConnection connection)
        {
            connection.CleanUp();
            SimulationIslandConnections.GiveBack(connection);
        }
    }
}