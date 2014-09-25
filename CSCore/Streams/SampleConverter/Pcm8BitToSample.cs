using System;

namespace CSCore.Streams.SampleConverter
{
    /// <summary>
    /// Converts a 8-bit PCM <see cref="IWaveSource"/> to a <see cref="ISampleSource"/>.
    /// </summary>
    public class Pcm8BitToSample : WaveToSampleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pcm8BitToSample"/> class.
        /// </summary>
        /// <param name="source">The underlying 8-bit POCM <see cref="IWaveSource"/> instance which has to get converted to a <see cref="ISampleSource"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentException">The format of the <paramref name="source"/> is not 8-bit PCM.</exception>
        public Pcm8BitToSample(IWaveSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (!source.WaveFormat.IsPCM() && source.WaveFormat.BitsPerSample != 8)
                throw new InvalidOperationException("Invalid format. Format has to 8 bit Pcm.");
        }

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="Pcm8BitToSample" /> and advances the position within the stream by the
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
        public unsafe override int Read(float[] buffer, int offset, int count)
        {
            Buffer = Buffer.CheckBuffer(count);
            int read = Source.Read(Buffer, 0, count);
            fixed (float* ptrBuffer = buffer)
            {
                float* ppbuffer = ptrBuffer + offset;
                for (int i = 0; i < read; i++)
                {
                    *(ppbuffer++) = Buffer[i] / 128f - 1.0f;
                }
            }
            return read;
        }
    }
}