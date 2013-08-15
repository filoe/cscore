using System;

namespace CSCore.Tags.ID3.Frames
{
    public class BinaryFrame : Frame
    {
        public Byte[] Data { get; private set; }

        public BinaryFrame(FrameHeader header)
            : base(header)
        {
        }

        protected override void Decode(byte[] content)
        {
            if (content.Length == 0)
                throw new ID3Exception("Contentlength is zero");

            Data = new byte[content.Length];
            Array.Copy(content, 0, Data, 0, Data.Length);
        }
    }
}