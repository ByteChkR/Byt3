using System.Drawing;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.IO;
using Byt3.Engine.Physics;
using Byt3.Engine.Physics.BEPUphysics.Entities;
using Byt3.Engine.Physics.BEPUphysics.Entities.Prefabs;
using Byt3.Engine.Rendering;
using Byt3.Engine.Tutorials.Tutorials.Components;
using OpenTK;

namespace Byt3.Engine.Tutorials.Tutorials
{
    public class RayCastingScene : AbstractScene
    {
        protected override void InitializeScene()
        {
            Add(DebugConsoleComponent.CreateConsole());
            BasicCamera bc =
                new BasicCamera(
                    Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(75), 16 / 9f, 0.1f, 1000f),
                    new Vector3(0, 5, 7)); //Creating a Basic Camera
            SetCamera(bc);
            Add(bc);
            bc.AddComponent(new MouseTrackerComponent());

            GameObject box = new GameObject(Vector3.UnitX * 3, "Box");
            LitMeshRendererComponent boxlmr = new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader,
                Prefabs.Cube,
                TextureLoader.ColorToTexture(Color.Red), 1);
            box.AddComponent(boxlmr);
            Add(box);

            GameObject box2 = new GameObject(Vector3.UnitX * -3, "Box");
            LitMeshRendererComponent box2lmr = new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader,
                Prefabs.Cube,
                TextureLoader.ColorToTexture(Color.Red), 1);
            box2.AddComponent(box2lmr);
            Add(box2);

            //Creating the Collider Shapes
            Entity boxShape = new Box(
                Physics.BEPUutilities.Vector3.Zero,
                2f,
                2f,
                2f);

            Entity box2Shape = new Box(
                Physics.BEPUutilities.Vector3.Zero,
                2f,
                2f,
                2f);


            //Creating A physics layer to be able to control which objects are meant to collide with each other
            int raycastLayerID = LayerManager.RegisterLayer("raycast", new Layer(1, 1));

            //Creating the Components for the Physics Engine
            //Note: There are different ways to get the LayerID than storing it.
            Collider boxCollider = new Collider(boxShape, raycastLayerID);
            Collider box2Collider = new Collider(box2Shape, LayerManager.LayerToName(raycastLayerID));


            //Adding the Components
            box.AddComponent(boxCollider);
            box2.AddComponent(box2Collider);
            //Making the Camera LookAt the origin
            bc.LookAt(Vector3.Zero);
        }
    }
}