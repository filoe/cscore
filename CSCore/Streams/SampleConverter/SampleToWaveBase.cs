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

        /*public virtual long Position
        {
            get { return InputToOutput(_source.Position); }
            set { _source.Position = OutputToInput(value); }
        }

        public virtual long Length
        {
            get { return InputToOutput(_source.Length); }
        }*/

        public long Position
        {
            get { return _source.Position * WaveFormat.BytesPerSample; }
            set { _source.Position = value / WaveFormat.BytesPerSample; }
        }

        public long Length
        {
            get { return _source.Length * WaveFormat.BytesPerSample; }
        }

        internal long InputToOutput(long position)
        {
            //long result = (long)(position * _ratio);
            long result = (long)(position * _ratio);
            result -= (result % _waveFormat.BlockAlign);
            return result;
        }

        internal long OutputToInput(long position)
        {
            //long result = (long)(position * _ratio);
            long result = (long)(position / _ratio);
            result -= (result % _source.WaveFormat.BlockAlign);
            return result;
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