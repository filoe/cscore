using System;
using System.Diagnostics;
using System.IO;

namespace CSCore.Codecs.MP3
{
    /// <summary>
    /// Represents an MP3 Frame.
    /// </summary>
    public class Mp3Frame
    {
        /// <summary>
        /// Maximum length of one single <see cref="Mp3Frame"/> in bytes.
        /// </summary>
        public const int MaxFrameLength = 0x4000; //16384

        private static readonly int[, ,] BitRates =
        {
            {
                // Version 1
                { 0, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448 },   // Layer 1
                { 0, 32, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 384 },      // Layer 2
                { 0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320 },       // Layer 3
            },
            {
                // Version 2 & 2.5
                { 0, 32, 48, 56, 64, 80, 96, 112, 128, 144, 160, 176, 192, 224, 256 },      // Layer 1
                { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160 },           // Layer 2
                { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160 },           // Layer 3 (same as
                // layer 2)
            }
        };

        private static readonly int[,] SamplesPerFrame =
        {
            //Version 1
            {
                384,    //Layer 1
                1152,   //Layer 2
                1152    //Layer 3
            },
            //Version 2-2.5
            {
                384,    //Layer 1
                1152,   //Layer 2
                576     //Layer 3
            }
        };

        private static readonly int[,] SampleRates =
        {
            //Version 1
            {
                44100,
                48000,
                32000
            },
            //Version 2
            {
                22050,
                24000,
                16000
            },
            //Version 2.5
            {
                11025,
                12000,
                8000
            }
        };

        private readonly Stream _stream;
        private long _streamPosition, _dataPosition;
        private byte[] _headerBuffer;

