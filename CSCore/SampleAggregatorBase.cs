using System;

namespace CSCore
{
    /// <summary>
    ///     Base class for most of the sample sources.
    /// </summary>
    public class SampleAggregatorBase : ISampleAggregator
    {
        private bool _disposed;
        private ISampleSource _baseSource;

        /// <summary>
        ///     Creates a new instance of the <see cref="SampleAggregatorBase" /> class.
        /// </summary>
        /// <param name="source">Underlying base source which provides audio data.</param>
        public SampleAggregatorBase(ISampleSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            _baseSource = source;
            DisposeBaseSource = true;
        }

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="SampleAggregatorBase" /> and advances the position within the stream by
        ///     the number of samples read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of floats. When this method returns, the <paramref name="buffer" /> contains the specified
        ///     float array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        ///     <paramref name="count" /> - 1) replaced by the floats read from the current source.
        /// </param>
        /// <param name="offset">
        ///     The zero-based offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of samples to read from the current source.</param>
        /// <returns>The total number of samples read into the buffer.</returns>
        public virtual int Read(float[] buffer, int offset, int count)
        {
            if (offset % WaveFormat.Channels != 0)
                throw new ArgumentOutOfRangeException("offset");
            if (count % WaveFormat.Channels != 0)
                throw new ArgumentOutOfRangeException("count");

            return BaseSource.Read(buffer, offset, count);
        }

        /// <summary>
        ///     Gets the <see cref="IAudioSource.WaveFormat" /> of the waveform-audio data.
        /// </summary>
        public virtual WaveFormat WaveFormat
        {
            get { return BaseSource.WaveFormat; }
        }

        /// <summary>
        ///     Gets or sets the position in samples.
        /// </summary>
        public virtual long Position
        {
            get { return CanSeek ? BaseSource.Position : 0; }
            set
            {
                if(CanSeek)
                    BaseSource.Position = value;
                else
                    throw new InvalidOperationException("Underlying BaseSource is not readable.");
            }
        }

        /// <summary>
        ///     Gets the length in samples.
        /// </summary>
        public virtual long Length
        {
            get { return CanSeek ? BaseSource.Length : 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IAudioSource"/> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get { return BaseSource.CanSeek; }
        }

        /// <summary>
        ///     Gets or sets the underlying sample source.
        /// </summary>
        public virtual ISampleSource BaseSource
        {
            get { return _baseSource; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _baseSource = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value which indicates whether to dispose the <see cref="BaseSource" />
        ///     on calling <see cref="Dispose(bool)" />.
        /// </summary>
        public bool DisposeBaseSource { get; set; }

        /// <summary>
        ///     Disposes the <see cref="SampleAggregatorBase" /> and the underlying <see cref="BaseSource" />.
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
        ///     Disposes the <see cref="SampleAggregatorBase" /> and the underlying <see cref="BaseSource" />.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (DisposeBaseSource && BaseSource != null)
            {
                BaseSource.Dispose();
                _baseSource = null;
            }
        }

        /// <summary>
        ///     Destructor which calls <see cref="Dispose(bool)" />.
        /// </summary>
        ~SampleAggregatorBase()
        {
            Dispose(false);
        }
    }
}