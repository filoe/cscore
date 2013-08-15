using System.Text;

namespace CSCore.Tags.ID3.Frames
{
    public class UserDefiniedTextFrame : TextFrame
    {
        public string Description { get; private set; }

        public UserDefiniedTextFrame(FrameHeader header)
            : base(header)
        {
        }

        protected override void Decode(byte[] content)
        {
            if (content.Length == 0)
                throw new ID3Exception("Contentlength is zero");

            var info = Header.GetFrameInformation();
            bool url = false;
            url = (info != null) && info.ID == ID3.Frames.FrameID.UserURLLinkFrame;

            var encoding0 = ID3Utils.GetEncoding(content, 0, 1);
            Encoding encoding1 = encoding0;
            if (url)
                encoding1 = ID3Utils.Iso88591;

            int read;
            Description = ID3Utils.ReadString(content, 1, -1, encoding0, out read);

            if (content.Length < read + 1)
                throw new ID3Exception("Frame does not contain any text");

            base.Decode(content, read + 1, -1, encoding1, out read);
        }
    }
}