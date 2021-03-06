﻿using System.Collections.Generic;
using Byt3.Engine.Physics.BEPUphysics.Constraints;
using Byt3.Engine.Physics.BEPUphysics.DeactivationManagement;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;

namespace Byt3.Engine.Physics.BEPUphysics.Entities
{
    ///<summary>
    /// Convenience collection for easily scanning the two entity constraints connected to an entity.
    ///</summary>
    public class EntitySolverUpdateableCollection : IEnumerable<SolverUpdateable>
    {
        private readonly RawList<SimulationIslandConnection> connections;


        /// <summary>
        /// Constructs a new constraint collection.
        /// </summary>
        /// <param name="connections">Solver updateables to enumerate over.</param>
        public EntitySolverUpdateableCollection(RawList<SimulationIslandConnection> connections)
        {
            this.connections = connections;
        }

        IEnumerator<SolverUpdateable> IEnumerable<SolverUpdateable>.GetEnumerator()
        {
            return new Enumerator(connections);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(connections);
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>Enumerator for the collection.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(connections);
        }

        ///<summary>
        /// Enumerator for the EntityConstraintCollection.
        ///</summary>
        public struct Enumerator : IEnumerator<SolverUpdateable>
        {
            private readonly RawList<SimulationIslandConnection> connections;
            private int index;

            /// <summary>
            /// Constructs an enumerator for the solver updateables list.
            /// </summary>
            /// <param name="connections">List of solver updateables to enumerate.</param>
            public Enumerator(RawList<SimulationIslandConnection> connections)
            {
                this.connections = connections;
                index = -1;
                Current = null;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public SolverUpdateable Current { get; private set; }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current => Current;

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
            public bool MoveNext()
            {
                while (++index < connections.Count)
                {
                    if (!connections.Elements[index].SlatedForRemoval)
                    {
                        Current = connections.Elements[index].Owner as SolverUpdateable;
                        if (Current != null)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
            public void Reset()
            {
                index = -1;
                Current = null;
            }
        }
    }
}