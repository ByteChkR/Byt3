#region Using Directives

using System;

#endregion

namespace Byt3.OpenCL
{
    /// <summary>
    /// Represents the abstract base class for all OpenCL objects, that are represented by a handle.
    /// </summary>
    public abstract class HandleBase : IDisposable, IEquatable<HandleBase>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="HandleBase"/> instance.
        /// </summary>
        /// <param name="handle">The handle that represents the OpenCL object.</param>
        protected HandleBase(IntPtr handle)
        {
            Handle = handle;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the handle to the OpenCL object.
        /// </summary>
        internal IntPtr Handle { get; private set; }

        /// <summary>
        /// Gets a value that determines whether the object has alread been disposed of.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        #endregion

        #region Public Operators

        /// <summary>
        /// Checks if both operands are equal.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand</param>
        /// <returns>Returns <c>true</c> if both objects point to the same underlying unmanaged memory and <c>false</c> otherwise.</returns>
        public static bool operator ==(HandleBase leftOperand, HandleBase rightOperand)
        {
            // Checks if both are null, or both are same instance, in that case true is returned
            if (ReferenceEquals(leftOperand, rightOperand))
            {
                return true;
            }

            // Checks if one is null, but not both, in that case false is returned
            if (ReferenceEquals(leftOperand, null) || ReferenceEquals(null, rightOperand))
            {
                return false;
            }

            // Returns true if both point to the same underlying unmanaged memory
            return leftOperand.Handle.ToInt64() == rightOperand.Handle.ToInt64();
        }

        /// <summary>
        /// Checks if both operands are unequal.
        /// </summary>
        /// <param name="leftOperand">The left operand.</param>
        /// <param name="rightOperand">The right operand</param>
        /// <returns>Returns <c>false</c> if both objects point to the same underlying unmanaged memory and <c>true</c> otherwise.</returns>
        public static bool operator !=(HandleBase leftOperand, HandleBase rightOperand)
        {
            return !(leftOperand == rightOperand);
        }

        #endregion

        #region Object Implementation

        /// <summary>
        /// Checks if the specified object is equal to this.
        /// </summary>
        /// <param name="obj">The other object that is to be tested for equality.</param>
        /// <returns>Returns <c>true</c> if both objects point to the same underlying unmanaged memory and <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            // Checks if the other object is null, in that case it is not equal to this object
            if (obj == null)
            {
                return false;
            }

            // Checks if the other object can be cast into a HandleBase, if not, then it is not equal to this object
            HandleBase otherHandleBase = obj as HandleBase;
            if (otherHandleBase == null)
            {
                return false;
            }

            // Checks if the handles, that both objects contain are the same, in that case they are equal, if not they are not equal
            return Handle == otherHandleBase.Handle;
        }

        /// <summary>
        /// Checks if the specified object is equal to this.
        /// </summary>
        /// <param name="otherHandleBase">The other object that is to be tested for equality.</param>
        /// <returns>Returns <c>true</c> if both objects point to the same underlying unmanaged memory and <c>false</c> otherwise.</returns>
        public bool Equals(HandleBase otherHandleBase)
        {
            // Checks if the other object is null, in that case it is not equal to this object
            if (otherHandleBase == null)
            {
                return false;
            }

            // Checks if the handles, that both objects contain are the same, in that case they are equal, if not they are not equal
            return Handle == otherHandleBase.Handle;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>Returns a hash code for the current object, which is the numeric representation of the pointer that points to the underlying unmanaged memory.</returns>
        public override int GetHashCode()
        {
            return Handle.ToInt32();
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Disposes of the resources that have been acquired.
        /// </summary>
        /// <param name="disposing">Determines whether managed object or managed and unmanaged resources should be disposed of.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Checks if the object has alread been disposed of
            if (!IsDisposed)
            {
                // Sets the handle to null
                Handle = IntPtr.Zero;

                // Since the context has been disposed of, the is disposed flag is set to true, so that it is not called twice
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Destructs the <see cref="HandleBase"/> instance.
        /// </summary>
        ~HandleBase()
        {
            // Makes sure that unmanaged resources get disposed of eventually
            Dispose(false);
        }

        /// <summary>
        /// Disposes of all acquired resources.
        /// </summary>
        public void Dispose()
        {
            // Disposes of all acquired resources
            Dispose(true);

            // Since the resources have already been disposed of, the destructor does not need to be called anymore
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}