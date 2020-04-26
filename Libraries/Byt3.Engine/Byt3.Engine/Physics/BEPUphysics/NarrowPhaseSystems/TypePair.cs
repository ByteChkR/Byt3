using System;

namespace Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems
{
    ///<summary>
    /// Pair of types.
    ///</summary>
    public struct TypePair : IEquatable<TypePair>
    {
        //Currently this requires some reflective labor.  If perhaps the broad phase entries had some sort of 'id'... and they simply return that int through the interface.
        //Could be 'faster' assuming the supporting logic that creates the ids to begin with isn't too obtuse.
        ///<summary>
        /// First type in the pair.
        ///</summary>
        public Type A;

        ///<summary>
        /// Second type in the pair.
        ///</summary>
        public Type B;

        ///<summary>
        /// Constructs a new type pair.
        ///</summary>
        ///<param name="a">First type in the pair.</param>
        ///<param name="b">Second type in the pair.</param>
        public TypePair(Type a, Type b)
        {
            A = a;
            B = b;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            //TODO: Use old hash code system?
            return A.GetHashCode() + B.GetHashCode();
        }


        #region IEquatable<TypePair> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(TypePair other)
        {
            return other.A == A && other.B == B || other.B == A && other.A == B;
        }

        #endregion
    }
}