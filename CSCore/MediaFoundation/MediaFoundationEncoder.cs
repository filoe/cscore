using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using CSCore.Codecs.AAC;
using CSCore.DMO;
using CSCore.Win32;

namespace CSCore.MediaFoundation
{
    /// <summary>
    ///     Provides encoding through the Mediafoundation.
    /// </summary>
    public class MediaFoundationEncoder : IDisposable, IWriteable
    {
        private readonly Stream _targetBaseStream;
        private bool _disposed;
        private long _position;
        private MFSinkWriter _sinkWriter;
        private int _sourceBytesPerSecond;
        private int _streamIndex;
        private MFMediaType _targetMediaType;
        private IMFByteStream _targetStream;

        static MediaFoundationEncoder()
        {
            MediaFoundationCore.Startup();
        }

        internal MediaFoundationEncoder()
        {
        }

        /// <summary>
        ///     Creates an new instance of the MediaFoundationEncoder.
        /// </summary>
        /// <param name="inputMediaType">Mediatype of the source which gets encoded.</param>
        /// <param name="stream">Stream which will be used to store the encoded data.</param>
        /// <param name="targetMediaType">The format, the data gets encoded to.</param>
        /// <param name="containerType">See TranscodeContainerTypes-class.</param>
        public MediaFoundationEncoder(Stream stream, MFMediaType inputMediaType, MFMediaType targetMediaType,
            Guid containerType)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanWrite)
                throw new ArgumentException("Stream is not writeable.");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.");

            if (inputMediaType == null)
                throw new ArgumentNullException("inputMediaType");
            if (targetMediaType == null)
                throw new ArgumentNullException("targetMediaType");

            if (containerType == Guid.Empty)
                throw new ArgumentException("containerType");

            _targetBaseStream = stream;
            _targetMediaType = targetMediaType;

            SetTargetStream(stream, inputMediaType, targetMediaType, containerType);
        }

        /// <summary>
        ///     Gets the total length of all encoded data.
        /// </summary>
        public TimeSpan EncodedDuration
        {
            get { return TimeSpan.FromTicks(_position); }
        }

        /// <summary>
        ///     Gets the underlying stream which operates as encoding target.
        /// </summary>
        public Stream TargetBaseStream
        {
            get { return _targetBaseStream; }
        }

        /// <summary>
        ///     Gets the  OutputMediaType.
        /// </summary>
        public MFMediaType OutputMediaType
        {
            get { return _targetMediaType; }
        }

        /// <summary>
        /// Gets the <see cref="MFSinkWriter"/> which is used to write.
        /// </summary>
        protected MFSinkWriter SinkWriter
        {
            get { return _sinkWriter; }
            set { _sinkWriter = value; }
        }

        /// <summary>
        /// Gets the destination stream which is used to store the encoded audio data.
        /// </summary>
        protected IMFByteStream TargetStream
        {
            get { return _targetStream; }
            set { _targetStream = value; }
        }

        /// <summary>
        ///     Releases all resources used by the encoder and finalizes encoding.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Encodes raw data.
        /// </summary>
        /// <param name="buffer">Buffer which contains raw data to encode.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin encoding bytes to the underlying stream.</param>
        /// <param name="count">The number of bytes to encode.</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            CheckForDisposed();

            if (count <= 0)
                return;

            int bytesToWrite = Math.Min(_sourceBytesPerSecond * 4, count);

            long written = WriteBlock(buffer, offset, bytesToWrite, _streamIndex, _position, _sourceBytesPerSecond);
            _position += written;
        }

        /// <summary>
        /// Encodes the whole <paramref name="source"/> with the specified <paramref name="encoder"/>. The encoding process stops as soon as the <see cref="IWaveSource.Read"/> method of the specified <paramref name="source"/> returns 0.
        /// </summary>
        /// <param name="encoder">The encoder which should be used to encode the audio data.</param>
        /// <param name="source">The <see cref="IWaveSource"/> which provides the raw audio data to encode.</param>
        public static void EncodeWholeSource(MediaFoundationEncoder encoder, IWaveSource source)
        {
            var buffer = new byte[source.WaveFormat.BytesPerSecond * 4];
            int read;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                Debug.WriteLine(String.Format("{0:#00.00}%", source.Position / (double) source.Length * 100));
                encoder.Write(buffer, 0, read);
            }
        }

        /// <summary>
        ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/hh162907(v=vs.85).aspx for supported input and output
        ///     types.
        /// </summary>
// ReSharper disable once InconsistentNaming
        public static MediaFoundationEncoder CreateMP3Encoder(WaveFormat sourceFormat, string targetFilename,
            int bitRate = 192000)
        {
            return CreateMP3Encoder(sourceFormat, File.Open(targetFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite),
                bitRate);
        }

        /// <summary>
        ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/hh162907(v=vs.85).aspx for supported input and output
        ///     types.
        /// </summary>
