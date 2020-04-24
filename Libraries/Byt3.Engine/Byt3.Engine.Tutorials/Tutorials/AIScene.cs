using Byt3.Engine.Core;
using Byt3.Engine.Debug;
using Byt3.Engine.Physics;
using Byt3.Engine.Rendering;
using Byt3.Engine.Tutorials.Tutorials.Components;
using OpenTK;
using MathHelper = OpenTK.MathHelper;
using Vector3 = OpenTK.Vector3;

namespace Byt3.Engine.Tutorials.Tutorials
{
    public class AIScene : AbstractScene
    {
        protected override void InitializeScene()
        {
            Add(DebugConsoleComponent.CreateConsole());

            LayerManager.RegisterLayer("raycast", new Layer(1, 1));

            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(75f),  //Field of View Vertical
                16f / 9f, //Aspect Ratio
                0.1f, //Near Plane
                1000f); //Far Plane

            BasicCamera bc = new BasicCamera(proj, Vector3.UnitY * 15);
            bc.Rotate(Vector3.UnitX, MathHelper.DegreesToRadians(-90));
            Add(bc); //Adding the BasicCamera(That is a gameobject under the hood) to the scene to receive events
            SetCamera(bc); //Sets the Camera as the "active" camera that the scene will be rendered from.
            bc.AddComponent(new AStarTestComponent()); //Adding the AStar Test Component to the Camera
        }




    }
}