using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.DSP
{
    //Based on http://www.earlevel.com/main/2011/01/02/biquad-formulas/
    public class BiQuad
    {
        protected double a0, a1, a2, b1, b2;
        protected double fc, Q, peakGain;
        protected double z1, z2;

        public static BiQuad CreatePeakEQFilter(int sampleRate, double frequency, double bandWidth, double peakGainDB)
        {
            BiQuad res = new BiQuad();
            res.Q = bandWidth;
            res.fc = frequency / sampleRate;
            res.peakGain = peakGainDB;
            res.CalcBiquad();
            return res;
        }

        private BiQuad()
        {
            
        }

        public float Process(float input)
        {
            double o = input * a0 + z1;
            z1 = input * a1 + z2 - b1 * o;
            z2 = input * a2 - b2 * o;
            return (float)o;
        }

        public void Process(float[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Process(input[i]);
            }
        }

        public void SetPeakGain(double peakGainDB)
        {
            peakGain = peakGainDB;
            CalcBiquad();
        }

        public void SetQ(float q)
        {
            Q = q;
            CalcBiquad();
        }

        public void SetFrequency(double frequency, int sampleRate)
        {
            this.fc = frequency / sampleRate;
            CalcBiquad();
        }

        private void CalcBiquad()
        {
            double norm;
            double V = Math.Pow(10, Math.Abs(peakGain) / 20.0);
            double K = Math.Tan(Math.PI * fc);
            if (peakGain >= 0)
            {
                norm = 1 / (1 + 1 / Q * K + K * K);
                a0 = (1 + V / Q * K + K * K) * norm;
                a1 = 2 * (K * K - 1) * norm;
                a2 = (1 - V / Q * K + K * K) * norm;
                b1 = a1;
                b2 = (1 - 1 / Q * K + K * K) * norm;
            }
            else
            {
                norm = 1 / (1 + V / Q * K + K * K);
                a0 = (1 + 1 / Q * K + K * K) * norm;
                a1 = 2 * (K * K - 1) * norm;
                a2 = (1 - 1 / Q * K + K * K) * norm;
                b1 = a1;
                b2 = (1 - V / Q * K + K * K) * norm;
            }
        }
    }
}
