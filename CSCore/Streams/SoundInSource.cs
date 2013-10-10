using CSCore.SoundIn;
using System;

namespace CSCore.Streams
{
    public class SoundInSource : IWaveSource
    {
        private ISoundIn _soundIn;
        private WriteableBufferingSource _buffer;

        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        public ISoundIn SoundIn
        {
            get { return _soundIn; }
        }

        public SoundInSource(ISoundIn soundIn)
        {
            if (soundIn == null)
                throw new ArgumentNullException("soundIn");

            _buffer = new WriteableBufferingSource(soundIn.WaveFormat);
            _buffer.FillWithZeros = false;
            _soundIn = soundIn;
            _soundIn.DataAvailable += OnDataAvailable;
        }

        private void OnDataAvailable(object sender, DataAvailableEventArgs e)
        {
            int written = _buffer.Write(e.Data, 0, e.ByteCount);
            if (e.ByteCount > 0 && DataAvailable != null)
                DataAvailable(this, e);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int read = _buffer.Read(buffer, 0, count);
            return read;
        }

        public WaveFormat WaveFormat
        {
            get { return SoundIn.WaveFormat; }
        }

        public long Position
        {
            get
            {
                return 0;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public long Length
        {
            get { return 0; }
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (SoundIn != null)
                {
                    SoundIn.DataAvailable -= OnDataAvailable;
                    _soundIn = null;
                }

                if (_buffer != null)
                {
                    _buffer.Dispose();
                    _buffer = null;
                }
            }
            _disposed = true;
        }

        ~SoundInSource()
        {
            Dispose(false);
        }
    }
}