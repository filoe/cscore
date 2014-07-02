using CSCore.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// The <see cref="MediaFoundationDecoder"/> is a generic decoder for all installed Mediafoundation codecs.
    /// </summary>
    public class MediaFoundationDecoder : IWaveSource
    {
        private IMFByteStream _byteStream;
        private MFSourceReader _reader;
        private WaveFormat _waveFormat;
        private Stream _stream;
        private readonly Object _lockObj = new Object();

        private long _length;
        private long _position; //could not find a possibility to find out the position -> we have to track the position ourselves.
        private readonly bool _hasFixedLength;

        private byte[] _decoderBuffer;
        private int _decoderBufferOffset;
        private int _decoderBufferCount;

        static MediaFoundationDecoder()
        {
            MediaFoundationCore.Startup(); //make sure that the MediaFoundation is started up.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFoundationDecoder"/> class.
        /// </summary>
        /// <param name="uri">Uri which points to a audio source which can be decoded.</param>
        public MediaFoundationDecoder(string uri)
        {
            if (String.IsNullOrEmpty(uri))
                throw new ArgumentNullException("uri");

            _hasFixedLength = true;

            _reader = Initialize(MediaFoundationCore.CreateSourceReaderFromUrl(uri));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFoundationDecoder"/> class.
        /// </summary>
        /// <param name="stream">Stream which holds audio data which can be decoded.</param>
        public MediaFoundationDecoder(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");

            stream = new ComStream(stream);
            _stream = stream;
            _byteStream = MediaFoundationCore.IStreamToByteStream((IStream)stream);
            _reader = Initialize(_byteStream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFoundationDecoder"/> class.
        /// </summary>
        /// <param name="byteStream">Stream which holds audio data which can be decoded.</param>
        public MediaFoundationDecoder(IMFByteStream byteStream)
        {
            if (byteStream == null)
                throw new ArgumentNullException("byteStream");
            _byteStream = byteStream;
            _reader = Initialize(_byteStream);
        }

        private MFSourceReader Initialize(IMFByteStream stream)
        {
            MediaFoundationCore.Startup();
            return Initialize(MediaFoundationCore.CreateSourceReaderFromByteStream(stream, IntPtr.Zero));
        }

        private MFSourceReader Initialize(MFSourceReader reader)
        {
            MediaFoundationCore.Startup();

            try
            {
                reader.SetStreamSelection(MFInterops.MF_SOURCE_READER_ALL_STREAMS, false);
                reader.SetStreamSelection(MFInterops.MF_SOURCE_READER_FIRST_AUDIO_STREAM, true);

                using (var mediaType = MFMediaType.CreateEmpty())
                {
                    mediaType.MajorType = AudioSubTypes.MediaTypeAudio;
                    mediaType.SubType = AudioSubTypes.Pcm; //variable??

                    reader.SetCurrentMediaType(MFInterops.MF_SOURCE_READER_FIRST_AUDIO_STREAM, mediaType);
                }

                using (var currentMediaType = reader.GetCurrentMediaType(MFInterops.MF_SOURCE_READER_FIRST_AUDIO_STREAM))
                {
                    if (currentMediaType.MajorType != AudioSubTypes.MediaTypeAudio)
                        throw new InvalidOperationException(String.Format("Invalid Majortype set on sourcereader: {0}.", currentMediaType.MajorType.ToString()));

                    AudioEncoding encoding;
                    if (currentMediaType.SubType == AudioSubTypes.Pcm)
                        encoding = AudioEncoding.Pcm;
                    else if (currentMediaType.SubType == AudioSubTypes.IeeeFloat)
                        encoding = AudioEncoding.IeeeFloat;
                    else
                        throw new InvalidOperationException(String.Format("Invalid Subtype set on sourcereader: {0}.", currentMediaType.SubType.ToString()));

                    _waveFormat = new WaveFormat(currentMediaType.SampleRate, currentMediaType.BitsPerSample, currentMediaType.Channels, encoding);
                }

                reader.SetStreamSelection(MFInterops.MF_SOURCE_READER_FIRST_AUDIO_STREAM, true);

                if (_hasFixedLength)
                    _length = GetLength(reader);

                return reader;
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        private long GetLength(MFSourceReader reader)
        {
            lock (_lockObj)
            {
                try
                {
                    if (reader == null)
                        return 0;

                    PropertyVariant value = reader.GetPresentationAttribute(MFInterops.MF_SOURCE_READER_MEDIASOURCE, MediaFoundationAttributes.MF_PD_DURATION);
                    var length = ((value.HValue) * _waveFormat.BytesPerSecond) / 10000000L;
                    value.Dispose();
                    return length;
                }
                catch (Exception)
                {
                    //if (e.Result == (int)HResult.MF_E_ATTRIBUTENOTFOUND)
                    //    return 0;
                    //throw;
                    return 0;
                }
            }
        }

        private void SetPosition(long value)
        {
            if (CanSeek)
            {
                lock (_lockObj)
                {
                    long hnsPos = (10000000L * value) / WaveFormat.BytesPerSecond;
                    var propertyVariant = PropertyVariant.CreateLong(hnsPos);
                    _reader.SetCurrentPosition(Guid.Empty, propertyVariant);
                    _decoderBufferCount = 0;
                    _decoderBufferOffset = 0;
                    _position = value;
                }
            }
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="MediaFoundationDecoder" /> and advances the position within the stream by the
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
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length < count)
                throw new ArgumentException("Length is too small.", "buffer");

            lock (_lockObj)
            {
                int read = 0;

                if (_reader == null || _disposed)
                    return read;

                if (_decoderBufferCount > 0)
                {
                    read += CopyDecoderBuffer(buffer, offset + read, count - read);
                }

                while (read < count)
                {
                    MFSourceReaderFlag flags;
                    long timestamp;
                    int actualStreamIndex;
                    using (var sample = _reader.ReadSample(MFInterops.MF_SOURCE_READER_FIRST_AUDIO_STREAM, 0, out actualStreamIndex, out flags, out timestamp))
                    {
                        if (flags != MFSourceReaderFlag.None)
                            break;

                        using (MFMediaBuffer mediaBuffer = sample.ConvertToContinousBuffer())
                        {
                            int maxlength, currentlength;
                            IntPtr pdata = mediaBuffer.Lock(out maxlength, out currentlength);
                            _decoderBuffer = _decoderBuffer.CheckBuffer(currentlength);
                            Marshal.Copy(pdata, _decoderBuffer, 0, currentlength);
                            _decoderBufferCount = currentlength;
                            _decoderBufferOffset = 0;

                            int tmp = CopyDecoderBuffer(buffer, offset + read, count - read);
                            read += tmp;

                            mediaBuffer.Unlock();
                        }
                    }
                }

                _position += read;

                return read;
            }
        }

        private int CopyDecoderBuffer(byte[] destBuffer, int offset, int count)
        {
            count = Math.Min(count, _decoderBufferCount);
            Array.Copy(_decoderBuffer, _decoderBufferOffset, destBuffer, offset, count);
            _decoderBufferCount -= count;
            _decoderBufferOffset += count;

            if (_decoderBufferCount == 0)
                _decoderBufferOffset = 0;

            return count;
        }

        private bool _disposed;

        /// <summary>
        /// Disposes the <see cref="MediaFoundationDecoder"/>.
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
        /// Disposes the <see cref="MediaFoundationDecoder"/> and its internal resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (_lockObj)
            {
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }
                if (_byteStream != null)
                {
                    Marshal.ReleaseComObject(_byteStream);
                    _byteStream = null;
                }
                if (_stream != null)
                {
                    _stream.Dispose();
                    _stream = null;
                }
            }
        }

        /// <summary>
        /// Destructor which calls <see cref="Dispose(bool)"/>.
        /// </summary>
        ~MediaFoundationDecoder()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the format of the decoded audio data.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        /// Gets or sets the position of the output stream in bytes.
        /// </summary>
        public long Position
        {
            get
            {
                return _position;
            }
            set
            {
                SetPosition(value);
            }
        }

        /// <summary>
        /// Gets the total length of the decoded audio.
        /// </summary>
        public long Length
        {
            get
            {
                if (_hasFixedLength)
                    return _length;
                return GetLength(_reader);
            }
        }

        /// <summary>
        /// Gets a value which indicates whether the seeking is supported. True means that seeking is supported. False means that seeking is not supported.
        /// </summary>
        public bool CanSeek
        {
            get { return _reader.CanSeek; }
        }
    }
}