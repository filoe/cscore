namespace CSCore
{
    /// <summary>
    ///     Defines the base for all audio streams which provide samples instead of raw byte data.
    /// </summary>
    /// <remarks>
    ///     Compared to the <see cref="IWaveSource" />, the <see cref="ISampleSource" /> provides samples instead of raw bytes.
    ///     That means that the <see cref="IAudioSource.Length" /> and the <see cref="IAudioSource.Position" /> properties
    ///     are expressed in samples.
    ///     Also the <see cref="IReadableAudioSource{T}.Read" /> method provides samples instead of raw bytes.
    /// </remarks>
    public interface ISampleSource : IReadableAudioSource<float>
    {
        /*/// <summary>
        ///     Reads a sequence of samples from the <see cref="ISampleSource" /> and advances the position within the stream by the
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
        */
    }

    /// <summary>
    ///     Defines the base for all <see cref="ISampleSource" /> aggregators.
    /// </summary>
    public interface ISampleAggregator : ISampleSource, IAggregator<float, ISampleSource>
    {
    }

    /// <summary>
    ///     Defines the base for all <see cref="IWaveSource" /> aggregators.
    /// </summary>
    public interface IWaveAggregator : IWaveSource, IAggregator<byte, IWaveSource>
    {
    }

    /// <summary>
    ///     Defines the base for all aggregators.
    /// </summary>
    /// <typeparam name="T">The type of data, the aggregator provides.</typeparam>
    /// <typeparam name="TAggregator">The type of the aggreator type.</typeparam>
    public interface IAggregator<in T, out TAggregator>
        : IReadableAudioSource<T> where TAggregator : IReadableAudioSource<T>
    {
        /// <summary>
        ///     Gets the underlying <see cref="IReadableAudioSource{T}" />.
        /// </summary>
        /// <value>
        ///     The underlying <see cref="IReadableAudioSource{T}" />.
        /// </value>
        TAggregator BaseSource { get; }
    }
}