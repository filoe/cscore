using System;

namespace CSCore.SoundIn
{
    public class DataAvailableEventArgs : EventArgs
    {
        public byte[] Data { get; private set; }
        public int ByteCount { get; private set; }
        public WaveFormat Format { get; private set; }

        public DataAvailableEventArgs(byte[] data, int bytecount, WaveFormat format)
        {
            Data = data;
            ByteCount = bytecount;
            Format = format;
        }
    }
}
