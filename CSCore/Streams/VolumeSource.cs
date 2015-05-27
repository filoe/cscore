using System;

namespace CSCore.Streams
{
    /// <summary>
    /// Provides the ability to adjust the volume of an audio stream.
    /// </summary>
    public class VolumeSource : SampleAggregatorBase
    {
        private float _volume = 1f;

        /// <summary>
        /// Gets or sets the volume specified by a value in the range from 0.0 to 1.0.
        /// </summary>
        public virtual float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (_volume < 0 || _volume > 1)
                    throw new ArgumentOutOfRangeException("value");
                _volume = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeSource"/> class.
        /// </summary>
        /// <param name="source">The underlying base source.</param>
        public VolumeSource(ISampleSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
        }

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="VolumeSource" /> and advances the position within the stream by
        ///     the number of samples read. After reading the samples, the volume of the read samples gets manipulated.
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
            int read = base.Read(buffer, offset, count);

            for (int i = offset; i < read + offset; i++)
            {
                buffer[i] *= Volume;
            }

            return read;
        }
    }
}