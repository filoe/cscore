using System.Text;

namespace CSCore.Tags.ID3.Frames
{
    public class TextFrame : Frame
    {
        public virtual string Text { get; protected set; }

        public TextFrame(FrameHeader header)
            : base(header)
        {
        }

        protected override void Decode(byte[] content)
        {
            if (content == null || content.Length < 1)
                return;

            Encoding encoding = ID3Utils.GetEncoding(content, 0, 1);
            int read;
            Decode(content, 0, -1, encoding, out read);
        }

        protected void Decode(byte[] content, int offset, int count, Encoding encoding, out int read)
        {
            if (content.Length == 0)
                throw new ID3Exception("Contentlength is zero");

            Text = ID3Utils.ReadString(content, 0, content.Length - 1, encoding, out read);
        }
    }
}