// ReSharper disable once InconsistentNaming
        public static MediaFoundationEncoder CreateMP3Encoder(WaveFormat sourceFormat, Stream targetStream,
            int bitRate = 192000)
        {
            if (sourceFormat == null)
                throw new ArgumentNullException("sourceFormat");
            if (targetStream == null)
                throw new ArgumentNullException("targetStream");
            if (targetStream.CanWrite != true)
                throw new ArgumentException("Stream not writeable.", "targetStream");

            MFMediaType targetMediaType = FindBestMediaType(MFMediaTypes.MFAudioFormat_MP3, sourceFormat.SampleRate,
                sourceFormat.Channels, bitRate);
            MFMediaType sourceMediaType = MediaFoundationCore.MediaTypeFromWaveFormat(sourceFormat);

            if (targetMediaType == null)
                throw new PlatformNotSupportedException("No MP3-Encoder was found.");

            return new MediaFoundationEncoder(targetStream, sourceMediaType, targetMediaType,
                TranscodeContainerTypes.MFTranscodeContainerType_MP3);
        }

        /// <summary>
        ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/ff819498(v=vs.85).aspx for supported input and output
        ///     types.
        /// </summary>
// ReSharper disable once InconsistentNaming
        public static MediaFoundationEncoder CreateWMAEncoder(WaveFormat sourceFormat, string targetFilename,
            int bitRate = 192000)
        {
            return CreateWMAEncoder(sourceFormat, File.Open(targetFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite),
                bitRate);
        }

        /// <summary>
        ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/ff819498(v=vs.85).aspx for supported input and output
        ///     types.
        /// </summary>
// ReSharper disable once InconsistentNaming
        public static MediaFoundationEncoder CreateWMAEncoder(WaveFormat sourceFormat, Stream targetStream,
            int bitRate = 192000)
        {
            if (sourceFormat == null)
                throw new ArgumentNullException("sourceFormat");
            if (targetStream == null)
                throw new ArgumentNullException("targetStream");
            if (targetStream.CanWrite != true)
                throw new ArgumentException("Stream not writeable.", "targetStream");

            MFMediaType targetMediaType = FindBestMediaType(MFMediaTypes.MFAudioFormat_WMAudioV8,
                sourceFormat.SampleRate, sourceFormat.Channels, bitRate);
            MFMediaType sourceMediaType = MediaFoundationCore.MediaTypeFromWaveFormat(sourceFormat);

            if (targetMediaType == null)
                throw new PlatformNotSupportedException("No WMA-Encoder was found.");

            return new MediaFoundationEncoder(targetStream, sourceMediaType, targetMediaType,
                TranscodeContainerTypes.MFTranscodeContainerType_ASF);
        }

        /// <summary>
        ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/dd742785(v=vs.85).aspx for supported input and output
        ///     types.
        /// </summary>
// ReSharper disable once InconsistentNaming
        public static MediaFoundationEncoder CreateAACEncoder(WaveFormat sourceFormat, string targetFilename,
            int bitRate = 192000)
        {
            return CreateAACEncoder(sourceFormat, File.Open(targetFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite),
                bitRate);
        }

        /// <summary>
        ///     See http://msdn.microsoft.com/en-us/library/windows/desktop/dd742785(v=vs.85).aspx for supported input and output
        ///     types.
        /// </summary>
