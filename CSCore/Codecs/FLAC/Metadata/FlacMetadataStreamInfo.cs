using System;
using System.IO;
using System.Text;

namespace CSCore.Codecs.FLAC
{
    public class FlacMetadataStreamInfo : FlacMetadata
    {
        public unsafe FlacMetadataStreamInfo(Stream stream, Int32 length, bool lastBlock)
            : base(FlacMetaDataType.StreamInfo, lastBlock, length)
        {
            //http://flac.sourceforge.net/format.html#metadata_block_streaminfo
            BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
            try
            {
                MinBlockSize = reader.ReadInt16();
                MaxBlockSize = reader.ReadInt16();
            }
            catch (IOException e)
            {
                throw new FlacException(e, FlacLayer.Metadata);
            }
            int bytesToRead = (240 / 8) - 16;
            byte[] buffer = reader.ReadBytes(bytesToRead);
            if (buffer.Length != bytesToRead)
                throw new FlacException(new EndOfStreamException("Could not read StreamInfo-content"), FlacLayer.Metadata);

            fixed (byte* b = buffer)
            {
                FlacBitReader bitreader = new FlacBitReader(b, 0);
                MinFrameSize = bitreader.ReadBits(24);
                MaxFrameSize = bitreader.ReadBits(24);
                SampleRate = (int)bitreader.ReadBits(20);
                Channels = 1 + (int)bitreader.ReadBits(3);
                BitsPerSample = 1 + (int)bitreader.ReadBits(5);
                TotalSamples = (long)bitreader.ReadBits64(36);
                MD5 = new String(reader.ReadChars(16));
            }
        }

        public short MinBlockSize { get; private set; }

        public short MaxBlockSize { get; private set; }

        public uint MaxFrameSize { get; private set; }

        public uint MinFrameSize { get; private set; }

        public int SampleRate { get; private set; }

        public int Channels { get; private set; }

        public int BitsPerSample { get; private set; }

        /// <summary>
        /// 0 = Unknown
        /// </summary>
        public long TotalSamples { get; private set; }

        public string MD5 { get; private set; }
    }
}