using System;

namespace CSCore
{
    /// <summary>
    ///     Base class for all wave aggregators.
    /// </summary>
    public abstract class WaveAggregatorBase : IWaveAggregator
    {
        private IWaveSource _baseStream;
        private bool _disposeBaseSource = true;
        private bool _disposed;

        /// <summary>
        ///     Creates a new instance of WaveAggregatorBase.
        /// </summary>
        protected WaveAggregatorBase()
        {
        }

        /// <summary>
        ///     Creates a new instance of WaveAggregatorBase.
        /// </summary>
        /// <param name="baseStream">Underlying base stream.</param>
        protected WaveAggregatorBase(IWaveSource baseStream)
            : this()
        {
            if (baseStream == null)
                throw new ArgumentNullException("baseStream");

            _baseStream = baseStream;
        }

        /// <summary>
        ///     Gets or sets a value whether to dispose the <see cref="BaseStream" />
        ///     on calling <see cref="Dispose(bool)" />.
        /// </summary>
        protected bool DisposeBaseSource
        {
            get { return _disposeBaseSource; }
            set { _disposeBaseSource = value; }
        }

        /// <summary>
        ///     Gets or sets the underlying base stream of the WaveAggregator.
        /// </summary>
        public virtual IWaveSource BaseStream
        {
            get { return _baseStream; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "BaseStream must not be null.");
                _baseStream = value;
            }
        }

        /// <summary>
        ///     Gets the output WaveFormat.
        /// </summary>
        public virtual WaveFormat WaveFormat
        {
            get { return BaseStream.WaveFormat; }
        }


        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="BaseStream"/> and advances the position within the stream by the
        ///     number of bytes read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of bytes. When this method returns, the <paramref name="buffer" /> contains the specified
        ///     byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        ///     <paramref name="count" /> - 1) replaced by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        ///     The zero-based byte offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the current source.</param>
        /// <returns>The total number of bytes read into the buffer.</returns>
        public virtual int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        /// <summary>
        ///     Gets or sets the position of the source.
        /// </summary>
        public virtual long Position
        {
            get { return CanSeek ? BaseStream.Position : 0; }
            set
            {
                if(CanSeek)
                    BaseStream.Position = value;
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     Gets the length of the source.
        /// </summary>
        public virtual long Length
        {
            get { return CanSeek ? BaseStream.Length : 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IWaveStream"/> supports seeking.
        /// </summary>
        public virtual bool CanSeek
        {
            get { return BaseStream.CanSeek; }
        }

        /// <summary>
        ///     Disposes the source and releases all allocated resources.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        ///     Disposes the <see cref="BaseStream" /> and releases all allocated resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (DisposeBaseSource)
            {
                if (BaseStream != null)
                    BaseStream.Dispose();
                _baseStream = null;
            }
        }

        /// <summary>
        ///     Destructor which calls <see cref="Dispose(bool)" />.
        /// </summary>
        ~WaveAggregatorBase()
        {
            Dispose(false);
        }
    }
}