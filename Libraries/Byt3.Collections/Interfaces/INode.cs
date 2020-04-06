using System;
using Byt3.Collections.Enums;

namespace Byt3.Collections.Interfaces
{
    public interface INode : IComparable<INode>
    {
        INodeState INodeState { get; set; }
        float INodeCost { get; }
        INode INodeParentNode { get; set; }
        float INodeCurrentCost { get; set; }
        float INodeEstimatedCost { get; set; }
        bool INodeIsActive { get; }
        INode[] INodeConnectedNodes { get; }
        IVec3 INodePosition { get; }
    }
}
