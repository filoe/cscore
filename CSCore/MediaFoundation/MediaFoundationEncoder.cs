using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSCore.Win32;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace CSCore.MediaFoundation
{
    /// <summary>
    /// Provides encoding through the Mediafoundation.
    /// </summary>
    public class MediaFoundationEncoder : IDisposable, IWritable
    {
        public static void EncodeWholeSource(MediaFoundationEncoder encoder, IWaveSource source)
        {
            byte[] buffer = new byte[source.WaveFormat.BytesPerSecond * 4];
            int read = 0;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                Debug.WriteLine(String.Format("{0:#00.00}%", (double)source.Position / (double)source.Length * 100));
                encoder.Write(buffer, 0, read);
            }
        }

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/hh162907(v=vs.85).aspx for supported input and output types.
        /// </summary>
        public static MediaFoundationEncoder CreateMP3Encoder(WaveFormat sourceFormat, string targetFilename, int bitRate = 192000)
        {
            return CreateMP3Encoder(sourceFormat, File.Open(targetFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite), bitRate);
        }

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/hh162907(v=vs.85).aspx for supported input and output types.
        /// </summary>
        public static MediaFoundationEncoder CreateMP3Encoder(WaveFormat sourceFormat, Stream targetStream, int bitRate = 192000)
        {
            if (sourceFormat == null)
                throw new ArgumentNullException("sourceFormat");
            if (targetStream == null)
                throw new ArgumentNullException("targetStream");
            if (targetStream.CanWrite != true)
                throw new ArgumentException("Stream not writeable.", "targetStream");

            var targetMediaType = FindBestMediaType(MediaFoundation.MFMediaTypes.MFAudioFormat_MP3, sourceFormat.SampleRate, sourceFormat.Channels, bitRate);
            var sourceMediaType = MediaFoundationCore.MediaTypeFromWaveFormat(sourceFormat);

            if (targetMediaType == null)
                throw new PlatformNotSupportedException("No MP3-Encoder was found.");

            return new MediaFoundationEncoder(targetStream, sourceMediaType, targetMediaType, TranscodeContainerTypes.MFTranscodeContainerType_MP3);
        }

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/ff819498(v=vs.85).aspx for supported input and output types.
        /// </summary>
        public static MediaFoundationEncoder CreateWMAEncoder(WaveFormat sourceFormat, string targetFilename, int bitRate = 192000)
        {
            return CreateWMAEncoder(sourceFormat, File.Open(targetFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite), bitRate);
        }

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/ff819498(v=vs.85).aspx for supported input and output types.
        /// </summary>
        public static MediaFoundationEncoder CreateWMAEncoder(WaveFormat sourceFormat, Stream targetStream, int bitRate = 192000)
        {
            if (sourceFormat == null)
                throw new ArgumentNullException("sourceFormat");
            if (targetStream == null)
                throw new ArgumentNullException("targetStream");
            if (targetStream.CanWrite != true)
                throw new ArgumentException("Stream not writeable.", "targetStream");

            var targetMediaType = FindBestMediaType(MediaFoundation.MFMediaTypes.MFAudioFormat_WMAudioV8, sourceFormat.SampleRate, sourceFormat.Channels, bitRate);
            var sourceMediaType = MediaFoundationCore.MediaTypeFromWaveFormat(sourceFormat);

            if (targetMediaType == null)
                throw new PlatformNotSupportedException("No WMA-Encoder was found.");

            return new MediaFoundationEncoder(targetStream, sourceMediaType, targetMediaType, TranscodeContainerTypes.MFTranscodeContainerType_ASF);
        }

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd742785(v=vs.85).aspx for supported input and output types.
        /// </summary>
        public static MediaFoundationEncoder CreateAACEncoder(WaveFormat sourceFormat, string targetFilename, int bitRate = 192000)
        {
            return CreateAACEncoder(sourceFormat, File.Open(targetFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite), bitRate);
        }

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd742785(v=vs.85).aspx for supported input and output types.
        /// </summary>
        public static MediaFoundationEncoder CreateAACEncoder(WaveFormat sourceFormat, Stream targetStream, int bitRate = 192000)
        {
            return new CSCore.Codecs.AAC.AACEncoder(sourceFormat, targetStream, bitRate, TranscodeContainerTypes.MFTranscodeContainerType_MPEG4);
        }

        protected static MFMediaType FindBestMediaType(Guid audioSubType, int sampleRate, int channels, int bitRate)
        {
            var mediaTypes = MediaFoundationCore.GetEncoderMediaTypes(audioSubType);
            var n = mediaTypes.Where(x => x.SampleRate == sampleRate && x.Channels == channels);
            var availableMediaTypes = n.Select(x => new
            {
                mediaType = x,
                dif = Math.Abs(bitRate - (x.AverageBytesPerSecond * 8))
            });

            return availableMediaTypes.OrderBy(x => x.dif).Select(x => x.mediaType).FirstOrDefault();
        }

        private IMFByteStream _targetStream;
        private MFSinkWriter _sinkWriter;
        private int _streamIndex;

        private Stream _targetBaseStream;

        private MFMediaType _inputMediaType, _targetMediaType;
        private int _sourceBytesPerSecond;
        private long _position = 0;

        internal MediaFoundationEncoder()
        {
        }

        /// <summary>
        /// Creates an new instance of the MediaFoundationEncoder. 
        /// </summary>
        /// <param name="inputMediaType">Mediatype of the source which gets encoded.</param>
        /// <param name="stream">Stream which will be used to store the encoded data.</param>
        /// <param name="targetMediaType">The format, the data gets encoded to.</param>
        /// <param name="containerType">See TranscodeContainerTypes-class.</param>
        public MediaFoundationEncoder(Stream stream, MFMediaType inputMediaType, MFMediaType targetMediaType, Guid containerType)
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
            _inputMediaType = inputMediaType;
            _targetMediaType = targetMediaType;

            SetTargetStream(stream, inputMediaType, targetMediaType, containerType);
        }

        public void Initialize()
        {

        }

        protected void SetTargetStream(Stream stream, MFMediaType inputMediaType, MFMediaType targetMediaType, Guid containerType)
        {
            IMFAttributes attributes = null;
            try
            {
                _targetStream = MediaFoundationCore.IStreamToByteStream(new ComStream(stream));

                MFByteStreamCapsFlags flags = MFByteStreamCapsFlags.None;
                int result = _targetStream.GetCapabilities(ref flags);

                attributes = MediaFoundationCore.CreateEmptyAttributes(2);
                attributes.SetUINT32(MediaFoundationAttributes.MF_READWRITE_ENABLE_HARDWARE_TRANSFORMS, 1);
                attributes.SetGUID(MediaFoundationAttributes.MF_TRANSCODE_CONTAINERTYPE, containerType);

                _sinkWriter = MediaFoundationCore.CreateSinkWriterFromMFByteStream(_targetStream, attributes);

                _streamIndex = _sinkWriter.AddStream(targetMediaType);
                _sinkWriter.SetInputMediaType(_streamIndex, inputMediaType, null);

                _inputMediaType = inputMediaType;
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

        /// <summary>
        /// Encodes raw data.
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

        private readonly Guid z = new Guid(0x9cdf01d9, 0xa0f0, 0x43ba, 0xb0, 0x77, 0xea, 0xa0, 0x6c, 0xbd, 0x72, 0x8a);

        /// <returns>Ticks, NO BYTES!</returns>
        private long WriteBlock(byte[] buffer, int offset, int count, int streamIndex, long positionInTicks, int sourceBytesPerSecond)
        {
            int bytesToWrite = count;

            using (MFMediaBuffer mfBuffer = new MFMediaBuffer(MediaFoundationCore.CreateMemoryBuffer(bytesToWrite)))
            {
                using (MFSample sample = new MFSample(MediaFoundationCore.CreateEmptySample()))
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
                    _sinkWriter.WriteSample(_streamIndex, sample);

                    return ticks;
                }
            }
        }

        private static long BytesToNanoSeconds(int byteLength, int bytesPerSecond)
        {
            return (10000000L * byteLength) / bytesPerSecond;
        }

        /// <summary>
        /// Gets the total length of all encoded data.
        /// </summary>
        public TimeSpan EncodedDuration
        {
            get { return TimeSpan.FromTicks(_position); }
        }

        /// <summary>
        /// Gets the underlying stream which operates as encoding target.
        /// </summary>
        public Stream TargetBaseStream
        {
            get { return _targetBaseStream; }
        }

        /// <summary>
        /// Gets the  OutputMediaType.
        /// </summary>
        public MFMediaType OutputMediaType
        {
            get { return _targetMediaType; }
        }

        protected MFSinkWriter SinkWriter
        {
            get { return _sinkWriter; }
            set { _sinkWriter = value; }
        }

        protected IMFByteStream TargetStream
        {
            get { return _targetStream; }
            set { _targetStream = value; }
        }

        protected void CheckForDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("MediaFoundationEncoder");
        }

        private bool _disposed;
        /// <summary>
        /// Releases all resources used by the encoder and finalizes encoding.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        ~MediaFoundationEncoder()
        {
            Dispose(false);
        }
    }
}
