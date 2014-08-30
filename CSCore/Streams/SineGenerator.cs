using System;

namespace CSCore.Streams
{
    /// <summary>
    /// Generates a sine wave.
    /// </summary>
    public class SineGenerator : ISampleSource
    {
        /// <summary>
        /// Gets or sets the frequency of the sine wave.
        /// </summary>
        public double Frequency
        {
            get { return _frequency; }
            set
            {
                if(value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _frequency = value;
            }
        }

        /// <summary>
        /// Gets or sets the amplitude of the sine wave.
        /// </summary>
        public double Amplitude
        {
            get { return _amplitude; }
            set
            {
                if(value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("value");
                _amplitude = value;
            }
        }

        /// <summary>
        /// Gets or sets the phase of the sine wave.
        /// </summary>
        public double Phase { get; set; }


        private readonly WaveFormat _waveFormat;
        private double _frequency;
        private double _amplitude;

        /// <summary>
        /// 1000Hz, 0.5 amplitude, 0.0 phase
        /// </summary>
        public SineGenerator()
            : this(1000, 0.5, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SineGenerator"/> class.
        /// </summary>
        /// <param name="frequency">Specifies the frequency of the sine wave in Hz.</param>
        /// <param name="amplitude">Specifies the amplitude of the sine wave. Use a value between 0 and 1.</param>
        /// <param name="phase">Specifies the initial phase. Use a value between 0 and 1.</param>
        public SineGenerator(double frequency, double amplitude, double phase)
        {
            if(frequency <= 0)
                throw new ArgumentOutOfRangeException("frequency");
            if(amplitude < 0 || amplitude > 1)
                throw new ArgumentOutOfRangeException("amplitude");

            Frequency = frequency;
            Amplitude = amplitude;
            Phase = phase;

            _waveFormat = new WaveFormat(44100, 32, 1, AudioEncoding.IeeeFloat);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (Phase > 1)
                Phase = 0;

            for (int i = offset; i < count; i++)
            {
                float sine = (float)(Amplitude * Math.Sin(Frequency * Phase * Math.PI * 2));
                buffer[i] = sine;

                Phase += (1.0 / WaveFormat.SampleRate);
            }

            return count;
        }

        /// <summary>
        ///     Gets the <see cref="IWaveStream.WaveFormat" /> of the waveform-audio data.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public long Position
        {
            get { return 0; }
            set { throw new InvalidOperationException(); }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public long Length
        {
            get { return 0; }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="disposing">Not used.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose managed
            }
        }

        ~SineGenerator()
        {
            Dispose(false);
        }
    }
}