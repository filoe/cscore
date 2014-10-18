using System;
using System.Runtime.InteropServices;

namespace CSCore.Win32
{
    /// <summary>
    /// Represents a native 4 byte boolean value.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct NativeBool : IEquatable<NativeBool>
    {
        /// <summary>
        /// Represents the boolean value true as a <see cref="NativeBool"/>.
        /// </summary>
        public static readonly NativeBool True = new NativeBool(true);
        /// <summary>
        /// Represents the boolean value false as a <see cref="NativeBool"/>.
        /// </summary>
        public static readonly NativeBool False = new NativeBool(false);

        private readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeBool"/> structure based on a boolean value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        public NativeBool(bool value)
        {
            _value = value ? 1 : 0;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a <see cref="NativeBool"/> object.
        /// </summary>
        /// <param name="obj">A <see cref="NativeBool"/> value to compare to this instance.</param>
        /// <returns>true if obj has the same value as this instance; otherwise, false.</returns>
        public bool Equals(NativeBool obj)
        {
            return _value == obj._value;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>true if obj is a <see cref="NativeBool"/> and has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is NativeBool || obj is Boolean)
                return Equals((NativeBool)obj);

            return false;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for the current <see cref="NativeBool"/>.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

#pragma warning disable 1591
        public static bool operator ==(NativeBool left, NativeBool right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NativeBool left, NativeBool right)
        {
            return !(left == right);
        }

        public static implicit operator bool(NativeBool value)
        {
            return value._value != 0;
        }

        public static implicit operator NativeBool(bool value)
        {
            return new NativeBool(value);
        }
#pragma warning restore 1591


        /// <summary>
        /// Converts the value of this instance to its equivalent string representation (either "True" or "False").
        /// </summary>
        /// <returns>String representation of this instance.</returns>
        public override string ToString()
        {
            return this ? "True" : "False";
        }
    }
}