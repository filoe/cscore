using System;

namespace CSCore.SoundIn
{
    public class DataAvailableEventArgs : EventArgs
    {
        public byte[] Data { get; private set; }

        public int ByteCount { get; private set; }

        public int Offset { get; private set; }

        public WaveFormat Format { get; private set; }

        public DataAvailableEventArgs(byte[] data, int offset, int bytecount, WaveFormat format)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (bytecount <= 0 || bytecount > data.Length)
                throw new ArgumentOutOfRangeException("bytecount");
            if (format == null)
                throw new ArgumentNullException("format");

            Offset = offset;
            Data = data;
            ByteCount = bytecount;
            Format = format;
        }
    }
}