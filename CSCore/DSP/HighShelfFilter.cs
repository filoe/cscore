/*
 * These implementations are based on http://www.earlevel.com/main/2011/01/02/biquad-formulas/
 */

using System;

namespace CSCore.DSP
{
    /// <summary>
    /// Used to apply a highshelf-filter to a signal.
    /// </summary>
    public class HighShelfFilter : BiQuad
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HighShelfFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The filter's corner frequency.</param>
        /// <param name="gainDB">Gain value in dB.</param>
        public HighShelfFilter(int sampleRate, double frequency, double gainDB)
            : base(sampleRate, frequency)
        {
            GainDB = gainDB;
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected override void CalculateBiQuadCoefficients()
        {
            const double sqrt2 = 1.4142135623730951;
            double k = Math.Tan(Math.PI * Frequency / SampleRate);
            double v = Math.Pow(10, Math.Abs(GainDB) / 20.0);
            double norm;
            if (GainDB >= 0)
            {    // boost
                norm = 1 / (1 + sqrt2 * k + k * k);
                A0 = (v + Math.Sqrt(2 * v) * k + k * k) * norm;
                A1 = 2 * (k * k - v) * norm;
                A2 = (v - Math.Sqrt(2 * v) * k + k * k) * norm;
                B1 = 2 * (k * k - 1) * norm;
                B2 = (1 - sqrt2 * k + k * k) * norm;
            }
            else
            {    // cut
                norm = 1 / (v + Math.Sqrt(2 * v) * k + k * k);
                A0 = (1 + sqrt2 * k + k * k) * norm;
                A1 = 2 * (k * k - 1) * norm;
                A2 = (1 - sqrt2 * k + k * k) * norm;
                B1 = 2 * (k * k - v) * norm;
                B2 = (v - Math.Sqrt(2 * v) * k + k * k) * norm;
            }
        }
    }
}