        /// <summary>
        /// Creates a new instance of the <see cref="Mp3Frame"/> class based on a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> which provides MP3 data.</param>
        /// <returns>A new instance of the <see cref="Mp3Frame"/> class based on the specified <paramref name="stream"/>.</returns>
        public static Mp3Frame FromStream(Stream stream)
        {
            Mp3Frame frame = new Mp3Frame(stream);
            return frame.FindFrame(stream, true) ? frame : null;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Mp3Frame"/> class based on a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> which provides MP3 data.</param>
        /// <param name="data">Byte array which recieves the content of the <see cref="Mp3Frame"/>.</param>
        /// <returns>A new instance of the <see cref="Mp3Frame"/> class based on the specified <paramref name="stream"/>.</returns>        
        public static Mp3Frame FromStream(Stream stream, ref byte[] data)
        {
            Mp3Frame frame = new Mp3Frame(stream);
            if (frame.FindFrame(stream, false))
            {
                data = data.CheckBuffer(frame.FrameLength);
                Array.Copy(frame._headerBuffer, 0, data, 0, 4);
                int read = stream.Read(data, 4, frame.FrameLength - 4);
                if (read != frame.FrameLength - 4)
                    return null;

                return frame;
            }

            return null;
        }

        private Mp3Frame(Stream stream)
        {
            _stream = stream;
        }

        private bool FindFrame(Stream stream, bool seek)
        {
            if (stream == null) 
                throw new ArgumentNullException("stream");
            if (!stream.CanRead) 
                throw new ArgumentException("Stream not readable.");

            byte[] buffer = new byte[4];

            if ((stream.Read(buffer, 0, buffer.Length)) < 4)
            {
                Debug.WriteLine("Stream is EOF.");
                return false;
            }

            //int totalRead = 0;

            _streamPosition = stream.Position;

            while (!ParseFrame(buffer) || MPEGLayer != MpegLayer.Layer3)
            {
                for (int i = 0; i < 3; i++)
                    buffer[i] = buffer[i + 1];

                if ((stream.Read(buffer, 3, 1)) < 1)
                {
                    Debug.WriteLine("Mp3Frame::FindFrame: Stream EOF.");
                    return false;
                }

                /*totalRead += read;
                if (totalRead > MaxFrameLength)
                {
                    Context.Current.Logger.Error("Could not find a MP3 Frame.", loggerLocation);
                    return false;
                }*/

                _streamPosition = stream.Position;
            }

            _headerBuffer = buffer;
            _dataPosition = stream.Position;

            if (_stream.CanSeek && seek)
                _stream.Position += FrameLength - 4;

            if (FrameLength > MaxFrameLength)
                return false;

            return true;
        }

        /// <summary>
        /// Reads data from the <see cref="Mp3Frame"/>.
        /// </summary>
        /// <param name="buffer">Buffer which will receive the read data.</param>
        /// <param name="offset">Zero-based index at which to begin storing data within the <paramref name="buffer"/>.</param>
        /// <returns>The number of read bytes.</returns>
        public int ReadData(ref byte[] buffer, int offset)
        {
            long currentPosition = _stream.Position;

            buffer = buffer.CheckBuffer(FrameLength + offset);
            _stream.Position = _streamPosition;
            int read = _stream.Read(buffer, offset, FrameLength);

            _stream.Position = currentPosition;

            return read;
        }

        //see http://mpgedit.org/mpgedit/mpeg_format/mpeghdr.htm
        private bool ParseFrame(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length < 4) 
                throw new ArgumentException("buffer has to bigger than 3");

            //11111111                    //111
            if ((buffer[0] == 0xFF) && (buffer[1] & 0xE0) == 0xE0 /*11100...*/)
            {
                /*
                 * 2. Byte:
                 * 1111 1111 (1. Byte --> frame sync)
                 * 1110 0000 = frame sync (11 bits)
                 * 0001 1000 = mpeg vers        --> 11000 = 0x18 --> 000...     = >> 3
                 * 0000 0110 = layer index      --> 110   = 0x06 --> 00000...   = >>
                 * 0000 0001 = protection bit  (--> 1     = 0x1) --> 0000000...
                 *
                 */

                MPEGVersion = (MpegVersion)((buffer[1] & 0x18) >> 3);
                if (MPEGVersion == MpegVersion.Reserved)
                {
                    Debug.WriteLine("Mp3Frame::ParseFrame: MPEGVERSION = MpegVersion.Reserved");
                    return false;
                }

                MPEGLayer = (MpegLayer)((buffer[1] & 0x6) >> 1);
                if (MPEGLayer == MpegLayer.Reserved)
                {
                    Debug.WriteLine("Mp3Frame::ParseFrame: MPEGLayer == MpegLayer.Reserved");
                    return false;
                }

                int mpegLayerIndex = 0;
                if (MPEGLayer == MpegLayer.Layer1)
                    mpegLayerIndex = 0;
                else if (MPEGLayer == MpegLayer.Layer2)
                    mpegLayerIndex = 1;
                else
                    mpegLayerIndex = 2;

                CrcEnabled = (buffer[1] & 0x01) == 0x00;

                /*
                 * Byte 3:
                 *
                 * 1111 0000 = Bitrate --> 0xF0 --> >> 4
                 * 0000 1100 = Sampling rate ferquency --> 0xC --> >> 2
                 * 0000 0010 = Padding bit --> 0x02 -->
                 * 0000 0001 = Private bit --> 0x01 -->
                 */

                /*
                 * BitRate
                 *
                 */
                int bitrateIndex = (buffer[2] & 0xF0) >> 4;
                if (bitrateIndex >= 0x0F)
                {
                    //Reserved --> 1111
                    return false;
                }

                BitRate = BitRates[(MPEGVersion == MpegVersion.Version1) ? 0 : 1, mpegLayerIndex, bitrateIndex] * 1000;
                if (BitRate == 0) 
                    return false;

                /*
                 * SamplingFrequenzy
                 *
                 */
                int samplingIndex = (buffer[2] & 0xC) >> 2;
                if (samplingIndex == 0x3 /* 11 */)
                    return false; //reserved

                if (MPEGVersion == MpegVersion.Version25)
                    SampleRate = SampleRates[2, samplingIndex];
                else if (MPEGVersion == MpegVersion.Version2)
                    SampleRate = SampleRates[1, samplingIndex];
                else //version 1
                    SampleRate = SampleRates[0, samplingIndex];

                SampleCount = SamplesPerFrame[MPEGVersion == MpegVersion.Version1 ? 0 : 1, mpegLayerIndex];

                Padding = ((buffer[2] & 0x02) == 0x02);
                bool privateBit = ((buffer[2] & 0x01) == 0x01);

                /*
                 *
                 * Byte 4
                 * 1100 0000 = ChannelMode --> 0xC0 --> >> 6
                 * 0011 0000 = Mode Extension --> 0x30 --> >> 4
                 * 0000 1000 = CopyRight --> 0x08 >> bool
                 * 0000 0100 = Original --> 0x4 >> bool
                 * 0000 0011 = Emphasis --> 0x3 >> nothing
                 *
                 */
                ChannelMode = (Mp3ChannelMode)((buffer[3] & 0xC0) >> 6);
                ChannelExtension = ((buffer[3] & 0x30) >> 4); 
                CopyRight = (buffer[3] & 0x08) == 0x08;
                Original = (buffer[3] & 0x04) == 0x04;
                Emphasis = (buffer[3] & 0x03);

                int coef = SampleCount / 8; //Coefficient
                int tmp = 0;
                tmp = (coef * BitRate / SampleRate + (Padding ? 1 : 0));
                tmp *= (MPEGLayer == MpegLayer.Layer1) ? 4 : 1;
                FrameLength = tmp;

                return (FrameLength <= MaxFrameLength);
            }
            return false;
        }

