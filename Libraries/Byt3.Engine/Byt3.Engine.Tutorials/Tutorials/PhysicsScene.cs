using System.Drawing;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.IO;
using Byt3.Engine.Physics;
using Byt3.Engine.Physics.BEPUphysics.Entities;
using Byt3.Engine.Physics.BEPUphysics.Entities.Prefabs;
using Byt3.Engine.Physics.BEPUphysics.Materials;
using Byt3.Engine.Rendering;
using Byt3.Engine.Tutorials.Tutorials.Components;
using OpenTK;
using MathHelper = OpenTK.MathHelper;
using Vector3 = OpenTK.Vector3;

namespace Byt3.Engine.Tutorials.Tutorials
{
    public class PhysicsScene : AbstractScene
    {
        protected override void InitializeScene()
        {
            Add(DebugConsoleComponent.CreateConsole());
            BasicCamera bc =
                new BasicCamera(
                    Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(75), 16 / 9f, 0.1f, 1000f),
                    new Vector3(0, 5, 30)); //Creating a Basic Camera
            SetCamera(bc);
            Add(bc);

            //Creating a Box that is meant to fall down on the kinetic box
            GameObject box = new GameObject(Vector3.UnitZ * -3 + Vector3.UnitY * 2, "Box");
            LitMeshRendererComponent lmr = new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader, Prefabs.Cube,
                TextureLoader.ColorToTexture(Color.Red), 1);
            box.AddComponent(lmr);
            Add(box);

            //Creating a Kinetic Box that will rotate slowly
            GameObject kinetic = new GameObject(Vector3.UnitZ * -3 + Vector3.UnitY * -2, "Box");
            LitMeshRendererComponent kineticlmr = new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader,
                Prefabs.Cube,
                TextureLoader.ColorToTexture(Color.Green), 1);
            kinetic.AddComponent(kineticlmr);
            kinetic.AddComponent(new RotateKineticBodyComponent());
            Add(kinetic);

            //A Large sphere that will act as a ground
            GameObject ground = new GameObject(Vector3.UnitZ * -3 + Vector3.UnitY * -1, "Box");
            LitMeshRendererComponent groundlmr = new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader,
                Prefabs.Sphere,
                TextureLoader.ColorToTexture(Color.Blue), 1);
            ground.AddComponent(groundlmr);
            Add(ground);
            ground.Scale = new Vector3(20, 20, 20);
            ground.LocalPosition = Physics.BEPUutilities.Vector3.UnitY * -25;

            //Creating the Collider Shapes
            Entity boxShape = new Box(
                Physics.BEPUutilities.Vector3.Zero,
                2f,
                2f,
                2f,
                1f);

            Entity kineticShape = new Box(
                Physics.BEPUutilities.Vector3.Zero,
                2f,
                2f,
                2f,
                1f);

            Entity groundShape = new Sphere(
                Vector3.Zero,
                20f);
            //Note: Not specifying the mass when creating makes the shape a static shape that is really cheap computatinally

            //Ground(Sphere) and the falling box is going to have 0 friction and maximum bounciness.
            Material groundPhysicsMaterial = new Material(0, 0, 1f);
            Material boxPhysicsMaterial = new Material(0, 0, 1f);

            //Creating A physics layer to be able to control which objects are meant to collide with each other
            int physicsLayerID = LayerManager.RegisterLayer("physics", new Layer(1, 1));

            //Creating the Components for the Physics Engine
            //Note: There are different ways to get the LayerID than storing it.
            Collider boxCollider = new Collider(boxShape, physicsLayerID);
            Collider groundCollider = new Collider(groundShape, "physics");
            Collider kineticCollider = new Collider(kineticShape, LayerManager.LayerToName(physicsLayerID));

            //Final Collider Setup
            //Kinetic becomes Kinetic
            kineticCollider.PhysicsCollider.BecomeKinematic();
            //Adding the Physics Materials
            boxCollider.PhysicsCollider.Material = boxPhysicsMaterial;
            groundCollider.PhysicsCollider.Material = groundPhysicsMaterial;

            //Adding the Components
            box.AddComponent(boxCollider);
            kinetic.AddComponent(kineticCollider);
            ground.AddComponent(groundCollider);

            //Making the Camera LookAt the origin
            bc.LookAt(Vector3.Zero);
        }
    }
}