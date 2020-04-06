using System.Collections.Generic;
using Byt3.Collections.Interfaces;

namespace Byt3.Collections
{
    public static class PathFinding
    {
        #region Pathfinding
        public static List<INode> FindPath(INode from, INode to)
        {
            INode current;
            Collections.PriorityQueue<INode> connectedNodes = new Collections.PriorityQueue<INode>();
            foreach (INode inode in from.INodeConnectedNodes)
            {
                connectedNodes.Enqueue(inode);
            }
            List<INode> doneNodes = new List<INode>();
            while (true)
            {
                if (connectedNodes.Count == 0)
                {
                    ResetNodes(doneNodes);
                    ResetNodes(connectedNodes.GetData());
                    return new List<INode>();
                }

                current = connectedNodes.Dequeue();
                doneNodes.Add(current);
                current.INodeState = Enums.INodeState.CLOSED;

                if (current == to)
                {
                    //Generate Path
                    List<INode> ret = GenerateNodePath(to);

                    //Reset Node Graph
                    ResetNodes(doneNodes);
                    ResetNodes(connectedNodes.GetData());
                    return ret;//Generated Path
                }
                else
                {
                    for (int i = 0; i < current.INodeConnectedNodes.Length; i++)
                    {
                        INode connected = current.INodeConnectedNodes[i];
                        if (!connected.INodeIsActive || connected.INodeState == Enums.INodeState.CLOSED) continue;
                        if (connected.INodeState == Enums.INodeState.UNTESTED)
                        {
                            connected.INodeParentNode = current;
                            connected.INodeCurrentCost = current.INodeCurrentCost + VectorMath.GetDistance(current.INodePosition, connected.INodePosition) * connected.INodeCost;
                            connected.INodeEstimatedCost = connected.INodeCurrentCost + VectorMath.GetDistance(connected.INodePosition, to.INodePosition);
                            connected.INodeState = Enums.INodeState.OPEN;
                            connectedNodes.Enqueue(connected);
                        }
                        if (current != connected)
                        {
                            float newCostCurrent = current.INodeCurrentCost + VectorMath.GetDistance(current.INodePosition, connected.INodePosition);
                            if (newCostCurrent < connected.INodeCurrentCost)
                            {
                                connected.INodeParentNode = current;
                                connected.INodeCurrentCost = newCostCurrent;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        public static void ResetNodes(List<INode> nodes)
        {
            foreach (INode node in nodes)
            {
                node.INodeCurrentCost = 0;
                node.INodeEstimatedCost = 0;
                node.INodeParentNode = null;
                node.INodeState = Enums.INodeState.UNTESTED;
            }
        }

        public static List<INode> GenerateNodePath(INode targetNode)
        {
            List<INode> ret = new List<INode>();
            INode current = targetNode;
            while (current != null)
            {
                ret.Add(current);
                current = current.INodeParentNode;
            }
            ret.Reverse();
            return ret;
        }

        
    }
}
