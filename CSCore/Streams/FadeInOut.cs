using System;

namespace CSCore.Streams
{
    /// <summary>
    ///     Provides the ability use an implementation of the <see cref="IFadeStrategy" /> interface fade waveform-audio data.
    /// </summary>
    public class FadeInOut : SampleAggregatorBase
    {
        private volatile IFadeStrategy _fadeStrategy;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FadeInOut" /> class.
        /// </summary>
        /// <param name="source">The underlying source to use.</param>
        public FadeInOut(ISampleSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
        }

        /// <summary>
        ///     Gets or sets the fade strategy to use.
        /// </summary>
        public IFadeStrategy FadeStrategy
        {
            get { return _fadeStrategy; }
            set
            {
                if (_fadeStrategy != value && value != null)
                {
                    _fadeStrategy = value;
                    _fadeStrategy.SampleRate = WaveFormat.SampleRate;
                    _fadeStrategy.Channels = WaveFormat.Channels;
                }
            }
        }

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="FadeInOut" /> class and advances the position within the stream by
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
        public override int Read(float[] buffer, int offset, int count)
        {
            int read = 0;
            while (read < count)
            {
                int r = base.Read(buffer, offset, count);
                if (r == 0)
                    break;

                if (_fadeStrategy != null)
                    _fadeStrategy.ApplyFading(buffer, offset, r);

                read += r;
                offset += r;
            }

            return read;
        }
    }
}