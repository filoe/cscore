using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Represents a flac metadata block.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Type:{MetaDataType}   LastBlock:{IsLastMetaBlock}   Length:{Length} bytes")]
    public class FlacMetadata
    {
        /// <summary>
        /// Reads and returns a single <see cref="FlacMetadata"/> from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the <see cref="FlacMetadata"/>.</param>
        /// <returns>Returns the read <see cref="FlacMetadata"/>.</returns>
        public unsafe static FlacMetadata FromStream(Stream stream)
        {
            bool lastBlock;
            FlacMetaDataType type;
            int length;

            byte[] b = new byte[4];
            if (stream.Read(b, 0, 4) <= 0)
                throw new FlacException(new EndOfStreamException("Could not read metadata"), FlacLayer.Metadata);

            fixed (byte* headerBytes = b)
            {
                FlacBitReader bitReader = new FlacBitReader(headerBytes, 0);

                lastBlock = bitReader.ReadBits(1) == 1;
                type = (FlacMetaDataType)bitReader.ReadBits(7);
                length = (int)bitReader.ReadBits(24);
            }

            FlacMetadata data;
            long streamStartPosition = stream.Position;
            if ((int)type < 0 || (int)type > 6)
                return null;

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

            stream.Seek(length - (stream.Position - streamStartPosition), SeekOrigin.Current);
            return data;
        }


        /// <summary>
        /// Reads all <see cref="FlacMetadata"/> from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the <see cref="FlacMetadata"/>.</param>
        /// <returns>All <see cref="FlacMetadata"/>.</returns>
        public static IEnumerable<FlacMetadata> ReadAllMetadataFromStream(Stream stream)
        {
            while (true)
            {
                FlacMetadata data = FromStream(stream);
                yield return data;

                if (data == null || data.IsLastMetaBlock)
                    break;
            }
        }

        /// <summary>
        /// Skips all <see cref="FlacMetadata"/> of the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream which contains the <see cref="FlacMetadata"/>.</param>
        public static void SkipMetadata(Stream stream)
        {
            ReadAllMetadataFromStream(stream).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacMetadata"/> class.
        /// </summary>
        /// <param name="type">The type of the metadata.</param>
        /// <param name="lastBlock">A value which indicates whether this is the last <see cref="FlacMetadata"/> block inside of the stream. <c>true</c> means that this is the last <see cref="FlacMetadata"/> block inside of the stream.</param>
        /// <param name="length">The length of <see cref="FlacMetadata"/> block inside of the stream in bytes. Does not include the metadata header.</param>
        protected FlacMetadata(FlacMetaDataType type, bool lastBlock, int length)
        {
            MetaDataType = type;
            IsLastMetaBlock = lastBlock;
            Length = length;
        }

        /// <summary>
        /// Gets the type of the <see cref="FlacMetadata"/>.
        /// </summary>
        public FlacMetaDataType MetaDataType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is the last <see cref="FlacMetadata"/> block.
        /// </summary>
        public Boolean IsLastMetaBlock { get; private set; }

        /// <summary>
        /// Gets the length of the <see cref="FlacMetadata"/> block inside of the stream in bytes.
        /// </summary>
        /// <remarks>The length does not include the metadata header.</remarks>
        public int Length { get; private set; }
    }
}