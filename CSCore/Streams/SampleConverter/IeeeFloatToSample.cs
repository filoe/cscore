using System;

namespace CSCore.Streams.SampleConverter
{
    /// <summary>
    /// Converts a 32-bit IeeeFloat <see cref="IWaveSource"/> to a <see cref="ISampleSource"/>.
    /// </summary>
    public class IeeeFloatToSample : WaveToSampleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IeeeFloatToSample"/> class.
        /// </summary>
        /// <param name="source">The underlying 32-bit IeeeFloat <see cref="IWaveSource"/> instance which has to get converted to a <see cref="ISampleSource"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentException">The format of the <paramref name="source"/> is not 32-bit IeeeFloat.</exception>
        public IeeeFloatToSample(IWaveSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (!source.WaveFormat.IsIeeeFloat() ||
                source.WaveFormat.BitsPerSample != 32)
                throw new ArgumentException("Invalid format. Format has to be 32 bit IeeeFloat");
        }

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="IeeeFloatToSample" /> and advances the position within the stream by the
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
            int bytesToRead = count * 4;
            Buffer = Buffer.CheckBuffer(bytesToRead);
            int read = Source.Read(Buffer, 0, bytesToRead);
            System.Buffer.BlockCopy(Buffer, 0, buffer, offset * 4, read);

            return read / 4;
        }
    }
}