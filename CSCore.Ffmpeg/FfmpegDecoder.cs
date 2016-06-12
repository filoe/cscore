using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace CSCore.Ffmpeg
{
    /// <summary>
    /// Generic ffmpeg based decoder.
    /// </summary>
    /// <remarks>
    /// Requires the ffmpeg libraries. In order to make sure that the library files are compatible with each other, only use the libraries provided in combination with CSCore.Ffmpeg.
    /// For more details see: http://ffmpeg.org/ 
    /// </remarks>
    public class FfmpegDecoder : IWaveSource
    {
        private readonly Uri _uri;

        static FfmpegDecoder()
        {
            InteropCalls.av_register_all();
            InteropCalls.avcodec_register_all();
        }

        private unsafe InteropCalls.AVFormatContext* _formatContext = null;
        private unsafe InteropCalls.AVStream* _stream = null;
        private readonly Stream _ioStream;

        private int _streamIndex;
        private readonly object _lockObject = new object();

        private byte[] _overflowBuffer = new byte[0];
        private int _overflowCount;
        private int _overflowOffset;
        private long _position;

        private FfmpegStream _ffmpegStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FfmpegDecoder"/> class based on a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="FfmpegException">Any ffmpeg error.</exception>
        /// <exception cref="System.ArgumentNullException">stream</exception>
        /// <exception cref="ArgumentException">Stream is not readable.</exception>
        /// <exception cref="System.OutOfMemoryException">Could not allocate FormatContext.</exception>
        /// <exception cref="System.NotSupportedException">
        /// DBL format is not supported.
        /// or
        /// Audio Sample Format not supported.
        /// </exception>
        public unsafe FfmpegDecoder(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            _ioStream = stream;

            _ffmpegStream = new FfmpegStream(stream);
            InteropCalls.AVIOContext* avioContext = (InteropCalls.AVIOContext*) _ffmpegStream.AvioContext;

            _formatContext = InteropCalls.avformat_alloc_context();
            if (_formatContext == null)
            {
                throw new OutOfMemoryException("Could not allocate FormatContext.");
            }

            _formatContext->pb = avioContext;
            fixed (InteropCalls.AVFormatContext** formatContext = &_formatContext)
            {
                InteropCalls.avformat_open_input(formatContext, "DUMMY-FILENAME", null, null);

                Initialize();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FfmpegDecoder"/> class based on a specified filename or url.
        /// </summary>
        /// <param name="uri">A uri containing a filename or url. </param>
        /// <exception cref="FfmpegException">
        /// Any ffmpeg error.
        /// </exception>
        /// <exception cref="System.NotSupportedException">
        /// DBL format is not supported.
        /// or
        /// Audio Sample Format not supported.
        /// </exception>
        /// <exception cref="ArgumentNullException">uri</exception>
        public unsafe FfmpegDecoder(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");
            _uri = uri;
            fixed (InteropCalls.AVFormatContext** formatContext = &_formatContext)
            {
                int result = InteropCalls.avformat_open_input(formatContext, uri.AbsoluteUri, null, null);
                FfmpegException.Try(result, "avformat_open_input");

                Initialize();
            }
        }

        private unsafe void Initialize()
        {
            int result = InteropCalls.avformat_find_stream_info(_formatContext, null);
            FfmpegException.Try(result, "avformat_find_stream_info");

            OpenCodecContext();

            int bitsPerSample;
            AudioEncoding encoding;
            switch (_stream->codec->sample_fmt)
            {
                case InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_U8:
                case InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_U8P:
                    bitsPerSample = 8;
                    encoding = AudioEncoding.Pcm;
                    break;
                case InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_S16:
                case InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_S16P:
                    bitsPerSample = 16;
                    encoding = AudioEncoding.Pcm;
                    break;
                case InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_S32:
                case InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_S32P:
                    bitsPerSample = 32;
                    encoding = AudioEncoding.Pcm;
                    break;
                case InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_FLT:
                case InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_FLTP:
                    bitsPerSample = 32;
                    encoding = AudioEncoding.IeeeFloat;
                    break;
                case InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_DBL:
                case InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_DBLP:
                    throw new NotSupportedException("DBL format is not supported.");
                default:
                    throw new NotSupportedException("Audio Sample Format not supported.");
            }

            var waveFormat = new WaveFormat(_stream->codec->sample_rate, bitsPerSample, _stream->codec->channels,
                encoding);
            WaveFormat = waveFormat;
        }

        private unsafe void OpenCodecContext()
        {
            int result = InteropCalls.av_find_best_stream(_formatContext, InteropCalls.AVMediaType.AVMEDIA_TYPE_AUDIO,
                -1, -1, null, 0);
            FfmpegException.Try(result, "av_find_best_stream");

            _streamIndex = result;
            var stream = _formatContext->streams[_streamIndex];
            var avCodecContext = stream->codec;
            var decoder = InteropCalls.avcodec_find_decoder(avCodecContext->codec_id);
            if (decoder == null)
            {
                throw new FfmpegException(
                    String.Format("Failed to find a decoder for CodecId {0}.", avCodecContext->codec_id),
                    "avcodec_find_decoder");
            }

            result = InteropCalls.avcodec_open2(avCodecContext, decoder, null);
            FfmpegException.Try(result, "avcodec_open2");

            _stream = stream;
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
                    bufferLength = DecodeNextFrame(ref _overflowBuffer, out packetPosition);
                }
                if (bufferLength <= 0)
                {
                    if (_uri != null && !_uri.IsFile)
                    {
                        //webstream: don't exit, maybe the connection was lost -> give it a try to recover
                        Thread.Sleep(10);
                    }
                    else
                    {
                        break; //no webstream -> exit
                    }
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

        private unsafe int DecodeNextFrame(ref byte[] buffer, out long packetPosition)
        {
            InteropCalls.AVFrame* frame = InteropCalls.av_frame_alloc();
            if (frame == null)
            {
                throw new OutOfMemoryException("Could not allocate frame.");
            }

            InteropCalls.AVPacket packet = new InteropCalls.AVPacket();
            var hPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            try
            {
                int offset = 0;
                do
                {
                    InteropCalls.av_init_packet(&packet);
                    packet.data = null;
                    packet.size = 0;

                    packetPosition = 0;

                    int result = InteropCalls.av_read_frame(_formatContext, &packet);
                    if (result < 0)
                        break;
                    InteropCalls.AVPacket clonedPacket = packet;
                    packetPosition = AvTimeToBytes(packet.pts);
                    try
                    {
                        do
                        {
                            try
                            {
                                int bufferLength = DecodePacket(ref buffer, offset, frame, &packet, out result);
                                if (bufferLength == 0)
                                    break;

                                offset += bufferLength;
                            }
                            catch (FfmpegException)
                            {
                                break;
                            }

                            packet.data += result;
                            packet.size -= result;
                        } while (packet.size > 0);
                    }
                    finally
                    {
                        InteropCalls.av_free_packet(&clonedPacket);
                    }
                } while (offset <= 0);

                return offset;
            }
            finally
            {
                InteropCalls.av_free(frame);
                hPacket.Free();
            }

        }

        private unsafe int DecodePacket(ref byte[] buffer, int offset, InteropCalls.AVFrame* frame, InteropCalls.AVPacket* packet, out int bytesConsumed)
        {
            var stream = _formatContext->streams[_streamIndex];
            var decoderContext = stream->codec;
            int gotFrame;

            int result = InteropCalls.avcodec_decode_audio4(decoderContext, frame, &gotFrame, packet);
            FfmpegException.Try(result, "avcodec_decode_audio4");

            bytesConsumed = Math.Min(result, packet->size);

            if (gotFrame != 0)
            {
                int dataSize = InteropCalls.av_get_bytes_per_sample(frame->format);
                if (dataSize < 0)
                {
                    throw new FfmpegException("Could not calculate data size.");
                }

                result = InteropCalls.av_samples_get_buffer_size(null, frame->channels, frame->nb_samples, frame->format, 1);
                FfmpegException.Try(result, "av_samples_get_buffer_size");
                int size = result;
                if (buffer.Length < offset + size)
                {
                    byte[] bufferTemp = new byte[offset + size];
                    Buffer.BlockCopy(buffer, 0, bufferTemp, 0, buffer.Length);
                    buffer = bufferTemp;
                }

                if (IsPlanar(frame->format))
                {
                    for (int c = 0; c < frame->channels; c++)
                    {
                        for (int i = 0; i < frame->nb_samples; i++)
                        {
                            if(dataSize == 1)
                            {
                                buffer[offset + i * frame->channels + c] = frame->extended_data[c][i];
                            }
                            else if (dataSize == 2)
                            {
                                buffer[offset + i * dataSize * frame->channels + c * dataSize] = frame->extended_data[c][i * dataSize];
                                buffer[offset + i * dataSize * frame->channels + c * dataSize + 1] = frame->extended_data[c][i * dataSize + 1];
                            }
                            else if (dataSize == 4)
                            {
                                buffer[offset + i * dataSize * frame->channels + c * dataSize] = frame->extended_data[c][i * dataSize];
                                buffer[offset + i * dataSize * frame->channels + c * dataSize + 1] = frame->extended_data[c][i * dataSize + 1];
                                buffer[offset + i * dataSize * frame->channels + c * dataSize + 2] = frame->extended_data[c][i * dataSize + 2];
                                buffer[offset + i * dataSize * frame->channels + c * dataSize + 3] = frame->extended_data[c][i * dataSize + 3];   
                            }
                        }
                    }

                    return dataSize * frame->channels * frame->nb_samples;
                }
                for (int i = 0; i < size; i++)
                {
                    buffer[i + offset] = frame->extended_data[0][i];
                }

                return size;
            }

            return 0;
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

        private bool IsPlanar(InteropCalls.AVSampleFormat sampleFormat)
        {
            return sampleFormat == InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_U8P ||
                   sampleFormat == InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_S16P ||
                   sampleFormat == InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_S32P ||
                   sampleFormat == InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_FLTP ||
                   sampleFormat == InteropCalls.AVSampleFormat.AV_SAMPLE_FMT_DBLP;
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="FfmpegDecoder" /> supports seeking.
        /// </summary>
        public unsafe bool CanSeek
        {
            get
            {
                if (_formatContext == null || _formatContext->pb == null)
                    return false;
                return _formatContext->pb->seekable == 1;
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
            set
            {
                SeekPosition(value);
            }
        }

        /// <summary>
        ///     Gets the length of the waveform-audio data in bytes.
        /// </summary>
        public long Length
        {
            get { return GetLength(); }
        }

        /// <summary>
        /// Releases all allocated resources used by the <see cref="FfmpegDecoder"/>.
        /// </summary>
        public unsafe void Dispose()
        {
            if (_stream != null && _stream->codec != null)
            {
                int result = InteropCalls.avcodec_close(_stream->codec);
                FfmpegException.Try(result, "avcodec_close");
            }

            if (_formatContext != null)
            {
                //formatContext got opened through a filename
                fixed (InteropCalls.AVFormatContext** p = &_formatContext)
                {
                    InteropCalls.avformat_close_input(p);
                }

                _formatContext = null;
            }

            if (_ffmpegStream != null)
            {
                _ffmpegStream.Dispose();
                _ffmpegStream = null;
            }
        }

        private unsafe void SeekPosition(long position)
        {
            //https://ffmpeg.org/doxygen/trunk/seek-test_8c-source.html
            double seconds = this.GetMilliseconds(position) / 1000.0;
            var time = seconds * _stream->time_base.den / _stream->time_base.num;
            lock (_lockObject)
            {
                int result = InteropCalls.avformat_seek_file(_formatContext, _streamIndex, long.MinValue, (long) time,
                    (long) time, 0);
                FfmpegException.Try(result, "avformat_seek_file");


                _position = position;
                _overflowCount = 0;
                _overflowOffset = 0;
            }
        }

        private unsafe long GetLength()
        {
            if (_stream == null)
                return 0;
            return AvTimeToBytes(_stream->duration);
        }

        private unsafe long AvTimeToBytes(long time)
        {
            double seconds = time * _stream->time_base.num / (double)_stream->time_base.den;
            long bytes = this.GetRawElements(TimeSpan.FromSeconds(seconds));
            return bytes;
        }
    }
}
