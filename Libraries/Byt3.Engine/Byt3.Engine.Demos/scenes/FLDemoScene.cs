using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.Demos.components;
using Byt3.Engine.IO;
using Byt3.Engine.OpenFL;
using Byt3.Engine.Rendering;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using Byt3.OpenFL.Parsing.Stages;
using OpenTK;

namespace Byt3.Engine.Demos.scenes
{
    public class FlDemoScene : AbstractScene
    {
        private RenderTarget splitCam;

        private FLInstructionSet iset;
        private BufferCreator creator;
        private FLProgramCheckBuilder checkPipeline;
        private FLParser parser;


        private Texture GenerateGroundTexture()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int texWidth = 1024;
            int texHeight = 1024;

            Texture tex = TextureLoader.ParameterToTexture(texWidth, texHeight, "GroundTextureCL");

            FLBuffer buffer =
                new FLBuffer(TextureLoader.TextureToMemoryBuffer(CLAPI.MainThread, tex, "BufferForFLProgram"), 128,
                    128);


            FLProgram program = parser.Process(new FLParserInput("assets/filter/examples/grass.fl")).Initialize(CLAPI.MainThread, iset);


            program.Run(buffer, true);

            FLBuffer result = program.GetActiveBuffer(false);
            byte[] dat = CLAPI.ReadBuffer<byte>(CLAPI.MainThread, result.Buffer, (int) result.Buffer.Size);
            //Create a texture from the output.
            TextureLoader.Update(tex, dat, 1024, 1024);


            Logger.Log(DebugChannel.Log, "Time for Ground Texture(ms): " + sw.ElapsedMilliseconds, 10);
            sw.Stop();
            return tex;
        }

        protected override void InitializeScene()
        {
            creator = BufferCreator.CreateWithBuiltInTypes();
            iset = FLInstructionSet.CreateWithBuiltInTypes(CLAPI.MainThread, "assets/kernel/");
            checkPipeline = FLProgramCheckBuilder.CreateDefaultCheckBuilder(iset, creator);
            parser = new FLParser(iset, creator);
            checkPipeline.Attach(parser, true);
            Mesh plane = MeshLoader.FileToMesh("assets/models/plane.obj");

            Texture texQuad = TextureLoader.ParameterToTexture(1024, 1024, "FLDisplayTextureQuad");
            Texture texSphere = TextureLoader.ParameterToTexture(1024, 1024, "FLDisplayTextureSphere");

            GameObject objSphere = new GameObject(new Vector3(1, 1, 0), "SphereDisplay");

            LitMeshRendererComponent sphereLmr = new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader,
                Prefabs.Sphere,
                texSphere, 1);
            objSphere.AddComponent(sphereLmr);
            sphereLmr.Textures = new[]
                {sphereLmr.Textures[0], DefaultFilepaths.DefaultTexture};

            objSphere.AddComponent(new RotatingComponent());

            GameObject objQuad = new GameObject(new Vector3(-1, 1, 0), "QuadDisplay");
            LitMeshRendererComponent quadLmr = new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader, plane,
                texQuad, 1);
            objQuad.AddComponent(quadLmr);
            quadLmr.Textures = new[]
                {quadLmr.Textures[0], DefaultFilepaths.DefaultTexture};

            objQuad.Rotate(new Vector3(1, 0, 0), MathHelper.DegreesToRadians(45));

            GameObject sourceCube = new GameObject(new Vector3(0, 10, 10), "Light Source");

            sourceCube.AddComponent(new LightComponent());
            sourceCube.AddComponent(new RotateAroundComponent {Slow = 0.15f});
            sourceCube.AddComponent(new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader, Prefabs.Cube,
                TextureLoader.ColorToTexture(Color.White), 1));

            GameObject uiText = new GameObject(new Vector3(0), "UIText");
            uiText.AddComponent(new FlGeneratorComponent(new List<LitMeshRendererComponent>
                {
                    sphereLmr, quadLmr
                },
                512,
                512, true));

            Add(sourceCube);
            Add(uiText);
            Add(DebugConsoleComponent.CreateConsole());
            Add(objSphere);
            Add(objQuad);

            GameObject bgObj = new GameObject(Vector3.UnitY * -3, "BG") {Scale = new Vector3(25, 1, 25)};


            bgObj.AddComponent(new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader, Prefabs.Cube,
                GenerateGroundTexture(), 1));
            Add(bgObj);


            BasicCamera mainCamera =
                new BasicCamera(
                    Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(75f),
                        GameEngine.Instance.Width / (float) GameEngine.Instance.Height, 0.01f, 1000f), Vector3.Zero);

            object mc = mainCamera;

            EngineConfig.LoadConfig("assets/configs/camera_fldemo.xml", ref mc);


            Add(mainCamera);
            SetCamera(mainCamera);

            GameObject camContainer = new GameObject("CamContainer");

            BasicCamera inPicCam =
                new BasicCamera(
                    Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(75f),
                        GameEngine.Instance.Width / (float) GameEngine.Instance.Height, 0.01f, 1000f), Vector3.Zero);
            inPicCam.Rotate(new Vector3(1, 0, 0), MathHelper.DegreesToRadians(0));
            inPicCam.Translate(new Vector3(0, 2, 4));
            inPicCam.AddComponent(new RotateAroundComponent());
            GameObject zeroPoint = new GameObject("Zero");
            Add(zeroPoint);
            LookAtComponent comp = new LookAtComponent();
            comp.SetTarget(zeroPoint);
            inPicCam.AddComponent(comp);
            Add(inPicCam);


            splitCam = new RenderTarget(inPicCam, 1, Color.FromArgb(0, 0, 0, 0))
            {
                MergeType = RenderTargetMergeType.Additive,
                ViewPort = new Rectangle(0, 0, (int) (GameEngine.Instance.Width * 0.3f),
                    (int) (GameEngine.Instance.Height * 0.3f))
            };

            Add(camContainer);
            GameEngine.Instance.AddRenderTarget(splitCam);
        }

        public override void OnDestroy()
        {
            GameEngine.Instance.RemoveRenderTarget(splitCam);
            splitCam.Dispose();
        }
    }
}