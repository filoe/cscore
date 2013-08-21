using CSCore.Utils;
using System;
using System.Diagnostics;
using System.IO;

namespace CSCore.Codecs.FLAC
{
    public class FlacFrameHeader
    {
        private int blocksize_hint = 0; //if bsindex == 6 || 7
        private int sampleRate_hint = 0; //if sampleRateIndex == 12 || 13 || 14

        public int BlockSize { get; set; }

        public int SampleRate { get; set; }

        public int Channels { get; set; }

        public ChannelAssignment ChannelAssignment { get; set; }

        public int BitsPerSample { get; set; }

        //union
        public FlacNumberType NumberType { get; set; }

        public ulong SampleNumber { get; set; }

        public uint FrameNumber { get; set; }

        public byte CRC8 { get; set; }

        public bool DoCRC { get; private set; }

        public bool HasError { get; private set; }

        public long StreamPosition { get; private set; }

        internal bool printErrors = true;

        public FlacFrameHeader(Stream stream)
            : this(stream, null, true)
        {
        }

        public FlacFrameHeader(Stream stream, FlacMetadataStreamInfo streamInfo)
            : this(stream, streamInfo, true)
        {
        }

        /// <param name="streamInfo">Can be null</param>
        public FlacFrameHeader(Stream stream, FlacMetadataStreamInfo streamInfo, bool doCrc)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (stream.CanRead == false) throw new ArgumentException("stream is not readable");
            //streamInfo can be null

            DoCRC = doCrc;
            StreamPosition = stream.Position;

            HasError = !ParseHeader(stream, streamInfo);
        }

        public unsafe FlacFrameHeader(ref byte* buffer, FlacMetadataStreamInfo streamInfo, bool doCrc)
            : this(ref buffer, streamInfo, doCrc, true)
        {
        }

        internal unsafe FlacFrameHeader(ref byte* buffer, FlacMetadataStreamInfo streamInfo, bool doCrc, bool logError)
        {
            printErrors = logError; //optimized for prescan

            DoCRC = doCrc;
            StreamPosition = -1;

            HasError = !ParseHeader(ref buffer, streamInfo);
        }

        protected unsafe virtual bool ParseHeader(Stream stream, FlacMetadataStreamInfo streamInfo)
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

