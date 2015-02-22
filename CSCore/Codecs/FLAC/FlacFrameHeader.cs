using System;
using System.Diagnostics;
using System.IO;

namespace CSCore.Codecs.FLAC
{
    /// <summary>
    /// Represents the header of a <see cref="FlacFrame"/>.
    /// </summary>
    public sealed class FlacFrameHeader
    {
        private int _blocksizeHint; //if bsindex == 6 || 7
        private int _sampleRateHint; //if sampleRateIndex == 12 || 13 || 14


        /// <summary>
        /// Gets number of samples, the frame contains.
        /// </summary>
        /// <value>
        /// The number of samples, the frame contains.
        /// </value>
        public int BlockSize { get; private set; }

        /// <summary>
        /// Gets the sample rate in Hz.
        /// </summary>
        /// <value>
        /// The sample rate in Hz.
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
        /// Gets the channel assignment.
        /// </summary>
        /// <value>
        /// The channel assignment.
        /// </value>
        public ChannelAssignment ChannelAssignment { get; private set; }

        /// <summary>
        /// Gets the bits per sample.
        /// </summary>
        /// <value>
        /// The bits per sample.
        /// </value>
        public int BitsPerSample { get; private set; }

        /// <summary>
        /// Gets a value which indicates whether the frame provides the <see cref="SampleNumber"/> or the <see cref="FrameNumber"/>.
        /// </summary>
        /// <value>
        /// A value which indicates whether the frame provides the <see cref="SampleNumber"/> or the <see cref="FrameNumber"/>.
        /// </value>
        public BlockingStrategy BlockingStrategy { get; private set; }

        /// <summary>
        /// Gets the frame's starting sample number.
        /// </summary>
        /// <value>
        /// The frame's starting sample number.
        /// </value>
        /// <remarks>Only available if the <see cref="BlockingStrategy"/> is set to <see cref="FLAC.BlockingStrategy.VariableBlockSize"/>.</remarks>
        public long SampleNumber { get; private set; }

        /// <summary>
        /// Gets the frame's number.
        /// </summary>
        /// <value>
        /// The frame's number.
        /// </value>
        /// <remarks>Only available if the <see cref="BlockingStrategy"/> is set to <see cref="FLAC.BlockingStrategy.FixedBlockSize"/>.</remarks>
        public int FrameNumber { get; private set; }

        /// <summary>
        /// Gets the 8-bit crc checksum of the frame header.
        /// </summary>
        /// <value>
        /// The 8-bit crc checksum of the frame header.
        /// </value>
        public byte Crc8 { get; private set; }

