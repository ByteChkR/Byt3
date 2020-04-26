using System.Drawing;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.Demos.components;
using Byt3.Engine.IO;
using Byt3.Engine.Physics;
using Byt3.Engine.Physics.BEPUphysics.Entities.Prefabs;
using Byt3.Engine.Rendering;
using OpenTK;

namespace Byt3.Engine.Demos.scenes
{
    public class PhysicsDemoScene : AbstractScene
    {
        protected override void InitializeScene()
        {
            int rayLayer = LayerManager.RegisterLayer("raycast", new Layer(1, 2));
            int hybLayer = LayerManager.RegisterLayer("hybrid", new Layer(1, 1 | 2));
            int physicsLayer = LayerManager.RegisterLayer("physics", new Layer(1, 1));
            LayerManager.DisableCollisions(rayLayer, physicsLayer);

            PhysicsDemoComponent phys = new PhysicsDemoComponent();

            AddComponent(phys); //Adding Physics Component to world.


            Add(DebugConsoleComponent.CreateConsole());

            GameObject bgObj = new GameObject(Vector3.UnitY * -3, "BG");
            bgObj.Scale = new Vector3(250, 1, 250);
            bgObj.AddComponent(new MeshRendererComponent(DefaultFilepaths.DefaultUnlitShader, Prefabs.Cube,
                TextureLoader.ColorToTexture(Color.Brown), 1));
            Collider groundCol = new Collider(new Box(Vector3.Zero, 500, 1, 500), hybLayer);
            bgObj.AddComponent(groundCol);
            Add(bgObj);

            GameObject boxO = new GameObject(Vector3.UnitY * 3, "Box");
            boxO.AddComponent(new MeshRendererComponent(DefaultFilepaths.DefaultUnlitShader, Prefabs.Cube,
                TextureLoader.ColorToTexture(Color.DarkMagenta), 1));
            boxO.AddComponent(new Collider(new Box(Vector3.Zero, 1, 1, 1), physicsLayer));
            boxO.Translate(new Vector3(55, 0, 35));
            Add(boxO);


            GameObject mouseTarget = new GameObject(Vector3.UnitY * -3, "BG");
            mouseTarget.Scale = new Vector3(1, 1, 1);
            mouseTarget.AddComponent(new MeshRendererComponent(DefaultFilepaths.DefaultUnlitShader, Prefabs.Sphere,
                TextureLoader.ColorToTexture(Color.GreenYellow), 1));

            Add(mouseTarget);


            BasicCamera c = new BasicCamera(
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(75f),
                    GameEngine.Instance.Width / (float) GameEngine.Instance.Height, 0.01f, 1000f), Vector3.Zero);
            c.Rotate(new Vector3(1, 0, 0), MathHelper.DegreesToRadians(-25));
            c.Translate(new Vector3(1, 30, 45));
            c.AddComponent(new CameraRaycaster(mouseTarget,  boxO));
            GameEngine.Instance.CurrentScene.Add(c);
            GameEngine.Instance.CurrentScene.SetCamera(c);
        }
    }
}