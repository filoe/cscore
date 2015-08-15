using System;
using System.IO;
using System.Runtime.InteropServices;
using CSCore.Win32;

namespace CSCore.MediaFoundation
{
    /// <summary>
    ///     The <see cref="MediaFoundationDecoder" /> is a generic decoder for all installed Mediafoundation codecs.
    /// </summary>
    public class MediaFoundationDecoder : IWaveSource
    {
        private readonly bool _hasFixedLength;
        private readonly Object _lockObj = new Object();
        private MFByteStream _byteStream;

        private byte[] _decoderBuffer;
        private int _decoderBufferCount;
        private int _decoderBufferOffset;
        private bool _disposed;
        private long _length;

        //could not find a possibility to find out the position -> we have to track the position ourselves.
        private long _position;

        private MFSourceReader _reader;
        private Stream _stream;
        private WaveFormat _waveFormat;
        private bool _positionChanged;

        static MediaFoundationDecoder()
        {
            MediaFoundationCore.Startup(); //make sure that the MediaFoundation is started up.
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MediaFoundationDecoder" /> class.
        /// </summary>
        /// <param name="url">Uri which points to an audio source which can be decoded.</param>
        public MediaFoundationDecoder(string url)
        {
            if (String.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            _hasFixedLength = true;

            _reader = Initialize(MediaFoundationCore.CreateSourceReaderFromUrl(url));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MediaFoundationDecoder" /> class.
        /// </summary>
        /// <param name="stream">Stream which provides the audio data to decode.</param>
        public MediaFoundationDecoder(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");

            stream = new ComStream(stream);
            _stream = stream;
            _byteStream = MediaFoundationCore.IStreamToByteStream((IStream) stream);
            _reader = Initialize(_byteStream);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MediaFoundationDecoder" /> class.
        /// </summary>
        /// <param name="byteStream">Stream which provides the audio data to decode.</param>
        public MediaFoundationDecoder(MFByteStream byteStream)
        {
            if (byteStream == null)
                throw new ArgumentNullException("byteStream");
            _byteStream = byteStream;
            _reader = Initialize(_byteStream);
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="MediaFoundationDecoder" /> and advances the position within the
        ///     stream by the
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
                    read += CopyDecoderBuffer(buffer, offset + read, count - read);

                while (read < count)
                {
                    MFSourceReaderFlags flags;
                    long timestamp;
                    int actualStreamIndex;
                    using (
                        MFSample sample = _reader.ReadSample(NativeMethods.MF_SOURCE_READER_FIRST_AUDIO_STREAM, 0,
                            out actualStreamIndex, out flags, out timestamp))
                    {
                        if (flags != MFSourceReaderFlags.None)
                            break;
                        var sampleTime = sample.GetSampleTime();
                        if (_positionChanged && timestamp > 0)
                        {
                            _position = NanoSecond100UnitsToSamples(sampleTime);
                            _positionChanged = false;
                        }

                        using (MFMediaBuffer mediaBuffer = sample.ConvertToContiguousBuffer())
                        {
                            using (MFMediaBuffer.LockDisposable @lock = mediaBuffer.Lock())
                            {
                                _decoderBuffer = _decoderBuffer.CheckBuffer(@lock.CurrentLength);
                                Marshal.Copy(@lock.Buffer, _decoderBuffer, 0, @lock.CurrentLength);
                                _decoderBufferCount = @lock.CurrentLength;
                                _decoderBufferOffset = 0;

                                int tmp = CopyDecoderBuffer(buffer, offset + read, count - read);
                                read += tmp;
                            }
                        }
                    }
                }

                _position += read;

                return read;
            }
        }

        /// <summary>
        ///     Disposes the <see cref="MediaFoundationDecoder" />.
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
        ///     Gets the format of the decoded audio data provided by the <see cref="Read" /> method.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        ///     Gets or sets the position of the output stream, in bytes.
        /// </summary>
        public long Position
        {
            get { return !_disposed ? _position : 0; }
            set
            {
                CheckForDisposed();
                SetPosition(value);
            }
        }

        /// <summary>
        ///     Gets the total length of the decoded audio, in bytes.
        /// </summary>
        public long Length
        {
            get
            {
                if (_disposed)
                    return 0;
                if (_hasFixedLength)
                    return _length;
                return GetLength(_reader);
            }
        }

        /// <summary>
        ///     Gets a value which indicates whether the seeking is supported. True means that seeking is supported. False means
        ///     that seeking is not supported.
        /// </summary>
        public bool CanSeek
        {
            get { return _reader.CanSeek; }
        }

        private MFSourceReader Initialize(MFByteStream byteStream)
        {
            return Initialize(MediaFoundationCore.CreateSourceReaderFromByteStream(byteStream.BasePtr, IntPtr.Zero));
        }

        private MFSourceReader Initialize(MFSourceReader reader)
        {
            try
            {
                reader.SetStreamSelection(NativeMethods.MF_SOURCE_READER_ALL_STREAMS, false);
                reader.SetStreamSelection(NativeMethods.MF_SOURCE_READER_FIRST_AUDIO_STREAM, true);

                using (MFMediaType mediaType = MFMediaType.CreateEmpty())
                {
                    mediaType.MajorType = AudioSubTypes.MediaTypeAudio;
                    mediaType.SubType = AudioSubTypes.Pcm; //variable??

                    reader.SetCurrentMediaType(NativeMethods.MF_SOURCE_READER_FIRST_AUDIO_STREAM, mediaType);
                }

                using (
                    MFMediaType currentMediaType =
                        reader.GetCurrentMediaType(NativeMethods.MF_SOURCE_READER_FIRST_AUDIO_STREAM))
                {
                    if (currentMediaType.MajorType != AudioSubTypes.MediaTypeAudio)
                    {
                        throw new InvalidOperationException(String.Format(
                            "Invalid Majortype set on sourcereader: {0}.", currentMediaType.MajorType));
                    }

                    AudioEncoding encoding = AudioSubTypes.EncodingFromSubType(currentMediaType.SubType);

                    ChannelMask channelMask;
                    if (currentMediaType.TryGet(MediaFoundationAttributes.MF_MT_AUDIO_CHANNEL_MASK, out channelMask))
                        //check whether the attribute is available
                    {
                        _waveFormat = new WaveFormatExtensible(currentMediaType.SampleRate,
                            currentMediaType.BitsPerSample, currentMediaType.Channels, currentMediaType.SubType,
                            channelMask);
                    }
                    else
                    {
                        _waveFormat = new WaveFormat(currentMediaType.SampleRate, currentMediaType.BitsPerSample,
                            currentMediaType.Channels, encoding);
                    }
                }

                reader.SetStreamSelection(NativeMethods.MF_SOURCE_READER_FIRST_AUDIO_STREAM, true);

                if (_hasFixedLength)
                    _length = GetLength(reader);

                return reader;
            }
            catch (Exception)
            {
                DisposeInternal(true);
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

                    using (
                        PropertyVariant value =
                            reader.GetPresentationAttribute(NativeMethods.MF_SOURCE_READER_MEDIASOURCE,
                                MediaFoundationAttributes.MF_PD_DURATION))
                    {
                        //bug: still, depending on the decoder, this returns imprecise values.
                        return NanoSecond100UnitsToSamples(value.HValue);
                    }
                }
                catch (Exception)
                {
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
                    long hnsPos = SamplesToNanoSecond100Units(value);
                    var propertyVariant = new PropertyVariant {HValue = hnsPos, DataType = VarEnum.VT_I8};
                    _reader.SetCurrentPosition(Guid.Empty, propertyVariant);
                    _decoderBufferCount = 0;
                    _decoderBufferOffset = 0;
                    _position = value;

                    _positionChanged = true;
                }
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

        private long NanoSecond100UnitsToSamples(long nanoSeconds100Units)
        {
            return (nanoSeconds100Units * WaveFormat.BytesPerSecond) / 10000000L;
        }

        private long SamplesToNanoSecond100Units(long samples)
        {
            return (10000000L * samples) / WaveFormat.BytesPerSecond;
        }

        /// <summary>
        ///     Disposes the <see cref="MediaFoundationDecoder" /> and its internal resources.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            lock (_lockObj)
            {
                DisposeInternal(disposing);
            }
        }

        private void DisposeInternal(bool disposing)
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
            if (_byteStream != null)
            {
                _byteStream.Dispose();
                _byteStream = null;
            }
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        private void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="MediaFoundationDecoder" /> class.
        /// </summary>
        ~MediaFoundationDecoder()
        {
            Dispose(false);
        }
    }
}