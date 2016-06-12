using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace CSCore.Ffmpeg
{
    internal class FfmpegStream : IDisposable
    {
        private readonly Stream _stream;

        private const int AvioBufferSize = 0x1000;
        private unsafe InteropCalls.AVIOContext* _avioContext = null;
        private readonly unsafe void* _avioBuffer;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly InteropCalls.AvioReadData _readData;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly InteropCalls.AvioSeek _seekData;

        private readonly List<GCHandle> _gcHandles = new List<GCHandle>();

        public unsafe IntPtr AvioContext
        {
            get { return (IntPtr) _avioContext; }
        }

        public unsafe FfmpegStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");
            _stream = stream;

            _readData = ReadData;
            _gcHandles.Add(GCHandle.Alloc(_readData));

            if (stream.CanSeek)
            {
                _seekData = Seek;
                _gcHandles.Add(GCHandle.Alloc(_seekData));
            }
            else
            {
                _seekData = null;
            }

            _avioBuffer = InteropCalls.av_malloc(AvioBufferSize);
            if (_avioBuffer == null)
            {
                throw new OutOfMemoryException("Could not allocate avio-buffer.");
            }

            _avioContext = InteropCalls.avio_alloc_context(
                (byte*)_avioBuffer,
                AvioBufferSize,
                0,
                null,
                _readData, null, _seekData);
            if (_avioContext == null)
            {
                throw new FfmpegException("Could not allocate avio-context.", "avio_alloc_context");
            }
        }

        private long Seek(IntPtr opaque, long offset, InteropCalls.SeekFlags whence)
        {
            if ((whence & InteropCalls.SeekFlags.SeekSize) == InteropCalls.SeekFlags.SeekSize)
                return _stream.Length;

            SeekOrigin origin;
            if((whence & InteropCalls.SeekFlags.SeekSet) == InteropCalls.SeekFlags.SeekSet)
                origin = SeekOrigin.Begin;
            else if((whence & InteropCalls.SeekFlags.SeekCur) == InteropCalls.SeekFlags.SeekCur)
                origin = SeekOrigin.Current;
            else if ((whence & InteropCalls.SeekFlags.SeekEnd) == InteropCalls.SeekFlags.SeekEnd)
                origin = SeekOrigin.End;
            else
                return -1;
            try
            {
                return _stream.Seek(offset, origin);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private int ReadData(IntPtr opaque, IntPtr buffer, int bufferSize)
        {
            byte[] managedBuffer = new byte[bufferSize];
            int read = 0;
            while (read < bufferSize)
            {
                int read0 = _stream.Read(managedBuffer, read, bufferSize - read);
                read += read0;
                if (read0 == 0)
                    break;
            }

            Marshal.Copy(managedBuffer, 0, buffer, Math.Min(read, bufferSize));

            return read;
        }

        public unsafe void Dispose()
        {
            if (_avioContext != null)
            {
                //free the allocated buffer
                InteropCalls.av_free(_avioContext->buffer);       

                //free the context itself
                InteropCalls.av_free(_avioContext);

                _avioContext = null;
            }

            foreach (var gcHandle in _gcHandles)
            {
                if(gcHandle.IsAllocated)
                    gcHandle.Free();
            }
        }
    }
}
