using System;
using CSCore.Ffmpeg.Interops;

namespace CSCore.Ffmpeg
{
    internal class AvFormatContext : IDisposable
    {
        private unsafe AVFormatContext* _formatContext;
        private AvStream _stream;

        public unsafe IntPtr FormatPtr
        {
            get { return (IntPtr) _formatContext; }
        }

        public int BestAudioStreamIndex { get; private set; }

        public AvStream SelectedStream
        {
            get { return _stream; }
        }

        public unsafe bool CanSeek
        {
            get
            {
                if (_formatContext == null || _formatContext->pb == null)
                    return false;
                return _formatContext->pb->seekable == 1;
            }
        }

        public double LengthInSeconds
        {
            get
            {
                if (SelectedStream == null)
                    return 0;
                var timebase = SelectedStream.Stream.time_base;
                return SelectedStream.Stream.duration * timebase.num / (double) timebase.den;
            }
        }

        public unsafe AVFormatContext FormatContext
        {
            get
            {
                if (_formatContext == null)
                    return default(AVFormatContext);
                return *_formatContext;
            }
        }

        public unsafe AvFormatContext(FfmpegStream stream)
        {
            _formatContext = FfmpegCalls.AvformatAllocContext();
            fixed (AVFormatContext** pformatContext = &_formatContext)
            {
                FfmpegCalls.AvformatOpenInput(pformatContext, stream.AvioContext);
            }
            Initialize();
        }

        public unsafe AvFormatContext(string url)
        {
            _formatContext = FfmpegCalls.AvformatAllocContext();
            fixed (AVFormatContext** pformatContext = &_formatContext)
            {
                FfmpegCalls.AvformatOpenInput(pformatContext, url);
            }
            Initialize();
        }

        private unsafe void Initialize()
        {
            FfmpegCalls.AvFormatFindStreamInfo(_formatContext);
            BestAudioStreamIndex = FfmpegCalls.AvFindBestStreamInfo(_formatContext);
             _stream = new AvStream((IntPtr)_formatContext->streams[BestAudioStreamIndex]);
        }

        public void SeekFile(double seconds)
        {
            var streamTimeBase = SelectedStream.Stream.time_base;
            var time = seconds * streamTimeBase.den / streamTimeBase.num;

            FfmpegCalls.AvFormatSeekFile(this, time);
        }

        public unsafe void Dispose()
        {
            GC.SuppressFinalize(this);

            if (SelectedStream != null)
            {
                SelectedStream.Dispose();
                _stream = null;
            }

            if (_formatContext != null)
            {
                fixed (AVFormatContext** pformatContext = &_formatContext)
                {
                    FfmpegCalls.AvformatCloseInput(pformatContext);
                }

                _formatContext = null;
                BestAudioStreamIndex = 0;
            }
        }

        ~AvFormatContext()
        {
            Dispose();
        }
    }
}