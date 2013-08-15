using System;

namespace CSCore.Streams.SampleConverter
{
    public abstract class SampleToWaveBase : IWaveSource
    {
        private WaveFormat _waveFormat;
        protected ISampleSource _source;
        private double _ratio;

        public SampleToWaveBase(ISampleSource source, int bits, AudioEncoding encoding)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (bits < 1)
                throw new ArgumentOutOfRangeException("bits");

            _waveFormat = new WaveFormat(source.WaveFormat.SampleRate, (short)bits, source.WaveFormat.Channels, encoding);
            _source = source;
            _ratio = 32.0 / bits;
        }

        public abstract int Read(byte[] buffer, int offset, int count);

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public virtual long Position
        {
            get { return (long)(_source.Position * _ratio); }
            set { _source.Position = (long)(value / _ratio); }
        }

        public virtual long Length
        {
            get { return (long)(_source.Length * _ratio); }
        }

        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            _source.Dispose();
        }

        ~SampleToWaveBase()
        {
            Dispose(false);
        }
    }
}