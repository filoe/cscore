using System;
using System.IO;
using System.Text;

// ReSharper disable once CheckNamespace
namespace CSCore.Codecs.FLAC
{
    /// <summary>
    ///     Represents the streaminfo metadata flac which provides general information about the flac stream.
    /// </summary>
    public class FlacMetadataStreamInfo : FlacMetadata
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FlacMetadataStreamInfo" /> class.
        /// </summary>
        /// <param name="stream">The stream which contains the <see cref="FlacMetadataStreamInfo"/>.</param>
        /// <param name="length">The length of the <see cref="FlacMetadataStreamInfo"/> inside of the stream in bytes. Does not include the metadata header.</param>
        /// <param name="lastBlock">
        ///     A value which indicates whether this is the last <see cref="FlacMetadata" /> block inside of
        ///     the stream. <c>true</c> means that this is the last <see cref="FlacMetadata" /> block inside of the stream.
        /// </param>
        public unsafe FlacMetadataStreamInfo(Stream stream, int length, bool lastBlock)
            : base(FlacMetaDataType.StreamInfo, lastBlock, length)
        {
            //http://flac.sourceforge.net/format.html#metadata_block_streaminfo
            var reader = new BinaryReader(stream, Encoding.ASCII);
            try
            {
                MinBlockSize = reader.ReadInt16();
                MaxBlockSize = reader.ReadInt16();
            }
            catch (IOException e)
            {
                throw new FlacException(e, FlacLayer.Metadata);
            }
            const int bytesToRead = (240 / 8) - 16;
            byte[] buffer = reader.ReadBytes(bytesToRead);
            if (buffer.Length != bytesToRead)
            {
                throw new FlacException(new EndOfStreamException("Could not read StreamInfo-content"),
                    FlacLayer.Metadata);
            }

            fixed (byte* b = buffer)
            {
                var bitreader = new FlacBitReader(b, 0);
                MinFrameSize = (int) bitreader.ReadBits(24);
                MaxFrameSize = (int) bitreader.ReadBits(24);
                SampleRate = (int) bitreader.ReadBits(20);
                Channels = 1 + (int) bitreader.ReadBits(3);
                BitsPerSample = 1 + (int) bitreader.ReadBits(5);
                TotalSamples = (long) bitreader.ReadBits64(36);
                Md5 = new String(reader.ReadChars(16));
            }
        }

        /// <summary>
        /// Gets the minimum size of the block in samples.
        /// </summary>
        /// <value>
        /// The minimum size of the block in samples.
        /// </value>
        public short MinBlockSize { get; private set; }

        /// <summary>
        /// Gets the maximum size of the block in samples.
        /// </summary>
        /// <value>
        /// The maximum size of the block in samples.
        /// </value>
        public short MaxBlockSize { get; private set; }

        /// <summary>
        /// Gets the maximum size of the frame in bytes.
        /// </summary>
        /// <value>
        /// The maximum size of the frame in bytes.
        /// </value>
        public int MaxFrameSize { get; private set; }

        /// <summary>
        /// Gets the minimum size of the frame in bytes.
        /// </summary>
        /// <value>
        /// The minimum size of the frame in bytes.
        /// </value>
        public int MinFrameSize { get; private set; }

        /// <summary>
        /// Gets the sample rate in Hz.
        /// </summary>
        /// <value>
        /// The sample rate.
        /// </value>
        public int SampleRate { get; private set; }

        /// <summary>
        /// Gets the number of channels.
        /// </summary>
        /// <value>
        /// The number of channels.
        /// </value>
        public int Channels { get; private set; }

        /// <summary>
        /// Gets the number of bits per sample.
        /// </summary>
        /// <value>
        /// The number of bits per sample.
        /// </value>
        public int BitsPerSample { get; private set; }

        /// <summary>
        /// Gets the total number of samples inside of the stream.
        /// </summary>
        public long TotalSamples { get; private set; }

        /// <summary>
        /// Gets MD5 signature of the unencoded audio data.
        /// </summary>
        /// <value>
        /// The MD5 signature of the unencoded audio data.
        /// </value>
        public string Md5 { get; private set; }
    }
}