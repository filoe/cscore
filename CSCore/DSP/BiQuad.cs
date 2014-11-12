using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSCore.DSP
{
    //Based on http://www.earlevel.com/main/2011/01/02/biquad-formulas/
    /// <summary>
    /// Represents a biquad-filter.
    /// </summary>
    public class BiQuad
    {
        protected double A0, A1, A2, B1, B2;
        protected double Fc, Q, GainDB;
        protected double Z1, Z2;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiQuad"/> class.
        /// </summary>
        protected BiQuad()
        {            
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
        protected void CalculateBiQuadCoefficients()
        {
            double norm;
            double v = Math.Pow(10, Math.Abs(GainDB) / 20.0);
            double k = Math.Tan(Math.PI * Fc);
            if (GainDB >= 0)
            {
                norm = 1 / (1 + 1 / Q * k + k * k);
                A0 = (1 + v / Q * k + k * k) * norm;
                A1 = 2 * (k * k - 1) * norm;
                A2 = (1 - v / Q * k + k * k) * norm;
                B1 = A1;
                B2 = (1 - 1 / Q * k + k * k) * norm;
            }
            else
            {
                norm = 1 / (1 + v / Q * k + k * k);
                A0 = (1 + 1 / Q * k + k * k) * norm;
                A1 = 2 * (k * k - 1) * norm;
                A2 = (1 - 1 / Q * k + k * k) * norm;
                B1 = A1;
                B2 = (1 - v / Q * k + k * k) * norm;
            }
        }
    }
}
