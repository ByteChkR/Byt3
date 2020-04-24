using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.IO;
using Byt3.Engine.Rendering;
using Byt3.Engine.Tutorials.Tutorials.Components;
using OpenTK;

namespace Byt3.Engine.Tutorials.Tutorials
{
    public class FLRunnerScene : AbstractScene
    {
        protected override void InitializeScene()
        {
            Add(DebugConsoleComponent.CreateConsole());
            Texture tex = TextureLoader.ParameterToTexture(128, 128, "FLRunnerScene+TexForFLRunner"); //Generating 2 Textures for our 2 boxes
            Texture tex2 = TextureLoader.ParameterToTexture(128, 128,"FLRunnerScene+Tex2ForFLRunner");

            //Adding Component that will do the FLRunner tasks
            AddComponent(new KeyTriggerComponent(tex, tex2));

            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(75f),  //Field of View Vertical
                16f / 9f, //Aspect Ratio
                0.1f, //Near Plane
                1000f); //Far Plane

            BasicCamera bc = new BasicCamera(proj, Vector3.Zero);
            Add(bc); //Adding the BasicCamera(That is a gameobject under the hood) to the scene to receive events
            SetCamera(bc); //Sets the Camera as the "active" camera that the scene will be rendered from.

            GameObject box = new GameObject(-Vector3.UnitZ * 4 + Vector3.UnitX, "Box"); //Creating a new Empty GameObject
            LitMeshRendererComponent lmr = new LitMeshRendererComponent( //Creating a Renderer Component
                DefaultFilepaths.DefaultLitShader, //The OpenGL Shader used(Unlit and Lit shaders are provided)
                Prefabs.Cube, //The Mesh that is going to be used by the MeshRenderer
                tex, //Diffuse Texture to put on the mesh
                1); //Render Mask (UI = 1 << 30)
            box.AddComponent(lmr); //Attaching the Renderer to the GameObject
            box.AddComponent(new RotateSelfComponent()); //Adding a component that rotates the Object on the Y-Axis
            Add(box); //Adding the Object to the Scene.

            GameObject box2 = new GameObject(-Vector3.UnitZ * 4 - Vector3.UnitX, "Box2"); //Creating a new Empty GameObject
            LitMeshRendererComponent lmr2 = new LitMeshRendererComponent( //Creating a Renderer Component
                DefaultFilepaths.DefaultLitShader, //The OpenGL Shader used(Unlit and Lit shaders are provided)
                Prefabs.Cube, //The Mesh that is going to be used by the MeshRenderer
                tex2, //Diffuse Texture to put on the mesh
                1); //Render Mask (UI = 1 << 30)
            box2.AddComponent(lmr2); //Attaching the Renderer to the GameObject
            box2.AddComponent(new RotateSelfComponent()); //Adding a component that rotates the Object on the Y-Axis
            Add(box2); //Adding the Object to the Scene.
        }
    }
}