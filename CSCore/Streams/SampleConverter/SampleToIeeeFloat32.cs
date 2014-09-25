using System;

namespace CSCore.Streams.SampleConverter
{
    /// <summary>
    /// Converts a <see cref="ISampleSource"/> to a 32-bit IeeeFloat <see cref="IWaveSource"/>.
    /// </summary>
    public class SampleToIeeeFloat32 : SampleToWaveBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleToIeeeFloat32"/> class.
        /// </summary>
        /// <param name="source">The underlying <see cref="ISampleSource"/> which has to get converted to a 32-bit IeeeFloat <see cref="IWaveSource"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public SampleToIeeeFloat32(ISampleSource source)
            : base(source, 32, AudioEncoding.IeeeFloat)
        {
            if(source == null)
                throw new ArgumentNullException("source");
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="SampleToIeeeFloat32" /> and advances the position within the stream by the
        ///     number of bytes read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of bytes. When this method returns, the <paramref name="buffer" /> contains the specified
        ///     byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        ///     <paramref name="count" /> - 1) replaced by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        ///     The zero-based byte offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the current source.</param>
        /// <returns>The total number of bytes read into the buffer.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            Buffer = Buffer.CheckBuffer(count / 4);
            int read = Source.Read(Buffer, offset / 4, count / 4);
            System.Buffer.BlockCopy(Buffer, 0, buffer, offset, read * 4);
            return read * 4;
        }
    }
}