using System;
using CSCore.Ffmpeg.Interops;

namespace CSCore.Ffmpeg
{
    internal sealed class AvioContext : IDisposable
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly FfmpegCalls.AvioReadData _readDataCallback;
        private readonly FfmpegCalls.AvioSeek _seekCallback;
        private readonly FfmpegCalls.AvioWriteData _writeDataCallback;
        private unsafe AVIOContext* _context;
        private AvioBuffer _buffer;
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable

        public unsafe IntPtr ContextPtr
        {
            get { return (IntPtr) _context; }
        }

        public unsafe AVIOContext Context
        {
            get
            {
                if (_context == null)
                    return default(AVIOContext);
                return *_context;
            }
        }


        public AvioContext(
            FfmpegCalls.AvioReadData readDataCallback, 
            FfmpegCalls.AvioSeek seekCallback) 
            : this(readDataCallback, seekCallback, null)
        {
        }

        public unsafe AvioContext(
            FfmpegCalls.AvioReadData readDataCallback, 
            FfmpegCalls.AvioSeek seekCallback, 
            FfmpegCalls.AvioWriteData writeDataCallback)
        {
            _readDataCallback = readDataCallback;
            _seekCallback = seekCallback;
            _writeDataCallback = writeDataCallback;

            //make sure that the buffer won't be disposed
            //the buffer may change. we always have to free _context->buffer
            _buffer = new AvioBuffer {SuppressAvFree = true}; 
            _context = FfmpegCalls.AvioAllocContext(_buffer, _writeDataCallback != null, IntPtr.Zero,
                _readDataCallback, _writeDataCallback, _seekCallback);
        }

        public unsafe void Dispose()
        {
            GC.SuppressFinalize(this);
            if (_context != null)
            {
                //free the allocated buffer
                //note: the internal buffer could have changed, and be != _buffer
                FfmpegCalls.AvFree((IntPtr)_context->buffer);

                //free the context itself
                FfmpegCalls.AvFree((IntPtr)_context);
                _context = null;
            }
        }

        ~AvioContext()
        {
            Dispose();
        }
    }
}