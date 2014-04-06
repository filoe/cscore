using CSCore.Utils.Buffer;
using System;

namespace CSCore.Streams
{
    /// <summary>
    /// Buffered WaveSource which overrides the allocated memory after the buffer got full. 
    /// </summary>
    public class WriteableBufferingSource : IWaveSource
    {
        private WaveFormat _waveFormat;
        private FixedSizeBuffer<byte> _buffer;
        private volatile object _bufferlock = new object();

        public bool FillWithZeros { get; set; }

        /// <summary>
        /// Creates an new instance of the WriteableBufferingSource class with a default Buffersize of 5 seconds.
        /// </summary>
        /// <param name="waveFormat">The WaveFormat of the source.</param>
        public WriteableBufferingSource(WaveFormat waveFormat)
            : this(waveFormat, waveFormat.BytesPerSecond * 5)
        {
        }

        /// <summary>
        /// Creates an new instance of the WriteableBufferingSource class.
        /// </summary>
        /// <param name="waveFormat">The WaveFormat of the source.</param>
        /// <param name="bufferSize">Buffersize in bytes</param>
        public WriteableBufferingSource(WaveFormat waveFormat, int bufferSize)
        {
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");
            if (bufferSize <= 0 || (bufferSize % waveFormat.BlockAlign) != 0)
                throw new ArgumentException("Invalid bufferSize.");

            _waveFormat = waveFormat;
            _buffer = new FixedSizeBuffer<byte>(bufferSize);
            FillWithZeros = true;   
        }

        public int Write(byte[] buffer, int offset, int count)
        {
            lock (_bufferlock)
            {
                return _buffer.Write(buffer, offset, count);
            }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            lock (_bufferlock)
            {
                int read = _buffer.Read(buffer, offset, count);
                if (FillWithZeros)
                {
                    if (read < count)
                        Array.Clear(buffer, offset + read, count - read);
                    return count;
                }
                else
                {
                    return read;
                }
            }
        }

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
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
            if (disposing)
            {
                //dispose managed
                _buffer.Dispose();
                _buffer = null;
            }
        }

        ~WriteableBufferingSource()
        {
            Dispose(false);
        }
    }
}