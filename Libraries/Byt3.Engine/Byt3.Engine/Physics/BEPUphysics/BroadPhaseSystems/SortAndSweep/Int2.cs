namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseSystems.SortAndSweep
{
    internal struct Int2
    {
        internal int Y;
        internal int Z;

        public override int GetHashCode()
        {
            return Y + Z;
        }


        internal int GetSortingHash()
        {
            return (int) (Y * 15485863L + Z * 32452843L);
        }

        public override string ToString()
        {
            return "{" + Y + ", " + Z + "}";
        }
    }
}