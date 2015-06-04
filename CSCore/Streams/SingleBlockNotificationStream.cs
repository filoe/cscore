using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Streams
{
    /// <summary>
    /// Fire the <see cref="SingleBlockRead"/> event after every block read.
    /// </summary>
    public class SingleBlockNotificationStream : SampleAggregatorBase
    {
        /// <summary>
        /// Occurs when the <see cref="Read"/> method reads a block.
        /// </summary>
        /// <remarks>If the <see cref="Read"/> method reads <c>n</c> during a single call, the <see cref="SingleBlockRead"/> event will get fired <c>n</c> times.</remarks>
        public event EventHandler<SingleBlockReadEventArgs> SingleBlockRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleBlockNotificationStream"/> class.
        /// </summary>
        /// <param name="source">Underlying base source which provides audio data.</param>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public SingleBlockNotificationStream(ISampleSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
        }

        /// <summary>
        /// Reads a sequence of samples from the <see cref="SampleAggregatorBase" /> and advances the position within the stream by
        /// the number of samples read. Fires the <see cref="SingleBlockRead"/> event for each block it reads (one block = (number of channels) samples).
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
            if (read != 0 && SingleBlockRead != null)
            {
                int channels = WaveFormat.Channels;
                for (int n = 0; n < read; n += channels)
                {
                    SingleBlockRead(this, new SingleBlockReadEventArgs(buffer, offset + n, channels));
                }
            }

            return read;
        }
    }
}
