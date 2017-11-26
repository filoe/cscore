using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CSCore.Ffmpeg
{
    /// <summary>
    ///     Generic FFmpeg based decoder.
    /// </summary>
    /// <remarks>
    ///     The <see cref="FfmpegDecoder" /> uses the FFmpeg libraries to decode audio files.
    ///     In order to make sure that the FFmpeg libraries are compatible with the <see cref="FfmpegDecoder" />,
    ///     use the binaries shipped with the CSCore.Ffmpeg project.
    ///     If a custom build is necessary, use the FFmpeg source code, from the CSCore git repository
    ///     (https://github.com/filoe/cscore).
    /// </remarks>
    public class FfmpegDecoder : IWaveSource
    {
        private readonly object _lockObject = new object();
        private readonly Uri _uri;
        private FfmpegStream _ffmpegStream;
        private AvFormatContext _formatContext;
        private bool _disposeStream = false;

        private byte[] _overflowBuffer = new byte[0];
        private int _overflowCount;
        private int _overflowOffset;
        private long _position;
        private Stream _stream;

        /// <summary>
        /// Gets a dictionary with found metadata.
        /// </summary>
        public Dictionary<string, string> Metadata
        {
            get
            {
                if(_formatContext == null)
                    return new Dictionary<string, string>();
                return _formatContext.Metadata;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FfmpegDecoder" /> class based on a specified filename or url.
        /// </summary>
        /// <param name="url">A url containing a filename or web url. </param>
        /// <exception cref="FfmpegException">
        ///     Any ffmpeg error.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        ///     DBL format is not supported.
        ///     or
        ///     Audio Sample Format not supported.
        /// </exception>
        /// <exception cref="ArgumentNullException">uri</exception>
        public FfmpegDecoder(string url)
        {
            const int invalidArgument = unchecked((int) 0xffffffea);

            _uri = new Uri(url);
            try
            {
                _formatContext = new AvFormatContext(url);
                Initialize();
            }
            catch (FfmpegException ex)
            {
                if (ex.ErrorCode == invalidArgument && "avformat_open_input".Equals(ex.Function, StringComparison.OrdinalIgnoreCase))
                {
                    if (!TryInitializeWithFileAsStream(url))
                        throw;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FfmpegDecoder" /> class based on a <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="FfmpegException">Any ffmpeg error.</exception>
        /// <exception cref="System.ArgumentNullException">stream</exception>
        /// <exception cref="ArgumentException">Stream is not readable.</exception>
        /// <exception cref="System.OutOfMemoryException">Could not allocate FormatContext.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     DBL format is not supported.
        ///     or
        ///     Audio Sample Format not supported.
        /// </exception>
        public FfmpegDecoder(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            InitializeWithStream(stream, false);
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="FfmpegDecoder" /> and advances the position within the
        ///     stream by the
        ///     number of bytes read.
        /// </summary>
        /// <param name="buffer">
        ///     An array of bytes. When this method returns, the <paramref name="buffer" /> contains the specified
        ///     array of bytes with the values between <paramref name="offset" /> and (<paramref name="offset" /> +
        ///     <paramref name="count" /> - 1) replaced by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        ///     The zero-based offset in the <paramref name="buffer" /> at which to begin storing the data
        ///     read from the current stream.
        /// </param>
        /// <param name="count">The maximum number of bytes to read from the current source.</param>
        /// <returns>The total number of bytes read into the buffer.</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;
            count -= count % WaveFormat.BlockAlign;
            int fetchedOverflows = GetOverflows(buffer, ref offset, count);
            read += fetchedOverflows;

            while (read < count)
            {
                long packetPosition;
                int bufferLength;
                lock (_lockObject)
                {
                    using (var frame = new AvFrame(_formatContext))
                    {
                        double seconds;
                        bufferLength = frame.ReadNextFrame(out seconds, ref _overflowBuffer);
                        packetPosition = this.GetRawElements(TimeSpan.FromSeconds(seconds));
                    }
                }
                if (bufferLength <= 0)
                {
                    if (_uri != null && !_uri.IsFile)
                    {
                        //webstream: don't exit, maybe the connection was lost -> give it a try to recover
                        Thread.Sleep(10);
                    }
                    else
                        break; //no webstream -> exit
                }
                int bytesToCopy = Math.Min(count - read, bufferLength);
                Array.Copy(_overflowBuffer, 0, buffer, offset, bytesToCopy);
                read += bytesToCopy;
                offset += bytesToCopy;

                _overflowCount = bufferLength > bytesToCopy ? bufferLength - bytesToCopy : 0;
                _overflowOffset = bufferLength > bytesToCopy ? bytesToCopy : 0;

                _position = packetPosition + read - fetchedOverflows;
            }

            if (fetchedOverflows == read)
            {
                //no new packet was decoded -> add the read bytes to the position
                _position += read;
            }

            return read;
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="FfmpegDecoder" /> supports seeking.
        /// </summary>
        public bool CanSeek
        {
            get
            {
                if (_formatContext == null)
                    return false;
                return _formatContext.CanSeek;
            }
        }

        /// <summary>
        ///     Gets the <see cref="IAudioSource.WaveFormat" /> of the waveform-audio data.
        /// </summary>
        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        ///     Gets or sets the current position in bytes.
        /// </summary>
        public long Position
        {
            get { return _position; }
            set { SeekPosition(value); }
        }

        /// <summary>
        ///     Gets the length of the waveform-audio data in bytes.
        /// </summary>
        public long Length
        {
            get
            {
                if (_formatContext == null || _formatContext.SelectedStream == null)
                    return 0;
                return this.GetRawElements(TimeSpan.FromSeconds(_formatContext.LengthInSeconds));
            }
        }

        /// <summary>
        ///     Releases all allocated resources used by the <see cref="FfmpegDecoder" />.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            GC.SuppressFinalize(this);

            if (disposing)
            {
                if (_disposeStream && _stream != null)
                {
                    _stream.Dispose();
                    _stream = null;
                }

                if (_formatContext != null)
                {
                    _formatContext.Dispose();
                    _formatContext = null;
                }

                if (_ffmpegStream != null)
                {
                    _ffmpegStream.Dispose();
                    _ffmpegStream = null;
                }
            }
        }

        private void Initialize()
        {
            WaveFormat = _formatContext.SelectedStream.GetSuggestedWaveFormat();
        }

        private void InitializeWithStream(Stream stream, bool disposeStream)
        {
            _stream = stream;
            _disposeStream = disposeStream;

            _ffmpegStream = new FfmpegStream(stream);
            _formatContext = new AvFormatContext(_ffmpegStream);
            Initialize();
        }

        private bool TryInitializeWithFileAsStream(string filename)
        {
            if (!File.Exists(filename))
                return false;

            Stream stream = null;
            try
            {
                stream = File.OpenRead(filename);
                InitializeWithStream(stream, true);
                return true;
            }
            catch (Exception)
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
                return false;
            }
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="FfmpegDecoder" /> class.
        /// </summary>
        ~FfmpegDecoder()
        {
            Dispose(false);
        }

        private void SeekPosition(long position)
        {
            //https://ffmpeg.org/doxygen/trunk/seek-test_8c-source.html
            double seconds = this.GetMilliseconds(position) / 1000.0;
            lock (_lockObject)
            {
                _formatContext.SeekFile(seconds);

                _position = position;
                _overflowCount = 0;
                _overflowOffset = 0;
            }
        }

        private int GetOverflows(byte[] buffer, ref int offset, int count)
        {
            if (_overflowCount != 0 && _overflowBuffer != null && count > 0)
            {
                int bytesToCopy = Math.Min(count, _overflowCount);
                Array.Copy(_overflowBuffer, _overflowOffset, buffer, offset, bytesToCopy);

                _overflowCount -= bytesToCopy;
                _overflowOffset += bytesToCopy;
                offset += bytesToCopy;
                return bytesToCopy;
            }
            return 0;
        }
    }
}