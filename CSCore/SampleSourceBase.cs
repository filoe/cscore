using CSCore.Streams.SampleConverter;
using CSCore.Utils.Buffer;
using System;

namespace CSCore
{
    public class SampleSourceBase : ISampleSource
    {
        protected ISampleSource _source;

        public SampleSourceBase(IWaveStream source)
        {
            if (source == null) throw new ArgumentNullException("source");

            if (source is ISampleSource)
                _source = (source as ISampleSource);
            else
            {
                _source = WaveToSampleBase.CreateConverter(source as IWaveSource);
            }
        }

        public virtual int Read(float[] buffer, int offset, int count)
        {
            if (offset % WaveFormat.Channels != 0)
                throw new ArgumentOutOfRangeException("offset");
            if (count % WaveFormat.Channels != 0)
                throw new ArgumentOutOfRangeException("count");

            return _source.Read(buffer, offset, count);
        }

        public virtual int Read(byte[] buffer, int offset, int count)
        {
            UnsafeBuffer ubuffer = new UnsafeBuffer(buffer);
            int read = Read(ubuffer.FloatBuffer, offset / 4, count / 4);
            return read * 4;
        }

        public virtual WaveFormat WaveFormat
        {
            get { return _source.WaveFormat; }
        }

        public virtual long Position
        {
            get
            {
                return _source.Position;
            }
            set
            {
                _source.Position = value;
            }
        }

        public virtual long Length
        {
            get { return _source.Length; }
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

        ~SampleSourceBase()
        {
            Dispose(false);
        }
    }
}