using System;
using System.Collections.Generic;
using System.Linq;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.IO;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL;
using Byt3.OpenFL.Common.Buffers;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using Byt3.OpenFL.Common.ProgramChecks;
using Byt3.OpenFL.Common.ProgramChecks.Optimizations;
using Byt3.OpenFL.Threading;
using OpenTK;

namespace HorrorOfBindings.mapgenerator
{
    public static class TextureGenerator
    {
        private const int TEXTURE_RESOLUTION = 512;

        private static bool _initPerlin = false;
        private static Texture[] wallTextures;
        private static Texture[] wallSpecTextures;
        private static Texture playerSphereTexture;
        private static Texture playerSphereSpecTexture;
        private static FLScriptRunner runner;
        private static bool runnerInit;

        public static Action OnProcessFinished;

        private static void ProcessFinished()
        {
            OnProcessFinished?.Invoke();
        }

        public static void Initialize(bool multiThread)
        {
            if (runnerInit)
            {
                return;
            }

            runnerInit = true;
            if (multiThread)
            {
                runner = new FlMultiThreadScriptRunner(ProcessFinished);
            }
            else
            {
                runner = new FLScriptRunner(CLAPI.MainThread);
            }
            runner.AddProgramCheck(new ConvBRndCPU2GPUOptimization());
            runner.AddProgramCheck(new ConvIRndCPU2GPUOptimization());
            runner.AddProgramCheck(new RemoveUnusedFunctionsOptimization());
            runner.AddProgramCheck(new InlineJumpInstructionsOptimization());
            InitPerlin();
        }

        public static Texture GetTexture(int type)
        {
            if (!_initPerlin)
            {
                InitPerlin();
            }

            return wallTextures[type];
        }

        public static Texture GetSTexture(int type)
        {
            if (!_initPerlin)
            {
                InitPerlin();
            }

            return wallSpecTextures[type];
        }

        public static void Process()
        {
            runner.Process();
        }

        public static void Reset()
        {
            _initPerlin = false;
            playerSphereTexture.Dispose();
            playerSphereSpecTexture.Dispose();
            for (int i = 0; i < wallTextures.Length; i++)
            {
                wallTextures[i].Dispose();
            }

            for (int i = 0; i < wallSpecTextures.Length; i++)
            {
                wallSpecTextures[i].Dispose();
            }
        }

        public static Texture GetPlayerTexture()
        {
            if (!_initPerlin)
            {
                InitPerlin();
            }

            return playerSphereTexture;
        }

        private static void InitPerlin()
        {
            _initPerlin = true;

            //Texture tex = TextureLoader.ParameterToTexture(512, 512);

            //FlExecutionContext exec =new FlExecutionContext("assets/filter/game/perlin.fl", tex, new Dictionary<string, Texture>(), null);
            //runner.Enqueue(exec);

            playerSphereTexture = TextureLoader.ParameterToTexture(TEXTURE_RESOLUTION, TEXTURE_RESOLUTION, "PlayerSphere");
            playerSphereSpecTexture = TextureLoader.ParameterToTexture(TEXTURE_RESOLUTION, TEXTURE_RESOLUTION, "PlayerSphereSpec");
            CreatePlayerTexture(playerSphereTexture, playerSphereSpecTexture);


            wallTextures = new Texture[2];
            wallSpecTextures = new Texture[2];
            for (int i = 0; i < 2; i++)
            {
                wallTextures[i] = TextureLoader.ParameterToTexture(TEXTURE_RESOLUTION, TEXTURE_RESOLUTION, "WallTex_" + i);
                wallSpecTextures[i] = TextureLoader.ParameterToTexture(TEXTURE_RESOLUTION, TEXTURE_RESOLUTION, "WallTex_Spec_" + i);
                CreateWallTexture(wallTextures[i], wallSpecTextures[i], i);
            }
        }

        public static void CreateGroundTexture(Texture destTexture, Texture specTexture)
        {
            runner.Enqueue(GetExecutionContext("assets/filter/game/cobble_grass.fl", destTexture, specTexture));
        }

        public static void CreateWallTexture(Texture destTexture, Texture specTexture, int i)
        {
            runner.Enqueue(GetExecutionContext($"assets/filter/game/wall{i}.fl", destTexture, specTexture));
        }

        public static void CreatePlayerTexture(Texture destTexture, Texture specTexture)
        {
            runner.Enqueue(GetExecutionContext($"assets/filter/game/tennisball.fl", destTexture, specTexture));
        }

        public static void CreateBoundsTexture(Texture destTexture, Texture specTexture)
        {
            runner.Enqueue(GetExecutionContext($"assets/filter/game/concrete.fl", destTexture, specTexture));
        }

        private static FlScriptExecutionContext GetExecutionContext(string file, Texture dest, Texture specular)
        {
            //Dictionary<string, Texture> otherTex = new Dictionary<string, Texture>()
            //    {{"result", dest}, {"specularOut", specular}};
            return new FlScriptExecutionContext(file, TextureLoader.TextureToByteArray(dest), (int)dest.Width, (int)dest.Height, program => Apply(dest, specular, program));
        }

        private static void Apply(Texture dest, Texture spec, FLProgram prog)
        {
            GameWindow gw = new GameWindow(1,1);
            gw.MakeCurrent();
            FLBuffer buf = prog.GetActiveBuffer(false);
            TextureLoader.Update(prog.Instance, dest, buf.Buffer);
            if (prog.BufferNames.Contains("specularOut"))
                TextureLoader.Update(prog.Instance, spec, prog.GetBufferWithName("specularOut", false).Buffer);

            prog.FreeResources();
            gw.Dispose();
            //GameEngine.Instance.MakeCurrent();
        }
    }
}