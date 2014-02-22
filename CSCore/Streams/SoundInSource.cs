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

        /// <summary>
        /// Creates an new instance of SoundInSource with a default bufferSize of 5 seconds.
        /// </summary>
        /// <param name="soundIn">The soundIn which provides recorded data.</param>
        /// <remarks>
        /// Note that soundIn has to be already initialized.
        /// Note that old data ("old" gets specified by the bufferSize) gets overridden. 
        /// For example, if the bufferSize is about 5 seconds big, data which got recorded 6 seconds ago, won't be available anymore.
        /// </remarks>
        public SoundInSource(ISoundIn soundIn)
            : this(soundIn, soundIn.WaveFormat.BytesPerSecond * 5)
        {
        }

        /// <summary>
        /// Creates an new instance of SoundInSource with a default bufferSize of 5 seconds.
        /// </summary>
        /// <param name="soundIn">The soundIn which provides recorded data.</param>
        /// <param name="bufferSize">Size of the buffer in bytes.</param>
        /// <remarks>
        /// Note that soundIn has to be already initialized.
        /// Note that old data ("old" gets specified by the bufferSize) gets overridden. 
        /// For example, if the bufferSize is about 5 seconds big, data which got recorded 6 seconds ago, won't be available anymore.
        /// </remarks>
        public SoundInSource(ISoundIn soundIn, int bufferSize)
        {
            if (soundIn == null)
                throw new ArgumentNullException("soundIn");
            //bufferSize gets validated by WriteableBufferingSource

            _buffer = new WriteableBufferingSource(soundIn.WaveFormat, bufferSize);
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
            int read = _buffer.Read(buffer, offset, count);

            if(FillWithZeros && read < count)
                Array.Clear(buffer, offset + read, count - read);

            return FillWithZeros ? count : read;
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

        public bool FillWithZeros { get; set; }

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