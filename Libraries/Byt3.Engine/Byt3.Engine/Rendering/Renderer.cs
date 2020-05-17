using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Byt3.Engine.Core;
using Byt3.Engine.Debug;
using Byt3.Utilities.Exceptions;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Byt3.Engine.Rendering
{
    /// <summary>
    /// Renderer that does all the drawing
    /// </summary>
    public class Renderer : IDisposable
    {
        /// <summary>
        /// Different Render Types
        /// </summary>
        public enum RenderType
        {
            Opaque,
            Transparent
        }

        /// <summary>
        /// List of Light Components in the Scene
        /// </summary>
        internal static List<LightComponent> Lights = new List<LightComponent>();

        private readonly RenderTargetMergeStage mergeStage;

        private readonly Dictionary<int, bool> renderListDirtyFlag = new Dictionary<int, bool>();

        private readonly Dictionary<int, List<RenderingComponent>> renderLists =
            new Dictionary<int, List<RenderingComponent>>();

        /// <summary>
        /// Default Render Target(Game World)
        /// </summary>
        private readonly RenderTarget rt;

        /// <summary>
        /// Default Render Target(UI)
        /// </summary>
        private readonly RenderTarget rt1;

        /// <summary>
        ///  A list of render targets
        /// </summary>
        private readonly List<RenderTarget> targets = new List<RenderTarget>();

        /// <summary>
        /// The Clear color of the two standard Render targets(World/UI)
        /// </summary>
        private Color clearColor = Color.FromArgb(0, 0, 0, 0);

        /// <summary>
        /// Internal Constructor
        /// </summary>
        internal Renderer()
        {
            mergeStage = new RenderTargetMergeStage();
            GameObject.AttachedRenderersChanged += OnRenderListChanged;

            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            rt = new RenderTarget(null, 1, clearColor) {MergeType = RenderTargetMergeType.Additive};
            AddRenderTarget(rt);
            rt1 = new RenderTarget(new ScreenCamera(), 1 << 30, clearColor)
            {
                MergeType = RenderTargetMergeType.Additive
            };
            AddRenderTarget(rt1);
        }

        /// <summary>
        /// Current target id
        /// </summary>
        private int CurrentTarget { get; set; }

        /// <summary>
        /// The Clear color of the two standard Render targets(World/UI)
        /// </summary>
        public Color ClearColor
        {
            set
            {
                clearColor = value;
                rt1.ClearColor = rt.ClearColor = clearColor;
            }
            get => clearColor;
        }

        public void Dispose()
        {
            rt.Dispose();
            rt1.Dispose();


            foreach (RenderTarget renderTarget in targets)
            {
                renderTarget.Dispose();
            }

            targets.Clear();
            renderListDirtyFlag.Clear();
            renderLists.Clear();

            mergeStage.Dispose();
        }

        private void OnRenderListChanged(RenderListChangeType type, RenderingComponent component)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if ((targets[i].PassMask & component.RenderQueue) != 0)
                {
                    renderListDirtyFlag[targets[i].PassMask] = true;
                }
            }
        }

        /// <summary>
        /// Adds a render target to the Render Target List
        /// </summary>
        /// <param name="target">The Target to Add</param>
        public void AddRenderTarget(RenderTarget target)
        {
            targets.Add(target);
            renderListDirtyFlag[target.PassMask] = true;
            renderLists[target.PassMask] = new List<RenderingComponent>();
            targets.Sort();
        }

        /// <summary>
        /// Removes a render target from the Render Target List
        /// </summary>
        /// <param name="target">The Target to Remove</param>
        public void RemoveRenderTarget(RenderTarget target)
        {
            for (int i = targets.Count - 1; i >= 0; i--)
            {
                RenderTarget renderTarget = targets[i];
                if (renderTarget.FrameBuffer == target.FrameBuffer)
                {
                    renderLists.Remove(renderTarget.PassMask);
                    renderListDirtyFlag.Remove(renderTarget.PassMask);
                    targets.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// Creates a Render Queue that is ordered and is only containing objects of the specified types
        /// </summary>
        /// <param name="renderTarget">The Render Target ID</param>
        /// <param name="view">The View Matrix of the Camera Associated with the Render Target</param>
        /// <param name="type">The Render Type</param>
        /// <returns>A sorted list of renderer contexts</returns>
        private List<RenderingComponent> CreateRenderQueue(int renderTarget, Matrix4 view)
        {
            if (!renderListDirtyFlag[renderTarget])
            {
                return renderLists[renderTarget];
            }

            renderListDirtyFlag[renderTarget] = false;
            if (!renderLists.ContainsKey(renderTarget))
            {
                renderLists[renderTarget] = new List<RenderingComponent>();
            }
            else
            {
                renderLists[renderTarget].Clear();
            }

            foreach (GameObject renderer in GameObject.ObjsWithAttachedRenderers)
            {
                RenderingComponent context = renderer.RenderingComponent;
                if ((renderer.RenderingComponent.RenderQueue & renderTarget) != 0)
                    //if (MaskHelper.IsContainedInMask(renderer.RenderingComponent.RenderQueue, renderTarget, false) &&
                    //    context.RenderType == type)
                {
                    context.PrecalculateMv(view);
                    renderLists[renderTarget].Add(context);
                }
            }

            renderLists[renderTarget].Sort();
            return renderLists[renderTarget];
        }

        /// <summary>
        /// Renders all targets and merges them into a single frame buffer
        /// </summary>
        /// <param name="scene">The Scene to be drawn</param>
        public void RenderAllTargets(AbstractScene scene)
        {
            scene.ComputeWorldTransformCache(Matrix4.Identity); //Compute all the World transforms and cache them


            MemoryTracer.AddSubStage("Render Target loop");
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].IsDisposed)
                {
                    throw new Byt3Exception("Use of Disposed Render Target");
                }

                MemoryTracer.NextStage("Rendering Render Target: " + i);
                CurrentTarget = i;
                RenderTarget target = targets[i];

                ICamera c = target.PassCamera ?? scene.Camera;

                if (c != null)
                {
                    GL.Viewport(target.ViewPort.X, target.ViewPort.Y, target.ViewPort.Width, target.ViewPort.Height);
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, target.FrameBuffer);

                    GL.ClearColor(target.ClearColor);

                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    Matrix4 vmat = c.ViewMatrix;

                    List<RenderingComponent> opaque = CreateRenderQueue(target.PassMask, vmat);
                    Render(opaque, vmat, c);
                    //List<RenderingComponent> transparent =
                    //    CreateRenderQueue(target.PassMask, vmat, RenderType.Transparent);
                    //Render(transparent, vmat, c);


                    GL.Viewport(0, 0, GameEngine.Instance.Width, GameEngine.Instance.Height);
                }
            }

            MemoryTracer.ReturnFromSubStage();


            mergeStage.MergeAndDisplayTargets(targets.Where(x => x.MergeType != RenderTargetMergeType.None).ToList());
        }

        /// <summary>
        /// Renders the Render Queue
        /// </summary>
        /// <param name="contexts">The Queue of Render Contexts</param>
        /// <param name="viewM">the Viewing Matrix</param>
        /// <param name="cam">The ICamera</param>
        public static void Render(List<RenderingComponent> contexts, Matrix4 viewM, ICamera cam)
        {
            for (int i = 0; i < contexts.Count; i++)
            {
                if (contexts[i].Owner != null)
                {
                    contexts[i].Render(viewM, cam.Projection);
                }
            }
        }
    }
}