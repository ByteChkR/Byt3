using Byt3.Engine.Physics.BEPUutilities.DataStructures;
using Byt3.Engine.Physics.BEPUutilities.ResourceManagement;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseSystems.SortAndSweep
{
    internal class SortedGrid2DSet
    {
        private readonly UnsafeResourcePool<GridCell2D> cellPool = new UnsafeResourcePool<GridCell2D>();
        //TODO: The cell set is the number one reason why Grid2DSortAndSweep fails in corner cases.
        //One option:
        //Instead of trying to maintain a sorted set, stick to a dictionary + RawList combo.
        //The update phase can add active cell-object pairs to a raw list.  Could do bottom-up recreation too, though contention might be an issue.
        //Another option: Some other parallel-enumerable set, possibly with tricky hashing.

        internal RawList<GridCell2D> cells = new RawList<GridCell2D>();

        internal int count;

        internal bool TryGetIndex(ref Int2 cellIndex, out int index, out int sortingHash)
        {
            sortingHash = cellIndex.GetSortingHash();
            int minIndex = 0; //inclusive
            int maxIndex = count; //exclusive
            index = 0;
            while (maxIndex - minIndex > 0
            ) //If the testing interval has a length of zero, we've done as much as we can.
            {
                index = (maxIndex + minIndex) / 2;
                if (cells.Elements[index].sortingHash > sortingHash)
                {
                    maxIndex = index;
                }
                else if (cells.Elements[index].sortingHash < sortingHash)
                {
                    minIndex = ++index;
                }
                else
                {
                    //Found an equal sorting hash!
                    //The hash can collide, and we cannot add an entry to 
                    //an incorrect index.  It would break the 'cell responsibility' 
                    //used by the cell update process to avoid duplicate overlaps.
                    //So, check if the index we found is ACTUALLY correct.
                    if (cells.Elements[index].cellIndex.Y == cellIndex.Y &&
                        cells.Elements[index].cellIndex.Z == cellIndex.Z)
                    {
                        return true;
                    }

                    //If it was not the correct index, let it continue searching.
                }
            }

            return false;
        }

        internal bool TryGetCell(ref Int2 cellIndex, out GridCell2D cell)
        {
            int index;
            int sortingHash;
            if (TryGetIndex(ref cellIndex, out index, out sortingHash))
            {
                cell = cells.Elements[index];
                return true;
            }

            cell = null;
            return false;
        }

        internal void Add(ref Int2 index, Grid2DEntry entry)
        {
            int cellIndex;
            int sortingHash;
            if (TryGetIndex(ref index, out cellIndex, out sortingHash))
            {
                cells.Elements[cellIndex].Add(entry);
                return;
            }

            GridCell2D cell = cellPool.Take();
            cell.Initialize(ref index, sortingHash);
            cell.Add(entry);
            cells.Insert(cellIndex, cell);
            count++;
        }

        internal void Remove(ref Int2 index, Grid2DEntry entry)
        {
            int cellIndex;
            int sortingHash;
            if (TryGetIndex(ref index, out cellIndex, out sortingHash))
            {
                cells.Elements[cellIndex].Remove(entry);
                if (cells.Elements[cellIndex].entries.Count == 0)
                {
                    //The cell is now empty.  Give it back to the pool.
                    GridCell2D toRemove = cells.Elements[cellIndex];
                    //There's no cleanup to do on the grid cell.
                    //Its list is empty, and the rest is just value types.
                    cells.RemoveAt(cellIndex);
                    cellPool.GiveBack(toRemove);
                    count--;
                }
            }
        }
    }
}