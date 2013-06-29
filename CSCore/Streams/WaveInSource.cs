using CSCore.SoundIn;
using System;

namespace CSCore.Streams
{
    public class WaveInSource : IWaveSource
    {
        WaveIn _wavein;
        BufferingSource _buffer;

        public WaveIn WaveIn
        {
            get { return _wavein; }
        }

        public WaveInSource(WaveIn wavein)
        {
            if (wavein == null)
                throw new ArgumentNullException("wavein");

            _buffer = new BufferingSource(wavein.WaveFormat);
            _wavein = wavein;
            _wavein.DataAvailable += WaveinDataAvailable;
        }

        private void WaveinDataAvailable(object sender, DataAvailableEventArgs e)
        {
            int written = _buffer.Write(e.Data, 0, e.ByteCount);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int read = _buffer.Read(buffer, 0, count);
            return count;
        }

        public WaveFormat WaveFormat
        {
            get { return WaveIn.WaveFormat; }
        }

        public long Position
        {
            get
            {
                return -1;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public long Length
        {
            get { return -1; }
        }

        public void Dispose()
        {
            Dispose(true);
			GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (WaveIn != null)
            {
                WaveIn.DataAvailable -= WaveinDataAvailable;
                _wavein = null;
            }

            if (_buffer != null)
            {
                _buffer.Dispose();
                _buffer = null;
            }
        }

        ~WaveInSource()
        {
            Dispose(false);
        }
    }
}
