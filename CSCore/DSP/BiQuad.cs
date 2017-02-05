/*
 * These implementations are based on http://www.earlevel.com/main/2011/01/02/biquad-formulas/
 */

using System;

namespace CSCore.DSP
{
    /// <summary>
    /// Represents a biquad-filter.
    /// </summary>
    public abstract class BiQuad
    {
        /// <summary>
        /// The a0 value.
        /// </summary>
        protected double A0;
        /// <summary>
        /// The a1 value.
        /// </summary>
        protected double A1;
        /// <summary>
        /// The a2 value.
        /// </summary>
        protected double A2;
        /// <summary>
        /// The b1 value.
        /// </summary>
        protected double B1;
        /// <summary>
        /// The b2 value.
        /// </summary>
        protected double B2;
        /// <summary>
        /// The q value.
        /// </summary>
        private double _q;
        /// <summary>
        /// The gain value in dB.
        /// </summary>
        private double _gainDB;
        /// <summary>
        /// The z1 value.
        /// </summary>
        protected double Z1;
        /// <summary>
        /// The z2 value.
        /// </summary>
        protected double Z2;

        private double _frequency;

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">value;The samplerate has to be bigger than 2 * frequency.</exception>
        public double Frequency
        {
            get { return _frequency; }
            set
            {
                if (SampleRate < value * 2)
                {
                    throw new ArgumentOutOfRangeException("value", "The samplerate has to be bigger than 2 * frequency.");
                }
                _frequency = value;
                CalculateBiQuadCoefficients();
            }
        }

        /// <summary>
        /// Gets the sample rate.
        /// </summary>
        public int SampleRate { get; private set; }

        /// <summary>
        /// The q value.
        /// </summary>
        public double Q
        {
            get { return _q; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _q = value;
                CalculateBiQuadCoefficients();
            }
        }

        /// <summary>
        /// Gets or sets the gain value in dB.
        /// </summary>
        public double GainDB
        {
            get { return _gainDB; }
            set
            {
                _gainDB = value;
                CalculateBiQuadCoefficients();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiQuad"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The frequency.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// sampleRate
        /// or
        /// frequency
        /// or
        /// q
        /// </exception>
        protected BiQuad(int sampleRate, double frequency)
            : this(sampleRate, frequency, 1.0 / Math.Sqrt(2))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BiQuad"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="q">The q.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// sampleRate
        /// or
        /// frequency
        /// or
        /// q
        /// </exception>
        protected BiQuad(int sampleRate, double frequency, double q)
        {
            if (sampleRate <= 0)
                throw new ArgumentOutOfRangeException("sampleRate");
            if (frequency <= 0)
                throw new ArgumentOutOfRangeException("frequency");
            if (q <= 0)
                throw new ArgumentOutOfRangeException("q");
            SampleRate = sampleRate;
            Frequency = frequency;
            Q = q;
            GainDB = 6;
        }

        /// <summary>
        /// Processes a single <paramref name="input"/> sample and returns the result.
        /// </summary>
        /// <param name="input">The input sample to process.</param>
        /// <returns>The result of the processed <paramref name="input"/> sample.</returns>
        public float Process(float input)
        {
            double o = input * A0 + Z1;
            Z1 = input * A1 + Z2 - B1 * o;
            Z2 = input * A2 - B2 * o;
            return (float)o;
        }

        /// <summary>
        /// Processes multiple <paramref name="input"/> samples.
        /// </summary>
        /// <param name="input">The input samples to process.</param>
        /// <remarks>The result of the calculation gets stored within the <paramref name="input"/> array.</remarks>
        public void Process(float[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Process(input[i]);
            }
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected abstract void CalculateBiQuadCoefficients();
    }
}
