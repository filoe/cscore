using System;

namespace CSCore.Streams
{
    /// <summary>
    /// Converts a mono source to a stereo source.
    /// </summary>
    public sealed class MonoToStereoSource : SampleAggregatorBase
    {
        private float[] _buffer;

        private readonly WaveFormat _waveFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoToStereoSource"/> class.
        /// </summary>
        /// <param name="source">The underlying mono source.</param>
        /// <exception cref="ArgumentException">The <paramref name="source"/> has more or less than one channel.</exception>
        public MonoToStereoSource(ISampleSource source)
            : base(source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (source.WaveFormat.Channels != 1)
                throw new ArgumentException("The WaveFormat of the source has be a mono format (one channel).", "source");
            _waveFormat = new WaveFormat(source.WaveFormat.SampleRate, 32, 2, AudioEncoding.IeeeFloat);
        }

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="MonoToStereoSource" /> and advances the position within the stream by
        ///     the number of samples read.
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
            int bytesToRead = count / 2;
            _buffer = _buffer.CheckBuffer(bytesToRead);

            int index = offset;
            int read = base.Read(_buffer, 0, bytesToRead);
            for (int i = 0; i < read; i++)
            {
                buffer[index++] = _buffer[i];
                buffer[index++] = _buffer[i];
            }

            return read * 2;
        }

        /// <summary>
        ///     Gets or sets the position in samples.
        /// </summary>
        public override long Position
        {
            get
            {
                return base.Position * 2;
            }
            set
            {
                base.Position = value / 2;
            }
        }

        /// <summary>
        ///     Gets the length in samples.
        /// </summary>
        public override long Length
        {
            get
            {
                return base.Length * 2;
            }
        }

        /// <summary>
        ///     Gets the <see cref="IAudioSource.WaveFormat" /> of the waveform-audio data.
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        ///     Disposes the <see cref="MonoToStereoSource" /> and the underlying <see cref="SampleAggregatorBase.BaseSource" />.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _buffer = null;
        }
    }
}