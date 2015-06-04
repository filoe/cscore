using System;

namespace CSCore.Codecs.MP3
{
    //see http://www.codeproject.com/KB/audio-video/mpegaudioinfo.aspx#XINGHeader and www.mp3-tech.org/programmer/sources/vbrheadersdk.zip.
    /// <summary>
    /// Defines a Xing-Header.
    /// </summary>
    public class XingHeader
    {
        private int _startIndex;
        private int _endIndex;

        private int _qualityIndicator = -1;
        private int _tocOffset = -1;
        private int _framesOffset = -1;
        private int _bytesOffset = -1;

        /// <summary>
        /// Gets the header flags of the <see cref="XingHeader"/>.
        /// </summary>
        public XingHeaderFlags HeaderFlags
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="XingHeader"/> of a <see cref="Mp3Frame"/>. If the <paramref name="frame"/> does not has an <see cref="XingHeader"/> the return value will be null.
        /// </summary>
        /// <param name="frame"><see cref="Mp3Frame"/> which should get checked whether it contains a <see cref="XingHeader"/>.</param>
        /// <returns><see cref="XingHeader"/> of the specified <paramref name="frame"/> or null.</returns>
        public static XingHeader FromFrame(Mp3Frame frame)
        {
            XingHeader header = new XingHeader();
            int offset = CalcOffset(frame);
            if (offset == -1) 
                return null;

            if (CheckForValidXingHeader(frame, offset))
            {
                header._startIndex = offset;
                offset = offset + 4;
            }
            else
            {
                return null;
            }

            header.HeaderFlags = (XingHeaderFlags)ReadHeaderFlags(frame, offset);
            offset = offset + 4;

            if ((header.HeaderFlags & XingHeaderFlags.Frames) != 0)
            {
                header._framesOffset = offset;
                offset += 4;
            }
            if ((header.HeaderFlags & XingHeaderFlags.Bytes) != 0)
            {
                header._bytesOffset = offset;
                offset += 4;
            }
            if ((header.HeaderFlags & XingHeaderFlags.Toc) != 0)
            {
                header._tocOffset = offset;
                offset += 100;
            }
            if ((header.HeaderFlags & XingHeaderFlags.QualityIndicator) != 0)
            {
                header._qualityIndicator = ReadHeaderFlags(frame, offset);
                offset += 4;
            }
            header._endIndex = offset;
            return header;
        }

        private static int CalcOffset(Mp3Frame frame)
        {
            int offset = 0;
            if (frame.MPEGVersion == MpegVersion.Version1)
            {
                if (frame.ChannelMode != Mp3ChannelMode.Mono)
                    offset = 32 + 4;
                else
                    offset = 17 + 4;
            }
            else if (frame.MPEGVersion == MpegVersion.Version2)
            {
                if (frame.ChannelMode != Mp3ChannelMode.Mono)
                    offset = 17 + 4;
                else
                    offset = 9 + 4;
            }
            else
            {
                return -1;
            }

            return offset;
        }

        private static bool CheckForValidXingHeader(Mp3Frame frame, int offset)
        {
            byte[] data = null;
            if (frame.ReadData(ref data, 0) < 4)
                return false;
            if (data[offset + 0] == 'X' && data[offset + 1] == 'i' && data[offset + 2] == 'n' && data[offset + 3] == 'g')
            {
                return true;
            }
            else
                return false;
        }

        private static int ReadHeaderFlags(Mp3Frame frame, int offset)
        {
            byte[] data = null;
            if (frame.ReadData(ref data, 0) < 4)
                throw new System.IO.EndOfStreamException();
            int i = 0;
            for (int j = 0; j <= 3; j++)
            {
                i = data[offset + j];
                i <<= 8;
            }

            return i;
        }
    }
}