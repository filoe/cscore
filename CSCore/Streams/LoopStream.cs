using System;

namespace CSCore.Streams
{
    /// <summary>
    ///     A Stream which can be used for endless looping.
    /// </summary>
    public class LoopStream : WaveAggregatorBase
    {
        private bool _enalbeLoop = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoopStream" /> class.
        /// </summary>
        /// <param name="source">The underlying <see cref="IWaveSource" />.</param>
        public LoopStream(IWaveSource source)
            : base(source)
        {
        }

        /// <summary>
        ///     Gets or sets whether looping is enabled.
        /// </summary>
        public bool EnableLoop
        {
            get { return _enalbeLoop; }
            set { _enalbeLoop = value; }
        }

        /// <summary>
        /// Occurs when the position of the <see cref="WaveAggregatorBase.BaseStream"/> gets reseted to zero and the next loop begins.
        /// </summary>
        public event EventHandler StreamFinished;

        /// <summary>
        ///     Reads from the underlying <see cref="WaveAggregatorBase.BaseStream" />. If the
        ///     <see cref="WaveAggregatorBase.BaseStream" /> does not provide any more data, its position gets reseted to zero.
        /// </summary>
        /// <param name="buffer">Buffer which receives the read data.</param>
        /// <param name="offset">Zero-based offset offset in the <paramref name="buffer" /> at which to begin storing data.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <returns>Actual number of read bytes.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = base.Read(buffer, offset, count);
            while (read < count)
            {
                int r = base.Read(buffer, offset + read, count - read);
                if (r == 0)
                {
                    if (StreamFinished != null)
                        StreamFinished(this, EventArgs.Empty);
                    if (EnableLoop)
                        Position = 0;
                    else
                        break;
                }
                read += r;
            }
            return read;
        }
    }
}