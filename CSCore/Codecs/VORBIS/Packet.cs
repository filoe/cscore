using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCore.Codecs.OGG;

namespace CSCore.Codecs.VORBIS
{
    public abstract class Packet
    {
        public int Length { get; protected set; }
        public long PageGranulePosition { get; set; }
        public long PageSequenceNumber { get; set; }
        public bool IsEndOfStream { get; set; }
        public bool IsContinued { get; set; }
        public bool IsContinuation { get; set; }
        public bool IsResync { get; set; }

        protected abstract int ReadNextByte();
        protected abstract int ReadBytes(byte[] buffer, int offset, int count);
        public abstract void AppendPacket(Packet packet);

        public uint ReadBits(int bits)
        {
            throw new NotImplementedException();
        }

        public bool ReadBit()
        {
            return ReadBits(1) == 1;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadBytes(int count)
        {
            throw new NotImplementedException();
        }

        public byte ReadByte()
        {
            throw new NotImplementedException();
        }

        public uint ReadUInt32()
        {
            throw new NotImplementedException();
        }

        public int ReadInt32()
        {
            throw new NotImplementedException();
        }

        public ushort ReadUInt16()
        {
            throw new NotImplementedException();
        }

        public short ReadInt16()
        {
            throw new NotImplementedException();
        }
    }
}