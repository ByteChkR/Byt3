using System;
using System.Collections.Generic;
using System.Drawing;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Utilities.Exceptions;
using OpenTK.Graphics.OpenGL;

namespace Byt3.Engine.Rendering
{
    /// <summary>
    /// Implements the Merging of Framebuffers
    /// </summary>
    public class RenderTargetMergeStage : IDisposable
    {
        private bool isDisposed;
        public void Dispose()
        {
            isDisposed = true;
            GL.DeleteVertexArray(_screenVao);
            _screenTarget0?.Dispose();
            _screenTarget1?.Dispose();
            _screenTarget0 = _screenTarget1 = null;
            foreach (KeyValuePair<RenderTargetMergeType, ShaderProgram> shaderProgram in _mergeTypes)
            {
                shaderProgram.Value.Dispose();
            }
            _mergeTypes.Clear();
            _init = false;
            _isOne = false;
        }

        /// <summary>
        /// Static Float Array that is used to create a screen space quad
        /// </summary>
        private static float[] _screenQuadVertexData =
        {
            // positions   // texCoords
            -1.0f, 1.0f, 0.0f, 1.0f,
            -1.0f, -1.0f, 0.0f, 0.0f,
            1.0f, -1.0f, 1.0f, 0.0f,

            -1.0f, 1.0f, 0.0f, 1.0f,
            1.0f, -1.0f, 1.0f, 0.0f,
            1.0f, 1.0f, 1.0f, 1.0f
        };

        /// <summary>
        /// flag to indicate if the MergeStage has been initialized
        /// </summary>
        private bool _init;

        /// <summary>
        /// The VAO of the Screen quad
        /// </summary>
        private int _screenVao;

        /// <summary>
        /// Render target 0 for the pingpong rendering
        /// </summary>
        private RenderTarget _screenTarget0;

        /// <summary>
        /// Render target 1 for the pingpong rendering
        /// </summary>
        private RenderTarget _screenTarget1;

        /// <summary>
        /// The shaders used for the different merge types
        /// </summary>
        private Dictionary<RenderTargetMergeType, ShaderProgram> _mergeTypes =
            new Dictionary<RenderTargetMergeType, ShaderProgram>();

        /// <summary>
        /// Flag that indicates what is the next input buffer and what is the next output buffer
        /// </summary>
        private bool _isOne;

        /// <summary>
        /// Initialization method
        /// </summary>
        private void Init()
        {
            _init = true;

            _screenTarget0 =
                new RenderTarget(new ScreenCamera(), int.MaxValue, Color.FromArgb(0, 0, 0, 0));

            _screenTarget1 =
                new RenderTarget(new ScreenCamera(), int.MaxValue, Color.FromArgb(0, 0, 0, 0));

            _screenVao = GL.GenVertexArray();
            int screenVbo = GL.GenBuffer();
            GL.BindVertexArray(_screenVao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, screenVbo);

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_screenQuadVertexData.Length * sizeof(float)),
                _screenQuadVertexData, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), IntPtr.Zero);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));


            DefaultFilepaths.DefaultMergeAddShader.AddUniformCache("destinationTexture");
            DefaultFilepaths.DefaultMergeAddShader.AddUniformCache("otherTexture");
            _mergeTypes.Add(RenderTargetMergeType.Additive, DefaultFilepaths.DefaultMergeAddShader);

            DefaultFilepaths.DefaultMergeMulShader.AddUniformCache("destinationTexture");
            DefaultFilepaths.DefaultMergeMulShader.AddUniformCache("otherTexture");

            _mergeTypes.Add(RenderTargetMergeType.Multiplicative, DefaultFilepaths.DefaultMergeMulShader);


            DefaultFilepaths.DefaultScreenShader.AddUniformCache("sourceTexture");
        }

        /// <summary>
        /// Flips input and output of GetTarget() and GetSource()
        /// </summary>
        private void Ping()
        {
            _isOne = !_isOne;
        }

        /// <summary>
        /// Returns the Current Target that will be drawn onto
        /// </summary>
        /// <returns></returns>
        private RenderTarget GetTarget()
        {
            return _isOne ? _screenTarget1 : _screenTarget0;
        }

        /// <summary>
        /// Returns the current Source that will be sampled
        /// </summary>
        /// <returns></returns>
        private RenderTarget GetSource()
        {
            return _isOne ? _screenTarget0 : _screenTarget1;
        }

        /// <summary>
        /// merges the targets and draws the results on the back buffer of the OpenGL Window
        /// </summary>
        /// <param name="targets"></param>
        public void MergeAndDisplayTargets(List<RenderTarget> targets)
        {
            if (isDisposed)
            {
                throw new Byt3Exception("Use of Disposed RenderMergeStage");
            }
            if (!_init)
            {
                Init();
            }



            MemoryTracer.AddSubStage("Merge Framebuffers");

            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            foreach (RenderTarget renderTarget in targets)
            {
                MemoryTracer.NextStage("Merge Framebuffer: " + renderTarget.PassMask);
                RenderTarget dst = GetTarget();
                RenderTarget src = GetSource();

                if (dst.IsDisposed || src.IsDisposed)
                {
                    throw new Byt3Exception("Use of Disposed RenderMergeStage");
                }

                _mergeTypes[renderTarget.MergeType].Use();

                GL.BindFramebuffer(FramebufferTarget.Framebuffer, dst.FrameBuffer);

                GL.ClearColor(dst.ClearColor);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                //GL.Uniform1(_mergeShader.GetUniformLocation("divWeight"), 1 / (float)divideCount);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.Uniform1(_mergeTypes[renderTarget.MergeType].GetUniformLocation("destinationTexture"), 0);
                GL.BindTexture(TextureTarget.Texture2D, src.RenderedTexture);

                GL.ActiveTexture(TextureUnit.Texture1);
                GL.Uniform1(_mergeTypes[renderTarget.MergeType].GetUniformLocation("otherTexture"), 1);
                GL.BindTexture(TextureTarget.Texture2D, renderTarget.RenderedTexture);

                GL.BindVertexArray(_screenVao);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);


                Ping();


            }

            MemoryTracer.ReturnFromSubStage();

            MemoryTracer.NextStage("Rendering To Screen");
            GL.Disable(EnableCap.Blend);

            Ping();
            DefaultFilepaths.DefaultScreenShader.Use();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

            GL.ClearColor(Color.FromArgb(168, 143, 50, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Uniform1(DefaultFilepaths.DefaultScreenShader.GetUniformLocation("sourceTexture"), 0);
            GL.BindTexture(TextureTarget.Texture2D, GetTarget().RenderedTexture);


            GL.BindVertexArray(_screenVao);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            GL.BindVertexArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);


            //Clear the ping pong buffers after rendering them to the screen
            //For whatever reason GL.Clear is not acting on the active framebuffer
            GL.ClearTexImage(_screenTarget1.RenderedTexture, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            GL.ClearTexImage(_screenTarget0.RenderedTexture, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);


            GL.Enable(EnableCap.DepthTest);
        }
    }
}