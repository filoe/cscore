using System;

namespace CSCore.Streams
{
    /// <summary>
    ///     Notifies the client when a specific number of samples got read and when the <see cref="Read" /> method got called.
    /// </summary>
    /// <remarks>Compared to the <see cref="NotificationSource" />, none of both events won't provide the read data.</remarks>
    public class SimpleNotificationSource : SampleAggregatorBase
    {
        private int _blockCount;
        private int _blocksRead;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SimpleNotificationSource" /> class.
        /// </summary>
        /// <param name="source">Underlying base source which provides audio data.</param>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public SimpleNotificationSource(ISampleSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
        }

        /// <summary>
        ///     Gets or sets the interval (in which to fire the <see cref="BlockRead" /> event) in blocks. One block equals one
        ///     sample for each channel.
        /// </summary>
        public int BlockCount
        {
            get { return _blockCount; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");
                _blockCount = value;
            }
        }

        /// <summary>
        ///     Gets or sets the interval (in which to fire the <see cref="BlockRead" /> event) in milliseconds.
        /// </summary>
        public int Interval
        {
            get { return (int) (1000.0 * ((double) BlockCount / WaveFormat.SampleRate)); }
            set
            {
                var v = (int) ((((double) value * WaveFormat.SampleRate)) / 1000.0);
                v = Math.Max(1, v);
                BlockCount = v;
            }
        }

        /// <summary>
        ///     Occurs when the <see cref="Read" /> method got called.
        /// </summary>
        public event EventHandler DataRead;

        /// <summary>
        ///     Occurs when a specified amount of data got read.
        /// </summary>
        /// <remarks>
        ///     The <see cref="Interval" />- or the <see cref="BlockCount" />-property specifies how many samples have to get
        ///     read to trigger the <see cref="BlockRead" /> event.
        /// </remarks>
        public event EventHandler BlockRead;

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="SampleAggregatorBase" /> and advances the position within the
        ///     stream by
        ///     the number of samples read. Triggers the <see cref="DataRead" /> event and if the [(number of total samples read) /
        ///     (number of channels)] %
        ///     <see cref="BlockCount" /> = 0, the <see cref="BlockRead" /> event gets triggered.
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
        /// <returns>
        ///     The total number of samples read into the buffer.
        /// </returns>
        public override int Read(float[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            int channels = WaveFormat.Channels;

            if (BlockRead != null)
            {
                for (int i = 0; i < read / channels; i++)
                {
                    _blocksRead++;
                    if (_blocksRead >= BlockCount)
                    {
                        if (BlockRead != null)
                            BlockRead(this, EventArgs.Empty);
                        _blocksRead = 0;
                    }
                }
            }

            if (DataRead != null)
                DataRead(this, EventArgs.Empty);

            return read;
        }
    }
}