using CSCore.Tags.ID3.Frames;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CSCore.Tags.ID3
{
    //http://id3.org/id3v2-00
    //http://id3.org/id3v2.4.0-structure
    //http://id3.org/id3v2.3.0
    public class ID3v2 : IEnumerable<Frame>
    {
        public static ID3v2 FromFile(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                return FromStream(stream);
            }
        }

        public static ID3v2 FromStream(Stream stream)
        {
            try
            {
                ID3v2 id3v2 = new ID3v2(stream);
                if (id3v2.ReadData(stream, true))
                    return id3v2;
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        private static ID3v2 FromStream(Stream stream, bool readData)
        {
            ID3v2 id3v2 = new ID3v2(stream);
            if (id3v2.ReadData(stream, readData))
                return id3v2;
            return null;
        }

        public static void SkipTag(Stream stream)
        {
            long streamOffset = stream.Position;
            if (FromStream(stream, false) == null)
                stream.Position = streamOffset;
        }

        private Stream _stream;
        private ID3v2Header _header;
        private ID3v2ExtendedHeader _extendedHeader;
        private ID3v2Footer _footer;
        private List<Frame> _frames = new List<Frame>();
        private ID3v2QuickInfo _quickInfo;

        private byte[] _content;

        public ID3v2Header Header { get { return _header; } }

        public ID3v2ExtendedHeader ExtendedHeader { get { return _extendedHeader; } }

        public ID3v2Footer Footer { get { return _footer; } }

        public ID3v2QuickInfo QuickInfo { get { return _quickInfo ?? (_quickInfo = new ID3v2QuickInfo(this)); } }

        public Frame this[FrameID id]
        {
            get
            {
                return this[FrameIDFactory2.GetID(id, Header.Version)];
            }
        }

        public Frame this[string id]
        {
            get
            {
                return _frames.Where((o) => o.FrameId == id).FirstOrDefault();
            }
        }

        protected ID3v2(Stream stream)
        {
            _stream = stream;
            _frames = new List<Frame>();
        }

        private bool ReadData(Stream stream, bool readData)
        {
            if ((_header = ID3v2Header.FromStream(stream)) != null)
            {
                byte[] buffer = new byte[_header.DataLength];
                if (stream.Read(buffer, 0, buffer.Length) < buffer.Length)
                    return false;

                if (_header.IsUnsync && _header.Version != ID3Version.ID3v2_4)
                {
                    buffer = UnSyncBuffer(buffer);
                }
                _content = buffer;
                MemoryStream contentStream = new MemoryStream(buffer);

                switch (_header.Version)
                {
                    case ID3Version.ID3v2_2:
                        Parse2();
                        break;

                    case ID3Version.ID3v2_3:
                        Parse3(contentStream);
                        break;

                    case ID3Version.ID3v2_4:
                        Parse4(contentStream);
                        break;

                    default:
                        throw new ID3Exception("Invalid Version: [2.{0};{1}]", _header.RawVersion[0], _header.RawVersion[1]);
                }

                if (readData)
                    ReadFrames(contentStream);

                contentStream.Dispose();

                return true;
            }
            else
            {
                Debug.WriteLine("ID3v2::ReadData: No ID3v2 Header found.");
                return false;
            }
        }

        private bool Parse2()
        {
            if (((int)_header.Flags & 0x3F) != 0)
                throw new ID3Exception("Invalid headerflags: 0x{0}.", ((int)_header.Flags).ToString("x"));

            return true;
        }

        private bool Parse3(Stream stream)
        {
            if ((_header.Flags & ID3v2HeaderFlags.ExtendedHeader) == ID3v2HeaderFlags.ExtendedHeader)
            {
                _extendedHeader = new ID3v2ExtendedHeader(stream, ID3Version.ID3v2_3);
            }
            if (((int)_header.Flags & 0x1F) != 0)
                throw new ID3Exception("Invalid headerflags: 0x{0}.", ((int)_header.Flags).ToString("x"));

            return true;
        }

        private bool Parse4(Stream stream)
        {
            if ((_header.Flags & ID3v2HeaderFlags.ExtendedHeader) == ID3v2HeaderFlags.ExtendedHeader)
            {
                _extendedHeader = new ID3v2ExtendedHeader(stream, ID3Version.ID3v2_4);
            }
            if ((_header.Flags & ID3v2HeaderFlags.FooterPresent) == ID3v2HeaderFlags.FooterPresent)
            {
                //footer vom orginal stream lesen - da im neuen stream kein footer vorhanden ist
                _footer = ID3v2Footer.FromStream(_stream);
                if (_footer == null) throw new ID3Exception("Invalid Id3Footer.");
            }
            if (((int)_header.Flags & 0x0F) != 0)
                throw new ID3Exception("Invalid headerflags: 0x{0}.", ((int)_header.Flags).ToString("x"));

            return true;
        }

        private void ReadFrames(Stream stream)
        {
            while (stream.Position < stream.Length)
            {
                if (stream.ReadByte() == 0)
                    break;
                else stream.Position--;
                //var header = new ID3v2FrameHeader(stream, Header.Version);
                var frame = Frame.FromStream(stream, this);
                if (frame != null)
                    _frames.Add(frame);
            }
        }

        private byte[] UnSyncBuffer(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream(buffer);
            UnsyncStream ustream = new UnsyncStream(memoryStream);
            byte[] result = new byte[buffer.Length];

            int read = ustream.Read(result, 0, result.Length);

            if (read < result.Length)
            {
                byte[] newresult = new byte[read];
                Buffer.BlockCopy(result, 0, newresult, 0, read);
                return newresult;
            }

            return result;
        }

        public IEnumerator<Frame> GetEnumerator()
        {
            return _frames.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _frames.GetEnumerator();
        }
    }
}