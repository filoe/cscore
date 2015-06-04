using System;
using System.Diagnostics;

namespace CSCore.Utils
{
    /// <summary>
    ///     Represents a complex number.
    /// </summary>
    [DebuggerDisplay("r: {Real} i: {Imaginary}")]
    public struct Complex : IEquatable<Complex>
    {
        /// <summary>
        ///     A complex number with a total length of zero.
        /// </summary>
        public static readonly Complex Zero = new Complex(0, 0);

        /// <summary>
        ///     Imaginary component of the complex number.
        /// </summary>
        public float Imaginary;

        /// <summary>
        ///     Real component of the complex number.
        /// </summary>
        public float Real;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Complex" /> structure.
        /// </summary>
        /// <param name="real">The real component of the complex number.</param>
        /// <remarks>The imaginary component of the complex number will be set to zero.</remarks>
        public Complex(float real)
            : this(real, 0.0f)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Complex" /> structure.
        /// </summary>
        /// <param name="real">The real component of the complex number.</param>
        /// <param name="img">The imaginary component of the complex number.</param>
        public Complex(float real, float img)
        {
            Real = real;
            Imaginary = img;
        }

        /// <summary>
        ///     Gets the absolute value of the complex number.
        /// </summary>
        public double Value
        {
            get { return Math.Sqrt(Real * Real + Imaginary * Imaginary); }
        }

        /// <summary>
        ///     Defines an implicit conversion of a complex number to a single-precision floating-point number.
        /// </summary>
        /// <param name="complex">Complex number.</param>
        /// <returns>The absolute value of the <paramref name="complex" />.</returns>
        public static implicit operator float(Complex complex)
        {
            return (float) complex.Value;
        }

        /// <summary>
        ///     Defines an implicit conversion of a complex number to a double-precision floating-point number.
        /// </summary>
        /// <param name="complex">Complex number.</param>
        /// <returns>The absolute value of the <paramref name="complex" />.</returns>
        public static implicit operator double(Complex complex)
        {
            return complex.Value;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="complex1">The complex1.</param>
        /// <param name="complex2">The complex2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Complex complex1, Complex complex2)
        {
            return complex1.Real == complex2.Real && complex1.Imaginary == complex2.Imaginary;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="complex1">The complex1.</param>
        /// <param name="complex2">The complex2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Complex complex1, Complex complex2)
        {
            return !(complex1 == complex2);
        }

        /// <summary>
        /// Indicates whether the current complex value is equal to another complex value.
        /// </summary>
        /// <param name="other">A complex value to compare with this complex value.</param>
        /// <returns>
        /// true if the current complex value is equal to the <paramref name="other" /> complex value; otherwise, false.
        /// </returns>
        public bool Equals(Complex other)
        {
            return Imaginary.Equals(other.Imaginary) && Real.Equals(other.Real);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Complex && Equals((Complex)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Imaginary.GetHashCode() * 397) ^ Real.GetHashCode();
            }
        }
    }
}