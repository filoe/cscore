using System;
using System.Collections.Generic;
using System.IO;

namespace CSCore.Codecs.FLAC
{
    [System.Diagnostics.DebuggerDisplay("Type:{MetaDataType}   LastBlock:{IsLastMetaBlock}   Length:{Length} bytes")]
    public class FlacMetadata
    {
        public unsafe static FlacMetadata FromStream(Stream stream)
        {
            bool lastBlock = false;
            FlacMetaDataType type = FlacMetaDataType.Undef;
            int length = 0;

            byte[] b = new byte[4];
            if (stream.Read(b, 0, 4) <= 0)
                throw new FlacException(new EndOfStreamException("Could not read metadata"), FlacLayer.Metadata);

            fixed (byte* headerBytes = b)
            {
                FlacBitReader bitReader = new FlacBitReader(headerBytes, 0);

                lastBlock = bitReader.ReadBits(1) == 1;
                type = (FlacMetaDataType)bitReader.ReadBits(7);
                length = (int)bitReader.ReadBits(24);
                ////1000 0000
                //if (((b[0] & 0x80) >> 7) == 1)
                //    lastBlock = true;
                //type = (FlacMetaDataType)(b[0] & 0x7F);
                //int length = (b[1] + (b[2] << 8) + (b[3] << 16));
            }

            FlacMetadata data;
            long streamStartPosition = stream.Position;
            if ((int)type < 0 || (int)type > 6)
                return null;
            else
            {
                switch (type)
                {
                    case FlacMetaDataType.StreamInfo:
                        data = new FlacMetadataStreamInfo(stream, length, lastBlock);
                        break;

                    case FlacMetaDataType.Seektable:
                        data = new FlacMetadataSeekTable(stream, length, lastBlock);
                        break;

                    default:
                        data = new FlacMetadata(type, lastBlock, length);
                        break;
                }
            }

            stream.Seek(length - (stream.Position - streamStartPosition), SeekOrigin.Current);
            return data;
        }

        public static List<FlacMetadata> AllDataFromStream(Stream stream)
        {
            List<FlacMetadata> metaDataCollection = new List<FlacMetadata>();
            FlacMetadata data;
            while (true)
            {
                data = FromStream(stream);
                if (data != null)
                    metaDataCollection.Add(data);

                if (data == null || data.IsLastMetaBlock)
                    return metaDataCollection;
            }
        }

        protected FlacMetadata(FlacMetaDataType type, bool lastBlock, Int32 length)
        {
            MetaDataType = type;
            IsLastMetaBlock = lastBlock;
            Length = length;
        }

        public FlacMetaDataType MetaDataType { get; private set; }

        public Boolean IsLastMetaBlock { get; private set; }

        public Int32 Length { get; private set; }
    }

    public enum FlacMetaDataType : int
    {
        StreamInfo = 0,
        Padding = 1,
        Application = 2,
        Seektable = 3,
        VorbisComment = 4,
        CUESheet = 5,
        Picture = 6,
        Undef = 7
    }
}