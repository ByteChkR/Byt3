using System;

namespace Byt3.Collections.Interfaces
{
    public interface IFillNode
    {
        IFillNode[] NodeConnectedNodes { get; }
        
        IComparable FillValue { get; set; }

    }
}
