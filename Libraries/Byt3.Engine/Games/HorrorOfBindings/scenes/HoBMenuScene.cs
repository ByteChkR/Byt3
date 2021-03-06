﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.IO;
using Byt3.Engine.Rendering;
using Byt3.Engine.UI;
using Byt3.Engine.UI.Animations;
using Byt3.Engine.UI.Animations.AnimationTypes;
using Byt3.Engine.UI.Animations.Interpolators;
using Byt3.Engine.UI.EventSystems;
using Byt3.OpenCL.Memory;
using Byt3.OpenCL.Wrapper;
using Byt3.OpenFL;
using Byt3.OpenFL.Common.DataObjects.ExecutableDataObjects;
using HorrorOfBindings.components;
using HorrorOfBindings.mapgenerator;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Color = System.Drawing.Color;

namespace HorrorOfBindings.scenes
{
    public class HoBMenuScene : AbstractScene
    {
        internal static Texture menubg;

        protected override void InitializeScene()
        {
            TextureGenerator.Initialize(true);


            BasicCamera mainCamera =
                new BasicCamera(
                    Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(75f),
                        GameEngine.Instance.Width / (float) GameEngine.Instance.Height, 0.01f, 1000f), Vector3.Zero);

            object mc = mainCamera;

            //EngineConfig.LoadConfig("assets/configs/camera_menu.xml", ref mc);
            Add(mainCamera);
            SetCamera(mainCamera);
            menubg = GenerateMenuBackground();
            UiImageRendererComponent bg =
                new UiImageRendererComponent(menubg.Copy(), false, 1, DefaultFilepaths.DefaultUiImageShader);

            GameObject bgobj = new GameObject("BG");
            bgobj.AddComponent(new BackgroundMover());
            bgobj.AddComponent(bg);
            Add(bgobj);
            //Positions are wrong(0.5 => 5) out of the screen because the correct positions are defined in CreateButtonAnimation.
            CreateButton("assets/textures/btn/btn", "Start Game", new Vector2(-5f, 0.5f), new Vector2(0.2f, 0.1f),
                CreateButtonAnimation(new Vector2(-0.5f, 0.5f), 0), btnStartGame);
            CreateButton("assets/textures/btn/btn", "Credits", new Vector2(-5f, 0.25f), new Vector2(0.2f, 0.1f),
                CreateButtonAnimation(new Vector2(-0.5f, 0.25f), 0.2f));
            CreateButton("assets/textures/btn/btn", "Exit", new Vector2(-5f, 0.0f), new Vector2(0.2f, 0.1f),
                CreateButtonAnimation(new Vector2(-0.5f, 0.0f), 0.4f), btnExit);
            DebugConsoleComponent c = DebugConsoleComponent.CreateConsole().GetComponent<DebugConsoleComponent>();
            Add(c.Owner);
        }

        private List<Animation> CreateButtonAnimation(Vector2 endPos, float delay)
        {
            LinearAnimation loadAnim = new LinearAnimation();
            loadAnim.Interpolator = new SmoothInterpolator();
            loadAnim.StartPos = new Vector2(endPos.X - 1, endPos.Y);
            loadAnim.EndPos = endPos;
            loadAnim.MaxAnimationTime = 1;
            loadAnim.Trigger = AnimationTrigger.OnLoad;
            loadAnim.AnimationDelay = delay;

            LinearAnimation clickAnim = new LinearAnimation();
            clickAnim.Interpolator = new Arc2Interpolator();
            clickAnim.StartPos = endPos;
            clickAnim.EndPos = endPos + Vector2.UnitY * 0.1f;
            clickAnim.MaxAnimationTime = 0.5f;
            clickAnim.Trigger = AnimationTrigger.OnEnter;
            return new List<Animation> {loadAnim, clickAnim};
        }


        private void btnStartGame(Button target)
        {
            GameEngine.Instance.InitializeScene<HoBGameScene>();
        }

        private void btnExit(Button target)
        {
            GameEngine.Instance.Exit();
        }


        private Texture GenerateMenuBackground()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int texWidth = 128;
            int texHeight = 128;
            FLRunner runner = new FLRunner(CLAPI.MainThread, "resources/kernel");
           FLProgram prog= runner.Run("assets/filter/game/menubg.fl", texWidth, texHeight);
            //Interpreter i = new Interpreter(Clapi.MainThread, "assets/filter/game/menubg.fl", DataTypes.Uchar1,
            //    Clapi.CreateEmpty<byte>(Clapi.MainThread, texWidth * texHeight * 4, MemoryFlag.ReadWrite), texWidth,
            //    texHeight, 1, 4, "assets/kernel/", true);

            //do
            //{
            //    i.Step();
            //} while (!i.Terminated);

            Texture tex = TextureLoader.ParameterToTexture(texWidth, texHeight, "MenugBG");
            TextureLoader.Update(CLAPI.MainThread,  tex, prog.GetActiveBuffer(false).Buffer);
            Logger.Log(DebugChannel.Log, "Time for Menu Background(ms): " + sw.ElapsedMilliseconds, 10);
            sw.Stop();
            prog.FreeResources();
            return tex;
        }


        private GameObject CreateButton(string buttonString, string Text, Vector2 Position, Vector2 Scale,
            List<Animation> animations, Action<Button> onClick = null, Action<Button> onEnter = null,
            Action<Button> onLeave = null, Action<Button> onHover = null)
        {
            GameObject container = new GameObject("BtnContainer");
            GameObject obj = new GameObject("Button");
            GameObject tObj = new GameObject("Text");
            Texture btnIdle = TextureLoader.FileToTexture(buttonString + ".png");
            Texture btnHover = TextureLoader.FileToTexture(buttonString + "H.png");
            Texture btnClick = TextureLoader.FileToTexture(buttonString + "C.png");
            Button btn = new Button(btnIdle, DefaultFilepaths.DefaultUiImageShader, 1, btnClick, btnHover, onClick,
                onEnter, onHover, onLeave);


            UiTextRendererComponent tr =
                new UiTextRendererComponent("Arial", false, 1, DefaultFilepaths.DefaultUiTextShader);
            obj.AddComponent(btn);
            tObj.AddComponent(tr);
            container.Add(obj);
            container.Add(tObj);
            Add(container);
            btn.Position = Position;
            btn.Scale = Scale;
            Vector2 textpos = Position;
            tr.Scale = Vector2.One * 2;
            tr.Center = true;
            tr.Position = textpos;
            tr.Text = Text;

            Animator anim = new Animator(animations, btn, tr);
            obj.AddComponent(anim);

            return obj;
        }
    }
}