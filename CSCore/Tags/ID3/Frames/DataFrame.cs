using System;

namespace CSCore.Tags.ID3.Frames
{
    //UFID or PRIV
    public class DataFrame : Frame
    {
        public string OwnerIdentifier { get; private set; }

        public byte[] Data { get; private set; }

        public DataFrame(FrameHeader header)
            : base(header)
        {
        }

        protected override void Decode(byte[] content)
        {
            if (content.Length == 0)
                throw new ID3Exception("Contentlength is zero");

            int read;
            OwnerIdentifier = ID3Utils.ReadString(content, 0, content.Length, ID3Utils.Iso88591, out read);
            Data = new byte[content.Length - read];
            if (Data.Length > 0)
            {
                Array.Copy(content, read, Data, 0, Data.Length);
            }
        }
    }
}