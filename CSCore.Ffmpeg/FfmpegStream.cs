using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CSCore.Ffmpeg
{
    internal sealed class FfmpegStream : IDisposable
    {
        private readonly Stream _stream;

        public AvioContext AvioContext { get; private set; }

        public FfmpegStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");

            _stream = stream;

            AvioContext = new AvioContext(ReadDataCallback, 
                stream.CanSeek ? new FfmpegCalls.AvioSeek(SeekCallback) : null, 
                stream.CanWrite ? new FfmpegCalls.AvioWriteData(WriteDataCallback) : null);
        }

        private long SeekCallback(IntPtr opaque, long offset, FfmpegCalls.SeekFlags whence)
        {
            if ((whence & FfmpegCalls.SeekFlags.SeekSize) == FfmpegCalls.SeekFlags.SeekSize)
                return _stream.Length;

            SeekOrigin origin;
            if ((whence & FfmpegCalls.SeekFlags.SeekSet) == FfmpegCalls.SeekFlags.SeekSet)
                origin = SeekOrigin.Begin;
            else if ((whence & FfmpegCalls.SeekFlags.SeekCur) == FfmpegCalls.SeekFlags.SeekCur)
                origin = SeekOrigin.Current;
            else if ((whence & FfmpegCalls.SeekFlags.SeekEnd) == FfmpegCalls.SeekFlags.SeekEnd)
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

        private int WriteDataCallback(IntPtr opaque, IntPtr buffer, int bufferSize)
        {
            byte[] managedBuffer = new byte[bufferSize];

            Marshal.Copy(buffer, managedBuffer, 0, bufferSize);
            _stream.Write(managedBuffer, 0, bufferSize);

            return bufferSize;
        }

        private int ReadDataCallback(IntPtr opaque, IntPtr buffer, int bufferSize)
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (AvioContext != null)
            {
                AvioContext.Dispose();
                AvioContext = null;
            }
        }

        ~FfmpegStream()
        {
            Dispose();
        }
    }
}