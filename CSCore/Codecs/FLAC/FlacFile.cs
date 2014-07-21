#define DIAGNOSTICS

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CSCore.Tags.ID3;

namespace CSCore.Codecs.FLAC
{
    /// <summary>
    ///     Provides a decoder for decoding flac (Free Lostless Audio Codec) data.
    /// </summary>
    public class FlacFile : IWaveSource
    {
        private readonly Stream _stream;
        private readonly WaveFormat _waveFormat;
        private readonly FlacMetadataStreamInfo _streamInfo;
        private readonly FlacPreScan _scan;

        private readonly object _bufferLock = new object();

        //overflow:
        private byte[] _overflowBuffer;

        private int _overflowCount;
        private int _overflowOffset;

        private int _frameIndex;

        /// <summary>
        ///     Gets a list with all found metadata fields.
        /// </summary>
        public List<FlacMetadata> Metadata { get; protected set; }

        /// <summary>
        ///     Gets the output <see cref="CSCore.WaveFormat" /> of the decoder.
        /// </summary>
        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        /// <summary>
        ///     Gets a value which indicates whether the seeking is supported. True means that seeking is supported; False means
        ///     that seeking is not supported.
        /// </summary>
        public bool CanSeek
        {
            get { return _scan != null; }
        }

        private FlacFrame _frame;

