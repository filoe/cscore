using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSCore.Codecs.OGG
{
    public class OggPageHeader
    {
        internal static readonly byte[] OggS = new byte[] { 0x4F, 0x67, 0x67, 0x53 };

        public static bool FromStream(Stream stream, out OggPageHeader header)
        {
            header = null;
            if (!FindSync(stream)) return false;

            return ParseHeader(stream, out header);
        }

        public unsafe static bool FindSync(Stream stream)
        {
            //performance optimierung
            if (stream.ReadByte() == OggS[0])
            {
                if (stream.ReadByte() == OggS[1] && stream.ReadByte() == OggS[2] && stream.ReadByte() == OggS[3])
                {
                    stream.Position -= 4;
                    return true;
                }
                stream.Position -= 1;
            }

            byte[] buffer = new byte[400];
            fixed (byte* ptrBuffer = buffer)
            {
                int read = 0;
                int total = 0;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0 && total < 65536)
                {
                    int c = 0;
                    byte* ptr = ptrBuffer;
                    while (c < read)
                    {
                        if (*(ptr++) == OggS[0] && *(ptr++) == OggS[1] &&
                           *(ptr++) == OggS[2] && *(ptr++) == OggS[3])
                        {
                            stream.Seek(-(read - c), SeekOrigin.Current);
                            return true;
                        }
                        c++;
                    }

                    total += read;
                }
            }

            return false;
        }

        //http://xiph.org/vorbis/doc/framing.html
        private unsafe static bool ParseHeader(Stream stream, out OggPageHeader resultHeader)
        {
            resultHeader = null;
            OggPageHeader header = new OggPageHeader();
            CRC32 crc = new CRC32();
            byte[] headerBuffer = new byte[27];

            if (stream.Read(headerBuffer, 0, headerBuffer.Length) != headerBuffer.Length)
            {
                return false;
            }

            if (headerBuffer[0] == OggS[0] && headerBuffer[1] == OggS[1] && headerBuffer[2] == OggS[2] && headerBuffer[3] == OggS[3] && //capture_pattern "OggS"
                headerBuffer[4] == 0) // stream_structure_version
            {
                header.HeaderType = (OggPageHeaderType)headerBuffer[5];
                header.GranulePosition = BitConverter.ToInt64(headerBuffer, 6);
                header.StreamSerial = BitConverter.ToInt32(headerBuffer, 14);
                header.PageSequenceNumber = BitConverter.ToInt32(headerBuffer, 18);
                header.Checksum = BitConverter.ToUInt32(headerBuffer, 22);

                crc.Add(headerBuffer, 0, 22);
                for (int i = 0; i < 4; i++) crc.Add(0); //checksum auslassen
                crc.Add(headerBuffer[26]);

                int segments = headerBuffer[26];
                int* packetSizes = stackalloc int[segments];

                byte[] rawSegmentTable = new byte[segments];
                if (stream.Read(rawSegmentTable, 0, rawSegmentTable.Length) != rawSegmentTable.Length)
                    return false;

                int currentPacket = 0;
                int totalSize = 0;

                for (int i = 0; i < segments; i++)
                {
                    byte size = rawSegmentTable[i];
                    packetSizes[currentPacket] += size;

                    if (size < 255) currentPacket++;

                    totalSize += size;
                }
                crc.Add(rawSegmentTable, 0, rawSegmentTable.Length);
                header.IsLastPacketContinues = rawSegmentTable[segments - 1] >= 255;

                if (header.IsLastPacketContinues) currentPacket++; //wenn nicht wurde bereits in schleife erhöht

                header.PacketSizes = new int[currentPacket];
                int contentLength = 0;
                for (int i = 0; i < currentPacket; i++)
                {
                    contentLength += header.PacketSizes[i] = packetSizes[i];
                }

                header.ContentLength = contentLength;

                header.DataOffset = stream.Position;

                byte[] buffer = new byte[totalSize];
                if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                    return false;

                crc.Add(buffer, 0, buffer.Length);
                header.Content = buffer;

                System.Diagnostics.Debug.Assert(crc.Value == header.Checksum);

                buffer = null;
                resultHeader = header;
                return true;
            }

            return false;
        }

        private OggPageHeader() { }

        public OggPageHeaderType HeaderType { get; private set; }
        public long GranulePosition { get; private set; }
        public int StreamSerial { get; private set; }
        public int PageSequenceNumber { get; private set; }
        public uint Checksum { get; private set; }
        public bool IsLastPacketContinues { get; private set; }
        public int[] PacketSizes { get; private set; }
        public long DataOffset { get; private set; }
        public int ContentLength { get; private set; }
        public byte[] Content { get; set; }
    }
}