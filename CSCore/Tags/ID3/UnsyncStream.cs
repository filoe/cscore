using System;
using System.IO;

namespace CSCore.Tags.ID3
{
    public class UnsyncStream : Stream
    {
        private readonly Stream _stream;

        public UnsyncStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (!stream.CanRead) throw new ArgumentException("stream not readable");

            _stream = stream;
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
            _stream.Flush();
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        private int _svalue = 0;

        public override int ReadByte()
        {
            int value = _stream.ReadByte();
            if (_svalue == 0xFF && value == 0x00)
            {
                value = _stream.ReadByte();
                if (value != 0x00 && value < 0xE0 && value != -1)
                    throw new ID3Exception("Invalid Unsync-Byte found");
            }
            _svalue = value;
            return value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int value = 0;
            int i = offset;
            while (i < offset + count && value != -1)
            {
                value = _stream.ReadByte();
                if (_svalue == 0xFF && value == 0x00)
                {
                    value = _stream.ReadByte();
                    if (value != 0x00 && value < 0xE0 && value != -1)
                        throw new ID3Exception("Invalid Unsync-Byte found");
                }
                if (value != -1)
                    buffer[i++] = (byte)(value & 0xFF);

                _svalue = value;
            }

            return i - offset;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}