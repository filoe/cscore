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
            //byte textEncoding = content[0];
            //Text = ID3Utils.ReadString(content, 1, content.Length - 1, ID3Utils.Iso88591);
            int read;
            Decode(content, 0, -1, ID3Utils.Iso88591, out read);
        }

        protected void Decode(byte[] content, int offset, int count, Encoding encoding, out int read)
        {
            if (content.Length == 0)
                throw new ID3Exception("Contentlength is zero");

            Text = ID3Utils.ReadString(content, 0, content.Length - 1, ID3Utils.Iso88591, out read);
        }
    }
}