        private FlacFrame Frame
        {
            get { return _frame ?? (_frame = FlacFrame.FromStream(_stream, _streamInfo)); }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlacFile" /> class.
        /// </summary>
        /// <param name="fileName">Filename which of a flac file which should be decoded.</param>
        public FlacFile(string fileName)
            : this(File.OpenRead(fileName))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlacFile" /> class.
        /// </summary>
        /// <param name="stream">Stream which contains flac data which should be decoded.</param>
        public FlacFile(Stream stream)
            : this(stream, FlacPreScanMethodMode.Default)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlacFile" /> class.
        /// </summary>
        /// <param name="stream">Stream which contains flac data which should be decoded.</param>
        /// <param name="scanFlag">Scan mode which defines how to scan the flac data for frames.</param>
        public FlacFile(Stream stream, FlacPreScanMethodMode scanFlag)
            : this(stream, scanFlag, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlacFile" /> class.
        /// </summary>
        /// <param name="stream">Stream which contains flac data which should be decoded.</param>
        /// <param name="scanFlag">Scan mode which defines how to scan the flac data for frames.</param>
        /// <param name="onscanFinished">
        ///     Callback which gets called when the pre scan processes finished. Should be used if the
        ///     <paramref name="scanFlag" /> argument is set the <see cref="FlacPreScanMethodMode.Async" />.
        /// </param>
        public FlacFile(Stream stream, FlacPreScanMethodMode scanFlag,
            Action<FlacPreScanFinishedEventArgs> onscanFinished)
        {
            if (stream == null)
                throw new ArgumentNullException();
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");

            _stream = stream;

            //skip ID3v2
            ID3v2.SkipTag(stream);

            //read fLaC sync
            var beginSync = new byte[4];
            int read = stream.Read(beginSync, 0, beginSync.Length);
            if (read < beginSync.Length)
                throw new EndOfStreamException("Can not read \"fLaC\" sync.");
            if (beginSync[0] == 0x66 && beginSync[1] == 0x4C && //Check for 'fLaC' signature
                beginSync[2] == 0x61 && beginSync[3] == 0x43)
            {
                //read metadata
                List<FlacMetadata> metadata = FlacMetadata.ReadAllMetadataFromStream(stream);

                Metadata = metadata;
                if (metadata == null || metadata.Count <= 0)
                    throw new FlacException("No Metadata found.", FlacLayer.Metadata);

                var streamInfo =
                    metadata.First(x => x.MetaDataType == FlacMetaDataType.StreamInfo) as FlacMetadataStreamInfo;
                if (streamInfo == null)
                    throw new FlacException("No StreamInfo-Metadata found.", FlacLayer.Metadata);

                _streamInfo = streamInfo;
                _waveFormat = new WaveFormat(streamInfo.SampleRate, (short) streamInfo.BitsPerSample,
                    (short) streamInfo.Channels, AudioEncoding.Pcm);
                Debug.WriteLine("Flac StreamInfo found -> WaveFormat: " + _waveFormat);
                Debug.WriteLine("Flac-File-Metadata read.");
            }
            else
                throw new FlacException("Invalid Flac-File. \"fLaC\" Sync not found.", FlacLayer.Top);

            //prescan stream
            if (scanFlag != FlacPreScanMethodMode.None)
            {
                var scan = new FlacPreScan(stream);
                scan.ScanFinished += (s, e) =>
                {
                    if (onscanFinished != null)
                        onscanFinished(e);
                };
                scan.ScanStream(_streamInfo, scanFlag);
                _scan = scan;
            }
        }

        /// <summary>
        ///     Reads a sequence of bytes from the <see cref="FlacFile" /> and advances the position within the stream by the
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
            int read = 0;
            count -= (count % WaveFormat.BlockAlign);

            lock (_bufferLock)
            {
                read += GetOverflows(buffer, ref offset, count);

                while (read < count)
                {
                    FlacFrame frame = Frame;
                    if (frame == null)
                        return 0;

                    while (!frame.NextFrame())
                    {
                        if (CanSeek) //go to next frame
                        {
                            if (++_frameIndex >= _scan.Frames.Count)
                                return 0;
                            _stream.Position = _scan.Frames[_frameIndex].StreamOffset;
                        }
                    }
                    _frameIndex++;

                    int bufferlength = frame.GetBuffer(ref _overflowBuffer, 0);
                    int bytesToCopy = Math.Min(count - read, bufferlength);
                    Array.Copy(_overflowBuffer, 0, buffer, offset, bytesToCopy);
                    read += bytesToCopy;
                    offset += bytesToCopy;

                    _overflowCount = ((bufferlength > bytesToCopy) ? (bufferlength - bytesToCopy) : 0);
                    _overflowOffset = ((bufferlength > bytesToCopy) ? (bytesToCopy) : 0);
                }
            }
#if DIAGNOSTICS
            _position += read;
#endif

            return read;
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

#if DIAGNOSTICS
        private long _position;
#endif

        /// <summary>
        ///     Gets or sets the position of the <see cref="FlacFile" /> in bytes.
        /// </summary>
        public long Position
        {
            get
            {
                if (!CanSeek)
                    return 0;

                lock (_bufferLock)
                {
#if !DIAGNOSTICS
                    if (_frameIndex == _scan.Frames.Count)
                        return Length;
                    return _scan.Frames[_frameIndex].SampleOffset * WaveFormat.BlockAlign + _overflowOffset;
#else
                    return _position;
#endif
                }
            }
            set
            {
                if (!CanSeek)
                    return;
                lock (_bufferLock)
                {
                    value = Math.Min(value, Length);
                    value = value > 0 ? value : 0;

                    for (int i = 0; i < _scan.Frames.Count; i++)
                    {
                        if ((value / WaveFormat.BlockAlign) <= _scan.Frames[i].SampleOffset)
                        {
                            _stream.Position = _scan.Frames[i].StreamOffset;
                            _frameIndex = i;
                            if (_stream.Position >= _stream.Length)
                                throw new EndOfStreamException("Stream got EOF.");
#if DIAGNOSTICS
                            _position = _scan.Frames[i].SampleOffset * WaveFormat.BlockAlign;
#endif
                            _overflowCount = 0;
                            _overflowOffset = 0;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the length of the <see cref="FlacFile" /> in bytes.
        /// </summary>
        public long Length
        {
            get
            {
                if (CanSeek)
                    return _scan.TotalSamples * WaveFormat.BlockAlign;
                return -1;
            }
        }

        /// <summary>
        ///     Disposes the <see cref="FlacFile" /> instance and disposes the underlying stream.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Disposes the <see cref="FlacFile" /> instance and disposes the underlying stream.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            lock (_bufferLock)
            {
                if (_frame != null)
                {
                    _frame.FreeBuffers();
                    _frame = null;
                }

                if (!_stream.IsClosed())
                    _stream.Dispose();
            }
        }

        /// <summary>
        ///     Destructor which calls the <see cref="Dispose(bool)" /> method.
        /// </summary>
        ~FlacFile()
        {
            Dispose(false);
        }
    }
}