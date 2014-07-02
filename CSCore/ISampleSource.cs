namespace CSCore
{
    /// <summary>
    ///     Defines the base for all audio streams which provide samples instead of raw byte data.
    /// </summary>
    public interface ISampleSource : IWaveStream
    {
        /// <summary>
        ///     Reads a sequence of samples from the <see cref="ISampleSource" /> and advances the position within the stream by
        ///     the
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
        int Read(float[] buffer, int offset, int count);
    }
}