        /// <summary>
        /// Gets the Mpeg Version.
        /// </summary>
        public MpegVersion MPEGVersion { get; private set; }

        /// <summary>
        /// Gets the Mpeg Layer.
        /// </summary>
        public MpegLayer MPEGLayer { get; private set; }

        /// <summary>
        /// Gets the bit rate.
        /// </summary>
        public int BitRate { get; private set; }

        /// <summary>
        /// Gets the sample rate.
        /// </summary>
        public int SampleRate { get; private set; }

        /// <summary>
        /// Gets the channel mode.
        /// </summary>
        public Mp3ChannelMode ChannelMode { get; private set; }

        /// <summary>
        /// Gets the number of channels.
        /// </summary>
        public short ChannelCount { get { return GetChannelCount(ChannelMode); }}

        /// <summary>
        /// Gets the number of samples
        /// </summary>
        public int SampleCount { get; private set; }

        /// <summary>
        /// Gets the length of the frame.
        /// </summary>
        public int FrameLength { get; private set; }

        /// <summary>
        /// Gets the channel extension.
        /// </summary>
        public int ChannelExtension { get; private set; }

        /// <summary>
        /// Gets a value which indicates whether the copyright flag is set (true means that the copyright flag is set).
        /// </summary>
        public bool CopyRight { get; private set; }

        /// <summary>
        /// Gets a value which indicates whether the original flag is set (true means that the original flag is set).
        /// </summary>
        public bool Original { get; private set; }

        /// <summary>
        /// Gets the emphasis.
        /// </summary>
        public int Emphasis { get; private set; }

        /// <summary>
        /// Gets the padding.
        /// </summary>
        public bool Padding { get; private set; }

        /// <summary>
        /// Gets a value which indicates whether the crc flag is set (true means that the crc flag is set).
        /// </summary>
        public bool CrcEnabled { get; private set; }

        private short GetChannelCount(Mp3ChannelMode channelsMode)
        {
            if (channelsMode == Mp3ChannelMode.Mono)
                return 1;
            if (channelsMode == Mp3ChannelMode.Stereo)
                return 2;
            if (channelsMode == Mp3ChannelMode.JointStereo)
                return 2;
            if (channelsMode == Mp3ChannelMode.DualChannel)
                return 2;

            return 0;
        }
    }
}