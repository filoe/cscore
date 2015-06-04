using System;

namespace CSCore.DSP
{
    /// <summary>
    /// Represents an BiQuad filter with a equalizer configuration.
    /// </summary>
    public class EqualizerBiQuadFilter : BiQuad
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualizerBiQuadFilter"/> class.
        /// </summary>
        /// <param name="sampleRate">The sampleRate of the audio data to process.</param>
        /// <param name="frequency">The center frequency to adjust.</param>
        /// <param name="bandWidth">The bandWidth.</param>
        /// <param name="peakGainDB">The gain value in dB.</param>
        public EqualizerBiQuadFilter(int sampleRate, double frequency, double bandWidth, double peakGainDB)
        {
            if(sampleRate <= 0)
                throw new ArgumentOutOfRangeException("sampleRate");
            if(frequency <= 0)
                throw new ArgumentOutOfRangeException("frequency");
            if (sampleRate < frequency * 2)
                throw new ArgumentOutOfRangeException("sampleRate", "The sampleRate has to be bigger than 2 * frequency.");

            if(bandWidth <= 0)
                throw new ArgumentOutOfRangeException("bandWidth");

            Q = bandWidth;
            Fc = frequency / sampleRate;
            GainDB = peakGainDB;

            _frequency = frequency;
            _sampleRate = sampleRate;

            CalculateBiQuadCoefficients();
        }

        private readonly double _frequency;
        private readonly int _sampleRate;

        /// <summary>
        /// Gets or sets the bandwidth.
        /// </summary>
        public double BandWidth
        {
            get { return Q; }
            set
            {
                if(value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                Q = value;
                CalculateBiQuadCoefficients();
            }
        }

        /// <summary>
        /// Gets or sets the gain value in dB.
        /// </summary>
        public new double GainDB
        {
            get { return base.GainDB; }
            set
            {
                base.GainDB = value;
                CalculateBiQuadCoefficients();
            }
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        public double Frequency
        {
            get { return _frequency; }
        }

        /// <summary>
        /// Gets the samplerate.
        /// </summary>
        public int SampleRate
        {
            get { return _sampleRate; }
        }
    }
}