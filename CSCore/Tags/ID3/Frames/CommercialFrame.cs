using System;

namespace CSCore.Tags.ID3.Frames
{
    public class CommercialFrame : Frame
    {
        public string Price { get; private set; }

        public string IsValidUntil { get; private set; }

        public string ContactURL { get; private set; }

        public ReceivedType ReceivedType { get; private set; }

        public string SellerName { get; private set; }

        public string Description { get; private set; }

        public string LogoMimeType { get; private set; }

        public System.Drawing.Image Image { get; private set; }

        public CommercialFrame(FrameHeader header)
            : base(header)
        {
        }

        protected override void Decode(byte[] content)
        {
            int offset = 1;
            int read;
            Price = ID3Utils.ReadString(content, offset, -1, ID3Utils.Iso88591, out read);
            offset += read;

            IsValidUntil = ID3Utils.ReadString(content, offset, -1, ID3Utils.Iso88591, out read);
            offset += read;

            ContactURL = ID3Utils.ReadString(content, offset, -1, ID3Utils.Iso88591, out read);
            offset += read;

            ReceivedType = (ReceivedType)content[offset];
            offset++;

            var encoding = ID3Utils.GetEncoding(content, 0, offset);
            offset++;

            SellerName = ID3Utils.ReadString(content, offset, -1, encoding, out read);
            offset += read;

            Description = ID3Utils.ReadString(content, offset, -1, encoding, out read);
            offset += read;

            if (offset < content.Length)
            {
                //we've got a attached logo
                LogoMimeType = ID3Utils.ReadString(content, offset, -1, ID3Utils.Iso88591, out read);
                offset += read;

                var logoData = new byte[content.Length - offset];
                Array.Copy(content, offset, logoData, 0, logoData.Length);

                Image = ID3Utils.DecodeImage(logoData, LogoMimeType);
            }
        }
    }
}