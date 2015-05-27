using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    /// <summary>
    /// Provides control over the balance between the left and the right channel of an audio source.
    /// </summary>
    [Obsolete("Use the DmoChannelResampler instead.")]
    public class PanSource : SampleAggregatorBase
    {
        private float _pan = 0.0f;

        /// <summary>
        /// Gets or sets the balance. The valid range is from -1 to 1. -1 will mute the right channel, 1 will mute left channel.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value is not within the specified range.</exception>
        public float Pan
        {
            get
            {
                return _pan;
            }
            set
            {
                if (value < -1 || value > 1)
                    throw new ArgumentOutOfRangeException("value");
                _pan = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanSource"/> class.
        /// </summary>
        /// <param name="source">Underlying base source which provides audio data.</param>
        /// <exception cref="System.ArgumentException">Source has to be stereo.</exception>
        public PanSource(ISampleSource source)
            : base(source)
        {
            if (source.WaveFormat.Channels != 2)
                throw new ArgumentException("Source has to be stereo.", "source");
        }

        /// <summary>
        /// Reads a sequence of samples from the <see cref="SampleAggregatorBase" /> and advances the position within the stream by
        /// the number of samples read.
        /// </summary>
        /// <param name="buffer">An array of floats. When this method returns, the <paramref name="buffer" /> contains the specified
        /// float array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        /// <paramref name="count" /> - 1) replaced by the floats read from the current source.</param>
        /// <param name="offset">The zero-based offset in the <paramref name="buffer" /> at which to begin storing the data
        /// read from the current stream.</param>
        /// <param name="count">The maximum number of samples to read from the current source.</param>
        /// <returns>
        /// The total number of samples read into the buffer.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Read samples has to be a multiple of two.</exception>
        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            if (read % 2 != 0)
                throw new InvalidOperationException("Read samples has to be a multiple of two.");

            float left = Math.Min(1, Pan + 1);
            float right = Math.Abs(Math.Max(-1, Pan - 1));

            for (int i = offset; i < offset + read; )
            {
                buffer[i++] *= left;
                buffer[i++] *= right;
            }

            return read;
        }
    }
}