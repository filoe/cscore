using System;
using System.IO;

namespace CSCore.Tags.ID3
{
    public class ID3v2Footer
    {
        public const int FooterLength = 10;

        public static ID3v2Footer FromStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (!stream.CanRead) throw new ArgumentException("stream not readable");
            if (!stream.CanSeek) throw new ArgumentException("stream not seekable");

            BinaryReader reader = new BinaryReader(stream);
            ID3v2Footer footer;

            byte[] buffer = new byte[FooterLength];
            int read = stream.Read(buffer, 0, buffer.Length);
            if (read < 10)
                throw new EndOfStreamException();

            if (buffer[0] == 0x49 && //I
               buffer[1] == 0x44 && //D
               buffer[2] == 0x33)
            {
                footer = new ID3v2Footer();

                footer.Version = (ID3Version)buffer[3];
                footer.RawVersion = new byte[] { buffer[3], buffer[4] };
                footer.Flags = (ID3v2HeaderFlags)buffer[5];

                /*footer.DataLength = buffer[6] * (1 << 21);
                footer.DataLength += buffer[7] * (1 << 14);
                footer.DataLength += buffer[8] * (1 << 7);
                footer.DataLength += buffer[9];*/
                footer.DataLength = ID3Utils.ReadInt32(buffer, 6, true);

                return footer;
            }

            stream.Position -= read;
            return null;
        }

        private ID3v2Footer()
        {
        }

        public ID3Version Version { get; private set; }

        public byte[] RawVersion { get; private set; }

        public long DataLength { get; private set; }

        public ID3v2HeaderFlags Flags { get; private set; }
    }
}