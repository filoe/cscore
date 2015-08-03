using System;

namespace CSCore.Streams.SampleConverter
{
    /// <summary>
    /// Converts a <see cref="ISampleSource"/> to a <see cref="IWaveSource"/>.
    /// </summary>
    public abstract class WaveToSampleBase : ISampleSource
    {
        private readonly WaveFormat _waveFormat;
        /// <summary>
        /// The underlying source which provides the raw data.
        /// </summary>
        internal protected IWaveSource Source;
        /// <summary>
        /// The buffer to use for reading from the <see cref="Source"/>.
        /// </summary>
        internal protected byte[] Buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaveToSampleBase"/> class.
        /// </summary>
        /// <param name="source">The underlying <see cref="IWaveSource"/> instance which has to get converted to a <see cref="ISampleSource"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> argument is null.</exception>
        protected WaveToSampleBase(IWaveSource source)
        {
            if (source == null) 
                throw new ArgumentNullException("source");

            Source = source;
            _waveFormat = (WaveFormat) source.WaveFormat.Clone();
            _waveFormat.BitsPerSample = 32;
            _waveFormat.SetWaveFormatTagInternal(AudioEncoding.IeeeFloat);
        }

        /// <summary>
        ///     Reads a sequence of samples from the <see cref="WaveToSampleBase" /> and advances the position within the stream by the
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
        public abstract int Read(float[] buffer, int offset, int count);

        /// <summary>
        ///     Gets the <see cref="CSCore.WaveFormat" /> of the waveform-audio data.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        ///     Gets or sets the current position in samples.
        /// </summary>
        public long Position
        {
            get { return CanSeek ? Source.Position / Source.WaveFormat.BytesPerSample :0; }
            set
            {
                if(CanSeek)
                    Source.Position = value * Source.WaveFormat.BytesPerSample;
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     Gets the length of the waveform-audio data in samples.
        /// </summary>
        public long Length
        {
            get { return CanSeek && Source.Length != 0 ? Source.Length / Source.WaveFormat.BytesPerSample : 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IAudioSource"/> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get { return Source.CanSeek; }
        }

        /// <summary>
        /// Disposes the <see cref="WaveToSampleBase"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes the <see cref="Source"/>.
        /// </summary>
        /// <param name="disposing">Not used.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Source != null)
            {
                Source.Dispose();
                Source = null;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="WaveToSampleBase"/> class.
        /// </summary>
        ~WaveToSampleBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Returns an implementation of the <see cref="ISampleSource"/> interface which converts the specified <paramref name="source"/> to a <see cref="ISampleSource"/>.
        /// </summary>
        /// <param name="source">The <see cref="IWaveSource"/> instance to convert.</param>
        /// <returns>Returns an implementation of the <see cref="ISampleSource"/> interface which converts the specified <paramref name="source"/> to a <see cref="ISampleSource"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="IAudioSource.WaveFormat"/> of the <paramref name="source"/> is not supported.</exception>
        public static ISampleSource CreateConverter(IWaveSource source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int bitsPerSample = source.WaveFormat.BitsPerSample;
            if (source.WaveFormat.IsPCM())
            {
                switch (bitsPerSample)
                {
                    case 8:
                        return new Pcm8BitToSample(source);

                    case 16:
                        return new Pcm16BitToSample(source);

                    case 24:
                        return new Pcm24BitToSample(source);

                    default:
                        throw new NotSupportedException("Waveformat is not supported. Invalid BitsPerSample value.");
                }
            }
            if (source.WaveFormat.IsIeeeFloat() && bitsPerSample == 32)
                return new IeeeFloatToSample(source);
            throw new NotSupportedException("Waveformat is not supported. Invalid WaveformatTag.");
        }
    }
}