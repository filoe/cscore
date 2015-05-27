using System;

namespace CSCore.Streams.SampleConverter
{
    /// <summary>
    /// Converts a <see cref="ISampleSource"/> to a <see cref="IWaveSource"/>.
    /// </summary>
    public abstract class SampleToWaveBase : IWaveSource
    {
        private readonly WaveFormat _waveFormat;
        /// <summary>
        /// The underlying source which provides samples.
        /// </summary>
        internal protected ISampleSource Source;
        /// <summary>
        /// The buffer to use for reading from the <see cref="Source"/>.
        /// </summary>
        internal protected float[] Buffer;
        private readonly double _ratio;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleToWaveBase"/> class.
        /// </summary>
        /// <param name="source">The underlying <see cref="ISampleSource"/> which has to get converted to a <see cref="IWaveSource"/>.</param>
        /// <param name="bits">The <see cref="CSCore.WaveFormat.BitsPerSample"/> of the Output-<see cref="WaveFormat"/>.</param>
        /// <param name="encoding">The <see cref="CSCore.WaveFormat.WaveFormatTag"/> of the Output-<see cref="WaveFormat"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Invalid number of bits per sample specified by the <paramref name="bits"/> argument.</exception>
        protected SampleToWaveBase(ISampleSource source, int bits, AudioEncoding encoding)
        {
            if (source == null) 
                throw new ArgumentNullException("source");
            if (bits < 1)
                throw new ArgumentOutOfRangeException("bits");

            _waveFormat = (WaveFormat) source.WaveFormat.Clone();
            _waveFormat.BitsPerSample = bits;
            _waveFormat.SetWaveFormatTagInternal(encoding);

            Source = source;
            _ratio = 32.0 / bits;
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="SampleToWaveBase" /> and advances the position within the stream by the
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
        public abstract int Read(byte[] buffer, int offset, int count);

        /// <summary>
        ///     Gets the <see cref="CSCore.WaveFormat"/> of the output waveform-audio data.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        ///     Gets or sets the current position.
        /// </summary>
        public long Position
        {
            get { return CanSeek ? Source.Position * WaveFormat.BytesPerSample : 0; }
            set
            {
                if(CanSeek)
                    Source.Position = value / WaveFormat.BytesPerSample;
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     Gets the length of the waveform-audio data.
        /// </summary>
        public long Length
        {
            get { return CanSeek ? Source.Length * WaveFormat.BytesPerSample : 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IAudioSource"/> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get { return Source.CanSeek; }
        }

        internal long InputToOutput(long position)
        {
            //long result = (long)(position * _ratio);
            long result = (long)(position * _ratio);
            result -= (result % _waveFormat.BlockAlign);
            return result;
        }

        internal long OutputToInput(long position)
        {
            //long result = (long)(position * _ratio);
            long result = (long)(position / _ratio);
            result -= (result % Source.WaveFormat.BlockAlign);
            return result;
        }

        private bool _disposed;

        /// <summary>
        /// Disposes the <see cref="SampleToWaveBase"/> instance.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Disposes the underlying <see cref="Source"/>.
        /// </summary>
        /// <param name="disposing">Not used.</param>
        protected virtual void Dispose(bool disposing)
        {
            Source.Dispose();
            Buffer = null;
        }

        /// <summary>
        /// Calls <see cref="Dispose(bool)"/>.
        /// </summary>
        ~SampleToWaveBase()
        {
            Dispose(false);
        }
    }
}