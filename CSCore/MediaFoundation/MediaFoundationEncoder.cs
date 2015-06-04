using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using CSCore.Codecs.AAC;
using CSCore.DMO;
using CSCore.Win32;

namespace CSCore.MediaFoundation
{
    /// <summary>
    ///     A generic encoder for all installed Mediafoundation-Encoders.
    /// </summary>
    public class MediaFoundationEncoder : IDisposable, IWriteable
    {
        private bool _disposed;
        private long _position;
        private MFSinkWriter _sinkWriter;
        private int _sourceBytesPerSecond;
        private int _streamIndex;
        private ComStream _targetBaseStream;
        private MFMediaType _targetMediaType;
        private MFByteStream _targetStream;

        static MediaFoundationEncoder()
        {
            MediaFoundationCore.Startup();
        }

        internal MediaFoundationEncoder()
        {
        }

        /// <summary>
        ///     Creates an new instance of the <see cref="MediaFoundationEncoder"/> class.
        /// </summary>
        /// <param name="inputMediaType">Mediatype of the source to encode.</param>
        /// <param name="stream">Stream which will be used to store the encoded data.</param>
        /// <param name="targetMediaType">The format of the encoded data.</param>
        /// <param name="containerType">See container type. For a list of all available container types, see <see cref="TranscodeContainerTypes"/>.</param>
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

            _targetMediaType = targetMediaType;

            SetTargetStream(stream, inputMediaType, targetMediaType, containerType);
        }

        /// <summary>
        ///     Gets the total duration of all encoded data.
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
        ///     Gets the media type of the encoded data.
        /// </summary>
        public MFMediaType OutputMediaType
        {
            get { return _targetMediaType; }
        }

        /// <summary>
        ///     Gets the <see cref="MFSinkWriter" /> which is used to write to the <see cref="TargetStream"/>.
        /// </summary>
        protected MFSinkWriter SinkWriter
        {
            get { return _sinkWriter; }
            set { _sinkWriter = value; }
        }

        /// <summary>
        ///     Gets the destination stream which is used to store the encoded audio data.
        /// </summary>
        protected MFByteStream TargetStream
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
        ///     Encodes raw audio data.
        /// </summary>
        /// <param name="buffer">A byte-array which contains raw data to encode.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin encoding bytes to the underlying stream.</param>
        /// <param name="count">The number of bytes to encode.</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            CheckForDisposed();

            if (count <= 0)
                return;

