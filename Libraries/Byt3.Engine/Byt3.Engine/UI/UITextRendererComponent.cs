﻿using System;
using System.Drawing;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpFont;

namespace Byt3.Engine.UI
{
    /// <summary>
    /// Implements A Text Renderer Component
    /// </summary>
    public class UiTextRendererComponent : UiElement
    {
        /// <summary>
        /// The VBO of the Quad used to draw each character
        /// </summary>
        protected int Vbo;

        /// <summary>
        /// The VAO of the Quad used to draw each character
        /// </summary>
        protected int Vao;

        /// <summary>
        /// Initialization flag
        /// </summary>
        protected bool Init;

        /// <summary>
        /// The Length of a single \t in UV coordinates
        /// </summary>
        public static float TabToSpaceCount = 0.1f;

        /// <summary>
        /// the Font that is used to draw
        /// </summary>
        private readonly GameFont font;

        /// <summary>
        /// the backing field for Text
        /// </summary>
        private string text = "HELLO";

        private bool cached;

        public bool Center;

        public Vector3 Color = Vector3.UnitX; // RED

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="fontName">The name of the Font</param>
        /// <param name="worldSpace">Is the Object in world space</param>
        /// <param name="alpha">Alpha value of the image</param>
        /// <param name="shader">The shader to be used</param>
        public UiTextRendererComponent(string fontName, bool worldSpace, float alpha, ShaderProgram shader) : base(
            shader, worldSpace, alpha)
        {
            font = DefaultFilepaths.DefaultFont;
        }


        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="font">The Font</param>
        /// <param name="worldSpace">Is the Object in world space</param>
        /// <param name="alpha">Alpha value of the image</param>
        /// <param name="shader">The shader to be used</param>
        public UiTextRendererComponent(GameFont font, bool worldSpace, float alpha, ShaderProgram shader) : base(
            shader, worldSpace, alpha)
        {
            this.font = font;
        }

        /// <summary>
        /// The text that is drawn
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if (!string.Equals(text, value))
                {
                    ContextInvalid = true;
                    text = value;
                }
            }
        }

        /// <summary>
        /// Wrapper for the Color Field
        /// </summary>
        public Color SystemColor
        {
            get => System.Drawing.Color.FromArgb((int) (Color.X * 255), (int) (Color.Y * 255), (int) (Color.Z * 255),
                255);
            set => Color = new Vector3(value.R / 255f, value.G / 255f, value.B / 255f);
        }

        /// <summary>
        /// Initialization Method that sets up the Screen Quad buffers
        /// </summary>
        private void SetUpTextResources()
        {
            Init = true;
            Vao = GL.GenVertexArray();
            Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * 4, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), IntPtr.Zero);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Renders the Text in Display Text to the screen
        /// </summary>
        /// <param name="viewMat">View matrix of the camera(unused)</param>
        /// <param name="projMat">View matrix of the camera(unused)</param>
        public override void Render(Matrix4 viewMat, Matrix4 projMat)
        {
            if (!Init)
            {
                SetUpTextResources();
            }


            if (!cached)
            {
                cached = true;
                Program.AddUniformCache("transform");
                Program.AddUniformCache("textColor");
                Program.AddUniformCache("sourceTexture");
            }

            int scrW = GameEngine.Instance.Width;
            int scrH = GameEngine.Instance.Height;

            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            Program.Use();


            Matrix4 m = Matrix4.Identity;

            GL.UniformMatrix4(Program.GetUniformLocation("transform"), false, ref m);


            GL.Uniform3(Program.GetUniformLocation("textColor"), Color.X, Color.Y, Color.Z);
            GL.Uniform1(Program.GetUniformLocation("sourceTexture"), 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(Vao);
            Vector2 pos = Position; //Hacked

            if (Center)
            {
                Vector2 v = font.GetRenderBounds(Text);
                v.X *= Scale.X;
                v.Y *= Scale.Y;
                pos = Position - v / 2;
            }

            float x = pos.X;
            float y = pos.Y;
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] == '\n')
                {
                    FaceMetrics fm = font.Metrics;
                    x = pos.X;
                    y -= fm.LineHeight / scrH * Scale.Y;
                    continue;
                }


                if (Text[i] == '\t')
                {
                    float len = x - pos.X;
                    float count = TabToSpaceCount - len % TabToSpaceCount;
                    float val = count;
                    x += val;
                    continue;
                }


                if (!font.TryGetCharacter(Text[i], out TextCharacter chr))
                {
                    font.TryGetCharacter('?', out chr);
                }

                float xpos = x + chr.BearingX / scrW * Scale.X;
                float ypos = y - (chr.Height - chr.BearingY) / scrH * Scale.Y;

                float w = chr.Width / (float) scrW * Scale.X;
                float h = chr.Height / (float) scrH * Scale.Y;


                //Remove Scale And initial position(start at (x,y) = 0)
                //Add Translation to Make text be centered at origin(-TotalTextWidth/2,-TotalTextHeight/2)
                //Multiply Verts by matrix or pass it in shader

                float[] verts =
                {
                    xpos, ypos + h, 0.0f, 1.0f,
                    xpos, ypos, 0.0f, 0.0f,
                    xpos + w, ypos, 1.0f, 0.0f,

                    xpos, ypos + h, 0.0f, 1.0f,
                    xpos + w, ypos, 1.0f, 0.0f,
                    xpos + w, ypos + h, 1.0f, 1.0f
                };

                if (chr.GlTexture != null)
                {
                    GL.BindTexture(TextureTarget.Texture2D, chr.GlTexture.TextureId);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
                    GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr) (sizeof(float) * verts.Length),
                        verts);

                    GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                }

                x += chr.Advance / scrW * Scale.X;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }
    }
}