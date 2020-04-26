using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems
{
    /// <summary>
    /// Contains the various factories that are used by default in the engine.
    /// </summary>
    public class Factories
    {
        private readonly RawList<NarrowPhasePairFactory> factories = new RawList<NarrowPhasePairFactory>();

        /// <summary>
        /// Constructs all factories.
        /// </summary>
        public Factories()
        {
            factories.Add(BoxBox = new NarrowPhasePairFactory<BoxPairHandler>());
            factories.Add(BoxSphere = new NarrowPhasePairFactory<BoxSpherePairHandler>());
            factories.Add(SphereSphere = new NarrowPhasePairFactory<SpherePairHandler>());
            factories.Add(ConvexConvex = new NarrowPhasePairFactory<GeneralConvexPairHandler>());
            factories.Add(TriangleConvex = new NarrowPhasePairFactory<TriangleConvexPairHandler>());
            factories.Add(CompoundConvex = new NarrowPhasePairFactory<CompoundConvexPairHandler>());
            factories.Add(CompoundCompound = new NarrowPhasePairFactory<CompoundPairHandler>());
            factories.Add(CompoundStaticMesh = new NarrowPhasePairFactory<CompoundStaticMeshPairHandler>());
            factories.Add(CompoundTerrain = new NarrowPhasePairFactory<CompoundTerrainPairHandler>());
            factories.Add(CompoundInstancedMesh = new NarrowPhasePairFactory<CompoundInstancedMeshPairHandler>());
            factories.Add(CompoundMobileMesh = new NarrowPhasePairFactory<CompoundMobileMeshPairHandler>());
            factories.Add(StaticMeshConvex = new NarrowPhasePairFactory<StaticMeshConvexPairHandler>());
            factories.Add(StaticMeshSphere = new NarrowPhasePairFactory<StaticMeshSpherePairHandler>());
            factories.Add(TerrainConvex = new NarrowPhasePairFactory<TerrainConvexPairHandler>());
            factories.Add(TerrainSphere = new NarrowPhasePairFactory<TerrainSpherePairHandler>());
            factories.Add(InstancedMeshConvex = new NarrowPhasePairFactory<InstancedMeshConvexPairHandler>());
            factories.Add(InstancedMeshSphere = new NarrowPhasePairFactory<InstancedMeshSpherePairHandler>());
            factories.Add(MobileMeshConvex = new NarrowPhasePairFactory<MobileMeshConvexPairHandler>());
            factories.Add(MobileMeshSphere = new NarrowPhasePairFactory<MobileMeshSpherePairHandler>());
            factories.Add(MobileMeshTriangle = new NarrowPhasePairFactory<MobileMeshTrianglePairHandler>());
            factories.Add(MobileMeshStaticMesh = new NarrowPhasePairFactory<MobileMeshStaticMeshPairHandler>());
            factories.Add(MobileMeshInstancedMesh = new NarrowPhasePairFactory<MobileMeshInstancedMeshPairHandler>());
            factories.Add(MobileMeshTerrain = new NarrowPhasePairFactory<MobileMeshTerrainPairHandler>());
            factories.Add(MobileMeshMobileMesh = new NarrowPhasePairFactory<MobileMeshMobileMeshPairHandler>());
            factories.Add(StaticGroupConvex = new NarrowPhasePairFactory<StaticGroupConvexPairHandler>());
            factories.Add(StaticGroupCompound = new NarrowPhasePairFactory<StaticGroupCompoundPairHandler>());
            factories.Add(StaticGroupMobileMesh = new NarrowPhasePairFactory<StaticGroupMobileMeshPairHandler>());
            factories.Add(DetectorVolumeConvex = new NarrowPhasePairFactory<DetectorVolumeConvexPairHandler>());
            factories.Add(DetectorVolumeMobileMesh = new NarrowPhasePairFactory<DetectorVolumeMobileMeshPairHandler>());
            factories.Add(DetectorVolumeCompound = new NarrowPhasePairFactory<DetectorVolumeCompoundPairHandler>());
        }

        /// <summary>
        /// Gets the factory for the box-box case.
        /// </summary>
        public NarrowPhasePairFactory<BoxPairHandler> BoxBox { get; }

        /// <summary>
        /// Gets the factory for the box-sphere case.
        /// </summary>
        public NarrowPhasePairFactory<BoxSpherePairHandler> BoxSphere { get; }

        /// <summary>
        /// Gets the factory for the sphere-sphere case.
        /// </summary>
        public NarrowPhasePairFactory<SpherePairHandler> SphereSphere { get; }

        /// <summary>
        /// Gets the factory for the convex-convex case.  This works for any two convexes, though some other special cases (e.g. box-box) supersede it.
        /// </summary>
        public NarrowPhasePairFactory<GeneralConvexPairHandler> ConvexConvex { get; }

        /// <summary>
        /// Gets the factory for the triangle-convex case.
        /// </summary>
        public NarrowPhasePairFactory<TriangleConvexPairHandler> TriangleConvex { get; }

        /// <summary>
        /// Gets the factory for the compound-convex case.
        /// </summary>
        public NarrowPhasePairFactory<CompoundConvexPairHandler> CompoundConvex { get; }

        /// <summary>
        /// Gets the factory for the compound-compound case.
        /// </summary>
        public NarrowPhasePairFactory<CompoundPairHandler> CompoundCompound { get; }

        /// <summary>
        /// Gets the factory for the compound-static mesh case.
        /// </summary>
        public NarrowPhasePairFactory<CompoundStaticMeshPairHandler> CompoundStaticMesh { get; }

        /// <summary>
        /// Gets the factory for the compound-terrain case.
        /// </summary>
        public NarrowPhasePairFactory<CompoundTerrainPairHandler> CompoundTerrain { get; }

        /// <summary>
        /// Gets the factory for the compound-instanced mesh case.
        /// </summary>
        public NarrowPhasePairFactory<CompoundInstancedMeshPairHandler> CompoundInstancedMesh { get; }

        /// <summary>
        /// Gets the factory for the compound-mobile mesh case.
        /// </summary>
        public NarrowPhasePairFactory<CompoundMobileMeshPairHandler> CompoundMobileMesh { get; }

        /// <summary>
        /// Gets the factory for the static mesh-convex case.
        /// </summary>
        public NarrowPhasePairFactory<StaticMeshConvexPairHandler> StaticMeshConvex { get; }

        /// <summary>
        /// Gets the factory for the static mesh-sphere case.
        /// </summary>
        public NarrowPhasePairFactory<StaticMeshSpherePairHandler> StaticMeshSphere { get; }

        /// <summary>
        /// Gets the factory for the terrain-convex case.
        /// </summary>
        public NarrowPhasePairFactory<TerrainConvexPairHandler> TerrainConvex { get; }

        /// <summary>
        /// Gets the factory for the terrain-sphere case.
        /// </summary>
        public NarrowPhasePairFactory<TerrainSpherePairHandler> TerrainSphere { get; }

        /// <summary>
        /// Gets the factory for the instanced mesh-convex case.
        /// </summary>
        public NarrowPhasePairFactory<InstancedMeshConvexPairHandler> InstancedMeshConvex { get; }

        /// <summary>
        /// Gets the factory for the instanced mesh-sphere case.
        /// </summary>
        public NarrowPhasePairFactory<InstancedMeshSpherePairHandler> InstancedMeshSphere { get; }

        /// <summary>
        /// Gets the factory for the mobile mesh-convex case.
        /// </summary>
        public NarrowPhasePairFactory<MobileMeshConvexPairHandler> MobileMeshConvex { get; }

        /// <summary>
        /// Gets the factory for the mobile mesh-sphere case.
        /// </summary>
        public NarrowPhasePairFactory<MobileMeshSpherePairHandler> MobileMeshSphere { get; }

        /// <summary>
        /// Gets the factory for the mobile mesh-triangle case.
        /// </summary>
        public NarrowPhasePairFactory<MobileMeshTrianglePairHandler> MobileMeshTriangle { get; }

        /// <summary>
        /// Gets the factory for the mobile mesh-static mesh case.
        /// </summary>
        public NarrowPhasePairFactory<MobileMeshStaticMeshPairHandler> MobileMeshStaticMesh { get; }

        /// <summary>
        /// Gets the factory for the mobile mesh-instanced mesh case.
        /// </summary>
        public NarrowPhasePairFactory<MobileMeshInstancedMeshPairHandler> MobileMeshInstancedMesh { get; }

        /// <summary>
        /// Gets the factory for the mobile mesh-terrain case.
        /// </summary>
        public NarrowPhasePairFactory<MobileMeshTerrainPairHandler> MobileMeshTerrain { get; }

        /// <summary>
        /// Gets the factory for the mobile mesh-mobile mesh case.
        /// </summary>
        public NarrowPhasePairFactory<MobileMeshMobileMeshPairHandler> MobileMeshMobileMesh { get; }

        /// <summary>
        /// Gets the factory for the static group-convex case.
        /// </summary>
        public NarrowPhasePairFactory<StaticGroupConvexPairHandler> StaticGroupConvex { get; }

        /// <summary>
        /// Gets the factory for the static group-compound case.
        /// </summary>
        public NarrowPhasePairFactory<StaticGroupCompoundPairHandler> StaticGroupCompound { get; }

        /// <summary>
        /// Gets the factory for the static group-mobile mesh case.
        /// </summary>
        public NarrowPhasePairFactory<StaticGroupMobileMeshPairHandler> StaticGroupMobileMesh { get; }

        /// <summary>
        /// Gets the factory for the detector volume-convex case.
        /// </summary>
        public NarrowPhasePairFactory<DetectorVolumeConvexPairHandler> DetectorVolumeConvex { get; }

        /// <summary>
        /// Gets the factory for the detector volume-mobile mesh case.
        /// </summary>
        public NarrowPhasePairFactory<DetectorVolumeMobileMeshPairHandler> DetectorVolumeMobileMesh { get; }

        /// <summary>
        /// Gets the factory for the detector volume-compound case.
        /// </summary>
        public NarrowPhasePairFactory<DetectorVolumeCompoundPairHandler> DetectorVolumeCompound { get; }

        /// <summary>
        /// Gets a collection of all the default factories.
        /// </summary>
        public ReadOnlyList<NarrowPhasePairFactory> All => new ReadOnlyList<NarrowPhasePairFactory>(factories);
    }
}