using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using Byt3.ADL;
using Byt3.Engine.Audio;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.IO;
using Byt3.Engine.Physics;
using Byt3.Engine.Rendering;
using Byt3.Engine.UI.EventSystems;
using Byt3.OpenCL;
using Byt3.Utilities.ManifestIO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace Byt3.Engine.Core
{
    /// <summary>
    /// Central class that is the "heart" of the Engine
    /// </summary>
    public class GameEngine : ALoggable<DebugChannel>, IDisposable
    {
        /// <summary>
        /// Private flag if the there is a scene change in progress
        /// </summary>
        private bool changeScene;

        private bool exiting;

        /// <summary>
        /// The Next scene to be initialized
        /// </summary>
        private Type nextScene;

        /// <summary>
        /// An internal update frame counter
        /// </summary>
        private int frameCounter;

        /// <summary>
        /// Renderer Instance
        /// </summary>
        protected Renderer Renderer;

        /// <summary>
        /// An internal render frame counter
        /// </summary>
        private int renderFrameCounter;

        /// <summary>
        /// The Window used to render
        /// </summary>
        private GameWindow window;

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="settings">Settings to be used</param>
        public GameEngine(EngineSettings settings): base(EngineDebugConfig.Settings)
        {
            if (settings != null)
            {
                SetSettings(settings);
            }

            ManifestReader.RegisterAssembly(Assembly.GetExecutingAssembly());
        }
        /// <summary>
        /// Is true when the Game Window has the Focus from the OS(e.g is in the foreground)
        /// </summary>
        public bool HasFocus => window.Focused;

        public bool ShowMouseCursor
        {
            get => window.CursorVisible;
            set => window.CursorVisible = value;
        }

        /// <summary>
        /// The Window Position
        /// </summary>
        public Vector2 WindowPosition
        {
            get
            {
                return new Vector2(window.Location.X, window.Location.Y);
            }
        }

        /// <summary>
        /// The Event System used by the Ui Systems
        /// </summary>
        public EventSystem UiSystem { get; private set; }

        /// <summary>
        /// The Settings the engine has been started with
        /// </summary>
        public EngineSettings Settings { get; private set; }

        /// <summary>
        /// Static Singleton Instance
        /// </summary>
        public static GameEngine Instance { get; private set; }

        /// <summary>
        /// The current scene
        /// </summary>
        public AbstractScene CurrentScene { get; private set; }

        /// <summary>
        /// Window Width
        /// </summary>
        public int Width => window.Width;

        /// <summary>
        /// Window Height
        /// </summary>
        public int Height => window.Height;

        /// <summary>
        /// The Window Size of the Game Window
        /// </summary>
        public Vector2 WindowSize => new Vector2(Width, Height);

        /// <summary>
        /// Returns the Window Info associated with the Game Window
        /// </summary>
        public IWindowInfo WindowInfo => window.WindowInfo;

        /// <summary>
        /// Property that returns the current AspectRatio
        /// </summary>
        public float AspectRatio => Width / (float)Height;


        /// <summary>
        /// Mouse Position in pixels
        /// </summary>
        public Vector2 MousePosition { get; private set; }

        public Vector2 MouseDelta { get; private set; }

        /// <summary>
        /// Sets the Game Window Context as active for the Calling thread.
        /// </summary>
        public void MakeCurrent()
        {
            window.MakeCurrent();
        }

        /// <summary>
        /// Applies the Engine Settings (only working when window is not started yet. Undefined behaviour when done so
        /// </summary>
        /// <param name="settings">The settings to be applied</param>
        public void SetSettings(EngineSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Initializes the Game Systems
        /// </summary>
        public void Initialize()
        {
            Instance = this;
            Logger.Log(DebugChannel.Log | DebugChannel.EngineCore, "Init started..", 10);
            InitializeWindow();
            InitializeRenderer();

            AudioManager.Initialize();

            PhysicsEngine.Initialize();
            UiSystem = new EventSystem();
        }


        /// <summary>
        /// Initializes the OpenGL Window and registers some handles
        /// </summary>
        private void InitializeWindow()
        {
            Logger.Log(DebugChannel.Log | DebugChannel.EngineCore, "Initializing Window..",  10);
            Logger.Log(DebugChannel.Log | DebugChannel.EngineCore,
                $"Width: {Settings.InitWidth} Height: {Settings.InitHeight}, Title: {Settings.Title}, FSAA Samples: {Settings.Mode.Samples}, Physics Threads: {Settings.PhysicsThreadCount}",
                9);

            StaticInit();
            WindowUnInit();
            WindowInit();
        }

        private void WindowInit()
        {
            window = new GameWindow(Settings.InitWidth, Settings.InitHeight, Settings.Mode, Settings.Title,
                Settings.WindowFlags);

            #region WindowHandles

            Input.Initialize(window);
            window.UpdateFrame += Update;
            window.Resize += OnResize;
            window.KeyDown += GameObject._KeyDown;
            window.KeyUp += GameObject._KeyUp;
            window.KeyPress += GameObject._KeyPress;
            window.Closing += WindowOnClosing;
            #endregion
        }

        private void WindowUnInit()
        {
            if (window != null)
            {
                window.UpdateFrame -= Update;
                window.Resize -= OnResize;
                window.KeyDown -= GameObject._KeyDown;
                window.KeyUp -= GameObject._KeyUp;
                window.KeyPress -= GameObject._KeyPress;
                window.Closing -= WindowOnClosing;
                window.RenderFrame -= OnRender;
                window.MouseMove -= Window_MouseMove;
                Input.Dispose();
                window.Dispose();
            }
        }

        private void WindowOnClosing(object sender, CancelEventArgs e)
        {
            RemoveSceneObjects();
            Dispose();
            Instance = null;
        }


        private bool staticInitialized;
        private void StaticInit()
        {
            if (staticInitialized) return;
            staticInitialized = true;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
        }

        /// <summary>
        /// Closes the Game Window
        /// </summary>
        public void Exit()
        {
            exiting = true;
        }

        private void RemoveSceneObjects()
        {
            CurrentScene?._Destroy();
            CurrentScene?.DestroyScene(); //Call on destroy on the scene itself.
            CurrentScene?.RemoveDestroyedObjects();
            CurrentScene = null;
        }

        private void InternalExit()
        {
            RemoveSceneObjects();

            window.Exit();
        }

        public void Dispose()
        {
            Prefabs.DisposeObjects();
            ShaderProgram.ResetLastUsedProgram();
            DefaultFilepaths.DisposeObjects();
            Renderer.Dispose();

            ManifestReader.ClearUnpackedFiles();

            WindowUnInit();


            HandleBase.DisposeAllHandles();
            EngineStatisticsManager.DisposeAllHandles();
        }


        private void CurrentDomainOnProcessExit(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Initializes the renderer
        /// </summary>
        private void InitializeRenderer()
        {
            //TODO

            Logger.Log(DebugChannel.Log | DebugChannel.EngineCore, "Initializing Renderer..", 10);
            Renderer = new Renderer();
            window.RenderFrame += OnRender;
            window.MouseMove += Window_MouseMove;
        }

        /// <summary>
        /// Wrapper to update the mouse position property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            MouseDelta = new Vector2(e.XDelta, e.YDelta);
            MousePosition = new Vector2(e.X, e.Y);
        }

        /// <summary>
        /// Function used to Load a new scene
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void InitializeScene<T>() where T : AbstractScene
        {
            InitializeScene(typeof(T));
        }

        /// <summary>
        /// Loads a new Scene
        /// </summary>
        /// <param name="sceneType">The Type of Scene</param>
        public void InitializeScene(Type sceneType)
        {
            if (!typeof(AbstractScene).IsAssignableFrom(sceneType))
            {
                return;
            }

            changeScene = true;
            nextScene = sceneType;
            Logger.Log(DebugChannel.Log | DebugChannel.EngineCore, "Initializing Scene..", 9);
        }

        /// <summary>
        /// Adds a Render Target to the Renderer
        /// </summary>
        /// <param name="target">Target to add</param>
        public void AddRenderTarget(RenderTarget target)
        {
            Renderer.AddRenderTarget(target);
        }

        /// <summary>
        /// Removes a Render Target from the Renderer
        /// </summary>
        /// <param name="target">Target to remove</param>
        public void RemoveRenderTarget(RenderTarget target)
        {
            Renderer.RemoveRenderTarget(target);
        }

        /// <summary>
        /// Runs the Engine
        /// </summary>
        public void Run()
        {
            Logger.Log(DebugChannel.Log | DebugChannel.EngineCore, "Running GameEngine Loop..",  10);
            window.VSync = VSyncMode.Off;
            exiting = false;
            window.Run(0, 0);
        }

        private AbstractComponent[] debugComponents= new AbstractComponent[0];
        public void SetDebugComponents(AbstractComponent[] components)
        {
            debugComponents = components;
        }

        /// <summary>
        /// Converts the Current Screen position to world space coordinates
        /// </summary>
        /// <param name="x">x in pixels</param>
        /// <param name="y">y in pixels</param>
        /// <returns></returns>
        public Vector3 ConvertScreenToWorldCoords(int x, int y)
        {
            Vector2 mouse;
            mouse.X = x;
            mouse.Y = y;
            Matrix4 proj = CurrentScene.Camera.Projection;
            Vector4 vector = UnProject(ref proj, CurrentScene.Camera.ViewMatrix, new Size(Width, Height), mouse);
            Vector3 coords = new Vector3(vector);
            return coords;
        }

        /// <summary>
        /// Unprojects a 2D vector with the specified Projection matrix
        /// </summary>
        /// <param name="projection">Projection to be unprojected</param>
        /// <param name="view">View</param>
        /// <param name="viewport">Viewport(Width/Height of the screen)</param>
        /// <param name="mouse">The mouse position in pixels</param>
        /// <returns></returns>
        private static Vector4 UnProject(ref Matrix4 projection, Matrix4 view, Size viewport, Vector2 mouse)
        {
            Vector4 vec;

            vec.X = 2.0f * mouse.X / viewport.Width - 1;
            vec.Y = -2.0f * mouse.Y / viewport.Height + 1;
            vec.Z = 0;
            vec.W = 1.0f;

            Matrix4 viewInv = Matrix4.Invert(view);
            Matrix4 projInv = Matrix4.Invert(projection);

            Vector4.Transform(ref vec, ref projInv, out vec);
            Vector4.Transform(ref vec, ref viewInv, out vec);

            if (vec.W > float.Epsilon || vec.W < float.Epsilon)
            {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }

            return vec;
        }

        /// <summary>
        /// Gets called from OpenTK whenever it is time for an update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Update(object sender, FrameEventArgs e)
        {
            if (exiting) return;

            frameCounter++;

            MemoryTracer.NextStage("Update Frame: " + frameCounter);


            UiSystem.Update();

            MemoryTracer.AddSubStage("Scene Update");
            CurrentScene?.Update((float)e.Time);

            if (exiting)
            {
                InternalExit();
                return;
            }


            MemoryTracer.NextStage("Physics Update");
            PhysicsEngine.Update((float)e.Time);


            EngineStatisticsManager.Update((float)e.Time);

            MemoryTracer.NextStage("ThreadManager Update");
            ThreadManager.CheckManagerStates();

            MouseDelta = Vector2.Zero;

            if (changeScene)
            {
                MemoryTracer.NextStage("Scene Intialization");
                changeScene = false;


                MemoryTracer.AddSubStage("Removing Old Scene");

                CurrentScene?._Destroy();

                CurrentScene?.DestroyScene(); //Call on destroy on the scene itself.

                MemoryTracer.NextStage("Removing World");

                CurrentScene?.RemoveDestroyedObjects();


                MemoryTracer.NextStage("Create New Scene");

                CurrentScene = (AbstractScene)Activator.CreateInstance(nextScene);

                MemoryTracer.NextStage("Initialize New Scene");

                CurrentScene._initializeScene();

                foreach (AbstractComponent abstractComponent in debugComponents)
                {
                    CurrentScene.AddComponent(abstractComponent);
                }

                MemoryTracer.ReturnFromSubStage();
            }


            //Cleanup
            MemoryTracer.NextStage("Clean up Destroyed Objects");
            CurrentScene?.RemoveDestroyedObjects();
            MemoryTracer.ReturnFromSubStage(); //Returning to root.
        }

        /// <summary>
        /// Event handler that changes the viewport of the Engine when it gets resitzed
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void OnResize(object o, EventArgs e)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
        }

        /// <summary>
        /// Gets called by opentk when it is time for a render update
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void OnRender(object o, FrameEventArgs e)
        {
            renderFrameCounter++;

            MemoryTracer.NextStage("Render Frame: " + renderFrameCounter);

            MemoryTracer.AddSubStage("Rendering Render Targets");

            Renderer.RenderAllTargets(CurrentScene);

            MemoryTracer.NextStage("Swapping Window Buffers");

            window.SwapBuffers();

            EngineStatisticsManager.Render((float)e.Time);

            MemoryTracer.ReturnFromSubStage();
        }
    }
}