﻿namespace Byt3.Engine.Physics.BEPUphysics.PositionUpdating
{
    ///<summary>
    /// A position updateable that can be updated continuously.
    ///</summary>
    public interface ICCDPositionUpdateable : IPositionUpdateable
    {
        /// <summary>
        /// Gets or sets the position update mode of the object.
        /// The position update mode defines the way the object
        /// interacts with continuous collision detection.
        /// </summary>
        PositionUpdateMode PositionUpdateMode { get; set; }

        ///<summary>
        /// Updates the time of impacts associated with the updateable.
        ///</summary>
        ///<param name="dt">Time step duration.</param>
        void UpdateTimesOfImpact(float dt);

        /// <summary>
        /// Updates the updateable using its continuous nature.
        /// </summary>
        /// <param name="dt">Time step duration.</param>
        void UpdatePositionContinuously(float dt);

        /// <summary>
        /// Resets the times of impact for pairs associated with this position updateable.
        /// </summary>
        void ResetTimesOfImpact();
    }
}