using System;
using CSCore.Ffmpeg.Interops;

namespace CSCore.Ffmpeg
{
    internal sealed class AvStream : IDisposable
    {
        private readonly unsafe AVStream* _stream;

        public unsafe AVStream Stream
        {
            get
            {
                if (_stream == null)
                    return default(AVStream);
                return *_stream;
            }
        }

        public unsafe WaveFormat GetSuggestedWaveFormat()
        {
            if(_stream == null)
                throw new InvalidOperationException("No stream selected.");

            int bitsPerSample;
            AudioEncoding encoding;
            switch (_stream->codec->sample_fmt)
            {
                case AVSampleFormat.AV_SAMPLE_FMT_U8:
                case AVSampleFormat.AV_SAMPLE_FMT_U8P:
                    bitsPerSample = 8;
                    encoding = AudioEncoding.Pcm;
                    break;
                case AVSampleFormat.AV_SAMPLE_FMT_S16:
                case AVSampleFormat.AV_SAMPLE_FMT_S16P:
                    bitsPerSample = 16;
                    encoding = AudioEncoding.Pcm;
                    break;
                case AVSampleFormat.AV_SAMPLE_FMT_S32:
                case AVSampleFormat.AV_SAMPLE_FMT_S32P:
                    bitsPerSample = 32;
                    encoding = AudioEncoding.Pcm;
                    break;
                case AVSampleFormat.AV_SAMPLE_FMT_FLT:
                case AVSampleFormat.AV_SAMPLE_FMT_FLTP:
                    bitsPerSample = 32;
                    encoding = AudioEncoding.IeeeFloat;
                    break;
                case AVSampleFormat.AV_SAMPLE_FMT_DBL:
                case AVSampleFormat.AV_SAMPLE_FMT_DBLP:
                    //dbl is converted by the AvFrame.DecodePacket method
                    bitsPerSample = 32;
                    encoding = AudioEncoding.IeeeFloat;
                    break;
                default:
                    throw new NotSupportedException("Audio Sample Format not supported.");
            }

            var waveFormat = new WaveFormat(_stream->codec->sample_rate, bitsPerSample, _stream->codec->channels,
                encoding);
            return waveFormat;
        }

        public unsafe AvStream(IntPtr stream)
        {
            if(stream == IntPtr.Zero)
                throw new ArgumentNullException("stream");

            _stream = (AVStream*)stream;

            var avCodecContext = _stream->codec;
            var decoder = FfmpegCalls.AvCodecFindDecoder(avCodecContext->codec_id);
            //will the codeccontext be freed by avformat_close_input automatically?
            FfmpegCalls.AvCodecOpen(avCodecContext, decoder);
        }

        public unsafe void Dispose()
        {
            GC.SuppressFinalize(this);
            if (_stream != null)
            {
                FfmpegCalls.AvCodecClose(_stream->codec);
            }
        }

        ~AvStream()
        {
            Dispose();
        }
    }
}