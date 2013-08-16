using System;
using System.IO;

namespace CSCore.Tags.ID3
{
    public class ID3v2Header
    {
        public const int HeaderLength = 10;

        public static ID3v2Header FromStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (!stream.CanRead) throw new ArgumentException("stream not readable");
            if (!stream.CanSeek) throw new ArgumentException("stream not seekable");

            BinaryReader reader = new BinaryReader(stream);
            ID3v2Header header;

            byte[] buffer = new byte[HeaderLength];
            int read = stream.Read(buffer, 0, buffer.Length);
            if (read < 10)
                throw new EndOfStreamException();

            if (buffer[0] == 0x49 && //I
               buffer[1] == 0x44 && //D
               buffer[2] == 0x33)
            {
                header = new ID3v2Header();

                header.Version = (ID3Version)buffer[3];
                header.RawVersion = new byte[] { buffer[3], buffer[4] };
                header.Flags = (ID3v2HeaderFlags)buffer[5];

                /*header.DataLength = buffer[6] * (1 << 21);
                header.DataLength += buffer[7] * (1 << 14);
                header.DataLength += buffer[8] * (1 << 7);
                header.DataLength += buffer[9];
                */
                header.DataLength = ID3Utils.ReadInt32(buffer, 6, true);

                return header;
            }

            stream.Position -= read;
            return null;
        }

        private ID3v2Header()
        {
        }

        public ID3Version Version { get; private set; }

        public byte[] RawVersion { get; private set; }

        public int DataLength { get; private set; }

        public ID3v2HeaderFlags Flags { get; private set; }

        public bool IsUnsync 
        { 
            get { return (Flags & ID3v2HeaderFlags.Unsynchronisation) == ID3v2HeaderFlags.Unsynchronisation; } 
        }
    }
}