            while (count > 0)
            {
                int bytesToWrite = Math.Min(_sourceBytesPerSecond * 4, count);
                long written = WriteBlock(buffer, offset, bytesToWrite, _streamIndex, _position, _sourceBytesPerSecond);
                count -= bytesToWrite;
                _position += written;
            }
        }

        /// <summary>
        ///     Sets and initializes the targetstream for the encoding process.
        /// </summary>
        /// <param name="stream">Stream which should be used as the targetstream.</param>
        /// <param name="inputMediaType">Mediatype of the raw input data to encode.</param>
        /// <param name="targetMediaType">Mediatype of the encoded data.</param>
        /// <param name="containerType">Container type which should be used.</param>
        protected void SetTargetStream(Stream stream, MFMediaType inputMediaType, MFMediaType targetMediaType,
            Guid containerType)
        {
            MFAttributes attributes = null;
            try
            {
                _targetBaseStream = new ComStream(stream);
                _targetStream = MediaFoundationCore.IStreamToByteStream(_targetBaseStream);

                attributes = new MFAttributes(2);
                attributes.SetUINT32(MediaFoundationAttributes.MF_READWRITE_ENABLE_HARDWARE_TRANSFORMS, 1);
                attributes.SetGuid(MediaFoundationAttributes.MF_TRANSCODE_CONTAINERTYPE, containerType);

                _sinkWriter = new MFSinkWriter(_targetStream, attributes);

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
                    _targetStream.Dispose();
                    _targetStream = null;
                }
                throw;
            }
            finally
            {
                if (attributes != null)
                    attributes.Dispose();
            }
        }

        //This method returns ticks, not bytes!
        private long WriteBlock(byte[] buffer, int offset, int count, int streamIndex, long positionInTicks,
            int sourceBytesPerSecond)
        {
            using (var mfBuffer = new MFMediaBuffer(count))
            {
                using (var sample = new MFSample())
                {
                    sample.AddBuffer(mfBuffer);

                    using (var @lock = mfBuffer.Lock())
                    {
                        Marshal.Copy(buffer, offset, @lock.Buffer, count);
                        mfBuffer.SetCurrentLength(count);
                    }

                    long ticks = BytesToNanoSeconds(count, sourceBytesPerSecond);

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
        ///     Disposes the <see cref="MediaFoundationEncoder" />.
        /// </summary>
        /// <param name="disposing">
        ///     True to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_sinkWriter != null)
                {
                    //thanks to martin48 (and naudio??) for providing the following source code (see http://cscore.codeplex.com/discussions/574280):
                    MFSinkWriterStatistics statistics = _sinkWriter.GetStatistics(_streamIndex);
                    if (statistics.ByteCountQueued > 0 || statistics.NumSamplesReceived > 0)
                        _sinkWriter.FinalizeWriting();

                    _sinkWriter.Dispose();
                    _sinkWriter = null;
                }
                if (_targetStream != null)
                {
                    _targetStream.Flush();
                    _targetStream.Dispose();
                    _targetStream = null;
                }
                if (_targetBaseStream != null && !_targetBaseStream.IsClosed())
                {
                    _targetBaseStream.Flush();
                    _targetBaseStream.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MediaFoundationEncoder"/> class.
        /// </summary>
        ~MediaFoundationEncoder()
        {
            Dispose(false);
        }

        #region create_encoder

        /// <summary>
        ///     Encodes the whole <paramref name="source" /> with the specified <paramref name="encoder" />. The encoding process
        ///     stops as soon as the <see cref="IReadableAudioSource{T}.Read" /> method of the specified <paramref name="source" />
        ///     returns 0.
        /// </summary>
        /// <param name="encoder">The encoder which should be used to encode the audio data.</param>
        /// <param name="source">The <see cref="IWaveSource" /> which provides the raw audio data to encode.</param>
        public static void EncodeWholeSource(MediaFoundationEncoder encoder, IWaveSource source)
        {
            if (encoder == null)
                throw new ArgumentNullException("encoder");
            if (source == null)
                throw new ArgumentNullException("source");
            var buffer = new byte[source.WaveFormat.BytesPerSecond * 4];
            int read;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                Debug.WriteLine(String.Format("{0:#00.00}%", source.Position / (double) source.Length * 100));
                encoder.Write(buffer, 0, read);
            }
        }

        /// <summary>
        ///     Returns a new instance of the <see cref="MediaFoundationEncoder"/> class, configured as mp3 encoder.
        /// </summary>
        /// <param name="sourceFormat">The input format, of the data to encode.</param>
        /// <param name="bitRate">The bitrate to use. The final bitrate can differ from the specified value.</param>        
        /// <param name="targetFilename">The file to write to.</param>
        /// <remarks>For more information about supported input and output formats, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/hh162907(v=vs.85).aspx"/>.</remarks>
        /// <returns>A new instance of the <see cref="MediaFoundationEncoder"/> class, configured as mp3 encoder.</returns>        
        // ReSharper disable once InconsistentNaming
        public static MediaFoundationEncoder CreateMP3Encoder(WaveFormat sourceFormat, string targetFilename,
            int bitRate = 192000)
        {
            return CreateMP3Encoder(sourceFormat, File.Open(targetFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite),
                bitRate);
        }

        /// <summary>
        ///     Returns a new instance of the <see cref="MediaFoundationEncoder"/> class, configured as mp3 encoder.
        /// </summary>
        /// <param name="sourceFormat">The input format, of the data to encode.</param>
        /// <param name="bitRate">The bitrate to use. The final bitrate can differ from the specified value.</param>        
        /// <param name="targetStream">The stream to write to.</param>
        /// <remarks>For more information about supported input and output formats, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/hh162907(v=vs.85).aspx"/>.</remarks>
        /// <returns>A new instance of the <see cref="MediaFoundationEncoder"/> class, configured as mp3 encoder.</returns>
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

            MFMediaType targetMediaType = FindBestMediaType(AudioSubTypes.MpegLayer3, sourceFormat.SampleRate,
                sourceFormat.Channels, bitRate);
            MFMediaType sourceMediaType = MediaFoundationCore.MediaTypeFromWaveFormat(sourceFormat);

            if (targetMediaType == null)
                throw new PlatformNotSupportedException("No MP3-Encoder was found.");

            return new MediaFoundationEncoder(targetStream, sourceMediaType, targetMediaType,
                TranscodeContainerTypes.MFTranscodeContainerType_MP3);
        }

        /// <summary>
        ///     Returns a new instance of the <see cref="MediaFoundationEncoder"/> class, configured as wma encoder.
        /// </summary>
        /// <param name="sourceFormat">The input format, of the data to encode.</param>
        /// <param name="bitRate">The bitrate to use. The final bitrate can differ from the specified value.</param>        
        /// <param name="targetFilename">The file to write to.</param>
        /// <remarks>For more information about supported input and output formats, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ff819498(v=vs.85).aspx"/>.</remarks>
        /// <returns>A new instance of the <see cref="MediaFoundationEncoder"/> class, configured as wma encoder.</returns>
        // ReSharper disable once InconsistentNaming
        public static MediaFoundationEncoder CreateWMAEncoder(WaveFormat sourceFormat, string targetFilename,
            int bitRate = 192000)
        {
            return CreateWMAEncoder(sourceFormat, File.Open(targetFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite),
                bitRate);
        }

        /// <summary>
        ///     Returns a new instance of the <see cref="MediaFoundationEncoder"/> class, configured as wma encoder.
        /// </summary>
        /// <param name="sourceFormat">The input format, of the data to encode.</param>
        /// <param name="bitRate">The bitrate to use. The final bitrate can differ from the specified value.</param>        
        /// <param name="targetStream">The stream to write to.</param>
        /// <remarks>For more information about supported input and output formats, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/ff819498(v=vs.85).aspx"/>.</remarks>
        /// <returns>A new instance of the <see cref="MediaFoundationEncoder"/> class, configured as wma encoder.</returns>        
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

            MFMediaType targetMediaType = FindBestMediaType(AudioSubTypes.WindowsMediaAudio,
                sourceFormat.SampleRate, sourceFormat.Channels, bitRate);
            MFMediaType sourceMediaType = MediaFoundationCore.MediaTypeFromWaveFormat(sourceFormat);

            if (targetMediaType == null)
                throw new PlatformNotSupportedException("No WMA-Encoder was found.");

            return new MediaFoundationEncoder(targetStream, sourceMediaType, targetMediaType,
                TranscodeContainerTypes.MFTranscodeContainerType_ASF);
        }

        /// <summary>
        ///     Returns a new instance of the <see cref="MediaFoundationEncoder"/> class, configured as aac encoder.
        /// </summary>
        /// <param name="sourceFormat">The input format, of the data to encode.</param>
        /// <param name="bitRate">The bitrate to use. The final bitrate can differ from the specified value.</param>        
        /// <param name="targetFilename">The file to write to.</param>
        /// <remarks>For more information about supported input and output formats, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd742785(v=vs.85).aspx"/>.</remarks>
        /// <returns>A new instance of the <see cref="MediaFoundationEncoder"/> class, configured as aac encoder.</returns>        
        // ReSharper disable once InconsistentNaming
        public static MediaFoundationEncoder CreateAACEncoder(WaveFormat sourceFormat, string targetFilename,
            int bitRate = 192000)
        {
            return CreateAACEncoder(sourceFormat, File.Open(targetFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite),
                bitRate);
        }

        /// <summary>
        ///     Returns a new instance of the <see cref="MediaFoundationEncoder"/> class, configured as aac encoder.
        /// </summary>
        /// <param name="sourceFormat">The input format, of the data to encode.</param>
        /// <param name="bitRate">The bitrate to use. The final bitrate can differ from the specified value.</param>        
        /// <param name="targetStream">The stream to write to.</param>
        /// <remarks>For more information about supported input and output formats, see <see href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd742785(v=vs.85).aspx"/>.</remarks>
        /// <returns>A new instance of the <see cref="MediaFoundationEncoder"/> class, configured as aac encoder.</returns>        
        // ReSharper disable once InconsistentNaming
        public static MediaFoundationEncoder CreateAACEncoder(WaveFormat sourceFormat, Stream targetStream,
            int bitRate = 192000)
        {
            return new AacEncoder(sourceFormat, targetStream, bitRate,
                TranscodeContainerTypes.MFTranscodeContainerType_MPEG4);
        }

        /// <summary>
        ///     Tries to find the <see cref="MediaType" /> which fits best the requested format specified by the parameters:
        ///     <paramref name="sampleRate" />, <paramref name="channels" />, <paramref name="bitRate" /> and
        ///     <paramref name="audioSubType" />.
        /// </summary>
        /// <param name="audioSubType">The audio subtype. For more information, see the <see cref="AudioSubTypes" /> class.</param>
        /// <param name="sampleRate">The requested sample rate.</param>
        /// <param name="channels">The requested number of channels.</param>
        /// <param name="bitRate">The requested bit rate.</param>
        /// <returns>
        ///     A <see cref="MediaType" /> which fits best the requested format. If no mediatype could be found the
        ///     <see cref="FindBestMediaType" /> method returns null.
        /// </returns>
        protected static MFMediaType FindBestMediaType(Guid audioSubType, int sampleRate, int channels, int bitRate)
        {
            MFMediaType[] mediaTypes = GetEncoderMediaTypes(audioSubType);
            IEnumerable<MFMediaType> n = mediaTypes.Where(x => x.SampleRate == sampleRate && x.Channels == channels);
            var availableMediaTypes = n.Select(x => new
            {
                mediaType = x,
                dif = Math.Abs(bitRate - (x.AverageBytesPerSecond * 8))
            });

            return availableMediaTypes.OrderBy(x => x.dif).Select(x => x.mediaType).FirstOrDefault();
        }

        /// <summary>
        /// Returns all <see cref="MFMediaType"/>s available for encoding the specified <paramref name="audioSubType"/>.
        /// </summary>
        /// <param name="audioSubType">The audio subtype to search available <see cref="MFMediaType"/>s for.</param>
        /// <returns>Available <see cref="MFMediaType"/>s for the specified <paramref name="audioSubType"/>. If the <see cref="GetEncoderMediaTypes"/> returns an empty array, no encoder for the specified <paramref name="audioSubType"/> was found.</returns>
        public static MFMediaType[] GetEncoderMediaTypes(Guid audioSubType)
        {
            try
            {
                IMFCollection collection;

                MediaFoundationException.Try(
                    NativeMethods.MFTranscodeGetAudioOutputAvailableTypes(audioSubType, MFTEnumFlags.All,
                        IntPtr.Zero, out collection),
                    "Interops",
                    "MFTranscodeGetAudioOutputAvailableTypes");
                try
                {
                    int count;
                    MediaFoundationException.Try(collection.GetElementCount(out count), "IMFCollection",
                        "GetElementCount");
                    MFMediaType[] mediaTypes = new MFMediaType[count];
                    for (int i = 0; i < count; i++)
                    {
                        IntPtr ptr;
                        MediaFoundationException.Try(collection.GetElement(i, out ptr), "IMFCollection", "GetElement");

                        mediaTypes[i] = new MFMediaType(ptr);
                    }

                    return mediaTypes;
                }
                finally
                {
                    Marshal.ReleaseComObject(collection);
                }
            }
            catch (MediaFoundationException ex)
            {
                if (ex.ErrorCode == unchecked((int)0xC00D36D5)) // MF_E_NOT_FOUND
                {
                    return Enumerable.Empty<MFMediaType>().ToArray();
                }

                throw;
            }
        }

        #endregion
    }
}