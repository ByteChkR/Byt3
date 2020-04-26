using System.Collections.Generic;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUutilities;
using Byt3.Engine.Physics.BEPUutilities.DataStructures;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseSystems.Hierarchies
{
    internal sealed class LeafNode : Node
    {
        private BroadPhaseEntry element;

        internal override Node ChildA => null;

        internal override Node ChildB => null;

        internal override BroadPhaseEntry Element => element;

        internal override bool IsLeaf => true;


        internal void Initialize(BroadPhaseEntry element)
        {
            this.element = element;
            BoundingBox = element.BoundingBox;
        }

        internal void CleanUp()
        {
            element = null;
        }

        internal override void GetOverlaps(ref BoundingBox boundingBox, IList<BroadPhaseEntry> outputOverlappedElements)
        {
            //Our parent already tested the bounding box.  All that's left is to add myself to the list.
            outputOverlappedElements.Add(element);
        }

        internal override void GetOverlaps(ref BoundingSphere boundingSphere,
            IList<BroadPhaseEntry> outputOverlappedElements)
        {
            outputOverlappedElements.Add(element);
        }

        internal override void GetOverlaps(ref Ray ray, float maximumLength,
            IList<BroadPhaseEntry> outputOverlappedElements)
        {
            outputOverlappedElements.Add(element);
        }

        internal override void GetOverlaps(Node opposingNode, DynamicHierarchy owner)
        {
            bool intersects;
            //note: This is never executed when the opposing node is the current node.
            if (opposingNode.IsLeaf)
            {
                //We're both leaves!  Our parents have already done the testing for us, so we know we're overlapping.
                owner.TryToAddOverlap(element, opposingNode.Element);
            }
            else
            {
                Node opposingChildA = opposingNode.ChildA;
                Node opposingChildB = opposingNode.ChildB;
                //If it's not a leaf, try to go deeper in the opposing hierarchy.
                BoundingBox.Intersects(ref opposingChildA.BoundingBox, out intersects);
                if (intersects)
                {
                    GetOverlaps(opposingChildA, owner);
                }

                BoundingBox.Intersects(ref opposingChildB.BoundingBox, out intersects);
                if (intersects)
                {
                    GetOverlaps(opposingChildB, owner);
                }
            }
        }

        internal override bool TryToInsert(LeafNode node, out Node treeNode)
        {
            InternalNode newTreeNode = InternalNode.nodePool.Take();
            BoundingBox.CreateMerged(ref BoundingBox, ref node.BoundingBox, out newTreeNode.BoundingBox);
            Vector3 offset;
            Vector3.Subtract(ref newTreeNode.BoundingBox.Max, ref newTreeNode.BoundingBox.Min, out offset);
            newTreeNode.currentVolume = offset.X * offset.Y * offset.Z;
            newTreeNode.childA = this;
            newTreeNode.childB = node;
            treeNode = newTreeNode;
            return true;
        }

        public override string ToString()
        {
            return element.ToString();
        }

        internal override void Analyze(List<int> depths, int depth, ref int nodeCount)
        {
            nodeCount++;
            depths.Add(depth);
        }

        internal override void Refit()
        {
            BoundingBox = element.boundingBox;
        }

        internal override void RetrieveNodes(RawList<LeafNode> leafNodes)
        {
            Refit();
            leafNodes.Add(this);
        }

        internal override void CollectMultithreadingNodes(int splitDepth, int currentDepth,
            RawList<Node> multithreadingSourceNodes)
        {
            //This could happen if there are almost no elements in the tree.  No biggie- do nothing!
        }

        internal override void PostRefit(int splitDepth, int currentDepth)
        {
            //This could happen if there are almost no elements in the tree.  Just do a normal leaf refit.
            BoundingBox = element.boundingBox;
        }

        internal override void GetMultithreadedOverlaps(Node opposingNode, int splitDepth, int currentDepth,
            DynamicHierarchy owner, RawList<DynamicHierarchy.NodePair> multithreadingSourceOverlaps)
        {
            bool intersects;
            //note: This is never executed when the opposing node is the current node.
            if (opposingNode.IsLeaf)
            {
                //We're both leaves!  Our parents have already done the testing for us, so we know we're overlapping.
                owner.TryToAddOverlap(element, opposingNode.Element);
            }
            else
            {
                Node opposingChildA = opposingNode.ChildA;
                Node opposingChildB = opposingNode.ChildB;
                if (splitDepth == currentDepth)
                {
                    //Time to add the child overlaps to the multithreading set!
                    BoundingBox.Intersects(ref opposingChildA.BoundingBox, out intersects);
                    if (intersects)
                    {
                        multithreadingSourceOverlaps.Add(new DynamicHierarchy.NodePair {a = this, b = opposingChildA});
                    }

                    BoundingBox.Intersects(ref opposingChildB.BoundingBox, out intersects);
                    if (intersects)
                    {
                        multithreadingSourceOverlaps.Add(new DynamicHierarchy.NodePair {a = this, b = opposingChildB});
                    }

                    return;
                }

                //If it's not a leaf, try to go deeper in the opposing hierarchy.
                BoundingBox.Intersects(ref opposingChildA.BoundingBox, out intersects);
                if (intersects)
                {
                    GetOverlaps(opposingChildA, owner);
                }

                BoundingBox.Intersects(ref opposingChildB.BoundingBox, out intersects);
                if (intersects)
                {
                    GetOverlaps(opposingChildB, owner);
                }
            }
        }

        internal override bool Remove(BroadPhaseEntry entry, out LeafNode leafNode, out Node replacementNode)
        {
            replacementNode = null;
            if (element == entry)
            {
                leafNode = this;
                return true;
            }

            leafNode = null;
            return false;
        }

        internal override bool RemoveFast(BroadPhaseEntry entry, out LeafNode leafNode, out Node replacementNode)
        {
            //The fastremove leaf node procedure is identical to the brute force approach.
            //We don't need to perform any bounding box test here; if they're equal, they're equal!
            replacementNode = null;
            if (element == entry)
            {
                leafNode = this;
                return true;
            }

            leafNode = null;
            return false;
        }

        internal override float MeasureSubtreeCost()
        {
            //Not much value in attempting to assign variable cost to leaves vs internal nodes for this diagnostic.
            Vector3 offset;
            Vector3.Subtract(ref BoundingBox.Max, ref BoundingBox.Min, out offset);
            return offset.X * offset.Y * offset.Z;
        }
    }
}