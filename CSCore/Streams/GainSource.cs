using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    /// <summary>
    /// Compared to the <see cref="VolumeSource"/>, the <see cref="Volume"/> property of the <see cref="GainSource"/> accepts any value.
    /// </summary>
    public class GainSource : VolumeSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GainSource"/> class.
        /// </summary>
        /// <param name="source">The underlying base source.</param>
        public GainSource(ISampleSource source) : base(source)
        {
            ClipOverflows = true;
        }

        /// <summary>
        /// Gets or sets the volume. A value of 1.0 will set the volume to 100%. A value of 0.0 will set the volume to 0%.
        /// </summary>
        /// <remarks>Since there is no validation of the value, this property can be used to set the gain value to any value.</remarks>
        public override float Volume { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Read"/> method should clip overflows. The default value is <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the <see cref="Read"/> method should clip overflows; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>Clipping the overflows means, that all samples which are not in the range from -1 to 1, will be clipped to that range.
        /// For example if a sample has a value of 1.3, it will be clipped to a value of 1.0.</remarks>
        public bool ClipOverflows { get; set; }

        /// <summary>
        /// Reads a sequence of samples from the <see cref="VolumeSource" /> and advances the position within the stream by
        /// the number of samples read. After reading the samples, the specified gain value will get applied and the overflows will be clipped (optionally).
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
        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            if (ClipOverflows)
            {
                for (int i = offset; i < read + offset; i++)
                {
                    buffer[i] = Math.Max(-1, Math.Min(buffer[i], 1));
                }
            }
            return read;
        }
    }
}
