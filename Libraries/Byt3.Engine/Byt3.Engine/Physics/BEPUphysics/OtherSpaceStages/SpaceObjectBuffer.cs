﻿using Byt3.Engine.Physics.BEPUutilities.DataStructures;

namespace Byt3.Engine.Physics.BEPUphysics.OtherSpaceStages
{
    ///<summary>
    /// Thead-safely buffers up space objects for addition and removal.
    ///</summary>
    public class SpaceObjectBuffer : ProcessingStage
    {
        private readonly ConcurrentDeque<SpaceObjectChange> objectsToChange = new ConcurrentDeque<SpaceObjectChange>();

        ///<summary>
        /// Constructs the buffer.
        ///</summary>
        ///<param name="space">Space that owns the buffer.</param>
        public SpaceObjectBuffer(Space space)
        {
            Enabled = true;
            Space = space;
        }

        ///<summary>
        /// Gets the space which owns this buffer.
        ///</summary>
        public Space Space { get; }

        ///<summary>
        /// Adds a space object to the buffer.
        /// It will be added to the space the next time the buffer is flushed.
        ///</summary>
        ///<param name="spaceObject">Space object to add.</param>
        public void Add(ISpaceObject spaceObject)
        {
            objectsToChange.Enqueue(new SpaceObjectChange(spaceObject, true));
        }

        /// <summary>
        /// Enqueues a removal request to the buffer.
        /// It will be processed the next time the buffer is flushed.
        /// </summary>
        /// <param name="spaceObject">Space object to remove.</param>
        public void Remove(ISpaceObject spaceObject)
        {
            objectsToChange.Enqueue(new SpaceObjectChange(spaceObject, false));
        }


        protected override void UpdateStage()
        {
            SpaceObjectChange change;
            while (objectsToChange.TryDequeueFirst(out change))
            {
                if (change.ShouldAdd)
                {
                    Space.Add(change.SpaceObject);
                }
                else
                {
                    Space.Remove(change.SpaceObject);
                }
            }
        }

        private struct SpaceObjectChange
        {
            public readonly ISpaceObject SpaceObject;

            //Could change to enumeration, or more generally, buffered 'action<ISpaceObject>' to perform on the space object.
            public readonly bool ShouldAdd;

            public SpaceObjectChange(ISpaceObject spaceObject, bool shouldAdd)
            {
                SpaceObject = spaceObject;
                ShouldAdd = shouldAdd;
            }
        }
    }
}