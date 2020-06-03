using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.IO;
using Byt3.Engine.Rendering;
using Byt3.Engine.Tutorials.Tutorials.Components;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL.Common;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.Buffers.BufferCreators;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.Instructions.InstructionCreators;
using Byt3.OpenFL.Common.Parsing.StageResults;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Parsing;
using OpenTK;

namespace Byt3.Engine.Tutorials.Tutorials
{
    public class FLScene : AbstractScene
    {
        private readonly Texture tex = TextureLoader.ParameterToTexture(128, 128, "FLScene+TextureForFLProgram");
        private FLProgramCheckBuilder checkPipeline;
        private BufferCreator creator;
        private FLInstructionSet iset;
        private FLParser parser;

        protected override void InitializeScene()
        {
            creator = BufferCreator.CreateWithBuiltInTypes();
            iset = FLInstructionSet.CreateWithBuiltInTypes(CLAPI.MainThread, "assets/kernel/");
            checkPipeline = FLProgramCheckBuilder.CreateDefaultCheckBuilder(iset, creator);
            parser = new FLParser(iset, creator);
            checkPipeline.Attach(parser, true);

            Add(DebugConsoleComponent.CreateConsole());
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(75f), //Field of View Vertical
                16f / 9f, //Aspect Ratio
                0.1f, //Near Plane
                1000f); //Far Plane

            BasicCamera bc = new BasicCamera(proj, Vector3.Zero);
            Add(bc); //Adding the BasicCamera(That is a gameobject under the hood) to the scene to receive events
            SetCamera(bc); //Sets the Camera as the "active" camera that the scene will be rendered from.


            GameObject box = new GameObject(-Vector3.UnitZ * 4, "Box"); //Creating a new Empty GameObject
            LitMeshRendererComponent lmr = new LitMeshRendererComponent( //Creating a Renderer Component
                DefaultFilepaths.DefaultLitShader, //The OpenGL Shader used(Unlit and Lit shaders are provided)
                Prefabs.Cube, //The Mesh that is going to be used by the MeshRenderer
                tex, //Diffuse Texture to put on the mesh
                1); //Render Mask (UI = 1 << 30)
            box.AddComponent(lmr); //Attaching the Renderer to the GameObject
            box.AddComponent(new RotateSelfComponent()); //Adding a component that rotates the Object on the Y-Axis
            Add(box); //Adding the Object to the Scene.


            FLBuffer buffer =
                new FLBuffer(TextureLoader.TextureToMemoryBuffer(CLAPI.MainThread, tex, "BufferForFLProgram"), 128,
                    128, 1);


            FLProgram program = parser.Process(new FLParserInput("assets/filter/red.fl"))
                .Initialize(CLAPI.MainThread, iset);


            program.Run(buffer, true);

            FLBuffer result = program.GetActiveBuffer(false);
            byte[] dat = CLAPI.ReadBuffer<byte>(CLAPI.MainThread, result.Buffer, (int) result.Buffer.Size);
            //Create a texture from the output.
            TextureLoader.Update(tex, dat, 128, 128);
            result.Dispose();
        }
    }
}