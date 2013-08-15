namespace CSCore.Tags.ID3.Frames
{
    public class OwnershipFrame : TextFrame
    {
        public string Price { get; private set; }

        public string PurchaseDate { get; private set; }

        public OwnershipFrame(FrameHeader header)
            : base(header)
        {
        }

        protected override void Decode(byte[] content)
        {
            int offset = 1;
            if (content.Length < 10)
                throw new ID3Exception("Invalid Contentlength");

            int read;
            Price = ID3Utils.ReadString(content, offset, -1, ID3Utils.Iso88591, out read);
            offset += read;

            PurchaseDate = ID3Utils.ReadString(content, offset, 8, ID3Utils.Iso88591);
            offset += 8;

            var encoding = ID3Utils.GetEncoding(content, 0, offset);
            Text = ID3Utils.ReadString(content, offset, -1, encoding);
        }
    }
}