namespace CSCore.Tags.ID3.Frames
{
    public class TermsOfUseFrame : TextFrame
    {
        public string Language { get; private set; }

        public TermsOfUseFrame(FrameHeader header)
            : base(header)
        {
        }

        protected override void Decode(byte[] content)
        {
            int offset = 0;
            var encoding = ID3Utils.GetEncoding(content, 0, 4);
            offset++;

            ID3Utils.ReadString(content, offset, 3, ID3Utils.Iso88591);
            offset += 3;

            Text = ID3Utils.ReadString(content, offset, -1, encoding);
        }
    }
}