// ReSharper disable once InconsistentNaming
        public static MediaFoundationEncoder CreateAACEncoder(WaveFormat sourceFormat, Stream targetStream,
            int bitRate = 192000)
        {
            return new AacEncoder(sourceFormat, targetStream, bitRate,
                TranscodeContainerTypes.MFTranscodeContainerType_MPEG4);
        }

        /// <summary>
        /// Tries to find the <see cref="MediaType"/> which fits best the requested format specified by the parameters: <paramref name="sampleRate"/>, <paramref name="channels"/>, <paramref name="bitRate"/> and <paramref name="audioSubType"/>.
        /// </summary>
        /// <param name="audioSubType">The audio subtype. See the <see cref="AudioSubTypes"/> class.</param>
        /// <param name="sampleRate">The requested sample rate.</param>
        /// <param name="channels">The requested number of channels.</param>
        /// <param name="bitRate">The requested bit rate.</param>
        /// <returns>A <see cref="MediaType"/> which fits best the requested format. If no mediatype could be found the <see cref="FindBestMediaType"/> method returns null.</returns>
        protected static MFMediaType FindBestMediaType(Guid audioSubType, int sampleRate, int channels, int bitRate)
        {
            MFMediaType[] mediaTypes = MediaFoundationCore.GetEncoderMediaTypes(audioSubType);
            IEnumerable<MFMediaType> n = mediaTypes.Where(x => x.SampleRate == sampleRate && x.Channels == channels);
            var availableMediaTypes = n.Select(x => new
            {
                mediaType = x,
                dif = Math.Abs(bitRate - (x.AverageBytesPerSecond * 8))
            });

            return availableMediaTypes.OrderBy(x => x.dif).Select(x => x.mediaType).FirstOrDefault();
        }

        /// <summary>
        /// Sets and initializes the targetstream for the encoding process.
        /// </summary>
        /// <param name="stream">Stream which should be used as the targetstream.</param>
        /// <param name="inputMediaType">Mediatype of the raw input data which should be encoded.</param>
        /// <param name="targetMediaType">Mediatype of the encoded data.</param>
        /// <param name="containerType">Container type which should be used. See the <see cref="TranscodeContainerTypes"/> class.</param>
        protected void SetTargetStream(Stream stream, MFMediaType inputMediaType, MFMediaType targetMediaType,
            Guid containerType)
        {
            IMFAttributes attributes = null;
            try
            {
                _targetStream = MediaFoundationCore.IStreamToByteStream(new ComStream(stream));

                var flags = MFByteStreamCapsFlags.None;
                _targetStream.GetCapabilities(ref flags); //TODO: Remove this call.

                attributes = MediaFoundationCore.CreateEmptyAttributes(2);
                attributes.SetUINT32(MediaFoundationAttributes.MF_READWRITE_ENABLE_HARDWARE_TRANSFORMS, 1);
                attributes.SetGUID(MediaFoundationAttributes.MF_TRANSCODE_CONTAINERTYPE, containerType);

                _sinkWriter = MediaFoundationCore.CreateSinkWriterFromMFByteStream(_targetStream, attributes);

                _streamIndex = _sinkWriter.AddStream(targetMediaType);
                _sinkWriter.SetInputMediaType(_streamIndex, inputMediaType, null);

                _targetMediaType = targetMediaType;
                _sourceBytesPerSecond = inputMediaType.AverageBytesPerSecond;

                //initialize the sinkwriter
                _sinkWriter.BeginWriting();
            }
            catch (Exception)
            {
                if (_sinkWriter != null)
                {
                    _sinkWriter.Dispose();
                    _sinkWriter = null;
                }
                if (_targetStream != null)
                {
                    _targetStream.Close();
                    Marshal.ReleaseComObject(_targetStream);
                    _targetStream = null;
                }
                throw;
            }
            finally
            {
                if (attributes != null)
                    Marshal.ReleaseComObject(attributes);
            }
        }

        /// <returns>Ticks, NO BYTES!</returns>
        private long WriteBlock(byte[] buffer, int offset, int count, int streamIndex, long positionInTicks,
            int sourceBytesPerSecond)
        {
            int bytesToWrite = count;

            using (var mfBuffer = new MFMediaBuffer(MediaFoundationCore.CreateMemoryBuffer(bytesToWrite)))
            {
                using (var sample = new MFSample(MediaFoundationCore.CreateEmptySample()))
                {
                    sample.AddBuffer(mfBuffer);

                    int currentLength, maxLength;
                    IntPtr bufferPtr = mfBuffer.Lock(out maxLength, out currentLength);

                    long ticks = BytesToNanoSeconds(count, sourceBytesPerSecond);
                    Marshal.Copy(buffer, offset, bufferPtr, count);
                    mfBuffer.SetCurrentLength(count);
                    mfBuffer.Unlock();

                    sample.SetSampleTime(positionInTicks);
                    sample.SetSampleDuration(ticks);
                    _sinkWriter.WriteSample(streamIndex, sample);

                    return ticks;
                }
            }
        }

        private static long BytesToNanoSeconds(int byteLength, int bytesPerSecond)
        {
            return (10000000L * byteLength) / bytesPerSecond;
        }

        private void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("MediaFoundationEncoder");
        }

        /// <summary>
        /// Disposes the <see cref="MediaFoundationEncoder"/>.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_sinkWriter != null)
                {
                    while (_sinkWriter.GetStatistics(_streamIndex).DwByteCountQueued > 0)
                    {
                        Thread.Sleep(50);
                    }
                    _sinkWriter.Flush(_streamIndex);
                    _sinkWriter.FinalizeWriting();
                    _sinkWriter.Dispose();
                    _sinkWriter = null;
                }
                if (_targetStream != null)
                {
                    _targetStream.Flush();
                    _targetStream.Close();
                    Marshal.ReleaseComObject(_targetStream);
                    _targetStream = null;
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Destructor which calls <see cref="Dispose(bool)"/>.
        /// </summary>
        ~MediaFoundationEncoder()
        {
            Dispose(false);
        }
    }
}