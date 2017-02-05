/*
 * These implementations are based on http://www.earlevel.com/main/2011/01/02/biquad-formulas/
 */

using System;

namespace CSCore.DSP
{
    /// <summary>
    /// Used to apply an peak-filter to a signal.
    /// </summary>
    public class PeakFilter : BiQuad
    {
        /// <summary>
        /// Gets or sets the bandwidth.
        /// </summary>
        public double BandWidth
        {
            get { return Q; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                Q = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeakFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sampleRate of the audio data to process.</param>
        /// <param name="frequency">The center frequency to adjust.</param>
        /// <param name="bandWidth">The bandWidth.</param>
        /// <param name="peakGainDB">The gain value in dB.</param>
        public PeakFilter(int sampleRate, double frequency, double bandWidth, double peakGainDB)
            : base(sampleRate, frequency, bandWidth)
        {
            GainDB = peakGainDB;
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected override void CalculateBiQuadCoefficients()
        {
            double norm;
            double v = Math.Pow(10, Math.Abs(GainDB) / 20.0);
            double k = Math.Tan(Math.PI * Frequency / SampleRate);
            double q = Q;

            if (GainDB >= 0) //boost
            {
                norm = 1 / (1 + 1 / q * k + k * k);
                A0 = (1 + v / q * k + k * k) * norm;
                A1 = 2 * (k * k - 1) * norm;
                A2 = (1 - v / q * k + k * k) * norm;
                B1 = A1;
                B2 = (1 - 1 / q * k + k * k) * norm;
            }
            else //cut
            {
                norm = 1 / (1 + v / q * k + k * k);
                A0 = (1 + 1 / q * k + k * k) * norm;
                A1 = 2 * (k * k - 1) * norm;
                A2 = (1 - 1 / q * k + k * k) * norm;
                B1 = A1;
                B2 = (1 - v / q * k + k * k) * norm;
            }
        }
    }
}