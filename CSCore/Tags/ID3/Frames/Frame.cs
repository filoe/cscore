using System;
using System.IO;

namespace CSCore.Tags.ID3.Frames
{
    public abstract class Frame
    {
        public static Frame FromStream(Stream stream, ID3v2 tag)
        {
            bool result = false;
            FrameHeader header = new FrameHeader(stream, tag.Header.Version);
            //Console.WriteLine(header.FrameID);
            long streamPosition = stream.Position + header.FrameSize;
            var frame = FrameFactory.Instance.TryGetFrame(header, tag.Header.Version, stream, out result);
            if (stream.Position != streamPosition)
                stream.Position = streamPosition;

            return frame;
        }

        public FrameHeader Header { get; private set; }

        public string FrameID { get { return Header.FrameID; } }

        public int FrameSize { get { return Header.FrameSize; } }

        public Frame(FrameHeader header)
        {
            Header = header;
        }

        public void DecodeContent(byte[] content)
        {
            if (content == null || content.Length < 1)
                throw new ArgumentException("content is null or length < 1");

            Decode(content);
        }

        protected abstract void Decode(byte[] content);
    }
}