using System;
using Byt3.Collections.Interfaces;

namespace Byt3.Collections.Algorithms
{
    public static class Manipulation
    {
        public static void FloodFill(IFillNode startNode, IComparable newValue)
        {
            IComparable oldValue = startNode.IFillValue; //Save our old value.
            foreach (IFillNode ifnode in startNode.INodeConnectedNodes)
            {
                if (ifnode.IFillValue.CompareTo(oldValue) == 0) //Connected node has the same old value
                {
                    FloodFill(ifnode, newValue); //Call the "child" node
                }

            }
            startNode.IFillValue = newValue; //Finally change the value on our root(all the children are done)
        }
    }
}
