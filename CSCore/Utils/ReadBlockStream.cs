using System;
using System.Diagnostics;
using System.IO;

namespace CSCore.Utils
{
    internal class ReadBlockStream : Stream
    {
        private long _position;

        private readonly Stream _stream;

        public ReadBlockStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (stream.CanRead == false)
                throw new ArgumentException("Can't read stream");

            _stream = stream;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;
            while (read < count)
            {
                read += _stream.Read(buffer, offset + read, count - read);
            }

            _position += read;
            return count;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new InvalidOperationException();
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override long Position
        {
            get
            {
                return _position;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }
    }
}