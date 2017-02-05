/*
 * These implementations are based on http://www.earlevel.com/main/2011/01/02/biquad-formulas/
 */

using System;

namespace CSCore.DSP
{
    /// <summary>
    /// Used to apply a notch-filter to a signal.
    /// </summary>
    public class NotchFilter : BiQuad
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotchFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="frequency">The filter's corner frequency.</param>
        public NotchFilter(int sampleRate, double frequency) 
            : base(sampleRate, frequency)
        {
        }

        /// <summary>
        /// Calculates all coefficients.
        /// </summary>
        protected override void CalculateBiQuadCoefficients()
        {
            double k = Math.Tan(Math.PI * Frequency / SampleRate);
            double norm = 1 / (1 + k / Q + k * k);
            A0 = (1 + k * k) * norm;
            A1 = 2 * (k * k - 1) * norm;
            A2 = A0;
            B1 = A1;
            B2 = (1 - k / Q + k * k) * norm;
        }
    }
}