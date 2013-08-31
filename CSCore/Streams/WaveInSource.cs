using CSCore.SoundIn;
using System;

namespace CSCore.Streams
{
    public class WaveInSource : IWaveSource
    {
        private ISoundIn _soundIn;
        private BufferingSource _buffer;

        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        public ISoundIn SoundIn
        {
            get { return _soundIn; }
        }

        public WaveInSource(WaveIn soundIn)
        {
            if (soundIn == null)
                throw new ArgumentNullException("soundIn");

            _buffer = new BufferingSource(soundIn.WaveFormat);
            _buffer.FillWithZeros = false;
            _soundIn = soundIn;
            _soundIn.DataAvailable += WaveinDataAvailable;
        }

        private void WaveinDataAvailable(object sender, DataAvailableEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("WaveinDataAvailable");
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
                    SoundIn.DataAvailable -= WaveinDataAvailable;
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

        ~WaveInSource()
        {
            Dispose(false);
        }
    }
}