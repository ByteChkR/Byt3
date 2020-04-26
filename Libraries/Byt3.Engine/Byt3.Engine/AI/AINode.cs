using System;
using System.Collections.Generic;
using Byt3.Engine.Core;
using Byt3.Engine.Debug;
using Byt3.Engine.Exceptions;

namespace Byt3.Engine.AI
{
    /// <summary>
    /// The AiNode class is used by the A* implementation
    /// </summary>
    public class AiNode : AbstractComponent, IComparable<AiNode>
    {
        private readonly List<AiNode> connections;

        /// <summary>
        /// Current Cost of the node in the current search query
        /// </summary>
        public float CurrentCost { get; set; }

        /// <summary>
        /// Estimated Cost of the node in the current search query
        /// </summary>
        public float EstimatedCost { get; set; }

        /// <summary>
        /// State of the node in the current search query
        /// </summary>
        public AiNodeState NodeState { get; set; }


        //Search Specific
        /// <summary>
        /// The node "where we came from"
        /// </summary>
        public AiNode ParentNode { get; set; }

        //Search Specific
        /// <summary>
        /// The flag that determines if the node is walkable
        /// </summary>
        public bool Walkable { get; set; }

        /// <summary>
        /// The multiplier that changes the walkcosts(distance from node to node) to traverse over this node.
        /// </summary>
        public float WalkCostMultiplier { get; set; } = 1;

        /// <summary>
        /// Public Constructor
        /// </summary>
        /// <param name="walkable">Walkable Flag</param>
        public AiNode(bool walkable)
        {
            connections = new List<AiNode>();
            Walkable = walkable;
        }

        /// <summary>
        /// Total Cost of the node in the current search query
        /// </summary>
        public float TotalCost => CurrentCost + EstimatedCost;

        /// <summary>
        /// The Number of connected nodes
        /// </summary>
        public int ConnectionCount => connections.Count;

        /// <summary>
        /// Comparable Implementation(Using the Total Costs)
        /// Override with Current Cost to have Djkstra Search
        /// Override with Estimated Cost to have "Aggressive" Search
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual int CompareTo(AiNode other)
        {
            return TotalCost.CompareTo(other.TotalCost);
        }

        /// <summary>
        /// Returns the connection at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AiNode GetConnection(int index)
        {
            if (index < 0 || index >= connections.Count)
            {
                Logger.Crash(new ItemNotFoundExeption("AI Node", "Could not find the AI Node at index: " + index),
                    true);
                return null;
            }

            return connections[index];
        }

        /// <summary>
        /// Adds a connection with the other node.
        /// </summary>
        /// <param name="other">The node to make the connection with</param>
        /// <param name="addReverse">adds the same connection in reverse to allow traveling in reverse</param>
        public void AddConnection(AiNode other, bool addReverse = true)
        {
            Logger.Log(DebugChannel.EngineAI | DebugChannel.Log, $"Creating Connection {this} => {other}", 5);
            bool thisContains = connections.Contains(other);
            bool otherContains = other.connections.Contains(this);
            if (thisContains && (!addReverse || otherContains))
            {
                Logger.Log(DebugChannel.EngineAI | DebugChannel.Warning, $"Connection already established in {this}",
                    10);
            }
            else if (!thisContains)
            {
                Logger.Log(DebugChannel.EngineAI | DebugChannel.Warning, $"Connection already established in {this}",
                    10);
                connections.Add(other);
            }

            if (!otherContains && addReverse)
            {
                Logger.Log(DebugChannel.EngineAI | DebugChannel.Log, $"Adding Node Connection in reverse.", 5);
                other.AddConnection(this, false); //Add the other way around
            }
        }

        /// <summary>
        /// Removes a connection to another node
        /// </summary>
        /// <param name="other">The connected node to be removed</param>
        /// <param name="removeReverse">Remove the reverse of this connection</param>
        public void RemoveConnection(AiNode other, bool removeReverse = true)
        {
            if (!connections.Contains(other))
            {
                Logger.Log(DebugChannel.EngineAI | DebugChannel.Warning, $"Connection not found in {this}", 10);
                return;
            }

            Logger.Log(DebugChannel.EngineAI | DebugChannel.Log, $"Removing Connection in {this}", 5);
            connections.Remove(other);
            if (removeReverse)
            {
                Logger.Log(DebugChannel.EngineAI | DebugChannel.Log, $"Removing reverse Node Connection.", 5);
                other.RemoveConnection(this, false);
            }
        }
    }
}