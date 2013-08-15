using System;

namespace CSCore.Streams
{
    public class SineGenerator : ISampleSource
    {
        private double _frequency = 1000;
        private double _amplitude = 0.5;
        private double _phase = 0;

        private WaveFormat _waveFormat;

        /// <summary>
        /// 1000Hz, 0.5 Amplitude, 0.0 phase
        /// </summary>
        public SineGenerator()
        {
            _waveFormat = new WaveFormat(44100, 32, 1, AudioEncoding.IeeeFloat);
        }

        public SineGenerator(double frequency, double amplitude, double phase)
            : this()
        {
            _frequency = frequency;
            _amplitude = amplitude;
            _phase = phase;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (_phase == 1f)
                _phase = 0;

            for (int i = offset; i < count; i++)
            {
                float sine = (float)(_amplitude * Math.Sin(_frequency * _phase * Math.PI * 2));
                buffer[i] = sine;

                _phase += (1.0 / WaveFormat.SampleRate);
            }

            return count;
        }

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public long Position
        {
            get { return 0; }
            set { throw new InvalidOperationException(); }
        }

        public long Length
        {
            get { return 0; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //dispose managed
            }
            //keine ressourcen zum freigeben
        }

        ~SineGenerator()
        {
            Dispose(false);
        }
    }
}