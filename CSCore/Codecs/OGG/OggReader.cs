using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSCore.Codecs.OGG
{
    public class OggReader
    {
        public Dictionary<int, OggPacketReader> _readers;
        Stream _stream;

        public OggReader(string fileName)
            : this(File.OpenRead(fileName))
        {
        }

        public OggReader(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (!stream.CanRead) throw new ArgumentException("stream not readable");

            _readers = new Dictionary<int, OggPacketReader>();
            _stream = stream;
        }

        public bool ReadPage()
        {
            OggPageHeader header;
            if (OggPageHeader.FromStream(_stream, out header))
            {
                OggPacketReader reader;
                if (!_readers.TryGetValue(header.StreamSerial, out reader))
                {
                    reader = new OggPacketReader(_stream);
                    _readers.Add(header.StreamSerial, reader);
                }
                reader.AddPackets(header, _stream);
            }

            return false;
        }
    }
}