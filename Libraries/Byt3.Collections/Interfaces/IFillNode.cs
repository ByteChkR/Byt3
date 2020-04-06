using System;

namespace Byt3.Collections.Interfaces
{
    public interface IFillNode
    {
        IFillNode[] INodeConnectedNodes { get; }
        
        IComparable IFillValue { get; set; }

    }
}
