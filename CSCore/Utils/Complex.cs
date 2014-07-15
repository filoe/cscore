using System;
using System.Diagnostics;

namespace CSCore.Utils
{
    /// <summary>
    ///     Represents a complex number.
    /// </summary>
    [DebuggerDisplay("r: {Real} i: {Imaginary}")]
    public struct Complex
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
    }
}