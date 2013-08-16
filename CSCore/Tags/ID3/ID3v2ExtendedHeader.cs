using System;
using System.IO;

namespace CSCore.Tags.ID3
{
    public class ID3v2ExtendedHeader
    {
        //länge des extendedheaders ohne die headerlength(4 byte)
        public int HeaderLength { get; private set; }

        public int PaddingSize { get; private set; }

        public ID3v2ExtendedHeaderFlags Flags { get; private set; }

        public byte[] CRC { get; private set; }

        //ID3v4
        public ID3v2TagSizeRestriction TagSizeRestriction { get; private set; }

        public ID3v2TextEncodingRestriction TextEncodingRestriction { get; private set; }

        public ID3v2TextFieldSizeRestriction TextFieldSizeRestriction { get; private set; }

        public ID3v2ImageEncodingRestriction ImageEncodingRestriction { get; private set; }

        public ID3v2ImageSizeRestriction ImageSizeRestriction { get; private set; }

        public ID3Version Version { get; private set; }

        public ID3v2ExtendedHeader(Stream stream, ID3Version version)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (!stream.CanRead) throw new ArgumentException("stream not readable");

            Version = version;

            if (version == ID3Version.ID3v2_3)
                Parse3(stream);
            else if (version == ID3Version.ID3v2_4)
                Parse4(stream);
            else
                throw new ID3Exception("Invalid version in extendedheader");
        }

        private void Parse3(Stream stream)
        {
            HeaderLength = ID3Utils.ReadInt32(stream, false);
            byte[] flags = ID3Utils.Read(stream, 2);
            PaddingSize = ID3Utils.ReadInt32(stream, false);

            if ((flags[0] & 0x7F) != 0 || flags[1] != 0)
                throw new ID3Exception("Invalid ExtendedHeaderflags");
            //Flags = (ID3v2ExtendedHeaderFlags)flags[0];

            if ((flags[0] & 0x80) != 0)
            {
                Flags |= ID3v2ExtendedHeaderFlags.CrcPresent;
                CRC = ID3Utils.Read(stream, 4);
            }
        }

        //http://id3.org/id3v2.4.0-structure
        private void Parse4(Stream stream)
        {
            HeaderLength = ID3Utils.ReadInt32(stream, false);
            byte[] flags = ID3Utils.Read(stream, 1);
            Flags = (ID3v2ExtendedHeaderFlags)flags[0];

            if ((Flags & ID3v2ExtendedHeaderFlags.CrcPresent) == ID3v2ExtendedHeaderFlags.CrcPresent)
                CRC = ID3Utils.Read(stream, 5);

            if ((Flags & ID3v2ExtendedHeaderFlags.Restrict) == ID3v2ExtendedHeaderFlags.Restrict)
            {
                //%ppqrrstt
                TagSizeRestriction = (ID3v2TagSizeRestriction)(flags[0] & 0xC0); //p --> last 2 bit
                TextEncodingRestriction = (ID3v2TextEncodingRestriction)(flags[0] & 0x20); //q
                TextFieldSizeRestriction = (ID3v2TextFieldSizeRestriction)(flags[0] & 0x18); //r
                ImageEncodingRestriction = (ID3v2ImageEncodingRestriction)(flags[0] & 0x4); //s
                ImageSizeRestriction = (ID3v2ImageSizeRestriction)(flags[0] & 0x3); //t
            }

            if ((flags[0] & 0x8F) != 0)
                throw new ID3Exception("Invalid Extendedheaderflags");
        }
    }
}