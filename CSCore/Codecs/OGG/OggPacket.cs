using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CSCore.Codecs.VORBIS;

namespace CSCore.Codecs.OGG
{
    public class OggPacket : VORBIS.Packet
    {
        OggPacket _nextPacket;
        Stream _stream;
        long _streamPosition;
        int _length;
        int _offset = 0;

        byte[] _buffer;
        int _boffset;

        public long StartOffset
        {
            get { return _streamPosition; }
        }

        public OggPacket(Stream stream, long streamPosition, int length)
        {
            //if (_stream == null)
            //    throw new ArgumentNullException("stream");
            //todo: argumentcheck?

            _stream = stream;
            _streamPosition = streamPosition;
            Length = length;
        }

        public void SetContentBuffer(byte[] buffer, int offset)
        {
            _buffer = buffer;
            _boffset = offset;
        }

        protected override int ReadBytes(byte[] buffer, int offset, int count)
        {
            int b = -1;
            int c = 0;
            while ((b = ReadNextByte()) > -1 && c < count)
            {
                buffer[offset + c] = (byte)b;
                c++;
            }

            return c;
        }

        protected override int ReadNextByte()
        {
            if (_offset == _length)
            {
                if (_nextPacket != null)
                    return _nextPacket.ReadNextByte();
                return -1;
            }
            else
            {
                if (_buffer != null)
                {
                    var result = _buffer[_boffset + _offset];
                    _offset++;
                    return result;
                }

                _stream.Seek(_offset + _streamPosition, SeekOrigin.Begin);
                var r = _stream.ReadByte();
                _offset++;
                return r;
            }
        }

        public override void AppendPacket(VORBIS.Packet packet)
        {
            if (!(packet is OggPacket))
                throw new ArgumentException("Invalid packet type");

            Length += packet.Length;
            if (_nextPacket != null)
                _nextPacket.AppendPacket(packet);
            else
                _nextPacket = packet as OggPacket;

            PageGranulePosition = packet.PageGranulePosition;
            PageSequenceNumber = packet.PageSequenceNumber;
        }
    }
}