using System;
using System.IO;

namespace CSCore.Codecs.RAW
{
    /// <summary>
    ///     Implementation of the <see cref="IWaveSource" /> interface which reads raw data from a <see cref="Stream" /> based
    ///     on a specified <see cref="WaveFormat" />.
    /// </summary>
    public class RawDataReader : IWaveSource
    {
        private readonly long _startPosition;
        private readonly WaveFormat _waveFormat;
        private bool _disposed;
        private Stream _stream;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RawDataReader" /> class.
        /// </summary>
        /// <param name="stream"><see cref="Stream" /> which contains raw waveform-audio data.</param>
        /// <param name="waveFormat">The format of the waveform-audio data within the <paramref name="stream" />.</param>
        public RawDataReader(Stream stream, WaveFormat waveFormat)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (waveFormat == null)
                throw new ArgumentNullException("waveFormat");
            if (!stream.CanRead)
                throw new ArgumentException("stream is not readable", "stream");

            if (stream.CanSeek)
                _startPosition = stream.Position;

            if (waveFormat.WaveFormatTag != AudioEncoding.Pcm &&
                waveFormat.WaveFormatTag != AudioEncoding.IeeeFloat &&
                waveFormat.WaveFormatTag != AudioEncoding.Extensible)
                throw new ArgumentException("Not supported encoding: {" + waveFormat.WaveFormatTag + "}");

            _stream = stream;
            _waveFormat = waveFormat;
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="RawDataReader" /> and advances the position within the stream by the
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
            CheckForDisposed();
            count -= (count & WaveFormat.BlockAlign);
            return _stream.Read(buffer, offset, count);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IAudioSource"/> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        ///     Gets the format of the raw data.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        ///     Gets or sets the position of the <see cref="RawDataReader" /> in bytes.
        /// </summary>
        public long Position
        {
            get { return _stream != null ? _stream.Position - _startPosition : 0; }
            set
            {
                CheckForDisposed();
                _stream.Position = _startPosition + value;
            }
        }

        private void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        /// <summary>
        ///     Gets the length of the <see cref="RawDataReader" /> in bytes.
        /// </summary>
        public long Length
        {
            get { return _stream != null ? _stream.Length - _startPosition : 0; }
        }

        /// <summary>
        ///     Disposes the <see cref="RawDataReader" /> and the underlying <see cref="Stream" />.
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
        ///     Disposes the <see cref="RawDataReader" /> and the underlying <see cref="Stream" />.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        /// <summary>
        /// Destructor which calls the <see cref="Dispose(bool)"/> method.
        /// </summary>
        ~RawDataReader()
        {
            Dispose(false);
        }
    }
}