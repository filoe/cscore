using System;
using System.Linq;

namespace CSCore.Streams
{
    /// <summary>
    /// Represents a peak meter.
    /// </summary>
    public class PeakMeter : SampleAggregatorBase
    {
        /// <summary>
        /// Gets the average value of all <see cref="ChannelPeakValues"/>.
        /// </summary>
        public float PeakValue
        {
            get
            {
                return ChannelPeakValues.Average();
            }
        }

        /// <summary>
        /// Gets the peak values for all channels. 
        /// </summary>
        public float[] ChannelPeakValues { get; private set; }

        /// <summary>
        /// Obsolete
        /// </summary>
        [Obsolete("Use the PeakMeter.Interval property instead.")]
        public int BlocksToProcess
        {
            get { return _blocksToProcess; }
            set { _blocksToProcess = value; }
        }

        /// <summary>
        /// Gets or sets the interval at which to raise the <see cref="PeakCalculated"/> event. 
        /// The interval is specified in milliseconds. 
        /// </summary>
        public int Interval
        {
            get { return (int)((1000.0 * _blocksToProcess) / WaveFormat.SampleRate); }
            set { _blocksToProcess = (int)((value / 1000.0) * WaveFormat.SampleRate); }
        }

        /// <summary>
        /// Event which gets raised when a new peak value is available. 
        /// </summary>
        public event EventHandler<PeakEventArgs> PeakCalculated;

        private int _blocksToProcess = 0;
        private int _blocksProcessed = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeakMeter"/> class.
        /// </summary>
        /// <param name="source">Underlying base source which provides audio data.</param>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public PeakMeter(ISampleSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            ChannelPeakValues = new float[source.WaveFormat.Channels];
            Interval = 250;
        }

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="SampleAggregatorBase" /> and advances the position within the stream by the
        ///     number of samples read.
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
            if (count % WaveFormat.Channels != 0)
                throw new ArgumentOutOfRangeException("count");
            if (offset % WaveFormat.Channels != 0)
                throw new ArgumentOutOfRangeException("offset");

            int read = base.Read(buffer, offset, count);

            int channels = WaveFormat.Channels;
            for (int i = offset; i < read; i++)
            {
                int channel = i % channels;
                ChannelPeakValues[channel] = Math.Max(ChannelPeakValues[channel], Math.Abs(buffer[i]));

                if (channel == channels - 1)
                    _blocksProcessed++;
                if(_blocksProcessed == _blocksToProcess)
                {
                    RaisePeakCalculated();
                    Reset();
                }
            }

            return read;
        }

        /// <summary>
        /// Sets all ChannelPeakValues to zero and resets the amount of processed blocks.
        /// </summary>
        public void Reset()
        {
            Array.Clear(ChannelPeakValues, 0, ChannelPeakValues.Length);
            _blocksProcessed = 0;
        }

        private void RaisePeakCalculated()
        {
            if (PeakCalculated != null)
                PeakCalculated(this, new PeakEventArgs(ChannelPeakValues, PeakValue));
        }
    }
}
