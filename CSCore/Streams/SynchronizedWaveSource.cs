using System;

namespace CSCore.Streams
{
    /// <summary>
    ///     A thread-safe (synchronized) wrapper around the specified a <see cref="IReadableAudioSource{T}"/>.
    /// </summary>
    /// <typeparam name="TBaseSource">The type of the underlying <see cref="IReadableAudioSource{T}"/>.</typeparam>
    /// <typeparam name="T">The type of the data read by the <see cref="Read"/> method.</typeparam>
    public class SynchronizedWaveSource<TBaseSource, T>
        : IAggregator<T, TBaseSource> where TBaseSource : class, IReadableAudioSource<T>
    {
        private readonly object _lockObj = new object();
        private TBaseSource _baseSource;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedWaveSource{TBaseSource, T}"/> class.
        /// </summary>
        /// <param name="baseWaveSource">The underlying source to synchronize.</param>
        public SynchronizedWaveSource(TBaseSource baseWaveSource)
        {
            BaseSource = baseWaveSource;
        }

        /// <summary>
        ///     Gets the output <see cref="CSCore.WaveFormat"/> of the <see cref="BaseSource" />.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get
            {
                lock (_lockObj)
                {
                    return BaseSource.WaveFormat;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the position of the <see cref="BaseSource" />.
        /// </summary>
        public long Position
        {
            get
            {
                lock (_lockObj)
                {
                    return BaseSource.Position;
                }
            }
            set
            {
                lock (_lockObj)
                {
                    BaseSource.Position = value;
                }
            }
        }

        /// <summary>
        ///     Gets the length of the <see cref="BaseSource" />.
        /// </summary>
        public long Length
        {
            get
            {
                lock (_lockObj)
                {
                    return BaseSource.Length;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="BaseSource" /> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get 
            {
                lock (_lockObj)
                {
                    return BaseSource.CanSeek;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="BaseSource" />.
        /// </summary>
        public TBaseSource BaseSource
        {
            get
            {
                lock (_lockObj)
                {
                    return _baseSource;
                }
            }
            set
            {
                lock (_lockObj)
                {
                    if (value == null)
                        throw new ArgumentNullException("value");
                    _baseSource = value;
                }
            }
        }

        /// <summary>
        ///     Reads a sequence of elements from the <see cref="BaseSource" /> and advances its position by the
        ///     number of elements read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of elements. When this method returns, the <paramref name="buffer" /> contains the specified
        ///     array of elements with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        ///     <paramref name="count" /> - 1) replaced by the elements read from the current source.
        /// </param>
        /// <param name="offset">
        ///     The zero-based offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of elements to read from the current source.</param>
        /// <returns>The total number of elements read into the buffer.</returns>
        public int Read(T[] buffer, int offset, int count)
        {
            lock (_lockObj)
            {
                return BaseSource.Read(buffer, offset, count);
            }
        }

        /// <summary>
        ///     Defines an explicit conversation of a <see cref="SynchronizedWaveSource{TBaseStream,T}" /> to its
        ///     <see cref="BaseSource" />.
        /// </summary>
        /// <param name="synchronizedWaveSource">Instance of the <see cref="SynchronizedWaveSource{TBaseStream,T}" />.</param>
        /// <returns>The <see cref="BaseSource" /> of the <paramref name="synchronizedWaveSource" />.</returns>
        public static explicit operator TBaseSource(SynchronizedWaveSource<TBaseSource, T> synchronizedWaveSource)
        {
            if (synchronizedWaveSource == null)
                throw new ArgumentNullException("synchronizedWaveSource");
            return synchronizedWaveSource.BaseSource;
        }

        /// <summary>
        ///     Disposes the <see cref="BaseSource" /> and releases all allocated resources.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected void Dispose(bool disposing)
        {
            lock (_lockObj)
            {
                if(BaseSource != null)
                    BaseSource.Dispose();
                _baseSource = null;
            }
        }

        /// <summary>
        ///     Disposes the <see cref="BaseSource" /> and releases all allocated resources.
        /// </summary>
        public void Dispose()
        {
            lock (_lockObj)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SynchronizedWaveSource{TBaseSource, T}"/> class.
        /// </summary>
        ~SynchronizedWaveSource()
        {
            lock (_lockObj)
            {
                Dispose(false);
            }
        }
    }
}