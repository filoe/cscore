using System;
using CSCore.Streams.SampleConverter;

namespace CSCore
{
    /// <summary>
    ///     Base class for most of the sample sources.
    /// </summary>
    public class SampleSourceBase : ISampleSource
    {
        /// <summary>
        ///     Underlying sample source.
        /// </summary>
        protected ISampleSource Source;

        private bool _disposed;

        /// <summary>
        ///     Creates a new instance of the <see cref="SampleSourceBase" /> class.
        /// </summary>
        /// <param name="source">Underlying base source which provides audio data.</param>
        public SampleSourceBase(IWaveStream source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source is ISampleSource)
                Source = (source as ISampleSource);
            else
                Source = WaveToSampleBase.CreateConverter(source as IWaveSource);
        }

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="SampleSourceBase" /> and advances the position within the stream by
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

            return Source.Read(buffer, offset, count);
        }

        /// <summary>
        ///     Gets the <see cref="IWaveStream.WaveFormat" /> of the waveform-audio data.
        /// </summary>
        public virtual WaveFormat WaveFormat
        {
            get { return Source.WaveFormat; }
        }

        /// <summary>
        ///     Gets or sets the position in samples.
        /// </summary>
        public virtual long Position
        {
            get { return CanSeek ? Source.Position : 0; }
            set
            {
                if(CanSeek)
                    Source.Position = value;
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     Gets the length in samples.
        /// </summary>
        public virtual long Length
        {
            get { return CanSeek ? Source.Length : 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IWaveStream"/> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get { return Source.CanSeek; }
        }

        /// <summary>
        ///     Disposes the <see cref="SampleSourceBase" /> and the underlying <see cref="Source" />.
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
        ///     Disposes the <see cref="SampleSourceBase" /> and the underlying <see cref="Source" />.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            Source.Dispose();
        }

        /// <summary>
        ///     Destructor which calls <see cref="Dispose(bool)" />.
        /// </summary>
        ~SampleSourceBase()
        {
            Dispose(false);
        }
    }
}