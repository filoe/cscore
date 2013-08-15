using System;
using System.IO;
using System.Linq;

namespace CSCore.Tags.ID3.Frames
{
    public class FrameHeader
    {
        public string FrameID { get; private set; }

        public int FrameSize { get; private set; }

        public FrameFlags Flags { get; private set; }

        public byte GroupIdentifier { get; private set; }

        private int _uncompressedSize, _dataLengthIndicator;
        private byte _encryptionMethod;

        public FrameIDFactory2.ID3v2FrameEntry GetFrameInformation()
        {
            return FrameIDFactory2.Frames.Where((x) => x.ID3v4ID == FrameID || x.ID3v3ID == FrameID || x.ID3v2ID == FrameID).First();
        }

        public FrameHeader(Stream stream, ID3Version version)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (!stream.CanRead) throw new ArgumentException("stream is not readable");

            switch (version)
            {
                case ID3Version.ID3v2_2:
                    Parse2(stream);
                    break;

                case ID3Version.ID3v2_3:
                    Parse3(stream);
                    break;

                case ID3Version.ID3v2_4:
                    Parse4(stream);
                    break;

                default:
                    throw new ID3Exception("Invalid ID3Version in Frameheader");
            }
        }

        private void Parse2(Stream stream)
        {
            byte[] buffer = ID3Utils.Read(stream, 6);
            FrameID = ID3Utils.ReadString(buffer, 0, 3, ID3Utils.Iso88591);
            FrameSize = ID3Utils.ReadInt32(buffer, 3, false, 3);

            //Flags not supported
            Flags = FrameFlags.None;
        }

        private void Parse3(Stream stream)
        {
            byte[] buffer = ID3Utils.Read(stream, 10);
            FrameID = ID3Utils.ReadString(buffer, 0, 4, ID3Utils.Iso88591);
            FrameSize = ID3Utils.ReadInt32(buffer, 4, false, 4);

            byte[] flags = new byte[] { buffer[8], buffer[9] };

            //%abc00000
            if ((flags[0] & 0x80) == 0)
                Flags |= FrameFlags.PreserveTagAltered;
            if ((flags[0] & 0x40) == 0)
                Flags |= FrameFlags.PreserveFileAltered;
            if ((flags[0] & 0x20) != 0)
                Flags |= FrameFlags.ReadOnly;
            //%ijk00000
            if ((flags[1] & 0x80) != 0)
            {
                Flags |= FrameFlags.Compressed;
                _uncompressedSize = ID3Utils.ReadInt32(stream, false);
                FrameSize -= 4;
            }
            if ((flags[1] & 0x40) != 0)
            {
                Flags |= FrameFlags.Encrypted;
                _encryptionMethod = ID3Utils.Read(stream, 1)[0];
                FrameSize -= 1;
            }
            if ((flags[1] & 0x20) != 0)
            {
                Flags |= FrameFlags.GroupIdentified;
                GroupIdentifier = ID3Utils.Read(stream, 1)[0];
                FrameSize -= 1;
            }
        }

        private void Parse4(Stream stream)
        {
            byte[] buffer = ID3Utils.Read(stream, 10);
            FrameID = ID3Utils.ReadString(buffer, 0, 4, ID3Utils.Iso88591);
            FrameSize = ID3Utils.ReadInt32(buffer, 4, true, 4);

            byte[] flags = new byte[] { buffer[8], buffer[9] };

            //%0abc0000 Framestatusflags
            if ((flags[0] & 0x40) == 0)
                Flags |= FrameFlags.PreserveTagAltered;
            if ((flags[0] & 0x20) == 0)
                Flags |= FrameFlags.PreserveFileAltered;
            if ((flags[0] & 0x10) != 0)
                Flags |= FrameFlags.ReadOnly;
            //%0h00kmnp Frameformatflags
            if ((flags[1] & 0x40) != 0)
            {
                Flags |= FrameFlags.GroupIdentified;
                GroupIdentifier = ID3Utils.Read(stream, 1)[0];
                FrameSize -= 1;
            }
            if ((flags[1] & 0x8) != 0)
                Flags |= FrameFlags.Compressed;
            if ((flags[1] & 0x4) != 0)
            {
                Flags |= FrameFlags.Encrypted;
                _encryptionMethod = ID3Utils.Read(stream, 1)[0];
                FrameSize -= 1;
            }
            if ((flags[1] & 0x2) != 0)
                Flags |= FrameFlags.UnsyncApplied; //todo: tag allgemein benachrichtigen?
            if ((flags[1] & 0x1) != 0)
            {
                Flags |= FrameFlags.DataLengthIndicatorPresent;
                _dataLengthIndicator = ID3Utils.ReadInt32(stream, true);
                FrameSize -= 4;
            }
        }
    }
}