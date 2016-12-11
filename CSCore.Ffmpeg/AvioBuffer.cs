using System;

namespace CSCore.Ffmpeg
{
    internal sealed class AvioBuffer : IDisposable
    {
        public int BufferSize { get; private set; }

        public IntPtr Buffer { get; private set; }

        public bool SuppressAvFree { get; set; }

        public AvioBuffer()
            : this(0x1000)
        {
        }

        public AvioBuffer(int bufferSize)
        {
            if(bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize");

            BufferSize = bufferSize;
            Buffer = FfmpegCalls.AvMalloc(bufferSize);
            SuppressAvFree = false;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (SuppressAvFree != true)
            {
                FfmpegCalls.AvFree(Buffer);
            }
        }

        ~AvioBuffer()
        {
            Dispose();
        }
    }
}