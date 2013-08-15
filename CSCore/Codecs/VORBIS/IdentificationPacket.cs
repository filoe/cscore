using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCore.Codecs.VORBIS
{
    public class IdentificationPacket
    {
        static readonly byte[] VorbisIdentifier = { 0x01, 0x76, 0x6f, 0x72, 0x62, 0x69, 0x73 }; //[0x01]VORBIS

        public uint VorbisVersion { get; private set; }
        public byte Channels { get; private set; }
        public uint SampleRate { get; private set; }
        public uint MaxBitrate { get; private set; }
        public uint MinBitrate { get; private set; }
        public byte BlockSize0 { get; private set; }
        public byte BlockSize1 { get; private set; }
        public bool FramingFlag { get; private set; }

        public IdentificationPacket(Packet packet)
        {
            VorbisVersion = packet.ReadUInt32();
            if (VorbisVersion != 0)
                throw new VorbisException("stream is undecodable", new VorbisException("VorbisVersion has to be zero"));

            Channels = packet.ReadByte();
            if (Channels < 1)
                throw new VorbisException("stream is undecodable", new VorbisException("Channels must be greater than zero"));

            SampleRate = packet.ReadUInt32();
            if (SampleRate < 1)
                throw new VorbisException("stream is undecodable", new VorbisException("Samplerate must be greater than zero"));

            MaxBitrate = packet.ReadUInt32();
            MinBitrate = packet.ReadUInt32();

            BlockSize0 = (byte)packet.ReadBits(4);
            BlockSize1 = (byte)packet.ReadBits(4);

            FramingFlag = packet.ReadBit();

            if ((Math.Log(BlockSize0, 2) % 1) != 0 || (Math.Log(BlockSize1, 2) % 1) != 0)
                throw new VorbisException("stream is undecodable", new VorbisException("BlockSize has to be a value of 2^n. B0: {0}; B1: {1}.", BlockSize0, BlockSize1));
            if (BlockSize0 > BlockSize1)
                throw new VorbisException("stream is undecodable", new VorbisException("BlockSize0 is bigger than BlockSize1. B0: {0}; B1: {1}.", BlockSize0, BlockSize1));

            if (FramingFlag == false)
                throw new VorbisException("stream is undecodable", new VorbisException("FramingFlag has to be nonzero."));
        }
    }
}