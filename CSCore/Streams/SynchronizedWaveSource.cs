using System;

namespace CSCore.Streams
{
    /// <summary>
    ///     A thread-safe (synchronized) wrapper around the specified <see cref="IWaveSource" /> object.
    /// </summary>
    public class SynchronizedWaveSource<TBaseStream>
        : WaveAggregatorBase
        where TBaseStream : IWaveSource
    {
        private readonly object _lockObj = new object();

        /// <summary>
        ///     Initializes a new synchronizedWaveSource of the <see cref="SynchronizedWaveSource{TBaseStream}" /> class.
        /// </summary>
        /// <param name="baseWaveSource">The <see cref="IWaveSource" /> object to synchronize.</param>
        public SynchronizedWaveSource(TBaseStream baseWaveSource)
            : base(baseWaveSource)
        {
        }

        /// <summary>
        ///     Gets the output WaveFormat of the <see cref="WaveAggregatorBase.BaseStream" />.
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get
            {
                lock (_lockObj)
                {
                    return base.WaveFormat;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the position of the <see cref="WaveAggregatorBase.BaseStream" />.
        /// </summary>
        public override long Position
        {
            get
            {
                lock (_lockObj)
                {
                    return base.Position;
                }
            }
            set
            {
                lock (_lockObj)
                {
                    base.Position = value;
                }
            }
        }

        /// <summary>
        ///     Gets the length of the underlying <see cref="WaveAggregatorBase.BaseStream" />.
        /// </summary>
        public override long Length
        {
            get
            {
                lock (_lockObj)
                {
                    return base.Length;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the underlying base stream.
        /// </summary>
        public override IWaveSource BaseStream
        {
            get
            {
                lock (_lockObj)
                {
                    return base.BaseStream;
                }
            }
            set
            {
                lock (_lockObj)
                {
                    if (value == null || value is TBaseStream)
                        base.BaseStream = value;
                    else
                        throw new ArgumentException("The value does not fit the specified TBaseStream type.", "value");
                }
            }
        }

        /// <summary>
        ///     Defines an explicit conversation of a <see cref="SynchronizedWaveSource{TBaseStream}" /> to its
        ///     <see cref="BaseStream" />.
        /// </summary>
        /// <param name="synchronizedWaveSource">Instance of the <see cref="SynchronizedWaveSource{TBaseStream}" />.</param>
        /// <returns>The <see cref="BaseStream" /> of the <paramref name="synchronizedWaveSource" />.</returns>
        public static explicit operator TBaseStream(SynchronizedWaveSource<TBaseStream> synchronizedWaveSource)
        {
            if (synchronizedWaveSource == null)
                throw new ArgumentNullException("synchronizedWaveSource");
            return (TBaseStream) synchronizedWaveSource.BaseStream;
        }

        /// <summary>
        ///     Reads from the underlying <see cref="WaveAggregatorBase.BaseStream" />.
        /// </summary>
        /// <param name="buffer">Buffer which receives the read data.</param>
        /// <param name="offset">Zero-based offset offset in the <paramref name="buffer" /> at which to begin storing data.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <returns>Actual number of read bytes.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (_lockObj)
            {
                return base.Read(buffer, offset, count);
            }
        }


        /// <summary>
        ///     Disposes the <see cref="WaveAggregatorBase.BaseStream" /> and releases all allocated resources.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            lock (_lockObj)
            {
                base.Dispose(disposing);
            }
        }
    }
}