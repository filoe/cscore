using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSCore.Codecs.OGG
{
    public class OggPacketReader
    {
        Stream _stream;
        Queue<OggPacket> _packets;

        bool _continues = false;

        public OggPacketReader(Stream stream)
        {
            if (stream == null)               
                throw new ArgumentNullException("stream");
            if (stream.CanRead == false) 
                throw new ArgumentException("stream not readable");
            if (stream.CanSeek == false) 
                throw new ArgumentException("stream not seekable");

            _stream = stream;
            _packets = new Queue<OggPacket>();
        }

        public void AddPackets(OggPageHeader header, Stream stream)
        {
            long offset = header.DataOffset;

            Queue<OggPacket> rawPackets = new Queue<OggPacket>();
            bool firstPacket = true;

            for (int i = 0; i < header.PacketSizes.Length; i++)
            {
                var p = new OggPacket(stream, offset, header.PacketSizes[i])
                {
                    PageGranulePosition = header.GranulePosition,
                    PageSequenceNumber = header.PageSequenceNumber,
                    IsContinued = (header.PacketSizes.Length - i == 1 & header.IsLastPacketContinues),
                    IsContinuation = (firstPacket & ((header.HeaderType & OggPageHeaderType.ContinuedPacket) == OggPageHeaderType.ContinuedPacket)),
                    IsEndOfStream = (firstPacket & ((header.HeaderType & OggPageHeaderType.LastPageOfLBS) == OggPageHeaderType.LastPageOfLBS))
                    //IsResync = (firstPacket & header.)
                };
                p.SetContentBuffer(header.Content, (int)(offset - header.DataOffset));
                rawPackets.Enqueue(p);  //new OggPacket() { Length = header.PacketSizes[i], StartOffset = offset });
                offset += header.PacketSizes[i];
                firstPacket = false;
            }

            if (_continues)
            {
                var packet = _packets.Last();
                ProcessPacket(packet, rawPackets);
            }

            while (rawPackets.Count > 0)
            {
                OggPacket packet = new OggPacket(stream, rawPackets.Peek().StartOffset, 0);  //new OggPacket() { StartOffset = rawPackets.Peek().StartOffset };
                ProcessPacket(packet, rawPackets);
                _packets.Enqueue(packet);
            }

            _continues = header.IsLastPacketContinues;
        }

        private void ProcessPacket(OggPacket packet, Queue<OggPacket> source)
        {
            if (source.Count <= 0) return;

            do
            {
                var p = source.Dequeue();
                packet.AppendPacket(p);
            } while (source.Count > 0 && source.Peek().IsContinued);
        }
    }
}