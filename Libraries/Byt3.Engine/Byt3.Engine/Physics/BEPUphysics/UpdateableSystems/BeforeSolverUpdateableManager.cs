﻿using Byt3.Engine.Physics.BEPUutilities.Threading;

namespace Byt3.Engine.Physics.BEPUphysics.UpdateableSystems
{
    ///<summary>
    /// Manages updateables that update before the solver.
    ///</summary>
    public class BeforeSolverUpdateableManager : UpdateableManager<IBeforeSolverUpdateable>
    {
        ///<summary>
        /// Constructs a manager.
        ///</summary>
        ///<param name="timeStepSettings">Time step settings to use.</param>
        public BeforeSolverUpdateableManager(TimeStepSettings timeStepSettings)
            : base(timeStepSettings)
        {
        }

        ///<summary>
        /// Constructs a manager.
        ///</summary>
        ///<param name="timeStepSettings">Time step settings to use.</param>
        /// <param name="parallelLooper">Parallel loop provider to use.</param>
        public BeforeSolverUpdateableManager(TimeStepSettings timeStepSettings, IParallelLooper parallelLooper)
            : base(timeStepSettings, parallelLooper)
        {
        }

        protected override void MultithreadedUpdate(int i)
        {
            if (simultaneouslyUpdatedUpdateables[i].IsUpdating)
            {
                simultaneouslyUpdatedUpdateables[i].Update(timeStepSettings.TimeStepDuration);
            }
        }

        protected override void SequentialUpdate(int i)
        {
            if (sequentiallyUpdatedUpdateables[i].IsUpdating)
            {
                sequentiallyUpdatedUpdateables[i].Update(timeStepSettings.TimeStepDuration);
            }
        }
    }
}