        private bool DoCrc { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has error; otherwise, <c>false</c>.
        /// </value>
        public bool HasError { get; private set; }

        /// <summary>
        /// Gets the stream position.
        /// </summary>
        /// <value>
        /// The stream position.
        /// </value>
        public long StreamPosition { get; private set; }

        internal bool PrintErrors = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacFrameHeader"/> class.
        /// </summary>
        /// <param name="stream">The underlying stream which contains the <see cref="FlacFrameHeader"/>.</param>
        public FlacFrameHeader(Stream stream)
            : this(stream, null, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacFrameHeader"/> class.
        /// </summary>
        /// <param name="stream">The underlying stream which contains the <see cref="FlacFrameHeader"/>.</param>
        /// <param name="streamInfo">The stream-info-metadata-block of the flac stream which provides some basic information about the flac framestream. Can be set to null.</param>
        public FlacFrameHeader(Stream stream, FlacMetadataStreamInfo streamInfo)
            : this(stream, streamInfo, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacFrameHeader"/> class.
        /// </summary>
        /// <param name="stream">The underlying stream which contains the <see cref="FlacFrameHeader"/>.</param>
        /// <param name="streamInfo">The stream-info-metadata-block of the flac stream which provides some basic information about the flac framestream. Can be set to null.</param>
        /// <param name="doCrc">A value which indicates whether the crc8 checksum of the <see cref="FlacFrameHeader"/> should be calculated.</param>
        public FlacFrameHeader(Stream stream, FlacMetadataStreamInfo streamInfo, bool doCrc)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (stream.CanRead == false) throw new ArgumentException("stream is not readable");
            //streamInfo can be null

            DoCrc = doCrc;
            StreamPosition = stream.Position;

            HasError = !ParseHeader(stream, streamInfo);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlacFrameHeader"/> class.
        /// </summary>
        /// <param name="buffer">The raw byte-data which contains the <see cref="FlacFrameHeader"/>.</param>
        /// <param name="streamInfo">The stream-info-metadata-block of the flac stream which provides some basic information about the flac framestream. Can be set to null.</param>
        /// <param name="doCrc">A value which indicates whether the crc8 checksum of the <see cref="FlacFrameHeader"/> should be calculated.</param>
        [CLSCompliant(false)]
        public unsafe FlacFrameHeader(ref byte* buffer, FlacMetadataStreamInfo streamInfo, bool doCrc)
            : this(ref buffer, streamInfo, doCrc, true)
        {
        }

        internal unsafe FlacFrameHeader(ref byte* buffer, FlacMetadataStreamInfo streamInfo, bool doCrc, bool logError)
        {
            PrintErrors = logError; //optimized for prescan

            DoCrc = doCrc;
            StreamPosition = -1;

            HasError = !ParseHeader(ref buffer, streamInfo);
        }

        private unsafe bool ParseHeader(Stream stream, FlacMetadataStreamInfo streamInfo)
        {
            const string loggerLocation = "FlacFrameHeader.ParseHeader(Stream, FlacMetadataStreamInfo)";

            byte[] headerBuffer = new byte[FlacConstant.FrameHeaderSize];
            if (stream.Read(headerBuffer, 0, headerBuffer.Length) == headerBuffer.Length)
            {
                fixed (byte* ptrBuffer = headerBuffer)
                {
                    byte* ptrSave = ptrBuffer;
                    byte* __ptrBuffer = ptrBuffer;
                    bool result = ParseHeader(ref __ptrBuffer, streamInfo);
                    stream.Position -= (headerBuffer.Length - (__ptrBuffer - ptrSave)); //todo

                    return result;
                }
            }
            else
            {
                Error("Not able to read Flac header - EOF?", loggerLocation);
                return false;
            }
        }

        private unsafe bool ParseHeader(ref byte* headerBuffer, FlacMetadataStreamInfo streamInfo)
        {
            const string loggerLocation = "FlacFrameHeader.ParseHeader(byte*, FlacMetadataStreamInfo)";
            int val;
            if (headerBuffer[0] == 0xFF && headerBuffer[1] >> 1 == 0x7C) //sync bits
            {
                if ((headerBuffer[1] & 0x02) != 0)
                {
                    Error("Invalid FlacFrame. Reservedbit_0 is 1", loggerLocation);
                    return false;
                }

                byte* __headerbufferPtr = headerBuffer;
                FlacBitReader reader = new FlacBitReader(__headerbufferPtr, 0);

                #region blocksize

                //blocksize
                val = headerBuffer[2] >> 4;
                int blocksize = -1;

                if (val == 0)
                {
                    Error("Invalid Blocksize value: 0", loggerLocation);
                    return false;
                }
                if (val == 1)
                    blocksize = 192;
                else if (val >= 2 && val <= 5)
                    blocksize = 576 << (val - 2);
                else if (val == 6 || val == 7)
                    _blocksizeHint = val;
                else if (val >= 8 && val <= 15)
                    blocksize = 256 << (val - 8);
                else
                {
                    Error("Invalid Blocksize value: " + val, loggerLocation);
                    return false;
                }
                BlockSize = blocksize;

                #endregion blocksize

                #region samplerate

                //samplerate
                val = headerBuffer[2] & 0x0F;
                int sampleRate = -1;

                if (val == 0)
                {
                    if (streamInfo != null)
                        sampleRate = streamInfo.SampleRate;
                    else
                    {
                        Error("Missing Samplerate. Samplerate Index = 0 && streamInfoMetaData == null.", loggerLocation);
                        return false;
                    }
                }
                else if (val >= 1 && val <= 11)
                    sampleRate = FlacConstant.SampleRateTable[val];
                else if (val >= 12 && val <= 14)
                    _sampleRateHint = val;
                else
                {
                    Error("Invalid SampleRate value: " + val, loggerLocation);
                    return false;
                }
                SampleRate = sampleRate;

                #endregion samplerate

                #region channels

                val = headerBuffer[3] >> 4; //cc: unsigned
                int channels;
                if ((val & 8) != 0)
                {
                    channels = 2;
                    if ((val & 7) > 2 || (val & 7) < 0)
                    {
                        Error("Invalid ChannelAssignment", loggerLocation);
                        return false;
                    }
                    ChannelAssignment = (ChannelAssignment)((val & 7) + 1);
                }
                else
                {
                    channels = val + 1;
                    ChannelAssignment = ChannelAssignment.Independent;
                }
                Channels = channels;

                #endregion channels

                #region bitspersample

                val = (headerBuffer[3] & 0x0E) >> 1;
                int bitsPerSample;
                if (val == 0)
                {
                    if (streamInfo != null)
                        bitsPerSample = streamInfo.BitsPerSample;
                    else
                    {
                        Error("Missing BitsPerSample. Index = 0 && streamInfoMetaData == null.", loggerLocation);
                        return false;
                    }
                }
                else if (val == 3 || val >= 7 || val < 0)
                {
                    Error("Invalid BitsPerSampleIndex", loggerLocation);
                    return false;
                }
                else
                    bitsPerSample = FlacConstant.BitPerSampleTable[val];

                BitsPerSample = bitsPerSample;

                #endregion bitspersample

                if ((headerBuffer[3] & 0x01) != 0) // reserved bit -> 0
                {
                    Error("Invalid FlacFrame. Reservedbit_1 is 1", loggerLocation);
                    return false;
                }

                reader.ReadBits(32); //skip the first 4 bytes since they got already processed

                //BYTE 4

                #region utf8

                //variable blocksize
                if ((headerBuffer[1] & 0x01) != 0 ||
                    (streamInfo != null && streamInfo.MinBlockSize != streamInfo.MaxBlockSize))
                {
                    ulong samplenumber;
                    if (reader.ReadUTF8_64(out samplenumber) && samplenumber != ulong.MaxValue)
                    {
                        BlockingStrategy = BlockingStrategy.VariableBlockSize;
                        SampleNumber = (long) samplenumber;
                    }
                    else
                    {
                        Error("Invalid UTF8 Samplenumber coding.", loggerLocation);
                        return false;
                    }
                }
                else //fixed blocksize
                {
                    uint framenumber;

                    if (reader.ReadUTF8_32(out framenumber) && framenumber != uint.MaxValue)
                    {
                        BlockingStrategy = BlockingStrategy.FixedBlockSize;
                        FrameNumber = (int) framenumber;
                    }
                    else
                    {
                        Error("Invalid UTF8 Framenumber coding.", loggerLocation);
                        return false;
                    }
                }

                #endregion utf8

                #region read hints

                //blocksize am ende des frameheaders
                if (_blocksizeHint != 0)
                {
                    val = (int)reader.ReadBits(8);
                    if (_blocksizeHint == 7)
                    {
                        val = (val << 8) | (int)reader.ReadBits(8);
                    }
                    BlockSize = val + 1;
                }

                //samplerate
                if (_sampleRateHint != 0)
                {
                    val = (int)reader.ReadBits(8);
                    if (_sampleRateHint != 12)
                    {
                        val = (val << 8) | (int)reader.ReadBits(8);
                    }
                    if (_sampleRateHint == 12)
                        SampleRate = val * 1000;
                    else if (_sampleRateHint == 13)
                        SampleRate = val;
                    else
                        SampleRate = val * 10;
                }

                #endregion read hints

                if (DoCrc)
                {
                    var crc8 = Utils.CRC8.Instance.CalcCheckSum(reader.Buffer, 0, reader.Position);
                    Crc8 = (byte) reader.ReadBits(8);
                    if (Crc8 != crc8)
                    {
                        Error("CRC8 missmatch", loggerLocation);
                        return false;
                    }
                }
                else
                {
                    Crc8 = (byte) reader.ReadBits(8);
                }

                headerBuffer += reader.Position;
                return true;
            }

            Error("Invalid Syncbits", loggerLocation);
            return false;
        }

        [Conditional("DEBUG")]
        internal void Error(string msg, string location)
        {
            if (PrintErrors)
                Debug.WriteLine(location + msg);
        }

        /// <summary>
        /// Indicates whether the format of the current <see cref="FlacFrameHeader"/> is equal to the format of another <see cref="FlacFrameHeader"/>.
        /// </summary>
        /// <param name="other">A <see cref="FlacFrameHeader"/> which provides the format to compare with the format of the current <see cref="FlacFrameHeader"/>.</param>
        /// <returns><c>true</c> if the format of the current <see cref="FlacFrameHeader"/> is equal to the format of the <paramref name="other"/> <see cref="FlacFrameHeader"/>.</returns>
        public bool IsFormatEqualTo(FlacFrameHeader other)
        {
            return (BitsPerSample == other.BitsPerSample &&
                    Channels == other.Channels &&
                    SampleRate == other.SampleRate);
        }
    }
}