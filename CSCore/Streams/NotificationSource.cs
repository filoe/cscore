using System;
using System.Collections.Generic;

namespace CSCore.Streams
{
    /// <summary>
    /// Notifies the client when a certain amount of data got read.
    /// </summary>
    /// <remarks>Can be used as some kind of a timer for playbacks,...</remarks>
    public class NotificationSource : SampleAggregatorBase
    {
        private readonly List<float> _buffer;

        private int _blockSize;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationSource" /> class.
        /// </summary>
        /// <param name="source">Underlying base source which provides audio data.</param>
        /// <exception cref="System.ArgumentNullException">source is null.</exception>
        public NotificationSource(ISampleSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            BlockCount = (int) (source.WaveFormat.SampleRate * (40.0 / 1000.0));
            _buffer = new List<float>(BlockCount * source.WaveFormat.Channels);
        }

        /// <summary>
        ///     Gets or sets the interval in blocks. One block equals one sample for each channel.
        /// </summary>
        public int BlockCount
        {
            get { return _blockSize; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");
                _blockSize = value;
            }
        }

        /// <summary>
        ///     Gets or sets the interval in milliseconds.
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
        ///     Occurs when a specified amount of data got read.
        /// </summary>
        /// <remarks>
        ///     The <see cref="Interval" />- or the <see cref="BlockCount" />-property specifies how many samples have to get
        ///     read to trigger the <see cref="BlockRead" /> event.
        /// </remarks>
        public event EventHandler<BlockReadEventArgs<float>> BlockRead;

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="SampleAggregatorBase" /> and advances the position within the
        ///     stream by
        ///     the number of samples read. When the [(number of total samples read) / (number of channels)] %
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

            for (int i = offset; i < offset + read;)
            {
                for (int n = 0; n < channels; n++)
                {
                    _buffer.Add(buffer[i++]);
                }
                if (_buffer.Count >= BlockCount * WaveFormat.Channels)
                {
                    if (BlockRead != null)
                    {
                        float[] b = _buffer.ToArray();
                        BlockRead(this, new BlockReadEventArgs<float>(b, b.Length));
                        _buffer.Clear();
                    }
                }
            }

            return read;
        }
    }
}