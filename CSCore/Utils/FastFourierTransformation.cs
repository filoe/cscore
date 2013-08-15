using System;

namespace CSCore.Utils
{
    public static class FastFourierTransformation
    {
        private const double minDB = -96;

        private const double minValueRaw = 0.0000677287; // - 96dB
        private const double minLog = minDB / 10;

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
            double intensity = 10 * Math.Log(CSCore.Utils.FastFourierTransformation.GetIntensity(c));
            if (intensity < minDB) intensity = minDB;

            double perc = intensity / minDB;
            return 1 - Math.Abs(perc);
        }

        public static unsafe void DoFFT(Complex[] data, int blockSize, bool forward)
        {
            int potenz = (int)Math.Log(blockSize, 2);

            Reverse(data, blockSize);

            int tn, tm;
            tn = 1;

            for (int j = 1; j <= potenz; j++)
            {
                Complex[] rComplex = Roatate(j, 1);
                tm = tn;
                tn <<= 1;

                for (int i = 0; i < tm; i++)
                {
                    Complex t = rComplex[i];

                    for (int e = i; e < blockSize; e += tn)
                    {
                        int o = e + tm;
                        Complex ce = data[e];
                        Complex co = data[o];

                        double tr = co.Real * t.Real - co.Imaginary * t.Imaginary;
                        double ti = co.Real * t.Imaginary + co.Imaginary * t.Real;

                        data[e].Real += tr;
                        data[o].Real = ce.Real - tr;

                        data[e].Imaginary += ti;
                        data[o].Imaginary = ce.Imaginary - ti;
                    }
                }
            }
            if (forward)
                Forward(data);
        }

        private static void Forward(Complex[] data)
        {
            int length = data.Length;
            for (int i = 0; i < length; i++)
            {
                data[i].Real /= length;
                data[i].Imaginary /= length;
            }
        }

        private static void Reverse(Complex[] data, int blockSize)
        {
            int j = 0;
            int i2 = blockSize >> 1;
            int l = 0;

            for (int i = 0; i < blockSize - 1; i++)
            {
                if (i < j)
                {
                    SWAP(data, i, j);
                }
                l = i2;

                while (l <= j)
                {
                    j = j - l;
                    l >>= 1;
                }
                j += l;
            }
        }

        private static Complex[] Roatate(int logIndex, int direction)
        {
            double uReal, uImg, angleReal, angleImg, angle, t;
            int n = 1 << (logIndex - 1);
            angle = (Math.PI / n) * direction;
            angleReal = Math.Cos(angle); angleImg = Math.Sin(angle);
            uReal = 1.0; uImg = 0.0;

            Complex[] tmp = new Complex[n];
            for (int i = 0; i < n; i++)
            {
                tmp[i].Real = uReal;
                tmp[i].Imaginary = uImg;

                t = uReal * angleImg + uImg * angleReal;
                uReal = uReal * angleReal - uImg * angleImg;
                uImg = t;
            }
            return tmp;
        }

        private static void SWAP(Complex[] data, int index, int index2)
        {
            Complex tmp = data[index];
            data[index] = data[index2];
            data[index2] = tmp;
        }
    }
}