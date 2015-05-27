using System;

namespace CSCore
{
    /// <summary>
    ///     Base class for all wave aggregators.
    /// </summary>
    public abstract class WaveAggregatorBase : IWaveSource, IAggregator<byte, IWaveSource>
    {
        private IWaveSource _baseSource;
        private bool _disposed;

        /// <summary>
        ///     Creates a new instance of <see cref="WaveAggregatorBase"/> class.
        /// </summary>
        protected WaveAggregatorBase()
        {
            DisposeBaseSource = true;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="WaveAggregatorBase"/> class.
        /// </summary>
        /// <param name="baseSource">Underlying base stream.</param>
        protected WaveAggregatorBase(IWaveSource baseSource)
            : this()
        {
            if (baseSource == null)
                throw new ArgumentNullException("baseSource");

            _baseSource = baseSource;
        }

        /// <summary>
        ///     Gets or sets a value which indicates whether to dispose the <see cref="BaseSource" />
        ///     on calling <see cref="Dispose(bool)" />.
        /// </summary>
        public bool DisposeBaseSource { get; set; }

        /// <summary>
        ///     Gets or sets the underlying base stream of the WaveAggregator.
        /// </summary>
        public virtual IWaveSource BaseSource
        {
            get { return _baseSource; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "BaseSource must not be null.");
                _baseSource = value;
            }
        }

        /// <summary>
        ///     Gets the output WaveFormat.
        /// </summary>
        public virtual WaveFormat WaveFormat
        {
            get { return BaseSource.WaveFormat; }
        }


        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="BaseSource"/> and advances the position within the stream by the
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
            return BaseSource.Read(buffer, offset, count);
        }

        /// <summary>
        ///     Gets or sets the position of the source.
        /// </summary>
        public virtual long Position
        {
            get { return CanSeek ? BaseSource.Position : 0; }
            set
            {
                if(CanSeek)
                    BaseSource.Position = value;
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     Gets the length of the source.
        /// </summary>
        public virtual long Length
        {
            get { return CanSeek ? BaseSource.Length : 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IAudioSource"/> supports seeking.
        /// </summary>
        public virtual bool CanSeek
        {
            get { return BaseSource.CanSeek; }
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
        ///     Disposes the <see cref="BaseSource" /> and releases all allocated resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (DisposeBaseSource)
            {
                if (BaseSource != null)
                    BaseSource.Dispose();
                _baseSource = null;
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