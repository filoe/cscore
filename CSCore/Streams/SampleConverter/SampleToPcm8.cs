using System;

namespace CSCore.Streams.SampleConverter
{
    /// <summary>
    /// Converts a <see cref="ISampleSource"/> to a 8-bit PCM <see cref="IWaveSource"/>.
    /// </summary>
    public class SampleToPcm8 : SampleToWaveBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleToPcm8"/> class.
        /// </summary>
        /// <param name="source">The underlying <see cref="ISampleSource"/> which has to get converted to a 8-bit PCM <see cref="IWaveSource"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public SampleToPcm8(ISampleSource source)
            : base(source, 8, AudioEncoding.Pcm)
        {
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="SampleToPcm8" /> and advances the position within the stream by the
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
            int sourceCount = count;
            Buffer = Buffer.CheckBuffer(sourceCount);

            int read = Source.Read(Buffer, 0, sourceCount);
            for (int i = offset; i < read; i++)
            {
                byte value = (byte)((Buffer[i] + 1) * 128f);
                buffer[i] = unchecked(value);
            }

            return read;
        }
    }
}