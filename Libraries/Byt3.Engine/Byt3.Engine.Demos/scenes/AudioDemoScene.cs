using System;
using System.Drawing;
using Byt3.Engine.Audio;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.Demos.components;
using Byt3.Engine.IO;
using Byt3.Engine.Rendering;
using OpenTK;

namespace Byt3.Engine.Demos.scenes
{
    public class AudioDemoScene : AbstractScene
    {
        private LookAtComponent camLookCommandComponent;
        private GameObject sourceCube;



        protected override void InitializeScene()
        {


            Add(DebugConsoleComponent.CreateConsole());

            GameObject bgObj = new GameObject(Vector3.UnitY * -3, "BG");
            bgObj.Scale = new Vector3(25, 1, 25);
            bgObj.AddComponent(new MeshRendererComponent(DefaultFilepaths.DefaultUnlitShader, Prefabs.Cube,
                TextureLoader.ColorToTexture(Color.MediumPurple), 1));
            Add(bgObj);

            BasicCamera c = new BasicCamera(
                Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(75f),
                    GameEngine.Instance.Width / (float) GameEngine.Instance.Height, 0.01f, 1000f), Vector3.Zero);
            c.Translate(new Vector3(0, 4, 0));
            camLookCommandComponent = new LookAtComponent();

            c.AddComponent(camLookCommandComponent);

            sourceCube = new GameObject(Vector3.UnitZ * -5, "Audio Source");
            
            AudioSourceComponent source = new AudioSourceComponent();
            sourceCube.AddComponent(source);
            sourceCube.AddComponent(new RotateAroundComponent());
            sourceCube.AddComponent(new MeshRendererComponent(DefaultFilepaths.DefaultUnlitShader, Prefabs.Cube,
                DefaultFilepaths.DefaultTexture, 1));
            if (!AudioLoader.TryLoad("assets/sounds/test_mono_16.wav", out AudioFile clip))
            {
                Console.ReadLine();
            }

            source.Clip = clip;
            source.Looping = true;
            source.Play();
            Add(sourceCube);

            AudioListener listener = new AudioListener();
            c.AddComponent(listener);
            Add(c);
            SetCamera(c);
        }
    }
}