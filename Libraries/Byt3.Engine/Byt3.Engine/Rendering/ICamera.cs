﻿using OpenTK;

namespace Byt3.Engine.Rendering
{
    /// <summary>
    /// Defines the Properties of a Camera
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// The projection matrix of the Camera
        /// </summary>
        Matrix4 Projection { get; }

        /// <summary>
        /// The view matrix of the Camera
        /// </summary>
        Matrix4 ViewMatrix { get; }

        /// <summary>
        /// Method to Get the World Transform of the Camera
        /// </summary>
        /// <returns></returns>
        Matrix4 GetWorldTransform();
    }
}