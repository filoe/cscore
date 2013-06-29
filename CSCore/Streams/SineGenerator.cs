using System;

namespace CSCore.Streams
{
    public class SineGenerator : ISampleSource
    {
        double _frequency = 1000;
        double _amplitude = 0.5;
        double _time = 0;

        WaveFormat _waveFormat;

        /// <summary>
        /// 1000Hz, 0.5 Amplitude
        /// </summary>
        public SineGenerator()
        {
            _waveFormat = new WaveFormat(44100, 32, 1, AudioEncoding.IeeeFloat);
        }

        public SineGenerator(double frequency, double amplitude, double time)
            : this()
        {
            _frequency = frequency;
            _amplitude = amplitude;
            _time = time;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (_time == 1f)
                _time = 0;

            for (int i = offset; i < count; i++)
            {
                float sine = (float)(_amplitude * Math.Sin(_frequency * _time * Math.PI * 2));
                buffer[i] = sine;

                _time += (1.0 / WaveFormat.SampleRate);
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
			if(disposing)
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
