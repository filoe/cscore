using System;

namespace CSCore.Utils
{
    public static class FastFourierTransformation
    {
        private const double MinDB = -96;

        private const double MinValueRaw = 0.0000677287; // - 96dB
        private const double MinLog = MinDB / 10;

        public static double GetIntensity(Complex c)
        {
            return Math.Sqrt(c.Real * c.Real + c.Imaginary * c.Imaginary);
        }

        public static double HammingWindow(int n, int frameSize)
        {
            return 0.54 - 0.46 * Math.Cos((2 * Math.PI * n) / (frameSize - 1));
        }

        public static float HammingWindowF(int n, int frameSize)
        {
            return 0.54f - 0.46f * (float)Math.Cos((2 * Math.PI * n) / (frameSize - 1));
        }

        public static double CalculatePercentage(Complex c)
        {
            double intensity = 10 * Math.Log(GetIntensity(c));
            if (intensity < MinDB) intensity = MinDB;

            double perc = intensity / MinDB;
            return 1 - Math.Abs(perc);
        }

        public static void FFT1(Complex[] data, int exponent, FFTMode mode)
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

                if (mode == FFTMode.Forward)
                {
                    v1 = (float)Math.Sqrt((1f - v0) / 2f);
                }
                else
                {
                    v1 = (float)-Math.Sqrt((1f - v0) / 2f);
                }
                v0 = (float)Math.Sqrt((1f + v0) / 2f);
            }

            if (mode == FFTMode.Forward)
            {
                Forward(data, c);
            }
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
            int l = 0;

            for (int n0 = 0; n0 < c - 1; n0++)
            {
                if (n0 < z)
                {
                    Swap(data, n0, z);
                }
                l = n1;

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

    public enum FFTMode
    {
        Forward,
        Backward
    }
}