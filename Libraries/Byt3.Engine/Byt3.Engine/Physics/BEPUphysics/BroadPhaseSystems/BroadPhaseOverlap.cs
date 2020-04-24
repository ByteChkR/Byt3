﻿using System;
using Byt3.Engine.Physics.BEPUphysics.BroadPhaseEntries;
using Byt3.Engine.Physics.BEPUphysics.CollisionRuleManagement;

namespace Byt3.Engine.Physics.BEPUphysics.BroadPhaseSystems
{
    /// <summary>
    /// A pair of overlapping BroadPhaseEntries.
    /// </summary>
    public struct BroadPhaseOverlap : IEquatable<BroadPhaseOverlap>
    {
        internal BroadPhaseEntry entryA;

        /// <summary>
        /// First entry in the pair.
        /// </summary>
        public BroadPhaseEntry EntryA => entryA;

        internal BroadPhaseEntry entryB;

        /// <summary>
        /// Second entry in the pair.
        /// </summary>
        public BroadPhaseEntry EntryB => entryB;

        internal CollisionRule collisionRule;

        /// <summary>
        /// Constructs an overlap.
        /// </summary>
        /// <param name="entryA">First entry in the pair.</param>
        /// <param name="entryB">Second entry in the pair.</param>
        public BroadPhaseOverlap(BroadPhaseEntry entryA, BroadPhaseEntry entryB)
        {
            this.entryA = entryA;
            this.entryB = entryB;
            collisionRule = CollisionRules.DefaultCollisionRule;
        }

        /// <summary>
        /// Constructs an overlap.
        /// </summary>
        /// <param name="entryA">First entry in the pair.</param>
        /// <param name="entryB">Second entry in the pair.</param>
        /// <param name="collisionRule">Collision rule calculated for the pair.</param>
        public BroadPhaseOverlap(BroadPhaseEntry entryA, BroadPhaseEntry entryB, CollisionRule collisionRule)
        {
            this.entryA = entryA;
            this.entryB = entryB;
            this.collisionRule = collisionRule;
        }

        /// <summary>
        /// Gets the collision rule calculated for the pair.
        /// </summary>
        public CollisionRule CollisionRule => collisionRule;

        /// <summary>
        /// Gets the hash code of the object.
        /// </summary>
        /// <returns>Hash code of the object.</returns>
        public override int GetHashCode()
        {
            //TODO: Use old prime-based system?
            return (int) ((entryA.hashCode + entryB.hashCode) * 0xd8163841);
        }


        #region IEquatable<BroadPhaseOverlap> Members

        /// <summary>
        /// Compares the overlaps for equality based on the involved entries.
        /// </summary>
        /// <param name="other">Overlap to compare.</param>
        /// <returns>Whether or not the overlaps were equal.</returns>
        public bool Equals(BroadPhaseOverlap other)
        {
            return other.entryA == entryA && other.entryB == entryB || other.entryA == entryB && other.entryB == entryA;
        }

        #endregion

        public override string ToString()
        {
            return "{" + entryA + ", " + entryB + "}";
        }
    }
}