        protected unsafe virtual bool ParseHeader(ref byte* headerBuffer, FlacMetadataStreamInfo streamInfo)
        {
            const string loggerLocation = "FlacFrameHeader.ParseHeader(byte*, FlacMetadataStreamInfo)";
            int x = -1; //tmp value to store in
            if (headerBuffer[0] == 0xFF && headerBuffer[1] >> 1 == 0x7C) //sync bits
            {
                if ((headerBuffer[1] & 0x02) != 0) // ...10 2. letzes bits muss 0 sein
                {
                    Error("Invalid FlacFrame. Reservedbit_0 is 1", loggerLocation);
                    return false;
                }

                byte* __headerbufferPtr = headerBuffer;
                FlacBitReader reader = new FlacBitReader(__headerbufferPtr, 0);

                #region blocksize

                //blocksize
                x = headerBuffer[2] >> 4;
                int blocksize = -1;

                if (x == 0)
                {
                    Error("Invalid Blocksize value: 0", loggerLocation);
                    return false;
                }
                else if (x == 1)
                    blocksize = 192;
                else if (x >= 2 && x <= 5)
                    blocksize = 576 << (x - 2);
                else if (x == 6 || x == 7)
                    blocksize_hint = x;
                else if (x >= 8 && x <= 15)
                    blocksize = 256 << (x - 8);
                else
                {
                    Error("Invalid Blocksize value: " + x, loggerLocation);
                    return false;
                }
                BlockSize = blocksize;

                #endregion blocksize

                #region samplerate

                //samplerate
                x = headerBuffer[2] & 0x0F;
                int sampleRate = -1;

                if (x == 0)
                {
                    if (streamInfo != null)
                        sampleRate = streamInfo.SampleRate;
                    else
                    {
                        Error("Missing Samplerate. Samplerate Index = 0 && streamInfoMetaData == null.", loggerLocation);
                        return false;
                    }
                }
                else if (x >= 1 && x <= 11)
                    sampleRate = FlacConstant.SampleRateTable[x];
                else if (x >= 12 && x <= 14)
                    sampleRate_hint = x;
                else
                {
                    Error("Invalid SampleRate value: " + x, loggerLocation);
                    return false;
                }
                SampleRate = sampleRate;

                #endregion samplerate

                #region channels

                x = headerBuffer[3] >> 4; //cc: unsigned
                int channels = -1;
                if ((x & 8) != 0)
                {
                    channels = 2;
                    if ((x & 7) > 2 || (x & 7) < 0)
                    {
                        Error("Invalid ChannelAssignment", loggerLocation);
                        return false;
                    }
                    else
                        ChannelAssignment = (ChannelAssignment)((x & 7) + 1);
                }
                else
                {
                    channels = x + 1;
                    ChannelAssignment = ChannelAssignment.Independent;
                }
                Channels = channels;

                #endregion channels

                #region bitspersample

                x = (headerBuffer[3] & 0x0E) >> 1;
                int bitsPerSample = -1;
                if (x == 0)
                {
                    if (streamInfo != null)
                        bitsPerSample = streamInfo.BitsPerSample;
                    else
                    {
                        Error("Missing BitsPerSample. Index = 0 && streamInfoMetaData == null.", loggerLocation);
                        return false;
                    }
                }
                else if (x == 3 || x >= 7 || x < 0)
                {
                    Error("Invalid BitsPerSampleIndex", loggerLocation);
                    return false;
                }
                else
                    bitsPerSample = FlacConstant.BitPerSampleTable[x];

                BitsPerSample = bitsPerSample;

                #endregion bitspersample

                if ((headerBuffer[3] & 0x01) != 0) // reserved bit -> 0
                {
                    Error("Invalid FlacFrame. Reservedbit_1 is 1", loggerLocation);
                    return false;
                }

                //reader.SkipBits(4 * 8); //erste 3 bytes headerbytes überspringen, da diese schon ohne reader verarbeitet
                reader.ReadBits(32);

                //BYTE 4

                #region utf8

                //variable blocksize
                if ((headerBuffer[1] & 0x01) != 0 ||
                    (streamInfo != null && streamInfo.MinBlockSize != streamInfo.MaxBlockSize))
                {
                    ulong samplenumber;
                    if (reader.ReadUTF8_64(out samplenumber) && samplenumber != ulong.MaxValue)
                    {
                        NumberType = FlacNumberType.SampleNumber;
                        SampleNumber = samplenumber;
                    }
                    else
                    {
                        Error("Invalid UTF8 Samplenumber coding.", loggerLocation);
                        return false;
                    }
                }
                else //fixed blocksize
                {
                    uint framenumber;// = reader.ReadUTF8();

                    if (reader.ReadUTF8_32(out framenumber) && framenumber != uint.MaxValue)
                    {
                        NumberType = FlacNumberType.FrameNumber;
                        FrameNumber = framenumber;
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
                if (blocksize_hint != 0)
                {
                    x = (int)reader.ReadBits(8);
                    if (blocksize_hint == 7)
                    {
                        x = (x << 8) | (int)reader.ReadBits(8);
                    }
                    BlockSize = x + 1;
                }

                //samplerate am ende des frameheaders
                if (sampleRate_hint != 0)
                {
                    x = (int)reader.ReadBits(8);
                    if (sampleRate_hint != 12)
                    {
                        x = (x << 8) | (int)reader.ReadBits(8);
                    }
                    if (sampleRate_hint == 12)
                        SampleRate = x * 1000;
                    else if (sampleRate_hint == 13)
                        SampleRate = x;
                    else
                        SampleRate = x * 10;
                }

                #endregion read hints

                //if (Channels == 1 && BitsPerSample == 24 && SampleRate == 44100)
                //    System.Diagnostics.Debugger.Break();

                if (DoCRC)
                {
                    var crc8 = CSMath.CRC8.Instance.CalcCheckSum(reader.Buffer, 0, reader.Position);
                    CRC8 = (byte)reader.ReadBits(8);
                    if (CRC8 != crc8)
                    {
                        Error("CRC8 missmatch", loggerLocation);
                        return false;
                    }
                }

                headerBuffer += reader.Position;
                return true;
            }

            Error("Invalid Syncbits", loggerLocation);
            return false;
        }

        internal void Error(string msg, string location)
        {
            if (printErrors)
                Debug.WriteLine(location + msg);
        }

        public bool CompareTo(FlacFrameHeader header)
        {
            return (BitsPerSample == header.BitsPerSample &&
                    Channels == header.Channels &&
                    SampleRate == header.SampleRate);
        }
    }

    public enum FlacNumberType
    {
        SampleNumber,
        FrameNumber
    }

    public enum ChannelAssignment
    {
        Independent = 0,
        LeftSide = 1,
        RightSide = 2,
        MidSide = 3,
    }
}