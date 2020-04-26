using System;

namespace Byt3.Engine.Core
{
    /// <summary>
    /// A Pooled object of type T
    /// </summary>
    /// <typeparam name="T">Type of Object</typeparam>
    public struct PooledObject<T> where T : IDisposable
    {
        /// <summary>
        /// The Object that it stores
        /// </summary>
        public readonly T Object;

        /// <summary>
        /// A flag that indicates if this object is currently in use
        /// </summary>
        public bool IsUsed { get; private set; }

        /// <summary>
        /// A reference to the containing pool
        /// </summary>
        public ObjectPool<T> ContainingPool { get; }

        /// <summary>
        /// The Pool handle
        /// </summary>
        public int PoolHandle { get; }


        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="value">The Value of Type T that will be stored in this object</param>
        /// <param name="containingPool">The containing pool</param>
        /// <param name="poolHandle">The pool handle</param>
        public PooledObject(T value, ObjectPool<T> containingPool, int poolHandle)
        {
            Object = value;
            IsUsed = false;
            ContainingPool = containingPool;
            PoolHandle = poolHandle;
        }

        /// <summary>
        /// Returns this object to the pool
        /// </summary>
        public void GiveBack()
        {
            ContainingPool?.Give(this);
        }

        /// <summary>
        /// Internal Function that sets the use state for a pooled object
        /// </summary>
        /// <param name="state"></param>
        internal void SetIsUsed(bool state)
        {
            IsUsed = state;
        }

        /// <summary>
        /// Implicit cast of Pooled Object of Type T into T
        /// </summary>
        /// <param name="pooledInstance"></param>
        public static implicit operator T(PooledObject<T> pooledInstance)
        {
            return pooledInstance.Object;
        }
    }
}