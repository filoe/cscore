using System;

namespace CSCore.Utils
{
    [System.Diagnostics.DebuggerDisplay("r: {Real} i: {Imaginary}")]
    public struct Complex
    {
        public static readonly Complex Zero = new Complex(0, 0);

        public Complex(float real)
            : this(real, 0.0f)
        {
        }

        public Complex(float real, float img)
        {
            Real = real;
            Imaginary = img;
        }

        public float Real;
        public float Imaginary;

        public static explicit operator float(Complex complex)
        {
            return complex.Value;
        }

        public float Value
        {
            get { return (float)Math.Sqrt(Real * Real + Imaginary * Imaginary); }
        }

        public float CalculateFFTPercentage()
        {
            return (float)FastFourierTransformation.CalculatePercentage(this);
        }
    }
}