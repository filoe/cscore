using CSCore.SoundIn;
using System;

namespace CSCore.Streams
{
    /// <summary>
    /// Represents an implementation of the <see cref="IWaveSource"/> interface which provides the data provided by a specified <see cref="ISoundIn"/> object.
    /// </summary>
    public class SoundInSource : IWaveSource
    {
        private ISoundIn _soundIn;
        private WriteableBufferingSource _buffer;

        /// <summary>
        /// Occurs when new data is available.
        /// </summary>
        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        /// <summary>
        /// Gets the underlying <see cref="ISoundIn"/> instance.
        /// </summary>
        public ISoundIn SoundIn
        {
            get { return _soundIn; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundInSource"/> class with a default bufferSize of 5 seconds.
        /// </summary>
        /// <param name="soundIn">The soundIn which provides recorded data.</param>
        /// <remarks>
        /// Note that soundIn has to be already initialized.
        /// Note that old data ("old" gets specified by the bufferSize) gets overridden. 
        /// For example, if the bufferSize is about 5 seconds big, data which got recorded 6 seconds ago, won't be available anymore.
        /// </remarks>
        public SoundInSource(ISoundIn soundIn)
            : this(ThrowIfSoundInNotInitialized(soundIn), soundIn.WaveFormat.BytesPerSecond * 5)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundInSource"/> class.
        /// </summary>
        /// <param name="soundIn">The soundIn which provides recorded data.</param>
        /// <param name="bufferSize">Size of the internal buffer in bytes.</param>
        /// <remarks>
        /// Note that soundIn has to be already initialized.
        /// Note that old data ("old" gets specified by the bufferSize) gets overridden. 
        /// For example, if the bufferSize is about 5 seconds big, data which got recorded 6 seconds ago, won't be available anymore.
        /// </remarks>
        public SoundInSource(ISoundIn soundIn, int bufferSize)
        {
            if (soundIn == null)
                throw new ArgumentNullException("soundIn");
            ThrowIfSoundInNotInitialized(soundIn);

            _buffer = new WriteableBufferingSource(soundIn.WaveFormat, bufferSize) {FillWithZeros = false};
            _soundIn = soundIn;
            _soundIn.DataAvailable += OnDataAvailable;
        }

        private void OnDataAvailable(object sender, DataAvailableEventArgs e)
        {
            _buffer.Write(e.Data, 0, e.ByteCount);
            if (e.ByteCount > 0 && DataAvailable != null)
                DataAvailable(this, e);
        }

        /// <summary>
        ///     Reads a sequence of bytes from the internal stream which holds recorded data and advances the position within the stream by the
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
        public int Read(byte[] buffer, int offset, int count)
        {
            int read = _buffer.Read(buffer, offset, count);

            if(FillWithZeros && read < count)
                Array.Clear(buffer, offset + read, count - read);

            return FillWithZeros ? count : read;
        }

        /// <summary>
        ///     Gets the <see cref="IAudioSource.WaveFormat" /> of the recorded data.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return SoundIn.WaveFormat; }
        }

        /// <summary>
        ///     Gets or sets the current position in bytes. This property is currently not supported. 
        /// </summary>
        public long Position
        {
            get
            {
                return 0;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the length in bytes. This property is currently not supported.
        /// </summary>
        public long Length
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IAudioSource"/> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets a value which indicates whether the <see cref="Read"/> method should always provide the requested amount of data.
        /// For the case that the internal buffer can't offer the requested amount of data, the rest of the requested bytes will be filled up with zeros.
        /// </summary>
        public bool FillWithZeros { get; set; }

        private bool _disposed;

        /// <summary>
        /// Disposes the <see cref="SoundInSource"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the <see cref="SoundInSource"/>.
        /// </summary>
        /// <param name="disposing">Not used.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (SoundIn != null)
                {
                    SoundIn.DataAvailable -= OnDataAvailable;
                    _soundIn = null;
                }

                if (_buffer != null)
                {
                    _buffer.Dispose();
                    _buffer = null;
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Destructor of the <see cref="SoundInSource"/> class which calls the <see cref="Dispose(bool)"/> method.
        /// </summary>
        ~SoundInSource()
        {
            Dispose(false);
        }

        private static ISoundIn ThrowIfSoundInNotInitialized(ISoundIn soundIn)
        {
            if(soundIn == null || soundIn.WaveFormat == null)
                throw new ArgumentException("The SoundIn has to be initialized. Make sure that the passed SoundIn is not null and provides the format of the recorded data.", "soundIn");

            return soundIn;
        }
    }
}