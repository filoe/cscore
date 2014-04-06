using System;

namespace CSCore.Codecs.MP3
{
    /// <summary>
    /// See http://www.codeproject.com/KB/audio-video/mpegaudioinfo.aspx#XINGHeader and www.mp3-tech.org/programmer/sources/vbrheadersdk.zip.
    /// </summary>
    public class XingHeader
    {
        private int startIndex;
        private int endIndex;

        private int vbrScale = -1;
        private int tocOffset = -1;
        private int framesOffset = -1;
        private int bytesOffset = -1;

        public XingHeaderFlags HeaderFlags
        {
            get;
            private set;
        }

        public static XingHeader FromFrame(MP3Frame frame)
        {
            XingHeader header = new XingHeader();
            int offset = CalcOffset(frame);
            if (offset == -1) 
                return null;

            if (CheckForValidXingHeader(frame, offset))
            {
                header.startIndex = offset;
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
                header.framesOffset = offset;
                offset += 4;
            }
            if ((header.HeaderFlags & XingHeaderFlags.Bytes) != 0)
            {
                header.bytesOffset = offset;
                offset += 4;
            }
            if ((header.HeaderFlags & XingHeaderFlags.Toc) != 0)
            {
                header.tocOffset = offset;
                offset += 100;
            }
            if ((header.HeaderFlags & XingHeaderFlags.VbrScale) != 0)
            {
                header.vbrScale = ReadHeaderFlags(frame, offset);
                offset += 4;
            }
            header.endIndex = offset;
            return header;
        }

        private static int CalcOffset(MP3Frame frame)
        {
            int offset = 0;
            if (frame.MPEGVersion == MpegVersion.Version1)
            {
                if (frame.ChannelMode != MP3ChannelMode.Mono)
                    offset = 32 + 4;
                else
                    offset = 17 + 4;
            }
            else if (frame.MPEGVersion == MpegVersion.Version2)
            {
                if (frame.ChannelMode != MP3ChannelMode.Mono)
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

        private static bool CheckForValidXingHeader(MP3Frame frame, int offset)
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

        private static int ReadHeaderFlags(MP3Frame frame, int offset)
        {
            byte[] data = null;
            if (frame.ReadData(ref data, 0) < 4)
                throw new System.IO.EndOfStreamException();
            int i = 0;
            i = data[offset + 0];
            i <<= 8;
            i |= data[offset + 1];
            i <<= 8;
            i |= data[offset + 2];
            i <<= 8;
            i |= data[offset + 3];

            return i;
        }
    }
}