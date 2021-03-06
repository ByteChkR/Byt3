﻿using System;
using Byt3.Engine.Core;
using OpenTK;

namespace Byt3.Engine.Rendering
{
    /// <summary>
    /// Abstract Rendering Component that implements all tasks that all renderer components have in common
    /// </summary>
    public abstract class RenderingComponent : AbstractComponent, IComparable<RenderingComponent>
    {
        /// <summary>
        /// Constructor for a render context
        /// </summary>
        /// <param name="program">The shader program to use</param>
        /// <param name="modelMatrix">The model matrix</param>
        /// <param name="worldSpace">Is the model in Camera Space</param>
        /// <param name="renderType">The Type of the Render Process</param>
        /// <param name="renderQueue">The render queue that is used</param>
        protected RenderingComponent(ShaderProgram program, bool worldSpace,
            Renderer.RenderType renderType, int renderQueue)
        {
            Program = program;
            WorldSpace = worldSpace;
            RenderType = renderType;
            RenderQueue = renderQueue;
        }

        /// <summary>
        /// the shader program that is used
        /// </summary>
        public ShaderProgram Program { get; set; }

        /// <summary>
        /// the render queue that determines the order in which the objects are drawn
        /// This only applies to Screen space Render Contexts. For World space the ordering is worked out by the distance to the camera.
        /// </summary>
        public int RenderQueue { get; set; }

        /// <summary>
        /// Cached version of the ModelView Matrix(is used for ordered rendering
        /// </summary>
        public Matrix4 Mv { get; private set; }

        /// <summary>
        /// The position of the Object in ModelView Space
        /// </summary>
        public Vector3 MvPosition { get; private set; }

        /// <summary>
        /// The Render type of the context
        /// </summary>
        public Renderer.RenderType RenderType { get; set; }

        /// <summary>
        /// A flag that indicates if the object is meant to be drawn in screen space or world space
        /// </summary>
        public bool WorldSpace { get; set; }

        /// <summary>
        /// CompareTo Implementation
        /// If both transparent and in camera space
        /// </summary>
        /// <param name="other">The Object to compare against</param>
        /// <returns></returns>
        public int CompareTo(RenderingComponent other)
        {
            return -CmpTo(other);
        }

        /// <summary>
        /// Abstract function that will be called when the object should be drawn
        /// </summary>
        /// <param name="viewMat">View Matrix</param>
        /// <param name="projMat">Projection Matrix</param>
        public abstract void Render(Matrix4 viewMat, Matrix4 projMat);

        public virtual void Render(Matrix4 viewMat, Matrix4 projMat, int depthMap)
        {
            Render(viewMat, projMat);
        }

        public virtual void Render(Matrix4 viewMat, Matrix4 projMat, ShaderProgram prog)
        {
            Render(viewMat, projMat);
        }

        /// <summary>
        /// CompareTo Implementation
        /// If both transparent and in camera space
        /// </summary>
        /// <param name="other">The Object to compare against</param>
        /// <returns></returns>
        private int CmpTo(RenderingComponent other)
        {
            if (RenderType == Renderer.RenderType.Transparent && other.RenderType == Renderer.RenderType.Transparent)
            {
                if (WorldSpace && other.WorldSpace)
                {
                    float d = MvPosition.LengthSquared - other.MvPosition.LengthSquared;
                    if (d > 0)
                    {
                        return 1;
                    }

                    return -1;
                }

                if (WorldSpace) // && !other.WorldSpace
                {
                    return -1;
                }

                if (other.WorldSpace) // && !WorldSpace
                {
                    return 1;
                }


                int ret = RenderQueue.CompareTo(other.RenderQueue);
                return ret;
            }

            if (other.RenderType == Renderer.RenderType.Transparent)
            {
                return 1;
            }

            if (RenderType == Renderer.RenderType.Transparent)
            {
                return -1;
            }

            return 0;
        }


        /// <summary>
        /// Precalculates the ModelView Matrix
        /// </summary>
        /// <param name="view">The view matrix</param>
        public void PrecalculateMv(Matrix4 view)
        {
            Mv = Owner.WorldTransformCache * view;
            MvPosition = new Vector3(new Vector4(Vector3.Zero, 1) * Mv);
        }
    }
}