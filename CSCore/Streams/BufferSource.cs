using System.Diagnostics;
using CSCore.Utils.Buffer;
using System;
using System.Threading;

namespace CSCore.Streams
{
    /// <summary>
    /// Reads data from the <see cref="WaveAggregatorBase.BaseSource"/> and stores the read data in a buffer.
    /// </summary>
    public class BufferSource : WaveAggregatorBase
    {
        private readonly Thread _bufferThread;
        private readonly Object _lockObject;

        private readonly FixedSizeBuffer<byte> _buffer;
        private bool _disposing;
        private volatile int _eofCounter = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferSource"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IWaveSource"/> to buffer.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="bufferSize"/> is out of range.</exception>
        public BufferSource(IWaveSource source, int bufferSize)
            : base(source)
        {
            if (bufferSize <= 0 || bufferSize % source.WaveFormat.BlockAlign != 0)
                throw new ArgumentOutOfRangeException("bufferSize");

            _buffer = new FixedSizeBuffer<byte>(bufferSize);
            _lockObject = new Object();

            _bufferThread = new Thread(BufferProc)
            {
                Priority = ThreadPriority.Normal,
                IsBackground = false
            };
            _bufferThread.Start();
        }

        /// <summary>
        ///     Reads a sequence of bytes from internal buffer and advances the position by the
        ///     number of bytes read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of bytes. When this method returns, the <paramref name="buffer" /> contains the specified
        ///     byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        ///     <paramref name="count" /> - 1) replaced by the bytes read from the <see cref="BufferSource"/>.
        /// </param>
        /// <param name="offset">
        ///     The zero-based byte offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the <see cref="BufferSource"/>.</param>
        /// <returns>The total number of bytes read into the buffer.</returns>
        /// <exception cref="ObjectDisposedException">BufferSource</exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_disposing)
                throw new ObjectDisposedException("BufferSource");

            int read0 = 0, read1;
            Array.Clear(buffer, offset, count - offset);

            do
            {
                read1 = _buffer.Read(buffer, offset + read0, count - read0);
                read0 += read1;
            } while (read0 < count
                     && !(read1 <= 0 && _eofCounter >= 5)); //if the buffering thread could not read any data for 5 times, we abort here

            return read0;
        }

        /// <summary>
        /// Resets/Clears the buffer.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">BufferSource</exception>
        public void ResetBuffer()
        {
            if (_disposing)
                throw new ObjectDisposedException("BufferSource");

            lock (_lockObject)
            {
                _buffer.Clear();
            }
        }

        private void BufferProc(object o)
        {
            byte[] byteBuffer = new byte[WaveFormat.BytesPerSecond / 2];
            do
            {
                if (_buffer.Buffered >= _buffer.Length * 0.85 && !_disposing)
                {
                    Thread.Sleep(50);
                    continue;
                }
                lock (_lockObject)
                {
                    int bytesToRead = Math.Min(byteBuffer.Length, _buffer.Length - _buffer.Buffered);
                    int read = base.Read(byteBuffer, 0, bytesToRead);
                    if (read > 0)
                    {
                        _buffer.Write(byteBuffer, 0, read);
                        _eofCounter = 0;
                    }
                    else
                    {
                        _eofCounter++;
                    }
                }
            } while (!_disposing);
        }

        /// <summary>
        /// Gets or sets the position of the source.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// BufferSource
        /// </exception>
        public override long Position
        {
            get
            {
                if (_disposing)
                    throw new ObjectDisposedException("BufferSource");

                lock (_lockObject)
                {
                    if (CanSeek)
                        return Math.Max(0, Math.Min(base.Position - _buffer.Buffered, Length));
                    return 0;
                }
            }
            set
            {
                if (_disposing)
                    throw new ObjectDisposedException("BufferSource");

                lock (_lockObject)
                {
                    base.Position = value;
                    ResetBuffer();
                }
            }
        }

        /// <summary>
        ///     Gets the length of the source.
        /// </summary>
        public override long Length
        {
            get
            {
                if (_disposing)
                    throw new ObjectDisposedException("BufferSource");

                lock (_lockObject)
                {
                    if(CanSeek)
                        return base.Length;
                    return 0;
                }
            }
        }

        /// <summary>
        ///     Disposes the <see cref="WaveAggregatorBase.BaseSource" /> and releases all allocated resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _disposing = true;
            if (_bufferThread.WaitForExit(400))
            {
                _buffer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
