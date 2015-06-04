using System;
using CSCore.Utils;

namespace CSCore.DSP
{
    /// <summary>
    /// Provides an Fast Fourier Transform implementation including a few utils method which are commonly used in combination with FFT (e.g. the hamming window function).
    /// </summary>
    public static class FastFourierTransformation
    {
        /// <summary>
        /// Obsolete. Use the <see cref="Complex.Value"/> property instead.
        /// <seealso cref="Complex.Value"/>
        /// </summary>
        /// <param name="c"></param>
        /// <returns>The intensity of the complex value <paramref name="c"/>.</returns>
        /// <remarks>sqrt(r² + i²)</remarks>
        [Obsolete("Use the Complex.Value property instead.")]
        public static double GetIntensity(Complex c)
        {
            return Math.Sqrt(c.Real * c.Real + c.Imaginary * c.Imaginary);
        }

        /// <summary>
        /// Implementation of the Hamming Window using double-precision floating-point numbers.
        /// </summary>
        /// <param name="n">Current index of the input signal.</param>
        /// <param name="N">Window width.</param>
        /// <returns>Hamming window multiplier.</returns>
// ReSharper disable once InconsistentNaming
        public static double HammingWindow(int n, int N)
        {
            //according to Wikipedia we could also use alpha=0.53836 and beta=0.46164
            const double alpha = 0.54; 
            const double beta = 0.46;
            return alpha - beta * Math.Cos((2 * Math.PI * n) / (N - 1));
        }

        /// <summary>
        /// Hamming window implementation using single-precision floating-point numbers.
        /// </summary>
        /// <param name="n">Current index of the input signal.</param>
        /// <param name="N">Window width.</param>
        /// <returns>Hamming Window multiplier.</returns>
// ReSharper disable once InconsistentNaming
        public static float HammingWindowF(int n, int N)
        {
            //according to Wikipedia we could also use alpha=0.53836 and beta=0.46164
            const float alpha = 0.54f;
            const float beta = 0.46f;
            return alpha - beta * (float)Math.Cos((2 * Math.PI * n) / (N - 1));
        }

        /// <summary>
        /// Computes an Fast Fourier Transform.
        /// </summary>
        /// <param name="data">Array of complex numbers. This array provides the input data and is used to store the result of the FFT.</param>
        /// <param name="exponent">The exponent n.</param>
        /// <param name="mode">The <see cref="FftMode"/> to use. Use <see cref="FftMode.Forward"/> as the default value.</param>
        public static void Fft(Complex[] data, int exponent, FftMode mode = FftMode.Forward)
        {
            //count; if exponent = 12 -> c = 2^12 = 4096
            int c = (int)Math.Pow(2, exponent);

            //binary inversion
            Inverse(data, c);

            int j0, j1, j2 = 1;
            float n0, n1, tr, ti, m;
            float v0 = -1, v1 = 0;

            //move to outer scope to optimize performance
            int j, i;

            for (int l = 0; l < exponent; l++)
            {
                n0 = 1;
                n1 = 0;
                j1 = j2;
                j2 <<= 1; //j2 * 2

                for (j = 0; j < j1; j++)
                {
                    for (i = j; i < c; i += j2)
                    {
                        j0 = i + j1;
                        //--
                        tr = n0 * data[j0].Real - n1 * data[j0].Imaginary;
                        ti = n0 * data[j0].Imaginary + n1 * data[j0].Real;
                        //--
                        data[j0].Real = data[i].Real - tr;
                        data[j0].Imaginary = data[i].Imaginary - ti;
                        //add
                        data[i].Real += tr;
                        data[i].Imaginary += ti;
                    }

                    //calc coeff
                    m = v0 * n0 - v1 * n1;
                    n1 = v1 * n0 + v0 * n1;
                    n0 = m;
                }

                if (mode == FftMode.Forward)
                {
                    v1 = (float)Math.Sqrt((1f - v0) / 2f);
                }
                else
                {
                    v1 = (float)-Math.Sqrt((1f - v0) / 2f);
                }
                v0 = (float)Math.Sqrt((1f + v0) / 2f);
            }

            if (mode == FftMode.Forward)
            {
                Forward(data, c);
            }
        }

        /// <summary>
        /// Computes an Fast Fourier Transform.
        /// </summary>
        /// <param name="data">Array of complex numbers. This array provides the input data and is used to store the result of the FFT.</param>
        /// <param name="exponent">The exponent n.</param>
        /// <param name="mode">The <see cref="FftMode"/> to use. Use <see cref="FftMode.Forward"/> as the default value.</param>
        [Obsolete("Use FastFourierTransform.Fft instead.")]
        public static void FFT1(Complex[] data, int exponent, FftMode mode)
        {
            Fft(data, exponent, mode);
        }

        private static void Forward(Complex[] data, int count)
        {
            int length = count;
            for (int i = 0; i < length; i++)
            {
                data[i].Real /= length;
                data[i].Imaginary /= length;
            }
        }

        private static void Inverse(Complex[] data, int c)
        {
            int z = 0;
            int n1 = c >> 1; //c / 2

            for (int n0 = 0; n0 < c - 1; n0++)
            {
                if (n0 < z)
                {
                    Swap(data, n0, z);
                }
                int l = n1;

                while (l <= z)
                {
                    z = z - l;
                    l >>= 1;
                }
                z += l;
            }
        }

        private static void Swap(Complex[] data, int index, int index2)
        {
            Complex tmp = data[index];
            data[index] = data[index2];
            data[index2] = tmp;
        }
    }
}